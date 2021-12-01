using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    // public float velMovimento;
    public  float right;
    [SerializeField] private Camera camera;
    // public AnimationCurve speedCurve;

    // public float time;
    public float speedMultiplier;
    private Vector2 smoothInput;
    [SerializeField] private float smoothInputSpeed = .2f;
    [SerializeField] private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;

    public GameObject shieldObject;
    public UnityEvent OnPlayerInivincibleHit;
    public Joystick joystick;

    Animator animator;
        
    private void Start()
    {
        shieldObject.SetActive(false);
        if (RunManager.instance.currentState == RunManager.State.Boss)
        {
            joystick = StateMachine.instance.UI.BossView.joystick;
            joystick.AxisOptions = AxisOptions.Horizontal;
        }
        else
        {
            joystick = StateMachine.instance.UI.GameView.joystick;
        }
        
        camera = SceneDataHolder.instance.mainCamera;
        right = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.transform.position.z)).x;

        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float horizontal;
        float vertical;
        if (Mathf.Abs(joystick.Horizontal) > 0.1f)
        {
            horizontal = joystick.Horizontal;
        }
        else
        {
            horizontal = 0;
        }
        if (Mathf.Abs(joystick.Vertical) > 0.1f)
        {
            vertical = joystick.Vertical;
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
    }
}
