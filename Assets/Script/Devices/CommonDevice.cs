using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;


public class CommonDevice : MonoBehaviour
{
    public enum Type
    {
        Machine,
        Generator,
        Adapter
    }
    [Header("Lights Settings")]
    public Transform firstButton;
    public Transform secondButton;

    [Header("Global Settings")]
    [HideInInspector]
    public int id;
    [HideInInspector]
    public bool isPowered;
    [HideInInspector]
    public bool isArea;
    [HideInInspector]
    public float powerNeed;
    [HideInInspector]
    public bool isActive;
    public Type type;
    [HideInInspector]
    private float powerGet;
    private CommonGenerator energySlot;
    public List<CommonDevice> slot;

    public string[] m_StatusText;

    [HideInInspector]
    public UnityEvent e_OnActivation;
    [HideInInspector]
    public UnityEvent<bool> e_OnShowHint;
    [HideInInspector]
    public UnityEvent<CommonDevice> e_OnConnect;
    [HideInInspector]
    public UnityEvent<float> e_OnGenerating;

    protected void InitEvents()
    {
        if (e_OnActivation == null)
            e_OnActivation = new UnityEvent();
        if (e_OnConnect == null)
            e_OnConnect = new UnityEvent<CommonDevice>();
        if (e_OnGenerating == null)
            e_OnGenerating = new UnityEvent<float>();
        if (e_OnShowHint == null)
            e_OnShowHint = new UnityEvent<bool>();

        e_OnActivation.AddListener(OnActivation);
        e_OnShowHint.AddListener(OnShowHint);
        e_OnConnect.AddListener(OnConnect);
        e_OnGenerating.AddListener(OnGenerating);

        m_StatusText = new string[10];
        id = Random.Range(1000, 9999);
        m_StatusText[0] = "<align=center><color=white>#" + id;
        m_StatusText[1] = "Activated: <color=red>OFF";
    }
    void Start()
    {
        isPowered = false;
        powerNeed = 100;
        isActive = false;

        InitEvents();
        m_StatusText[2] = "Powered: <color=red>False";

        m_StatusText[1] = "Activated: <color=red>OFF";
    }

    protected void UpdateOverlay()
    {
        string outMessage = "";

        foreach (string str in m_StatusText) 
        { 
            if(str != null) outMessage += str + "\n<color=white>";
        }

        GameObject.Find(gameObject.name + "/UI").transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().SetText(outMessage);
        Debug.Log(GameObject.Find(gameObject.name + "/UI").name);
        Debug.Log(outMessage);
    }
    protected virtual void Indication()
    {
        if (isArea) UpdateOverlay();//Debug.Log(GameObject.Find(gameObject.name + "/UI").transform.GetChild(1).transform.name);//GetComponent<TextMeshProUGUI>().SetText(m_StatusText.text);
        if (isActive)
            firstButton.GetComponent<MeshRenderer>().material.color = Color.green;
        else
            firstButton.GetComponent<MeshRenderer>().material.color = Color.red;

        if (isPowered)
            secondButton.GetComponent<MeshRenderer>().material.color = Color.green;
        else
            secondButton.GetComponent<MeshRenderer>().material.color = Color.red;
    }
    
    void FixedUpdate()
    {
        Indication();

    }

    protected void OnActivation() 
    {
        Debug.Log("Device activated: " + type);
        if (isActive)
        {
            isActive = false;
            m_StatusText[1] = "<align=left>Activated: <color=red>OFF";
        }
        else
        {
            isActive = true;
            m_StatusText[1] = "<align=left>Activated: <color=green>ON";
        }
    }
    protected void OnConnect(CommonDevice cd)
    {
        Debug.Log("Called Connect" + type);
        if (cd.type == Type.Generator)
        {
            energySlot = (CommonGenerator)cd;
            /*
            GameObject wire = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            wire.transform.Rotate(0,0,90);
            wire.transform.position = (transform.position + cd.transform.position) / 2;
            wire.transform.localScale = new Vector3 (0.1f,(transform.position.x - cd.transform.position.x),0.1f);
            */
        }
        else
        {
            slot.Add(cd);
            Debug.Log("Added element" + cd.type);
        }
        if (m_StatusText[3] == null)
            m_StatusText[3] = "Connected with: <color=green>" + cd.type;
        else
            m_StatusText[3] += "<#ffffff>, <color=green>" + cd.type;
    }
    protected void OnGenerating(float powerCount)
    {
        powerGet = powerCount;
        if (powerGet >= powerNeed)
        {
            isPowered = true;
            m_StatusText[2] = "Powered: <color=green>True";
        }
        else
        {
            isPowered = false;
            m_StatusText[2] = "Powered: <color=red>False";
        }
        Debug.Log("generating");
    }
    protected void OnShowHint(bool state)
    {
        isArea = state;
        GameObject.Find(gameObject.name + "/UI").GetComponent<Canvas>().enabled = state;
    }

}
