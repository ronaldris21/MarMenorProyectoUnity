using Michsky.MUIP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PopNotification : MonoBehaviour
{
    [SerializeField] private NotificationManager myNotification; // Variable

    public void mostrarNotificacion(string titulo, string descripcion, int time = 5)
    {
        Debug.Log("mostrarNotificacion");

        myNotification.title = titulo; // Change title
        myNotification.description = descripcion; // Change desc
        myNotification.UpdateUI(); // Update UI
        myNotification.timer = time;
        myNotification.enableTimer = true;
        myNotification.Open(); // Open notification
        
    }

    public void callNotification(string titulo, string descripcion, int time = 4)
    {
        Debug.Log("callNotification");
        mostrarNotificacion(titulo, descripcion,time);
    }

}