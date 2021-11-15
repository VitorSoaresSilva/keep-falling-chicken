using UnityEngine;
using UnityEngine.Events;

public class MenuView : BaseView
{
    [SerializeField]
    public UnityEvent OnStartClicked;
    public UnityAction OnQuitClicked;
    public UnityEvent OnConfigClicked;
    public UnityEvent OnStoreClicked;
    public GameObject creditsPanel;
    public void ClickStart()
    {
        OnStartClicked?.Invoke();
    }

    public void ClickQuit()
    {
        OnQuitClicked?.Invoke();
    }

    public void ClickConfig()
    {
        OnConfigClicked?.Invoke();
    }
    public void Credits(bool value)
    {
        creditsPanel.SetActive(value);
    }
    public void ClickStore()
    {
        OnStoreClicked?.Invoke();
    }
}
