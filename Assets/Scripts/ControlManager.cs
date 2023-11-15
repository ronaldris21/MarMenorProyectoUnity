using System;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class ControlManager : MonoBehaviour
{
    [SerializeField] ActionBasedController leftController;
    [SerializeField] ActionBasedController rightController;
    [SerializeField] const float gripThreshold = 0.9f;
    [SerializeField] const float RightPrimaryButtonThreshold = 0.9f;
    [SerializeField] GameObject XROrigin;
    [SerializeField] float moveSpeed = 2000f;
    [SerializeField] GameObject map;
    [SerializeField] float zoomFactor = 2f;
    [SerializeField] float maxZoom = 0.4f;
    [SerializeField] float boundaryDistance = 25f;
    [SerializeField] GameObject MenuCanvas;
    [SerializeField] GameObject ExitCanvas;
    [SerializeField] GameObject analyticsController;

    private UGS_Analytics analytics;
    private bool isExitCanvasActive = false;
    private DateTime lastPressedTime = DateTime.Now;
    private bool leftGripPressed = false;
    private bool rightGripPressed = false;
    private bool rightPrimaryButtonPressed = false;
    private bool leftPrimaryButtonPressed = false;
    private bool rightSecondaryButtonPressed = false;
    private bool leftSecondaryButtonPressed = false;
    private Vector3 resetPosition = new Vector3(0.469026893f, 10f, 24.0632038f);
    private enum GripMode { Both, Left, Right, None };
    private GripMode gripMode = GripMode.None;
    ControlManager.GripMode lastGripMode;
    private Vector3 lastLeftPosition;
    private Vector3 lastRightPosition;
    bool getFirstInput = true;
    bool getFirstInputBoth = true;
    Vector3 deltaLeft = new Vector3(0, 0, 0.1f);
    Vector3 deltaRight = new Vector3(0, 0, 0.1f);
    private Quaternion initialObjectRotation;
    float lastDistanceBetweenControllers = 0.25f;
    float deltaDistance;
    Vector3 midPoint;
    Vector3 initialLeftGripPosition = new Vector3(-21.41f, 11.35f, 0.94f);
    Vector3 initialRightGripPosition = new Vector3(-21.40f, 11.36f, 0.57f);
    
    
    private void OnEnable()
    {
        this.GetComponent<ActionManager>().controls.Enable();
    }

    void Update()
    {
        leftGripPressed = this.GetComponent<ActionManager>().leftGripInput > gripThreshold;
        rightGripPressed = this.GetComponent<ActionManager>().rightGripInput > gripThreshold;
        rightPrimaryButtonPressed = Convert.ToBoolean(this.GetComponent<ActionManager>().rightPrimaryButtonInput);
        leftPrimaryButtonPressed = Convert.ToBoolean(this.GetComponent<ActionManager>().leftPrimaryButtonInput);
        rightSecondaryButtonPressed = Convert.ToBoolean(this.GetComponent<ActionManager>().rightSecondaryButtonInput);
        leftSecondaryButtonPressed = Convert.ToBoolean(this.GetComponent<ActionManager>().leftSecondaryButtonInput);

        //On/Off Menu
        if (rightPrimaryButtonPressed)
        {
            MenuState();
        }

        if (leftSecondaryButtonPressed)
        {
            analytics.buttonSecondaryLeft();
            this.GetComponent<TutorialManager>().ActivateFunctionality();
        }

        // Reset position if secondary button is pressed
        if (rightSecondaryButtonPressed)
        {
            analytics.buttonSecondaryRight();
            XROrigin.transform.position = resetPosition;
        }

        if(leftPrimaryButtonPressed)
        {
            analytics.buttonPrimaryLeft();
            this.transform.Find("UIController").gameObject.transform.GetComponent<LocateMainCanvasInFrontUser>().LocateInMap();
        }

        if (leftGripPressed && !rightGripPressed) {
            gripMode = GripMode.Left;
        }
        else if(!leftGripPressed && rightGripPressed) {
            gripMode = GripMode.Right;
        }
        else if(leftGripPressed && rightGripPressed) {
            gripMode = GripMode.Both;
        }
        else{
            gripMode = GripMode.None;
        }

        switch (gripMode) {
            case GripMode.None:
                getFirstInput = true;
                getFirstInputBoth = true;
                lastGripMode = GripMode.None;
                break;
            case GripMode.Both:
                // ZOOM. BRING THE TWO CONTROLLERS CLOSER TO ZOOM IN BY COMPUTING THE DISTANCE
                // BETWEEN THEM. THE POINT TO ZOOM AROUND IS THE MIDDLE POINT BETWEEN THE TWO
                // XR INTERACTOR RAYS. THE PLAYER/CAMERA (XRORIGIN) IS BROUGHT CLOSER TO THE
                // MAP TO GET THE ZOOM EFFECT
                Vector3 currentLeftControllerPosition = leftController.transform.position;
                Vector3 currentRightControllerPosition = rightController.transform.position;
                float currentDistanceBetweenControllers = Vector3.Distance(currentLeftControllerPosition,
                                                                            currentRightControllerPosition);
                deltaDistance = currentDistanceBetweenControllers - lastDistanceBetweenControllers;

                if (getFirstInputBoth)
                {
                    leftController.GetComponent<XRRayInteractor>().TryGetCurrent3DRaycastHit(out RaycastHit leftRaycastHit); // Set Max Raycast Dist to 10000 in the XR Ray Interactor component for the left XR controllers from the inspector
                    rightController.GetComponent<XRRayInteractor>().TryGetCurrent3DRaycastHit(out RaycastHit rightRaycastHit); // Set Max Raycast Dist to 10000 in the XR Ray Interactor component for the right XR controllers from the inspector
                    midPoint = (leftRaycastHit.point + rightRaycastHit.point) * 0.5f;
                }

                Vector3 direction = XROrigin.transform.position - midPoint;
                if(Vector3.Distance(XROrigin.transform.position, midPoint) > maxZoom)
                {
                    XROrigin.transform.position -= direction * deltaDistance * zoomFactor;
                }       

                // ROTATION. SPIN ONE OF THE CONTROLLER AROUND THE OTHER ONE CLOCKWISE TO 
                // ROTATE THE MAP.
                // Add box collider to map first
                
                if (getFirstInputBoth)
                {
                    initialObjectRotation = map.transform.rotation;
                }

                Vector3 handDir1 = (initialLeftGripPosition - initialRightGripPosition).normalized; // direction vector of initial first and second hand position
                Vector3 handDir2 = (currentLeftControllerPosition - currentRightControllerPosition).normalized; // direction vector of current first and second hand position 

                Quaternion handRot = Quaternion.FromToRotation(handDir1, handDir2); // calculate rotation based on those two direction vectors
                Transform targetTransform = map.transform;
                Quaternion resultingQuaterion = handRot * initialObjectRotation;
                Quaternion q = map.transform.rotation;
                q.eulerAngles = new Vector3(0, resultingQuaterion.eulerAngles.y, 0);
                targetTransform.rotation = q;

                lastDistanceBetweenControllers = currentDistanceBetweenControllers;
                getFirstInputBoth = false;
                lastGripMode = GripMode.Both;
                break;

            case GripMode.Left:
                // TRANSLATION WITH LEFT CONTROLLER. WASD MOVEMENT PLUS ALTITUDE
                // To avoid map getting away too far due to transition from GripMode.Both to GripMode.None passing
                // through GripMode.Left for a few milliseconds
                if(lastGripMode == GripMode.Both && (gripMode == GripMode.Left || gripMode == GripMode.Right))
                {
                    break;
                }

                if(!getFirstInput)
                {
                    deltaLeft = (lastLeftPosition - leftController.transform.position);
                }

                XROrigin.transform.position = validposition(new Vector3(deltaLeft.x,
                                                    deltaLeft.y,
                                                    deltaLeft.z));/*+= new Vector3(deltaLeft.x,
                                                    deltaLeft.y,
                                                    deltaLeft.z) * moveSpeed * Time.deltaTime;*/
                lastLeftPosition = leftController.transform.position;
                getFirstInput = false;
                lastGripMode = GripMode.Left;
                break;
            
            case GripMode.Right:
                // TRANSLATION WITH RIGHT CONTROLLER
                // To avoid map getting away too far due to transition from GripMode.Both to GripMode.None passing
                // through GripMode.Right for a few milliseconds
                if(lastGripMode == GripMode.Both && (gripMode == GripMode.Left || gripMode == GripMode.Right))
                {
                    break;
                }

                if(!getFirstInput)
                {
                    deltaRight = (lastRightPosition - rightController.transform.position);
                }

                XROrigin.transform.position = validposition(new Vector3(deltaRight.x,
                                                    deltaRight.y,
                                                    deltaRight.z));
                lastRightPosition = rightController.transform.position;
                getFirstInput = false;
                lastGripMode = GripMode.Right;
                break;
        }

    }

    void Start()
    {
        analytics = analyticsController.GetComponent<UGS_Analytics>();
        MenuCanvas.SetActive(false);
        ExitCanvas.SetActive(false);
    }

    //Activar y desactivar men�
    private void MenuState()
    {
        long tics = (DateTime.Now.Ticks - lastPressedTime.Ticks) / 100000;
        if (tics > 100) {
                lastPressedTime = DateTime.Now;
            
                //Cambiar estado del men�
                isExitCanvasActive = !isExitCanvasActive;
                MenuCanvas.SetActive(isExitCanvasActive);
                ExitCanvas.SetActive(false);    

                if (isExitCanvasActive) {
                    ExitCanvas.SetActive(false);
                    MenuCanvas.GetComponent<LocateMainCanvasInFrontUser>().LocateInMap();
                }
                analytics.buttonPrimaryRight();
        }    
    }



    public Vector3 validposition(Vector3 movimiento)
    {

        Vector3 vectorfinal = XROrigin.transform.position + movimiento * moveSpeed * Time.deltaTime;

        if (Math.Abs(vectorfinal.x) > boundaryDistance || Math.Abs(vectorfinal.y) > boundaryDistance || Math.Abs(vectorfinal.z) > boundaryDistance)
        {
            return XROrigin.transform.position;
        }

        return vectorfinal;

    }

    private void OnDisable()
    {
        this.GetComponent<ActionManager>().controls.Disable();
    }
}
