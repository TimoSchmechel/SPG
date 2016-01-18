using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NameScript : MonoBehaviour
{
    public InputField nameInput;
  //  public string textString; -- see line 18

	void Start () {
        if (nameInput == null)
            nameInput = FindObjectOfType<InputField>();
	}

	void Update () {
       // textString = PlayerPrefs.GetString(GlobalScript.ppPlayerNameKey); --- wasnt doing anything
	}

    public void SaveName()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        PlayerPrefs.SetString(GlobalScript.ppPlayerNameKey, nameInput.text);
        PlayerPrefs.Save();

        //GlobalScript.instanceName = nameInput.text;
        //print("Name is "+GlobalScript.instanceName);
    }
}
