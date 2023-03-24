/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommonLights : MonoBehaviour
{
    public bool isTriggered;
    public bool active;
    public bool isPowered;
    public int firtButton;
    public int secondButton;

    public UnityEvent e_OnCollision;
    public UnityEvent<bool> e_OnActive;
    protected void Callback()
    {
        if (e_OnCollision == null)
            e_OnCollision = new UnityEvent();
        if (e_OnActive == null)
            e_OnActive = new UnityEvent<bool>();

        e_OnCollision.AddListener(OnCollision);
        e_OnActive.AddListener(OnActive);
    }

    void Start()
    {
        isTriggered = false;
        active = false;
        isPowered = false;
        firtButton = 0;
        secondButton = 0;

        Callback();
    }

    private void OnCollision(bool status)
    {
        isTriggered = status;
        GameObject.Find(gameObject.name + "/UI").GetComponent<Canvas>().enabled = status;
    }

    private void OnActive()
    {
        if (Input.GetKeyDown("E"))
            active = true;
        else
            active = false;
    }

    void Update()
    {
        OnActive();
        OnCollision();
        if (isTriggered)
        {
            if (active == true)
                transform.GetChild(firtButton).GetComponent<MeshRenderer>().material.color = Color.green;
            else
                transform.GetChild(firtButton).GetComponent<MeshRenderer>().material.color = Color.red;

            if (isPowered == true)
                transform.GetChild(secondButton).GetComponent<MeshRenderer>().material.color = Color.green;
            else
                transform.GetChild(secondButton).GetComponent<MeshRenderer>().material.color = Color.red;
        }
            
    }
}
*/