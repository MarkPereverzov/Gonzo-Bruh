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
        context = new Context();
        context.human = this;
    }
    private void PickUp(Interactable prop)
    {
        context.hand = prop;
    }
    private void Drop()
    {
        context.hand.collider.isTrigger = false;
        //var pos = context.hand.transform.position;
        var rigidbody = context.hand.GetComponent<Rigidbody>();
        context.hand = null;
        rigidbody.Sleep();
        //context.hand.trigger.enabled = true;
    }

    private void KeepProp()
    {
        if (context.hand != null)
        {
            context.hand.collider.isTrigger = true;
            context.hand.transform.rotation = transform.rotation;
            var hands = GameObject.Find(gameObject.name + "/Armature/Root_M/Spine1_M/Spine2_M/Chest_M/Scapula_R/Shoulder_R/Elbow_R/Wrist_R/Item");
            //Debug.Log(hands.transform.position);
            context.hand.transform.position = hands.transform.position;
            //context.hand.transform.position = GameObject.Find(gameObject.name + "/Mesh/Body/Body_Hands").transform.position;
            
            
            context.hand.OnShowHint(false);
                
            //Debug.Log(rhand_point.position);
        }
            
    }

    void Update()
    {
        
        if(observeableProp != null)
        {
            observeableProp.OnShowHint(true);
        }
        if (context.hand != null)
        {
            if (Input.GetKeyDown("q"))
            {
                context.hand.Activate(context);
            }
        }
        if (Input.GetKeyDown("r"))
        {
            Debug.Log("Key pressed F");
            if (context.hand == null)
            {
                PickUp(observeableProp);
                Debug.Log("Picked up prop !!");
            }
            else
            {
                Drop();
            }
        }
        KeepProp();
        if(observeableProp != null) 
            GameObject.Find(observeableProp.name + "/UI").transform.rotation = new Quaternion(0, eyeObject.rotation.y, 0, eyeObject.rotation.w);
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
            Debug.Log("Entered collision");
            var prop = collision.gameObject.GetComponent<Interactable>();
            if (prop != null)
            {
                observeableProp = prop;
                observeableProp.OnShowHint(true);
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
                if (prop == observeableProp)
                {
                    prop.OnShowHint(false);
                    observeableProp = null;
                }
            }
        }
    }
}
