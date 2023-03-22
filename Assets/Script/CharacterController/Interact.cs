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
            //cd.transform.GetChild(8).transform.GetChild(0).transform.GetComponent<CanvasRenderer>().GetMaterial().color = new Color(0f, 0f, 0f, 0.5490196f);
        }
            else if (currentCollision != null)
            {
                CommonDevice cd = currentCollision.transform.GetComponent<CommonDevice>();
            //cd.transform.GetChild(8).transform.GetChild(0).transform.GetComponent<CanvasRenderer>().GetMaterial().color = new Color(0f, 0f, 0f, 0f);
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
