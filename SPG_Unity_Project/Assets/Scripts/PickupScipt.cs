using UnityEngine;
using System.Collections;

public class PickupScipt : MonoBehaviour {

    public int amount = 5;

	private static int AMMO = 1;
    private static int HEALTH = 2;

    public int type;

    private float resetTimer;
    public float resetTimeCap = 4f;
    private bool startReset;

    // timer for the object to switch it back on
	void Update () {

        if (startReset)
        {
            resetTimer += Time.deltaTime;
            if(resetTimer >= resetTimeCap)
            {
                if (type == AMMO)
                {
                    GetComponent<BoxCollider>().enabled = true;
                }
                if (type == HEALTH)
                {
                    GetComponent<SphereCollider>().enabled = true;
                }
                GetComponent<MeshRenderer>().enabled = true;
                startReset = false;
            }
        }
	
	}

    //When Player Enters adds ammo to player script, sets collidder and rendered in BOX off. 
    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
			if(type == AMMO){
            col.GetComponent<Player>().currentAmmo += amount;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
			}

			if(type == HEALTH){
				col.GetComponent<Player>().currentHealth += amount;
				GetComponent<SphereCollider>().enabled = false;
				GetComponent<MeshRenderer>().enabled = false;
			}
        }

        startReset = true;
        resetTimer = 0;
    }
}
