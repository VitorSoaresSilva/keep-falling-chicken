using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldComponent : MonoBehaviour
{
    public int gold = 1;
    public GameObject player;
    private bool isMovingByMagnet;
    public void MagnetActive(GameObject player)
    {
        transform.parent = null;
        isMovingByMagnet = true;
        this.player = player;
    }

    private void Update()
    {
        if (!isMovingByMagnet) return;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.5f);
        
    }
}
