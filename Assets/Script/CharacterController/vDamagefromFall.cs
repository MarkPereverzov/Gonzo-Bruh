using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class vDamagefromFall : MonoBehaviour
{
    private bool alive;
    private float healpoint;
    public float speed;
    private Rigidbody rigidbody;
    private Transform gravestone;
    public float rotx;

    void Start()
    {
        alive = true;
        healpoint = 100;
        rigidbody = GetComponent<Rigidbody>();
        rotx = 0;
    }

    void Update()
    {
        if (healpoint <= 0)
        {
            alive = false;
        }
        if (!alive)
        {
            gravestone = gameObject.transform.GetChild(2);
            //gameObject.transform.DetachChildren();
            gravestone.gameObject.SetActive(true);
            //gameObject.SetActive(true);
        }
        if (rigidbody.velocity.y < speed)
        {
            speed = rigidbody.velocity.y;
        }
        //rotx += 0.2f;
        rigidbody.rotation = Quaternion.EulerAngles(rotx, rigidbody.rotation.y, rigidbody.rotation.z);
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Ground_01")
        {
            if (speed < -10)
            {
                healpoint += 3*speed;
            }
        }
        speed = 0;
        //speed = 0;
        Debug.Log(healpoint);
    }
}
  