using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public int magazineSize = 10;
    public int magazineAmmo;

    public float accuracy = 0; //value of 0 is inaccurate -- 100 is super accurate (every bullet will land in the same spot)

    public int damage; //needs to be accessed by SSM to deal appropriate damage, implement later

	// Use this for initialization
	void Start () {
        magazineAmmo = magazineSize;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //Returns accuracy as a float between 0 and 1, where 1 is inaccurate and 0 is accurate. This is used when determining the shooting location in playerShoot
    public float getNormalizedAccuracy()
    {
        return (100 - accuracy) / 100f;
    }
}
