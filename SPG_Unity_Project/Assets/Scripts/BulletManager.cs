using UnityEngine;
using System.Collections;
using System;

public class BulletManager : MonoBehaviour {

    public float speed = 1000f;
    public int damage = 25;

    public Player shooter;

    public GameObject splatter;

    // Makes bullet go foward
    void Start () {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed);
        GetComponent<MeshRenderer>().material.SetColor("_Color", shooter.GetComponent<Teams>().colour);
    }

    //Destroy when it hits anything. 
    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag != "Boundry")
        {
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            RaycastHit hit;

            //Instantiate(splatt, this.transform.position, Quaternion.FromToRotation(this.transform.forward, collision.transform.position));
            if(collision.transform.tag != "Player")
            {
                if (Physics.Raycast(this.transform.position, fwd, out hit, 100))
                {
                    GameObject tmpSplatter = Instantiate(splatter, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
                    tmpSplatter.transform.Rotate(new Vector3(0, UnityEngine.Random.Range(0, 360), 0));
                    tmpSplatter.transform.Find("Splatter").GetComponent<MeshRenderer>().material.SetColor("_Color", shooter.GetComponent<Teams>().colour);
                }
            }
        }
        Destroy(this.gameObject);

    }


}
