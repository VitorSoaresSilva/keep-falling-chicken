using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIController : MonoBehaviour
{
    public void CarregaCena(string nomeCena)
    {
        SceneManager.LoadScene(nomeCena);
    }

    public void DelayCena(string nomeCena)
    {
        StartCoroutine(DelayCarrega(nomeCena));
    }

    private IEnumerator DelayCarrega(string nomeCena)
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(nomeCena);
    }

    public void SairJogo()
    {
        Application.Quit();
    }
}
