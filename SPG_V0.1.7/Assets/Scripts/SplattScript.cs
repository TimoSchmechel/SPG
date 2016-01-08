using UnityEngine;
using System.Collections;

public class SplattScript : MonoBehaviour {
    private float killTimer = 0f;
    public float killTimeCap = 600f;



    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        killTimer += Time.deltaTime;
        if (killTimer >= killTimeCap)
        {
            Destroy(this.gameObject);
        }
    }
}
