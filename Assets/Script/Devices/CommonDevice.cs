using System.Runtime.InteropServices.ComTypes;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using TMPro;

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
    public bool isArea;
    [HideInInspector]
    public bool isActive;
    [HideInInspector]
    public bool isPowered;

    public Type type;

    [HideInInspector]
    public float powerHyi;
    [HideInInspector]
    public float powerGet;

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

    protected virtual void InitEvents()
    {
        if (e_OnActivation == null)
            e_OnActivation = new UnityEvent();
        if (e_OnConnect == null)
            e_OnConnect = new UnityEvent<CommonDevice>();
        if (e_OnShowHint == null)
            e_OnShowHint = new UnityEvent<bool>();

        e_OnActivation.AddListener(OnActivation);
        e_OnShowHint.AddListener(OnShowHint);
        e_OnConnect.AddListener(OnConnect);

        m_StatusText = new string[10];
        id = Random.Range(1000, 9999);
    }
    void Start()
    {
        isPowered = false;
        isActive = false;
        powerHyi = 80;

        InitEvents();

        m_StatusText[0] = "<align=center><color=white>#" + id + "\n";
        m_StatusText[1] = "<align=left>Activated: <color=red>OFF";
        m_StatusText[2] = "Power: <color=red>" + (float)powerGet + "<color=white>/<color=green>" + powerHyi;
    }

    public void UpdateOverlay()
    {
        string outMessage = "";
        foreach (string str in m_StatusText)
        { 
            if(str != null) outMessage += str + "\n<color=white>";
        }
        GameObject.Find(gameObject.name + "/UI").transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>().SetText(outMessage);
    }
    protected void OnActivation() 
    {
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
    public virtual void OnGeneratingText(float powerCount)
    {
        powerGet += powerCount;
        if (powerGet > powerHyi)
        {
            powerGet = powerHyi;
        }
        if (powerGet >= powerHyi)
        {
            isPowered = true;
            m_StatusText[2] = "Power: <color=green>" + (float)powerGet + "<color=white>/<color=green>" + powerHyi;

        }
        else
        {
            isPowered = false;
            m_StatusText[2] = "Power: <color=red>" + (float)powerGet + "<color=white>/<color=green>" + powerHyi;
        }
    }
    public void OnGenerating(float powerCount)
    {
        OnGeneratingText(powerCount);
    }
    protected void OnConnect(CommonDevice cd)
    {
        Debug.Log("Called Connect" + type);
        if (cd.type == Type.Generator)
        {
            energySlot = (CommonGenerator)cd;
        }
        else
        {
            slot.Add(cd);
            Debug.Log("Added element" + cd.type);
        }
        if (m_StatusText[4] == null)
            m_StatusText[4] = "Connected with: <color=green>" + cd.type + "#" + cd.id;
        else
            m_StatusText[4] += "<#ffffff>, <color=green>" + cd.type + "#" + cd.id;
    }
    protected void OnShowHint(bool state)   
    {
        isArea = state;
        GameObject.Find(gameObject.name + "/UI").GetComponent<Canvas>().enabled = state;
    }

}
