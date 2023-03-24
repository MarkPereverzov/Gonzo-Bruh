using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommonLights : MonoBehaviour
{
    public bool contact;
    public bool status;
    public int firtButton;
    public int secondButton;

    public UnityEvent e_OnCollision; 
    protected void Callback()
    {
        if (e_OnCollision == null)
            e_OnCollision = new UnityEvent();

        e_OnCollision.AddListener(OnCollision);
    }
    void Start()
    {
        contact = false;
        status = false;
        firtButton = 0;
        secondButton = 0;

        Callback();
    }

    private void OnCollision()
    {
       /* if( collision.gameObject.name == "Human")
            contact = true;
        else
            contact = false;*/
    }

    void Update()
    {
        if (contact);
            if (status)
                transform.GetChild(firtButton).GetComponent<MeshRenderer>().material.color = Color.green;
            else
                transform.GetChild(firtButton).GetComponent<MeshRenderer>().material.color = Color.red;
            if (status)
                transform.GetChild(secondButton).GetComponent<MeshRenderer>().material.color = Color.green;
            else
                transform.GetChild(secondButton).GetComponent<MeshRenderer>().material.color = Color.red;
    }
}
