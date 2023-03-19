using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public Material material;
    private Collider currentCollision;
    private bool isArea;
    private CommonDevice choosenDevice;

    void Start()
    {
        isArea = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isArea)
        {
            CommonDevice cd = currentCollision.transform.GetComponent<CommonDevice>();
            if (Input.GetKeyDown("e")) 
            {
                cd.e_OnActivation.Invoke();
            }
            if (Input.GetKeyDown("r"))
            {
                Debug.Log("Using " + cd.type);
                if (choosenDevice == null)
                    choosenDevice = cd;
                else 
                {
                    choosenDevice.e_OnConnect.Invoke(cd);
                    cd.e_OnConnect.Invoke(choosenDevice);
                    choosenDevice = null;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        isArea = true;
        currentCollision = collision;
        //Debug.Log("Trigger enter type " + collision.transform.GetComponent<CommonDevice>().type);
    }
    private void OnTriggerExit(Collider collision)
    {
         isArea = false;
        currentCollision = null;
    }
}
