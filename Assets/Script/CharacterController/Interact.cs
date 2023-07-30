//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public Transform eyeObject;
    public Material material;

    private Collider currentCollision;
    private CommonDevice choosenDevice;

    Interactable observeableProp;
    
    private bool isArea;
    public Context context;

    void Start()
    {
        isArea = false;
    }
    private void PickUp(Interactable prop)
    {
        context.hand = prop;
    }
    private void Drop()
    {
        context.hand = null;
    }

    private void KeepProp()
    {
        context.hand.transform.position = transform.position * 1.2f;
    }

    void FixedUpdate()
    {
        if (context.hand != null)
        {
            if (Input.GetKeyDown("e"))
            {
                context.hand.Activate(context);
            }
        }
        if (Input.GetKeyDown("f"))
        {
            if (context.hand == null){
                PickUp(observeableProp);
            }
            else {
                Drop();
            }
        }
        KeepProp();
        if (isArea)
        {
            CommonDevice cd = currentCollision.transform.GetComponent<CommonDevice>();
            context.currentDevice = cd;
            GameObject.Find(context.currentDevice.name + "/UI").transform.rotation = new Quaternion(0, eyeObject.rotation.y, 0, eyeObject.rotation.w);
            if (Input.GetKeyDown("e")) 
            {
                context.currentDevice.e_OnActivation.Invoke();
            }
            if (Input.GetKeyDown("f"))
            {
                Debug.Log("Using " + context.currentDevice.type);
                if (choosenDevice == null)
                    choosenDevice = cd;
                else 
                {
                    choosenDevice.e_OnConnect.Invoke(context.currentDevice);
                    context.currentDevice.e_OnConnect.Invoke(choosenDevice);
                    choosenDevice = null;
                }
            }
            //cd.transform.GetChild(8).transform.GetChild(0).transform.GetComponent<CanvasRenderer>().GetMaterial().color = new Color(0f, 0f, 0f, 0.5490196f);
        }
            else if (currentCollision != null)
            {
                CommonDevice cd = currentCollision.transform.GetComponent<CommonDevice>();
                context.currentDevice = cd;
            //cd.transform.GetChild(8).transform.GetChild(0).transform.GetComponent<CanvasRenderer>().GetMaterial().color = new Color(0f, 0f, 0f, 0f);
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<CommonDevice>() != null)
        {
            CommonDevice cd = collision.transform.GetComponent<CommonDevice>();
            context.currentDevice = cd;
            isArea = true;
            context.currentDevice.e_OnShowHint.Invoke(true);
            currentCollision = collision;
        }
        else
        {
            var prop = collision.gameObject.GetComponent<Interactable>();
            if (prop != null)
            {
                observeableProp = prop;
            }
        }
        //Debug.Log("Trigger enter type " + collision.transform.GetComponent<CommonDevice>().type);
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponent<CommonDevice>() != null)
        {
            CommonDevice cd = collision.transform.GetComponent<CommonDevice>();
            context.currentDevice = cd;
            isArea = false;
            context.currentDevice.e_OnShowHint.Invoke(false);
            context.currentDevice = null;
        }
        else
        {
            var prop = collision.gameObject.GetComponent<Interactable>();
            if (prop != null)
            {
                if (prop == observeableProp) observeableProp = null;
            }
        }
    }
}
