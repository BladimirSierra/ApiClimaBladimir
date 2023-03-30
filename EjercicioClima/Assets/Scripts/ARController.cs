using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking; //Libreria para hacer solicitudes a un servicio Web
using SimpleJSON; //Permite Manejar los datos Json Consumidos desde la Web
using TMPro; //Libreria ara el manejo de texto en la interfaz de usuario
using DG.Tweening; //Libreria para el manejo de animaciones

public class ARController : MonoBehaviour
{
    [SerializeField]
    private GameObject AR; //Tendra todos los elementos virtuales en la escena de Realidad Aumentada
    
    [SerializeField]
    private RawImage iconoClima; //Mostrara la imagen de un icono que sera leido desde la Web

    [SerializeField]  
    private TextMeshProUGUI ciudad; //Mostrara un texto con la data que sera leida desde el servicio Web.

    [SerializeField]  
    private TextMeshProUGUI pais;

    [SerializeField]  
    private TextMeshProUGUI temperatura;

    [SerializeField]  
    private TextMeshProUGUI clima;

    [SerializeField] 
    private RectTransform panelInput;

    [SerializeField] 
    private InputField inputCiudad;

    private string url_api = "http://api.weatherstack.com/current?access_key=2af56d1afa5c5dd7d3a08553ecbbbee8&query=";
    private string ciudadActual = "Madrid";
    private string url_img;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ObtenerInfo(ciudadActual));
    }

    IEnumerator ObtenerInfo(string city)
    {
        UnityWebRequest www = UnityWebRequest.Get(url_api + city);
        yield return www.SendWebRequest();

        if(www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            JSONNode info = JSON.Parse(www.downloadHandler.text);

            string nombre_ciudad = info["location"]["name"];
            string nombre_pais = info["location"]["country"];
            string temp = info ["current"]["temperature"];
            string url_icons = info ["current"]["weather_icons"][0];
            string nombre_clima = info ["current"]["weather_descriptions"][0];
            string IsDay = info ["current"]["is_day"];

            UnityWebRequest img = UnityWebRequestTexture.GetTexture(url_icons);
            yield return img.SendWebRequest();

            if(IsDay == "yes")
            {
                iconoClima.GetComponent<RawImage>().texture = DownloadHandlerTexture.GetContent(img); 
            }
            else
            {
                iconoClima.GetComponent<RawImage>().texture = DownloadHandlerTexture.GetContent(img); 
            }

            ciudad.text = nombre_ciudad;
            pais.text = nombre_pais;
            temperatura.text = temp + " °C";
            clima.text = nombre_clima;


            AR.SetActive(true);
        }
    }

    public void MostrarPanelInput()
    {
        panelInput.DOAnchorPos(new Vector2(0.0f, -145), 0.5f);
    }

    public void BuscarCiudad()
    {
        StartCoroutine(ObtenerInfo(inputCiudad.text));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}