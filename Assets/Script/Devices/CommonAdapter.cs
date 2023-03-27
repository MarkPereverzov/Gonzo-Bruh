using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommonAdapter : CommonDevice
{
    public int powerMax;
    public override void OnGeneratingText(float powerCount)
    {
        powerGet += powerCount;
        if (powerGet > powerHyi)
        {
            powerHyi = powerGet;
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
    void Start()
    {

    }
}
