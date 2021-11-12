using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class actionTeste : MonoBehaviour
{
    public SpawnSystem spawnSystem;
    public Scene scene;
    // Start is called before the first frame update
    void Start()
    {
        spawnSystem.batata = Batata;
        // scene
    }

    private void Batata()
    {
        Debug.Log("batata");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
