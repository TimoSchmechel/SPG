using UnityEngine;
using System.Collections;

public class SplattScript : MonoBehaviour {

    private float killTimer = 0f;
    public float killTimeCap = 6000f;



	void Update () {
        killTimer += Time.deltaTime;
        if (killTimer >= killTimeCap)
        {
            Destroy(this.gameObject);
        }
    }
}
