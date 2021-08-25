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
        Time.timeScale = 0;
    }

    void UnPause()
    {
        Time.timeScale = 1;
    }
    void Update()
    {
        if(Input.GetKeyDown (KeyCode.Escape))
        {
            if(isPause)
            {
                UnPause();
                Time.timeScale = 0f;
                PanelPause.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {

                Pause();
                Time.timeScale = 1f;
                PanelPause.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            isPause = !isPause;
        }
    }
}
