using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommonAdapter : CommonDevice
{
    public int powerMax;
    public Voltage output_volt_level;

    protected override void InitEvents()
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
    public override void OnGeneratingText(float powerCount)
    {
        powerGet += powerCount;
        if (powerGet >= powerHyi)
        {
            powerHyi = powerGet;
            isPowered = true;
            m_StatusText[2] = "Power: <color=green>" + (float)powerGet + "<color=white>/<color=green>" + powerHyi;

        }
        else
        {
            isPowered = false;
            m_StatusText[2] = "Power: <color=red>" + (float)powerGet + "<color=white>/<color=green>" + powerHyi;
        }
    }
    protected override void OnConnect(CommonDevice cd)
    {
        Debug.Log("Called Connect" + type);
        if (cd.type != Type.Adapter)
        {
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
                m_StatusText[4] = "Connected with: <color=green>" + cd.type + "#" + cd.id + " " + cd.input_volt_level;
            else
                m_StatusText[4] += "<#ffffff>, <color=green>" + cd.type + "#" + cd.id + " " + cd.input_volt_level;
        }
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
        output_volt_level = Voltage.FIRST;
    }
}
