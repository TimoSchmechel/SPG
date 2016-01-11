using UnityEngine;
using System.Collections;

public class MouseCursorToggle : MonoBehaviour {
    private bool toggle = true;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        


	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (toggle)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;

                toggle = false;
            } else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                toggle = true;
            }
        }
	}
}
