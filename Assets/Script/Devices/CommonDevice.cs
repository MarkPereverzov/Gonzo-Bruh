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

    public string m_StatusText;

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

        m_StatusText = "Activated: <color=red>OFF";
    }
    void Start()
    {
        isPowered = false;
        powerNeed = 100;
        active = false;
        type = Type.Machine;
        InitEvents();
    }

    protected virtual void UpdateOverlay()
    {
        GameObject.Find(gameObject.name + "/UI").transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().SetText(m_StatusText);
    }
    protected virtual void Indication()
    {
        if (inArea) UpdateOverlay();//Debug.Log(GameObject.Find(gameObject.name + "/UI").transform.GetChild(1).transform.name);//GetComponent<TextMeshProUGUI>().SetText(m_StatusText.text);
        if (active)
            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
        else
            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;

        if (isPowered)
            transform.GetChild(3).GetComponent<MeshRenderer>().material.color = Color.green;
        else
            transform.GetChild(3).GetComponent<MeshRenderer>().material.color = Color.red;

        if (powerGet == 0)
            transform.GetChild(3).GetComponent<MeshRenderer>().material.color = Color.white;
    }

    void Update()
    {
        Indication();
    }

    protected void OnActivation() 
    {
        Debug.Log("Device activated: " + type);
        if (active)
        {
            active = false;
            m_StatusText = "Activated: <color=red>OFF";
        }
        else
        {
            active = true;
            m_StatusText = "Activated: <color=green>ON";
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

    }
    protected void OnGenerating(float powerCount)
    {
        powerGet = powerCount;
        if (powerGet >= powerNeed)
        {
            isPowered = true;
        }
        else
        {
            isPowered = false;
        }
    }
    protected void OnShowHint(bool state)
    {
        inArea = state;
        GameObject.Find(gameObject.name + "/UI").GetComponent<Canvas>().enabled = state;
    }

}
