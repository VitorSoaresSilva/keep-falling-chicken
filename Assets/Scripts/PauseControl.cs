using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour
{
    bool isPause;
    public GameObject PanelPause;

    void Start()
    {
        isPause = true;
        PanelPause.SetActive(false);
    }

    void Pause()
    {
        Time.timeScale = 1;
    }

    void UnPause()
    {
        Time.timeScale = 0;
    }
    void Update()
    {
        if(Input.GetKeyDown (KeyCode.Escape))
        {
            if(isPause)
            {
                UnPause();
                PanelPause.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }
            else
            {

                Pause();
                PanelPause.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

            }
            isPause = !isPause;
        }
    }
}
