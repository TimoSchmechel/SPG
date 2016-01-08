using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NameScript : MonoBehaviour
{
    public InputField nameInput;
    public string textString;

	// Use this for initialization
	void Start () {
        if (nameInput == null)
            nameInput = FindObjectOfType<InputField>();
	}
	
	// Update is called once per frame
	void Update () {
        textString = PlayerPrefs.GetString(GlobalScript.ppPlayerNameKey);
	}

    public void SaveName()
    {
        PlayerPrefs.SetString(GlobalScript.ppPlayerNameKey, nameInput.text);
    }
}
