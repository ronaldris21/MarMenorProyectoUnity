using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    internal class LocateMainCanvasInFrontUser : MonoBehaviour
    {

        public GameObject cameraObject;
        public GameObject XROrigen;
        public GameObject canvasaMover;
        public float distance = 1f;


        public void LocateInMap() { 

           

            
            ///TODO: FALTA JUGAR CON LA ORIENTACION PARA QUE SE QUEDE EL CANVAS EN PERPENDICULAR A MI
            Camera cameraComponent = cameraObject.GetComponent<Camera>();


            Vector3 direction = cameraComponent.transform.forward;
            direction.Normalize();


            canvasaMover.transform.position = XROrigen.transform.position + (direction)* distance;


            //canvasaMover.transform.LookAt(-1* XROrigen.transform.position);
            canvasaMover.transform.rotation = cameraComponent.transform.rotation;

            canvasaMover.SetActive(true);

        }


        public void DesactivateCanvas()
        {
            canvasaMover.SetActive(false);

        }
    }
}
