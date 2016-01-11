using UnityEngine;
using System.Collections;

public class MouseCursorToggle : MonoBehaviour
{

    bool CursorLockedVar;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
        CursorLockedVar = (true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !CursorLockedVar)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = (false);
            CursorLockedVar = (true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && CursorLockedVar)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = (true);
            CursorLockedVar = (false);
        }
    }
}