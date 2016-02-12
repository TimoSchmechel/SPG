//level selection class..
//use with GUI system to select different levels.

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public InputField nameInput;
    public string [] levels;
    public int levelIndex = 0;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    //hook this into a GUI element to change levels
    public void SetLevelIndex(int i)
    {
        levelIndex = i;
    }

    public void LoadLevel(int i)
    {
        SetLevelIndex(i);
        LoadLevel();
    }

    public void LoadLevel()
    {
        PlayerPrefs.SetString(GlobalScript.ppPlayerNameKey, nameInput.text);
        SceneManager.LoadScene(levels[levelIndex]);
    }
}
