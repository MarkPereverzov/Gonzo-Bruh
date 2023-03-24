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
        Generator
    }
    public bool isPowered;
    public bool inArea;
    public float powerNeed;
    public bool active;
    public Type type;

    private float powerGet;
    private CommonGenerator energySlot;
    public List<CommonDevice> slot;

    public string[] m_StatusText;

    //public bool slot[4];
    public UnityEvent e_OnActivation;
    public UnityEvent<bool> e_OnShowHint;
    public UnityEvent<CommonDevice> e_OnConnect;
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

        m_StatusText[0] = "Activated: <color=red>OFF";
    }
    void Start()
    {
        isPowered = false;
        powerNeed = 100;
        active = false;

        type = Type.Machine;
        InitEvents();
        m_StatusText[1] = "\n<color=white>Powered: <color=red>False";
    }

    protected void UpdateOverlay()
    {
        string outMessage = "";

        foreach (string str in m_StatusText)
            if(str != null) outMessage += str;

        GameObject.Find(gameObject.name + "/UI").transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().SetText(outMessage);
    }
    protected virtual void Indication()
    {
        if (inArea) UpdateOverlay();//Debug.Log(GameObject.Find(gameObject.name + "/UI").transform.GetChild(1).transform.name);//GetComponent<TextMeshProUGUI>().SetText(m_StatusText.text);
        if (active)
            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
        else
            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;

        if (isPowered)
            transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.green;
        else
            transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.red;
    }

    void FixedUpdate()
    {
        Indication();

    }

    protected void OnActivation() 
    {
        Debug.Log("Device activated: " + type);
        if (active)
        {
            active = false;
            m_StatusText[0] = "Activated: <color=red>OFF";
        }
        else
        {
            active = true;
            m_StatusText[0] = "Activated: <color=green>ON";
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
        if (m_StatusText[2] == null)
            m_StatusText[2] = "\n<color=white>Connected with: <#00025E>" + cd.type;
        else
            m_StatusText[2] +=  "<#ffffff>,\n<#00025E>" + cd.type;
    }
    protected void OnGenerating(float powerCount)
    {
        powerGet = powerCount;
        if (powerGet >= powerNeed)
        {
            isPowered = true;
            m_StatusText[1] = "\n<color=white>Powered: <color=green>True <sprite index=0>";
        }
        else
        {
            isPowered = false;
            m_StatusText[1] = "\n<color=white>Powered: <color=red>False";
        }
        Debug.Log("generating");
    }
    protected void OnShowHint(bool state)
    {
        inArea = state;
        GameObject.Find(gameObject.name + "/UI").GetComponent<Canvas>().enabled = state;
    }

}
