using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public static int SINGLE_MODE = 0;
    public static int AUTO_MODE = 1;
    public static int BURST_MODE = 2;

    public int fireMode;

    public int burstSize = 3; //number of bullets fired per burst
    public float fireRate = 0.15f; //time in between shoots
    public float burstRate = 0.5f; //time in between bursts in burst mode 

    public int currentBurstShot; //used to manage burst

    public int magazineSize = 10;
    public int magazineAmmo;

    public float accuracy = 0; //value of 0 is inaccurate -- 100 is super accurate (every bullet will land in the same spot)

    public int damage; //needs to be accessed by SSM to deal appropriate damage, implement later

    // Use this for initialization
    void Start() {
        SetupWeapon();

    }

    public void SetupWeapon() { 
        magazineAmmo = magazineSize;
    }

    //Returns accuracy as a float between 0 and 1, where 1 is inaccurate and 0 is accurate. This is used when determining the shooting location in playerShoot
    public float getNormalizedAccuracy()
    {
        return (100 - accuracy) / 100f;
    }
}
