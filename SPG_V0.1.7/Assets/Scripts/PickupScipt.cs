using UnityEngine;
using System.Collections;

public class PickupScipt : MonoBehaviour {

    public int amount = 5;

	public bool ammo= false;
	public bool health = false;

    private float resetTimer;
    public float resetTimeCap = 4f;
    private bool startReset;

	// Use this for initialization
	void Start () {
        
}
	
	// Update is called once per frame

    // timer for the object to switch it back on
	void Update () {
        //this.gameObject.SetActive(true);
        if (startReset)
        {
            resetTimer += Time.deltaTime;
            if(resetTimer >= resetTimeCap)
            {
                GetComponent<BoxCollider>().enabled = true;
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
			if(ammo && !health){
            col.GetComponent<Player>().currentAmmo += amount;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
			}
			if(health && !ammo){
				col.GetComponent<Player>().currentHealth += amount;
				GetComponent<SphereCollider>().enabled = false;
				GetComponent<MeshRenderer>().enabled = false;
			}
        }

        startReset = true;
        resetTimer = 0;
    }
}
