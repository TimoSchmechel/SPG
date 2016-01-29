using UnityEngine;
using System.Collections;
using System;

public class BulletManager : MonoBehaviour {

    public float speed = 1000f;
    public int damage = 25;
    public bool damageSwitch = false;//the little switch we toggle to determine whether its delivered its damage or not...

    public Player shooter;

    public GameObject splatter;

    public Color bulletColor;
    public int bulletTeam =0;

    // Makes bullet go foward
    void setRandomColor()
    {
        int randomNO = UnityEngine.Random.Range(0, 3);
        print("Random = " + randomNO);
        if(shooter.GetComponent<Teams>().team ==1)
        {
            switch(randomNO)
            {
                case 0:
                    bulletColor = Color.red;
                    break;
                case 1:
                    bulletColor = Color.magenta;
                    break;
                case 2:
                    bulletColor = Color.yellow;
                    break;
            }

            bulletTeam = 1;
        }
        else
        {
            switch (randomNO)
            {
                case 0:
                    bulletColor = Color.blue;
                    break;
                case 1:
                    bulletColor = Color.cyan;
                    break;
                case 2:
                    bulletColor = Color.green;
                    break;
            }

            bulletTeam = 2;
        }
    }

    void Start () {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed);
        setRandomColor();
        GetComponent<MeshRenderer>().material.SetColor("_Color", bulletColor);
        //GetComponent<MeshRenderer>().material.SetColor("_Color", shooter.GetComponent<Teams>().colour);
    }

    //Destroy when it hits anything. 
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "Paintball")
        {
            if (collision.transform.tag != "Boundry")
            {
                Vector3 fwd = transform.TransformDirection(Vector3.forward);
                RaycastHit hit;

                //Instantiate(splatt, this.transform.position, Quaternion.FromToRotation(this.transform.forward, collision.transform.position));
                if (collision.transform.tag != "Player") //&& collision.transform.GetComponent<Teams>().team != bulletTeam)
                {
                    //print("BulletNO: " + bulletTeam + " TeamNO: " + collision.transform.GetComponent<Teams>().team);
                    if (Physics.Raycast(this.transform.position, fwd, out hit, 100))
                    {
                        GameObject tmpSplatter = Instantiate(splatter, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
                        tmpSplatter.transform.Rotate(new Vector3(0, UnityEngine.Random.Range(0, 360), 0));
                        tmpSplatter.transform.Find("Splatter").GetComponent<MeshRenderer>().material.SetColor("_Color", bulletColor);
                        //tmpSplatter.transform.Find("Splatter").GetComponent<MeshRenderer>().material.SetColor("_Color", shooter.GetComponent<Teams>().colour);
                    }
                }
            }

            Destroy(this.gameObject);
        }

    }


}
