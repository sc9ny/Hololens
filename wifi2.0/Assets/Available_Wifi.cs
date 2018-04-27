using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;


public class Available_Wifi : MonoBehaviour {

    public GameObject prefab;
    public Canvas canvas;
   private IWiFiAdapter<wifis> WiFiAdapter;

    TextMesh tt;
    ArrayList wifiList;
    bool newWifi = false;
    // Use this for initialization
    void Start() {
        WiFiAdapter = new UniversalWiFi();
       // CreatePanels();
    }

    private void Update()
    {
        var list = WiFiAdapter.getNetworks();
        for(int x = 0; x < list.Count; x++)
        {
            if (!wifiList.Contains(list[x]))
            {
                wifiList.Add(list[x]);
                newWifi = true;
            }
            else
            {
                newWifi = false;
            }              
        }

        if (newWifi == true)
        {
            StartCoroutine(DrawPanels(wifiList));
        }

        //var list = WiFiAdapter.getNetworks();
        /*for (int x = 0; x < list.Count; x++)
        {
            if (list[x].Signal == 3)
            {
                GameObject.Find("ID23").GetComponent<Renderer>().material.color = Color.black;
                GameObject.Find("ID11").GetComponent<Renderer>().material.color = Color.gray;
                GameObject.Find("ID17").GetComponent<Renderer>().material.color = Color.gray;
            }
            else if (list[x].Signal == 2)
            {
                GameObject.Find("ID17").GetComponent<Renderer>().material.color = Color.black;
                GameObject.Find("ID23").GetComponent<Renderer>().material.color = Color.black;
                GameObject.Find("ID11").GetComponent<Renderer>().material.color = Color.gray;
            }
            else if (list[x].Signal == 1)
            {
                GameObject.Find("ID17").GetComponent<Renderer>().material.color = Color.black;
                GameObject.Find("ID23").GetComponent<Renderer>().material.color = Color.black;
                GameObject.Find("ID11").GetComponent<Renderer>().material.color = Color.black;
            }
            else if (list[x].Signal == 0)
            {
                GameObject.Find("Wifi-bars").GetComponent<Renderer>().material.color = Color.red;
            }
            else if (list[x].Signal == 4)
            {
                GameObject.Find("Wifi-bars").GetComponent<Renderer>().material.color = Color.white;
            }
        }*/
        //tt.text = list.ToString();
        // tt.transform.position = posXYZ("Wifi-Name");
    }
    IEnumerator DrawPanels(ArrayList list)
    {
        CreatePanels(list);
        yield return null;
    } 

    public void CreatePanels(ArrayList list)
    {

        Vector3 posC = new Vector3(.133f, .133f + .85f, .615f - 9.115f);
        Vector3 posW = new Vector3(.096f, .157f + .85f, .6096f - 9.115f);
        Vector3 posB = new Vector3(.097f, .113f + .85f, .6077f - 9.115f);
        Vector3 posDB = new Vector3(.133f, .128f + .85f, .6108f - 9.115f);
        Vector3 scaleC = new Vector3(.01f, .05f, .11f);
        Vector3 scaleW = new Vector3(.01f, .01f, .01f);
        Vector3 scaleB = new Vector3(1e-05f, 1e-05f, 1e-05f);

        Vector3 rotC = transform.rotation.eulerAngles;
        Vector3 rotB = transform.rotation.eulerAngles;
        Vector3 rotDummy = transform.rotation.eulerAngles;

        rotC.y = 90.397f;
        rotB.x = -.333f;
        rotDummy.x = 0;
        var decPos = -.0547f;//56


        //var list = WiFiAdapter.getNetworks();
        //var wifi = list;

        for (int x = 0; x< list.Count; x++)
        {
            GameObject dummy = new GameObject("Wifi");
            // Fix for new panels
            Debug.Log(list[x]);
            if (x > 0)
            {
                posC = (new Vector3(posC.x, posC.y - decPos, posC.z ));
                posW = (new Vector3(posW.x, posW.y - decPos, posW.z));
                posB = (new Vector3(posB.x, posB.y - decPos, posB.z));
                posDB = (new Vector3(posDB.x, posDB.y - decPos, posDB.z));
            }
            // creates new panel
            var panel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            panel.transform.position = new Vector3(posC.x, posC.y, posC.z);
            panel.transform.localScale = new Vector3(scaleC.x, scaleC.y, scaleC.z);
            panel.transform.rotation = Quaternion.Euler(rotC);
            panel.AddComponent<ColorChange>();
            panel.transform.parent = dummy.transform;
            // creates Names of wifi
            GameObject Text = new GameObject("Wifi-Name");
            Text.AddComponent<TextMesh>();
            var textMesh = Text.GetComponent<TextMesh>();
            textMesh.color = Color.black;
            //textMesh.text = "Name: "+ list[x].Ssid;
            Text.transform.position = posW;
            Text.transform.localScale = scaleW;
            Text.transform.parent = dummy.transform;
            // creates wifi bars
            GameObject Bars = new GameObject("Wifi-Bars");
            Bars = Instantiate(prefab);
            Bars.transform.position = posB;
            Bars.transform.localScale = scaleB;
            Bars.transform.rotation = Quaternion.Euler(rotB);
            Bars.transform.parent = dummy.transform;
            
            

            //Create DB value
            GameObject DBText = new GameObject("DBs");
            DBText.AddComponent<TextMesh>();
            var DBtextMesh = DBText.GetComponent<TextMesh>();
            DBtextMesh.color = Color.black;
            //DBtextMesh.text = "DB "+ list[x].RSSI;
            DBText.transform.position = posDB;
            DBText.transform.localScale = scaleW;
            DBText.transform.parent = dummy.transform;
            // sets the panels to fixed position on canvas
            
            dummy.transform.parent = canvas.transform;
       
        }
    }

    private Vector3 posXYZ(string name)
    {
        var posXYZ = GameObject.Find(name).transform.position;
        return posXYZ;
    }





}
