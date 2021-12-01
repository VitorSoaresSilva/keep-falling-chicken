using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    private int attacks;
    private bool isMoving;
    private Vector3 targetMoving;
    private Vector3 lastTarget;
    [SerializeField] private int maxAttacks = 3;
    [SerializeField] private float maxDistanceMoving;
    [SerializeField] private float maxDistanceSides = 3;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject effect;
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
        lastTarget = initialPosition;
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
            // player wins
            if (RunManager.instanceExists)
            {
                Invoke(nameof(PlayerWin),1);
            }
        }
    }
    private void MoveSide()
    {
        int side = Random.Range(-1, 1);
        targetMoving = initialPosition + Vector3.right * (side * maxDistanceSides);
        while (targetMoving == lastTarget)
        {
            side = Random.Range(-1, 1);
            targetMoving = initialPosition + Vector3.right * (side * maxDistanceSides);
        }
        lastTarget = targetMoving;
        isMoving = true;
    }

    public void FinishMovingBackward()
    {
        Invoke(nameof(HandleAttacks),1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Instantiate(effect, other.transform.position,Quaternion.identity);
            if (RunManager.instanceExists)
            {
                Invoke(nameof(PlayerLoses),1);
            }
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
