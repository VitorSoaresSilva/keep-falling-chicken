using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts
{
    public class UIStartController : MonoBehaviour
    {
        [SerializeField] private Slider sliderGeneralAudio;
        [SerializeField] private Slider sliderMusicAudio;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private TextMeshProUGUI audioMusicText;
        [SerializeField] private TextMeshProUGUI audioGeneralText;

        private bool isLobbyLoaded = false;
        private AsyncOperation lobbyLoadin;

        private float minAudio;
        private float maxAudio;
        private void Awake()
        {
            //TODO: Get the saved config data
            minAudio = sliderGeneralAudio.minValue;
            maxAudio = sliderGeneralAudio.maxValue;
            SetMusicVolume();
            SetGeneralVolume();
            Debug.Log("Começou a carregar");
            StartCoroutine(LoadScene());
            // GameManager.instanceExists;
        }

        IEnumerator LoadScene()
        {
            
            yield return null;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("UILobby");
            asyncOperation.allowSceneActivation = false;
            //TODO: spawn loading page with set active false
            while (!asyncOperation.isDone)
            {
                //TODO: upodate text of loading page
                // text = (asyncOperation.progress * 100) + "%";
                if (asyncOperation.progress >= 0.9f)
                {
                    //Change the Text to show the Scene is ready
                    //Wait to you press the space key to activate the Scene
                    if (Input.GetKeyDown(KeyCode.Space))
                        asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }

        }


        public void ButtonStart()
        {
            GameManager.StartGame();
        }

        public void SetMusicVolume()
        {
            float percent = Mathf.Lerp(minAudio, maxAudio,sliderMusicAudio.value);
            audioMusicText.text = string.Concat((Math.Floor(percent * 100))," %");
            audioMixer.SetFloat("MusicVolume", Mathf.Log(sliderMusicAudio.value) * 20);
        }
        public void SetGeneralVolume()
        {
            float percent = Mathf.Lerp(minAudio, maxAudio,sliderGeneralAudio.value);
            audioGeneralText.text = string.Concat((Math.Floor(percent * 100))," %");
            audioMixer.SetFloat("Volume", Mathf.Log(sliderGeneralAudio.value) * 20);
        }

        public void HandleLobbySceneLoaded(AsyncOperation obj)
        {
            isLobbyLoaded = true;
            Debug.Log("Carregado");
        }
    }
}
