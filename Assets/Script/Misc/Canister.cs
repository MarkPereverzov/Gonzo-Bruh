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

    private Context context;

    public bool fill = false;

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
            else Debug.Log("Generator storage is full");
        }
        else
        {
            Debug.Log("Canister is empty");
        }
    }
    public override void Activate(Context ctx)
    {
        context = ctx;
        if (ctx.currentDevice != null)
        {
            if (fill)
                fill = false;
            else
                fill = true;
        }
        else
            fill = false;
    }
    public override void Indicate()
    {
        //verticalIndicator.localScale = new Vector3(verticalIndicator.localScale.x, (capacity / 100), verticalIndicator.localScale.z) ;
        //verticalIndicator.position =  new Vector3(verticalIndicator.position.x,-1f + (capacity/100),verticalIndicator.position.z);
    }
    public void FillCanister()
    { 
        
    }
    void Start()
    {
        m_StatusText = new string[10];
        id = Random.Range(100, 999);
        m_StatusText[0] = "<align=center><color=white>#" + id + "\n";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_StatusText[1] = capacity.ToString();
        UpdateOverlay();
        if(fill)
            FillDevice((CommonGenerator)context.currentDevice);
        //Indicate();
    }
}
