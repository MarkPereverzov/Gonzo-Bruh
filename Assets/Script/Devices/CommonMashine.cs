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
}
