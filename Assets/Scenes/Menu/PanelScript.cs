using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelScript : MonoBehaviour
{
    public GameObject Panel;
    int count = 1 ;

    public void ConfigPanel()
    {
        count++;
        if(count % 2 == 1)
        {
            Panel.gameObject.SetActive (false);
        }
        else
        {
            Panel.gameObject.SetActive (true);
        }
    }
}
