using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colisao : MonoBehaviour
{
    //Colisão com caixa(cubo vermelho)
    public AudioClip col;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DanoCaixa"))
        {
            GetComponent<AudioSource>().PlayOneShot(col);
        }
    }

    //Colisão com caixa(cubo azul)


}
