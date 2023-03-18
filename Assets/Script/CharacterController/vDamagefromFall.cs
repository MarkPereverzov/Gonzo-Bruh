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
    public GameObject gravestone;
    private Camera cam;

    void Start()
    {
        alive = true;
        healpoint = 100;
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (healpoint <= 0)
        {
            alive = false;
        }
        if (!alive)
        {
            Instantiate(gravestone, new Vector3(0, 0, 0), Quaternion.identity);
            gravestone = GameObject.Find("gravestone(Clone)");

            gravestone.transform.position = gameObject.transform.position;
            gravestone.transform.GetChild(0).gameObject.SetActive(false);
            gravestone.transform.GetChild(2).gameObject.SetActive(false);

            Destroy(gameObject);
            GameObject cam = GameObject.Find("Camera");
            cam.transform.position = gravestone.transform.position + new Vector3(-0.5f,2.5f,-2);
        }
        if (rigidbody.velocity.y < speed)
        {
            speed = rigidbody.velocity.y;
        }
        
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
  