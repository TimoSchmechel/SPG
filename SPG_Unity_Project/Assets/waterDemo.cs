using UnityEngine;
using System.Collections;

public class waterDemo : MonoBehaviour {

	public Material water;
	private float speed = 0.05f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float offset = Time.time * speed;
		water.SetTextureOffset ("_MainTex", new Vector2 (offset/2, offset/2));
		water.SetTextureOffset ("_DetailAlbedoMap", new Vector2 (offset*2, offset*2));
	}
}
