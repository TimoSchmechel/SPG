using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Player))]
public class HealthManager : MonoBehaviour {

    //Slider in canvas
    public Slider healthBar;

    //local player
    [SerializeField]
    private Player player;

	// Use this for initialization
	void Start () {
        player = GetComponent<Player>(); // get player component
        healthBar = GameObject.Find("HealthHUD").GetComponent<Slider>(); // find the slider in the canavs, we can change this to local health bar
	}
	
	// Update is called once per frame
	void Update () {
        healthBar.value = player.currentHealth; // sets slider to move with health. 
	}
}
