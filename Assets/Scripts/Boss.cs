using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    private int attacks;
    private bool isMoving;
    private Vector3 targetMoving;
    [SerializeField] private int maxAttacks = 3;
    [SerializeField] private float maxDistanceMoving;
    [SerializeField] private Animator animator;

    private Vector3 initialPosition;
    void Update()
    {
        if (!isMoving) return;
        
        transform.position = Vector3.MoveTowards(transform.position, targetMoving, maxDistanceMoving * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetMoving) < 0.01)
        {
            isMoving = false;
            animator.SetTrigger("Attack");
        }
    }

    public void Start()
    {
        initialPosition = transform.position;
        Invoke(nameof(HandleAttacks),4);
    }

    private void HandleAttacks()
    {
        if (attacks < maxAttacks)
        {
            attacks++;
            MoveSide();
        }
        else
        {
            Debug.Log("Player Win");
            if (RunManager.instanceExists)
            {
                Invoke(nameof(PlayerWin),2);
            }
        }
    }
    private void MoveSide()
    {
        int side = Random.Range(-1, 1);
        targetMoving = initialPosition + Vector3.right * (side * 3);
        isMoving = true;
    }

    public void FinishMovingBackward()
    {
        HandleAttacks();
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: spawn effect
        Debug.Log("Acertei");
        if (RunManager.instanceExists)
        {
            Invoke(nameof(PlayerLoses),2);
        }
    }
    private void PlayerWin()
    {
        RunManager.instance.OnBossWin.Invoke();
    }

    private void PlayerLoses()
    {
        RunManager.instance.OnPlayerLoses.Invoke();
    }
}
