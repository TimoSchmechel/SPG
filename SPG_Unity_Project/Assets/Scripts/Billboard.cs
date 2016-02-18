using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    Camera localCam;

	// Use this for initialization
	void Start ()
    {
        localCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(localCam.transform.position);
	}
}
