using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class GameManager : MonoBehaviour
    {
        public GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);
            if (!SceneManager.GetSceneByName("UIStart").isLoaded)
            {
                SceneManager.LoadScene("UIStart", LoadSceneMode.Additive);
            }
        }

        public static void StartGame()
        {
            
        }
        
    }
}
