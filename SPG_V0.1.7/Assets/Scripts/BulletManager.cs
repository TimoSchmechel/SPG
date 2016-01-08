﻿using UnityEngine;
using System.Collections;
using System;

public class BulletManager : MonoBehaviour {

    public float speed = 1000f;
    public int damage = 25;

    public GameObject splatt;

	// Makes bullet go foward
	void Start () {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed);
    }

    // Update is called once per frame
    void Update () {
	
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
                    Instantiate(splatt, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            }
        }


        Destroy(this.gameObject);

    }


}
