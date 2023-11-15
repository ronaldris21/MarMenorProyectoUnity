using Assets;
using Assets.CodeUtils;
using Assets.Scripts;
using ChartAndGraph;
using DLL_Models_Petitions.Models;
using DLL_Models_Petitions.Models.RespuestasPeticiones;
using Michsky.MUIP;
using SmartLagoonApiCalls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UI.Dates;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;


// Data from Request to RestClient.getSensorDataUsingDates()
public class SensorRangeData
{
    public string idLocation;
    public List<string> paramsTypeUnitName;
    public Dictionary<string, List<TimeValueData>> paramTimeValueDataDictionary;
    public SensorRangeData()
    {
        paramsTypeUnitName = new List<string>();
        paramTimeValueDataDictionary = new Dictionary<string, List<TimeValueData>>();
    }
}

public class TimeValueData
{
    public readonly float value;
    public readonly DateTime date;
    public readonly long timestamp;
    public TimeValueData(float value, DateTime date, long timestamp)
    {
        this.value = value;
        this.date = date;
        this.timestamp = timestamp;
    }
}

public class UIControllerChartsAndRangeData : MonoBehaviour
{
    public object resultados;
    public string idLocationSelected;
    public bool isActive;

    [SerializeField] GameObject analyticsController;
    private UGS_Analytics analytics;

    public GameObject topLeftMenuICON;
    public GameObject mainCanvas;
    public GameObject scrollViewRangeData;
    public GameObject chartObject;
    public GameObject datePickerObject_INIT;
    public GameObject datePickerObject_END;
    public string categoryName = "Player 1";
    private string selectedSensorName = "";
    public GameObject dropdownSensorTypesObject;  
    private SensorRangeData data;
    public GameObject SensorDataPanel;

    private GameObject scrollViewContent;
    private DateTime? dateInit;
    private DateTime? dateEnd;

    


    public GameObject textMaxObject;
    public GameObject textMinObject;
    public GameObject textAvgObject;

    public TextMeshProUGUI txtDateInit;
    public TextMeshProUGUI txtDateEnd;

    public GameObject deepthSelectorForBuoys;
    private bool isBuoy;

    private List<String> sensorNamesOnDropdown = new List<String>();
    public int maxMeshVertice = 300;

    public HorizontalSelector horizontalSelectorPaginator;

    // Start is called before the first frame update
    void Start()
    {
        analytics = analyticsController.GetComponent<UGS_Analytics>();

        if (!isActive)
        {
            DesactivateFunctionality();
        }

        scrollViewContent = scrollViewRangeData.transform.Find("Viewport").transform.Find("Content").gameObject;
        if(dateInit == null || dateEnd == null)
        {

            DatePicker datePickerEND = datePickerObject_END.GetComponent<DatePicker>();
            DatePicker datePickerINIT = datePickerObject_INIT.GetComponent<DatePicker>();

            dateEnd = DateTime.Now;
            dateInit = dateEnd.Value.AddDays(-15.0f);

            datePickerINIT.SelectedDate = dateInit.Value;
            datePickerEND.SelectedDate = dateEnd.Value;
        
            txtDateInit.SetText("Desde: "+dateInit.ToString());
            txtDateEnd.SetText("Hasta: "+ dateEnd.ToString());


            datePickerINIT.UpdateDisplay();
            datePickerEND.UpdateDisplay();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public double MilliTimeStamp(DateTime d2)
    {
        DateTime d1 = new DateTime(1970, 1, 1);
        TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);

        return ts.TotalMilliseconds;
    }

    //TODO: Posible problema, resulta que al convertir convierte bien, pero hay que tener cuidado
    //TODO: Posible problema, resulta que al convertir convierte bien, pero hay que tener cuidado
    private string getTimeStampProblemasHora(DateTime date)
    {
        return ((long)date
            .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local))
            .TotalMilliseconds).ToString() ;
    }

    private string getTimeStamp(DateTime date)
    {
        TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        DateTime gmtDateTime = TimeZoneInfo.ConvertTime(date, timeZone);
        long timestamp = (long)gmtDateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        return timestamp.ToString();
    }
    public async void ActivateFunctionality(string idLocation, Transform selectedHitTransform, bool isBuoy, bool refreshLocation=true)
    {

        if (idLocationSelected != idLocation)
            cleanChart();
        
        if (refreshLocation) //if it is a Buoy and main canvas is active, DONT LOCATETE IN MAP AGAIN
        {
            this.GetComponent<LocateMainCanvasInFrontUser>().LocateInMap();
        }


        this.isBuoy = isBuoy;

        mainCanvas.SetActive(true);
        Debug.Log("ActivateFunctionality: " + idLocation);

        this.GetComponent<UnitPolishers>().assignImage(idLocation);

        //ANALYTICS
        AnalyticsInfo(idLocation);
        //

        this.isActive = true;
        this.idLocationSelected = idLocation;

        this.deepthSelectorForBuoys.SetActive(isBuoy);
        if (isBuoy)
        {
            if (idLocation == this.GetComponent<UIController>().idBuoySensor0m)
            {
                deepthSelectorForBuoys.GetComponent<HorizontalSelector>().defaultIndex = 0;
                deepthSelectorForBuoys.GetComponent<HorizontalSelector>().SetupSelector();
            }
        }


        await retrieveRangeData();
    }

    public void AnalyticsInfo(string data) {

       string SensorType = this.GetComponent<UnitPolishers>().sensorType(data);
        analytics.deviceALL(SensorType);
        
    }


    public void DesactivateFunctionality()
    {
        this.idLocationSelected = null;
        this.isActive = false;
        mainCanvas.SetActive(false);
        //data = null;
        //UpdateDropdowmItems();

        this.deepthSelectorForBuoys.SetActive(false);
    }

    public void ActivateChartsCanvas()
    {
        //this.mainCanvas.SetActive(true);
    }

    public void DesactivateChartsCanvas()
    {
        //this.mainCanvas.SetActive(false);
    }


    private void UpdateDropdowmItems()
    {
        Debug.Log("updateDropdownItems"); 
        //TODO: DropDown no tiene DATOS!!!! Entonces hay que mostrarle al usuario la indicacion de que se debe de cambiar las fechas seleccionadas para poder mostrar datos 
        //TODO: no hay datos en el rango de fechas seleccionado --> Hacer una row nueva que muestre esto y que se quite 
        CustomDropdown customDropdown = dropdownSensorTypesObject.GetComponent<CustomDropdown>();
        customDropdown.items.Clear();
        customDropdown.selectedItemIndex = 0;

        UnitPolishers polisher = this.GetComponent<UnitPolishers>();


        sensorNamesOnDropdown.Clear();
        foreach (string sensor in data?.paramsTypeUnitName)
        {
            if (data.paramTimeValueDataDictionary[sensor].Count > 0)
            {
                sensorNamesOnDropdown.Add(sensor);
                String sensorNameView = polisher.polishSensorType(sensor).ToString();
                if (String.IsNullOrEmpty(selectedSensorName) || data.paramsTypeUnitName.Contains(selectedSensorName) == false )
                    selectedSensorName = sensor;
                
                customDropdown.CreateNewItem(sensorNameView, polisher.GetSensorIcon(sensor), true);

            }
        }

        if (customDropdown.items.Count==0)
        {
            this.GetComponent<PopNotification>().callNotification("No hay datos", "Intenta un rango de fechas más amplio");
        }

        customDropdown.UpdateItemLayout();
    }
    
    public void OnDatePickerSelectedEvent_INIT_DATE()
    {
        Debug.Log("OnDatePickerSelectedEvent_INIT_DATE");

        var componente = datePickerObject_INIT.GetComponent<DatePicker>();
        dateInit = componente.SelectedDate;
        
        Debug.Log(dateInit);
        Debug.Log(dateEnd);



        if(dateInit <= dateEnd)
        {
            retrieveRangeData();
            
        }
        else
        {
            this.GetComponent<PopNotification>().callNotification("Fecha inválida", "");
        }
    }


    //TODO: SATELLITES TIENE DATOS EN ENERO! 
    public void OnDatePickerSelectedEvent_END_DATE()
    {
        Debug.Log("OnDatePickerSelectedEvent_END_DATE");

        var componente = datePickerObject_END.GetComponent<DatePicker>();
        dateEnd = componente.SelectedDate;
        
        Debug.Log(dateInit);
        Debug.Log(dateEnd);

        if (dateInit <= dateEnd)
        {
            retrieveRangeData();

            //TODO: MODIFICAR CANVAS DE NOTIFICACTION SORT LAYER
        }
        else
        {
            this.GetComponent<PopNotification>().callNotification("Fecha invalida", "");
        }
    }

    public void OnDropdownSelectedChange()
    {
        Debug.Log("OnDropdownSelectedChange");
        CustomDropdown customDropdown = dropdownSensorTypesObject.GetComponent<CustomDropdown>();
        selectedSensorName = sensorNamesOnDropdown[customDropdown.selectedItemIndex];
        //TODO: CONVERTIR SENSOR NAME
        Debug.Log(selectedSensorName);
        initChart();

    }

    public void OnClickBtnDateInit()
    {
        datePickerObject_INIT.SetActive(!datePickerObject_INIT.activeSelf);
    }

    public void OnClickBtnDateEnd()
    {
        datePickerObject_END.SetActive(!datePickerObject_END.activeSelf);
    }

    //private void initChartSampleData()
    //{
    //    GraphChart Graph = chartObject.GetComponent<GraphChart>();  
    //    // clear the "Player 1" category. this category is defined using the GraphChart inspector
    //    Graph.DataSource.ClearCategory("Player 1");
    //    //// clear the "Player 2" category. this category is defined using the GraphChart inspector
    //    //Graph.DataSource.ClearCategory("Player 2");

    //    float x = 20;
    //    float lastX = 20;
    //    for (int i = 0; i < 20; i++)  //add random points to the graph
    //    {
    //        Graph.DataSource.AddPointToCategory("Player 1", System.DateTime.Now - System.TimeSpan.FromSeconds(x), Random.value * 20f + 10f);
    //        //Graph.DataSource.AddPointToCategory("Player 2", System.DateTime.Now - System.TimeSpan.FromSeconds(x), Random.value * 10f);
    //        x -= Random.value * 3f;
    //        lastX = x;
    //    }
    //}

    private void cleanChart()
    {
        this.horizontalSelectorPaginator.items.Clear();
        //this.horizontalSelectorPaginator.UpdateUI(); ///TODO: newBack Falta probar PAGINACION DE EL GRAFICO DE RANGE DATA

        Debug.Log("cleanChart");
        GraphChart graph = chartObject.GetComponent<GraphChart>();
        graph.DataSource.StartBatch();  // start a new update batch
        graph.DataSource.ClearCategory(categoryName);  // clear the categories we have created in the inspector
        graph.DataSource.EndBatch(); // end the update batch . this call will render the graph
    }

    private void updateAndCloseCalendarsWhenValidDateSelected()
    {

        txtDateEnd.SetText("Hasta: " + dateEnd.ToString());
        datePickerObject_END.SetActive(false);
        txtDateInit.SetText("Desde: " + dateInit.ToString());
        datePickerObject_INIT.SetActive(false);

    }
    private void initChart()
    {
        //chartObject.SetActive(true);   

        Debug.Log("initChar");
        cleanChart();

        CustomDropdown customDropdown = dropdownSensorTypesObject.GetComponent<CustomDropdown>();
        selectedSensorName = sensorNamesOnDropdown[customDropdown.selectedItemIndex];

        string unidad = this.GetComponent<UnitPolishers>().PolishUnit(selectedSensorName);
        if (String.IsNullOrEmpty(unidad))
            unidad = "unidades";

        GraphChart graph = chartObject.GetComponent<GraphChart>();
        Debug.Log(categoryName + ": nombre cat");
       
        
        if (graph != null && data.paramTimeValueDataDictionary.ContainsKey(selectedSensorName))
        {
            if (data.paramTimeValueDataDictionary[selectedSensorName].Count<=0)
            {
                Debug.Log("HERE: data.paramTimeValueDataDictionary[selectedSensorName].Count<=0");
                return;
            }



        //TODO: MAXIMO DE DATOS ANTIGUO
        //if (data.paramTimeValueDataDictionary[selectedSensorName].Count > maxMeshVertice)
        //{
        //    this.GetComponent<PopNotification>().callNotification("Demasiados datos", "Elige un rango de fechas menor");
        //    return;
        //}

            
            //Paginación de la gráfica
            int pagesCount = data.paramTimeValueDataDictionary[selectedSensorName].Count / maxMeshVertice + 1;
                
            this.horizontalSelectorPaginator.items.Clear();
            for (int i = 0; i < pagesCount; i++)
            {
                this.horizontalSelectorPaginator.CreateNewItem($"Datos gráfica {i+1} de {pagesCount}");
            }
            this.horizontalSelectorPaginator.UpdateUI();




            updateAndCloseCalendarsWhenValidDateSelected();

            float maxVal = data.paramTimeValueDataDictionary[selectedSensorName][0].value;
            float minVal = maxVal;
            float avg = 0;
            

            ///Calculate Global Max,Min and Avg
            foreach (TimeValueData dato in data.paramTimeValueDataDictionary[selectedSensorName])
            {
                if (dato.value>maxVal)
                    maxVal = dato.value;
                if (dato.value<minVal)
                    minVal=dato.value;
                avg += dato.value;

            }

            avg = avg / data.paramTimeValueDataDictionary[selectedSensorName].Count;

            textAvgObject.GetComponent<TextMeshProUGUI>().text = String.Format("Promedio:\n {0} {1}",avg, unidad) ;
            textMinObject.GetComponent<TextMeshProUGUI>().text = String.Format("Mínimo:\n {0} {1}", minVal, unidad);
            textMaxObject.GetComponent<TextMeshProUGUI>().text = String.Format("Máximo:\n {0} {1}", maxVal, unidad);

            //Show Graph Data
            updateGraphPointValues();


        }


    }

    private void updateGraphPointValues()
    {
        if (this.horizontalSelectorPaginator.items.Count == 0)
            return;

        GraphChart graph = chartObject.GetComponent<GraphChart>();

        graph.DataSource.StartBatch();  // start a new update batch
        graph.DataSource.ClearCategory(categoryName);  // clear the categories we have created in the inspector


            int pagesCount = horizontalSelectorPaginator.items.Count;
        int selectedIndex = horizontalSelectorPaginator.index;
        int counter = 0;
        for (int i = selectedIndex *maxMeshVertice ; i < data.paramTimeValueDataDictionary[selectedSensorName].Count && counter< maxMeshVertice ; i++, counter++)
        {
            TimeValueData dato = data.paramTimeValueDataDictionary[selectedSensorName][i];
            graph.DataSource.AddPointToCategory(categoryName, dato.date, dato.value, 1); //Point Size
        }

        graph.DataSource.EndBatch(); // end the update batch . this call will render the graph
    }

    public void OnUpdateHorizontalGraphSelectorPaginatorIndex()
    {
        if (this.horizontalSelectorPaginator.items.Count == 0)
            return;

        updateGraphPointValues();
    }

    public void clickOpenChartsButton()
    {
        //Debug.Log()
    }



    private async Task<bool> retrieveRangeData()
    {
        ///it uses the idSelectedLocation
        //String idLocation = hit.transform.gameObject.GetComponent<SensorTags>().locationId;
        String sensorTypeId = Singleton.Instance.sensorsIdLocationToDeviceTypeDictionary[idLocationSelected];
        SensorType[] sensores = Singleton.Instance.sensoresDataTypes_byLocationSensorType[sensorTypeId];
        string sensoresParametros = String.Join(",", sensores.Select(s => s.name).ToArray());

        ///TODO: Error - > getSensorDataUsingDates no funciona para los CAMARAS - CELESTES NO FUNCIONAN
        //////TODO: FECHAS Y CHARTS HTTP REQUEST
        data = await this.GetComponent<RestClient>().getSensorDataUsingDates(
                idLocationSelected, 
                sensorTypeId, 
                sensoresParametros,
                getTimeStamp(dateInit.Value), 
                getTimeStamp(dateEnd.Value));
        if (data == null)
            return false;


        UpdateDropdowmItems();

        if (data != null)
        {
            Debug.Log("RESULTADO retrieveRangeData OK");

            CustomDropdown customDropdown = dropdownSensorTypesObject.GetComponent<CustomDropdown>();
            int hasSelectedSensorName = customDropdown.items.Count(item => item.itemName == selectedSensorName);
            if (hasSelectedSensorName == 0)
            {
                /// it doesnt have the previous selectedSensorName
                /// So I manually asing the first avaiblable
                selectedSensorName = customDropdown.items.Count > 0 ? customDropdown.items[0].itemName : "";
            }

            ///selectedSensorName is
            if (String.IsNullOrEmpty(selectedSensorName))
            {
                ///TODO: AVISAR AL USUARIO QUE NO HAY DATOS DISPONIBLES PARA QUE LOS PUEDA VISUALIZAR EN EL RANGO SELECCIONADO
                customDropdown.selectedText.text = "No hay datos disponibles";
                this.GetComponent<PopNotification>().callNotification("No hay datos", "Intenta probar con otro rango de fechas");
            }

            initChart();

            return true;
        }
        Debug.Log("retrieveRangeData FAILED");
        return false;
    }


    private void ShowScrollViewRangeData()
    {
        // Delete indicators info from previous queries
        foreach (Transform child in scrollViewContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }



        foreach (string unitType in data.paramsTypeUnitName)
        {
            Debug.Log(unitType);

            foreach (TimeValueData slotData in data.paramTimeValueDataDictionary[unitType])
            {
                Debug.Log(String.Format("{0} {1}", slotData.value, slotData.timestamp));
                // Instatiate new text box to show the name and value of each device indicator.
                // There's already a text box placed in ScrollView/Viewport/Content named
                // "PanelDatosSensor"
                GameObject newSensorDataPanel = Instantiate(SensorDataPanel);
                newSensorDataPanel.transform.SetParent(scrollViewContent.transform, false);

                // Place the text box in the same location as PanelDatosSensor. Automatically
                // is placed below
                RectTransform SensorDataPanelTransform = SensorDataPanel.GetComponent<RectTransform>();
                RectTransform newSensorDataPanelTransform = newSensorDataPanel.GetComponent<RectTransform>();
                newSensorDataPanelTransform.anchorMin = SensorDataPanelTransform.anchorMin;
                newSensorDataPanelTransform.anchorMax = SensorDataPanelTransform.anchorMax;
                newSensorDataPanelTransform.anchoredPosition = SensorDataPanelTransform.anchoredPosition;
                newSensorDataPanelTransform.sizeDelta = SensorDataPanelTransform.sizeDelta;

                // Show the name and value of each device indicator
                TMP_Text DatosSensorTextTMP = SensorDataPanel.transform.Find("DatosSensorTextTMP").gameObject.GetComponent<TMP_Text>();
                string newSensorType = this.GetComponent<UnitPolishers>().polishSensorType(unitType);
                String unit = this.GetComponent<UnitPolishers>().PolishUnit(unitType);

                DatosSensorTextTMP.text = newSensorType + ": " + ((float)Math.Round((double)slotData.value, 2)).ToString() + " " + unit + " -- " +slotData.date;


                //// Assign icon
                //assignIcon(SensorDataPanel, sensorType);

            }

        }


    }

    
}
