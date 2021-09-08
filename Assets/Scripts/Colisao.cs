using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colisao : MonoBehaviour
{
    //Colis�o com caixa(cubo vermelho)
    public AudioClip col;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DanoCaixa")
        {
            GetComponent<AudioSource>().PlayOneShot(col);
        }
    }

    //Colis�o com caixa(cubo azul)


}
