using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

    public InputField nameInput;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void startGame()
    {
        PlayerPrefs.SetString(GlobalScript.ppPlayerNameKey, nameInput.text);
        Application.LoadLevel("MainLevel");
    }
}
