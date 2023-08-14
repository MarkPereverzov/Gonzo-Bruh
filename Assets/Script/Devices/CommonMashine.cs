using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommonMashine : CommonDevice
{
    public int powerNeed;

    protected override void InitEvents()
    {
        if (e_OnGenerating == null)
            e_OnGenerating = new UnityEvent<float>();

        e_OnGenerating.AddListener(OnGenerating);
    }
    void Start()
    {
        InitEvents();
    }
    protected new void OnGenerating(float powerCount)
    {
        powerGet = powerCount;
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
        Debug.Log("generating");
    }
    protected override void OnConnect(CommonDevice cd)
    {
        Debug.Log("Called Connect" + type);
        if (cd.type != Type.Machine)
        {
            if (cd.type == Type.Generator)
            {
                energySlot = (CommonGenerator)cd;
            }
            else if (cd.type == Type.Adapter)
            {
                energySlot = (CommonAdapter)cd;
            }
            if (m_StatusText[4] == null)
                m_StatusText[4] = "Connected with: <color=green>" + cd.type + "#" + cd.id + " " + cd.input_volt_level;
            else
                m_StatusText[4] += "<#ffffff>, <color=green>" + cd.type + "#" + cd.id + " " + cd.input_volt_level;
        }
    }
}
