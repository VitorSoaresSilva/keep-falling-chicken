using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CoinComponent coinComponent))
        {
            coinComponent.MagnetActive(GameManager.instance.playerTransform);
        }
    }
}
