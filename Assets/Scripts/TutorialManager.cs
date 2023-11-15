using Assets.CodeUtils;
using Assets.Scripts;
using Michsky.MUIP;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class TutorialData
{
    public string titleText { get; set; }
    public string mainText { get; set; }
    public GameObject anim { get; set; }
}

public class TutorialManager : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] TMP_Text mainTextTMP;
    [SerializeField] HorizontalSelector mySelector;

    [SerializeField] GameObject[] prefabs; // prefabs
    [SerializeField] String[] titulos; // prefabs
    [SerializeField] String[] contenidos; // prefabs
    [SerializeField] GameObject tutorialCanvas; // prefabs

    private List<GameObject> anims = new List<GameObject>(); // instantiated game objects
    private List<TutorialData> steps = new List<TutorialData>();

    // Start is called before the first frame update
    private void Start()
    {
        InitTutorial();
    }

    private void OnValueChange(int index)
    {
        mainTextTMP.text = steps[index].mainText;

        foreach (TutorialData step in steps)
        {
            step.anim.SetActive(false);
        }
        steps[index].anim.SetActive(true);
    }

    void InitTutorial()
    {
        Debug.Log("Start tutorial");
        // Show tutorial canvas in front of the user
        //this.GetComponent<LocateMainCanvasInFrontUser>().LocateInMap(); // Victor's way. Doesn't work
        tutorialCanvas.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance;
        tutorialCanvas.transform.rotation = Camera.main.transform.rotation;

        // Instantiate GIF animations GameObjects
        foreach (GameObject prefab in prefabs)
        {
            GameObject anim = Instantiate(prefab, tutorialCanvas.transform);
            anim.SetActive(false);
            anims.Add(anim);
        }
        anims[0].SetActive(true);

        //Set tutorial data for Horizontal Selector

       steps = new List<TutorialData>()
       {
            new TutorialData()
            {
                titleText = "Introducción",
                mainText = "Bienvenido. A continuación, se enseñan brevemente los movimientos e interacciones básicas para poder manejar la aplicación correctamente. Pulse en la flecha hacia la derecha de abajo para continuar.",
                anim = anims[0],
            },
            new TutorialData()
            {
                titleText = "Desplazamiento",
                mainText = "Para moverse por el mapa, mantenga presionado el botón de agarre lateral en cualquiera de los dos controladores y desplace el controlador en el espacio.",
                anim = anims[1],
            },
            new TutorialData()
            {
                titleText = "Rotación",
                mainText = "Para rotar el mapa, posicione cualquiera de los dos mandos en el centro frente a usted, mantenga presionado el botón de agarre lateral en los dos controladores de manera simultánea, y gire el otro controlador alrededor del primero.",
                anim = anims[2],
            },
            new TutorialData()
            {
                titleText = "Zoom",
                mainText = "Para hacer zoom, junte los dos controladores en el centro frente a usted, mantenga presionado el botón de agarre lateral en los dos controladores de manera simultánea, y separe gradualmente ambos controladores.",
                anim = anims[3],
            },
            new TutorialData()
            {
                titleText = "Menú de muñeca",
                mainText = "Para visualizar el menú desde el que activar los dispositivos, gire la muñeca izquierda hasta posicionar la palma de la mano hacia arriba.",
                anim = anims[4],
            },
            new TutorialData()
            {
                titleText = "Selección de dispositivos",
                mainText = "Para seleccionar un dispositivo, apunte con el láser proyectado desde cualquiera de los dos controladores y presione el gatillo correspondiente.",
                anim = anims[5],
            },
            new TutorialData()
            {
                titleText = "Abrir Menú de pausa",
                mainText = "Para abrir el menú de dipositivos donde consultar su estado de forma más detallada, presione el botón X en el controlador izquierdo.",
                anim = anims[7],
            },
            new TutorialData()
            {
                titleText = "Vuelta a la posición inicial",
                mainText = "Si se desea volver a la posición inicial, presione el botón B en el controlador derecho.",
                anim = anims[6],
            },
            new TutorialData()
            {
                titleText = "Abrir Menú de pausa",
                mainText = "Para abrir el menú general de opciones, presione el botón A en el controlador derecho.",
                anim = anims[7],
            },
       };

       // Handle Horizontal Selector items
       mySelector.items.Clear();
        foreach (TutorialData step in steps)
        {
            mySelector.CreateNewItem(step.titleText);
        }

        mySelector.defaultIndex = 0;
        mySelector.SetupSelector();
        mySelector.UpdateUI();
        mySelector.onValueChanged.AddListener(OnValueChange);
    }


    public void ActivateFunctionality()
    {
        tutorialCanvas.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance;
        tutorialCanvas.transform.rotation = Camera.main.transform.rotation;
        tutorialCanvas.SetActive(true);

    }

    public void DesactivateFunctionality()
    {
        tutorialCanvas.SetActive(false);
    }

}
