using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMagnet : MonoBehaviour
{
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out GoldComponent goldComponent))
        {
            goldComponent.MagnetActive(player);
        }
    }
}
