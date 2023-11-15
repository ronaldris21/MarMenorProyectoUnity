using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Analytics;
public class UGS_Analytics : MonoBehaviour
{

    //INICIALIZAR ANALYTICS
    async void Start()
    {
        await UnityServices.InitializeAsync();

        Debug.Log($" ESCENA MAR MENOR Started UGS Analytics Sample with user ID: {AnalyticsService.Instance.GetAnalyticsUserID()}");

        GiveConsent();

    }

    public void GiveConsent()
    {
        AnalyticsService.Instance.StartDataCollection();

        Debug.Log($"Consent has been provided.");
    }

    //EVENTOS TOGGLES
    public void toggleSatelite()
    {
        sendDataToAnalytics("toggleSatelite");
    }
    public void toggleCamara()
    {
        sendDataToAnalytics("toggleCamara");
    }
    public void toggleAEMET()
    {
        sendDataToAnalytics("toggleAEMET");
    }
    public void toggleSIAM_IMIDA()
    {
        sendDataToAnalytics("toggleSIAM_IMIDA");
    }
    public void toggleBUOY()
    {
        sendDataToAnalytics("toggleBUOY");
    }

    //EVENTOS DISPOSITIVOS

    public void deviceALL(string device)
    {

        if ("CameraSensor" == device)
        {
            sendDataToAnalytics("deviceCamara");
        }
        else if ("SatelliteSensor" == device)
        {
            sendDataToAnalytics("deviceSatellite");
        }
        else if ("AemetSensor" == device)
        {
            sendDataToAnalytics("deviceAEMET");
        }
        else if ("BuoySensor" == device)
        {
            sendDataToAnalytics("deviceBUOY");
        }
        else if ("ImidaSensor" == device)
        {
            sendDataToAnalytics("deviceSIAM_IMIDA");
        }

    }
 

    //EVENTOS BOTONES
    public void buttonAllDevice()
    {
        sendDataToAnalytics("buttonAllDevice");
    }
    public void buttonHideAllDevice()
    {
        sendDataToAnalytics("buttonHideAllDevice");
    }
    public void buttonToRealExit()
    {
        sendDataToAnalytics("buttonToRealExit");
    }
    public void buttonOptions()
    {
        sendDataToAnalytics("buttonOptions");
    }
    public void buttonExitYES()
    {
        sendDataToAnalytics("buttonExitYES");
    }
    public void buttonExitNO()
    {
        sendDataToAnalytics("buttonExitNO");
    }

    //EVENTOS INTERACCION CON EL MANDO
    public void buttonPrimaryRight()
    {
        sendDataToAnalytics("buttonPrimaryRight");
    }
    public void buttonSecondaryRight()
    {
        sendDataToAnalytics("buttonSecondaryRight");
    }
    public void buttonPrimaryLeft()
    {
        sendDataToAnalytics("buttonPrimaryLeft");
    }
    public void buttonSecondaryLeft()
    {
        sendDataToAnalytics("buttonSecondaryLeft");
    }

    private void sendDataToAnalytics(string data) {
        AnalyticsService.Instance.CustomData(data);

        Debug.Log($"Informacion de "+ data);

        AnalyticsService.Instance.Flush();
    }

}