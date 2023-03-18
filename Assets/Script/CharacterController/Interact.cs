using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public Material material;
    private Collider currentCollision;
    private bool isArea;

    void Start()
    {
        isArea = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isArea)
        {
            if (Input.GetKeyDown("e")) 
            {
                currentCollision.transform.GetComponent<CommonDevice>().e_OnActivation.Invoke();
            }

        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "device")
        {
            isArea = true;
            currentCollision = collision;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.name == "device")
        {
            isArea = false;
        }
    }
}
