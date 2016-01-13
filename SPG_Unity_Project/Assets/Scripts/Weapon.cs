using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public int magazineSize = 10;
    public int magazineAmmo;
    public int damage; //needs to be accessed by SSM to deal appropriate damage, implement later

	// Use this for initialization
	void Start () {
        magazineAmmo = magazineSize;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
