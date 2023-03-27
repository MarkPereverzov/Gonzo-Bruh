using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonIndication : MonoBehaviour
{
    public CommonDevice Device;
    void Start()
    {

    }
    public void Indication()
    {
        if (Device.isArea) Device.UpdateOverlay();
        {
            if (Device.isActive)
                Device.firstButton.GetComponent<MeshRenderer>().material.color = Color.green;
            else
                Device.firstButton.GetComponent<MeshRenderer>().material.color = Color.red;

            if (Device.isPowered)
                Device.secondButton.GetComponent<MeshRenderer>().material.color = Color.green;
            else
                Device.secondButton.GetComponent<MeshRenderer>().material.color = Color.red;
        }

    }
    void FixedUpdate()
    {
        Indication();
    }
}
