//using Microsoft.Unity.VisualStudio.Editor;
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

    public Transform eyeObject;

    void Start()
    {
        isArea = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isArea)
        {
            CommonDevice cd = currentCollision.transform.GetComponent<CommonDevice>();
            GameObject.Find(cd.name + "/UI").transform.rotation = new Quaternion(0,eyeObject.rotation.y,0,eyeObject.rotation.w);
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
        else if (currentCollision != null)
        {
            CommonDevice cd = currentCollision.transform.GetComponent<CommonDevice>();
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        CommonDevice cd = collision.transform.GetComponent<CommonDevice>();
        isArea = true;
        cd.e_OnShowHint.Invoke(true);
        currentCollision = collision;
        //Debug.Log("Trigger enter type " + collision.transform.GetComponent<CommonDevice>().type);
    }
    private void OnTriggerExit(Collider collision)
    {
        CommonDevice cd = collision.transform.GetComponent<CommonDevice>();
        isArea = false;
        cd.e_OnShowHint.Invoke(false);
    }
}
