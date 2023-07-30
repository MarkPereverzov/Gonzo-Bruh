using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.WSA;

public class Canister : Interactable
{
    public const float FUEL_MAX = 100f;
    public float capacity = 100f;
    public float filling_speed = 0.5f;

    [Header("Lights Settings")]
    public Transform verticalIndicator;
    // Start is called before the first frame update
    public void FillDevice(CommonGenerator generator)
    {
        if (capacity - filling_speed >= 0)
        {
            if (generator.fuel + filling_speed <= CommonGenerator.FUEL_MAX)
            {
                capacity -= filling_speed;
                generator.fuel += filling_speed;
            }
            Debug.Log("Generator storage is full");
        }
        else
        {
            Debug.Log("Canister is empty");
        }
    }
    public override void Activate(Context ctx)
    {
        if (ctx.currentDevice != null)
            FillDevice((CommonGenerator)ctx.currentDevice);
    }
    public void FillCanister()
    { 
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_StatusText[0] = capacity.ToString();
    }
}
