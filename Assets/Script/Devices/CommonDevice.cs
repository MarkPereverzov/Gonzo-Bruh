using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Events;

public class CommonDevice : MonoBehaviour
{
    //public bool isRunning;
    //public bool slot[4];
    private bool active;
    public UnityEvent e_OnActivation;
    void Start()
    {
        active = false;
        if (e_OnActivation == null)
            e_OnActivation = new UnityEvent();

        e_OnActivation.AddListener(OnActivation);
    }

    void Update()
    {
        if (active)
            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
        else
            transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
    }

    void OnActivation() 
    {
        if (active)
            active = false;
        else 
            active = true;
    }
}
