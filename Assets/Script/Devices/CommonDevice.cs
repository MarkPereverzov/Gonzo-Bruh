using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
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
    public float powerNeed;
    public bool active;
    public Type type;

    private float powerGet;
    private CommonGenerator energySlot;
    public List<CommonDevice> slot;

    //public bool slot[4];
    public UnityEvent e_OnActivation;
    public UnityEvent<CommonDevice> e_OnConnect;
    public UnityEvent<float> e_OnGenerating;
    void Start()
    {
        isPowered = false;
        powerNeed = 100;
        active = false;
        type = Type.Machine;

        if (e_OnActivation == null)
            e_OnActivation = new UnityEvent();
        if (e_OnConnect == null)
            e_OnConnect = new UnityEvent<CommonDevice>();
        if (e_OnGenerating == null)
            e_OnGenerating = new UnityEvent<float>();

        e_OnActivation.AddListener(OnActivation);
        e_OnConnect.AddListener(OnConnect);
        e_OnGenerating.AddListener(OnGenerating);
    }

    public virtual void Indication()
    {
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

    public void OnActivation() 
    {
        Debug.Log("Device activated: " + type);
        if (active)
            active = false;
        else 
            active = true;
    }
    public void OnConnect(CommonDevice cd)
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
    public void OnGenerating(float powerCount)
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

}
