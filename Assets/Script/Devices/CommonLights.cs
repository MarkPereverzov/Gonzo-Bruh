using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommonLights : MonoBehaviour
{
    public bool isTriggered;
    public bool active;
    public bool isPowered;
    public int firstButton;
    public int secondButton;

    UnityEvent e_OnCollision = new UnityEvent();
    UnityEvent e_OnActive = new UnityEvent();
    void Start()
    {
        isTriggered = false;
        active = false;
        isPowered = false;
        firstButton = 0;
        secondButton = 0;

        e_OnCollision.AddListener(OnCollision);
        e_OnActive.AddListener(OnActive);
    }
    void OnCollision()
    {
        if (isTriggered)
        {
            if (active)
            {
                transform.GetChild(firstButton).GetComponent<MeshRenderer>().material.color = Color.green;
                Debug.Log("Active Green");
            }
            else
            {
                transform.GetChild(firstButton).GetComponent<MeshRenderer>().material.color = Color.red;
                Debug.Log("Active Red");
            }
            if (isPowered)
            {
                transform.GetChild(secondButton).GetComponent<MeshRenderer>().material.color = Color.green;
                Debug.Log("Powered Green");
            }
            else
            {
                transform.GetChild(secondButton).GetComponent<MeshRenderer>().material.color = Color.white;
                Debug.Log("Powered Red");
            }
        }
    }
    void OnActive()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Key pressed");
        }
    }
    void Update()
    {
        OnActive();
        OnCollision();
    }
    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Human")
        {
            Debug.Log("Collision Enter");
            isTriggered = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Human")
        {
            Debug.Log("Collision Exit");
            isTriggered = false;
        }
    }
}