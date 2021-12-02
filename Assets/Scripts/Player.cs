using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public float speedMultiplier;
    private Vector2 smoothInput;
    private Vector2 smoothInputVelocity;

    public GameObject shieldObject;
    public GameObject magnetObject;
    public UnityEvent OnPlayerInivincibleHit;
    public Joystick joystick;

    Animator animator;
    [SerializeField] private GameObject effectDamage;
        
    private void Start()
    {
        shieldObject.SetActive(false);
        magnetObject.SetActive(false);
        if (RunManager.instance.currentState == RunManager.State.Boss)
        {
            joystick = StateMachine.instance.UI.BossView.joystick;
            joystick.AxisOptions = AxisOptions.Horizontal;
        }
        else
        {
            joystick = StateMachine.instance.UI.GameView.joystick;
        }
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float horizontal;
        float vertical;
        if (Mathf.Abs(joystick.Horizontal) > 0.1f)
        {
            if (joystick.Horizontal > 0)
            {
                horizontal = 1;
            }
            else
            {
                horizontal = -1;
            }
        }
        else
        {
            horizontal = 0;
        }
        if (Mathf.Abs(joystick.Vertical) > 0.1f)
        {
            if (joystick.Vertical > 0)
            {
                vertical = 1;
            }
            else
            {
                vertical = -1;
            }
        }
        else
        {
            vertical = 0;
        }
        Vector3 input = new Vector2(horizontal, vertical);
        transform.position  += input * speedMultiplier * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if (PowerUpsManager.instance.playerInvincible)
            {
                OnPlayerInivincibleHit?.Invoke();
            }
            else
            {
                RunManager.instance.OnPlayerTakeDamage?.Invoke();
                Instantiate(effectDamage,transform.position,Quaternion.identity,transform);
                animator.SetTrigger("Hit");
            }
            EnemiesManager.instance.Destroy(other.transform);
        }else if (other.TryGetComponent(out CoinComponent coinComponent))
        {
            RunManager.instance.CollectGold(coinComponent.gold);
            PowerUpsManager.instance.CollectPowerUp(PowerUpTypes.dash);
            //TODO: use a pool here
            Destroy(other.gameObject);
        }else if (other.TryGetComponent(out PowerUpComponent powerUpComponent))
        {
            PowerUpsManager.instance.CollectPowerUp(powerUpComponent.type);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("ZequinhaIsNear"))
        {
            RunManager.instance.ZequinhaIsNear();
        }else if (other.CompareTag("Geraldo"))
        {
            animator.SetTrigger("Hit");
        }
    }
}
