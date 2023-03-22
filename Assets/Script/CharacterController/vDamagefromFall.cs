using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class vDamagefromFall : MonoBehaviour
{
    private bool alive;
    private bool dead;
    private float healpoint;
    public float speed;
    public float rot;
    private Rigidbody rigidbody;
    public GameObject gravestone;
    private Camera cam;

    void Start()
    {
        alive = true;
        dead = false;
        healpoint = 100;
        rigidbody = GetComponent<Rigidbody>();
        rot = 0;
    }

    void Update()
    {
        transform.rotation = new Quaternion(rot,transform.rotation.y,transform.rotation.z,transform.rotation.w);
        if (healpoint <= 0)
        {
            alive = false;
        }
        if (!alive)
        {
            if (!dead)
            {
                Instantiate(gravestone, new Vector3(0, 0, 0), Quaternion.identity);
                dead = true;
                gravestone = GameObject.Find("gravestone(Clone)");

                gravestone.transform.position = gameObject.transform.position;
                gravestone.transform.GetChild(0).gameObject.SetActive(false);
                gravestone.transform.GetChild(2).gameObject.SetActive(false);

                Destroy(gameObject);
                Destroy(rigidbody);
            }
                GameObject cam = GameObject.Find("Camera");
                cam.transform.position = gravestone.transform.position + new Vector3(-0.5f, 2.5f, -2);
            
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
            speed = 0;
        }
 
    }
   
}
  