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
    private void Start()
    {
        shieldObject.SetActive(false);
        joystick = StateMachine.instance.UI.GameView.joystick;
        
        camera = SceneDataHolder.instance.mainCamera;
        right = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.transform.position.z)).x;
    }

    void Update()
    {
        // #if UNITY_EDITOR
        //     float horizontal = Input.GetAxis("Horizontal");
        //     float vertical = Input.GetAxis("Vertical");
        // #else
        //     Vector2 input 
        // #endif
        float horizontal;
        float vertical;
        if (joystick.Horizontal > 0.1f)
        {
            horizontal = 1;
        }
        else if(joystick.Horizontal < -0.1f)
        {
            horizontal = -1;
        }
        else
        {
            horizontal = 0;
        }
        if (joystick.Vertical > 0.1f)
        {
            vertical = 1;
        }
        else if(joystick.Vertical < -0.1f)
        {
            vertical = -1;
        }
        else
        {
            vertical = 0;
        }
        Vector2 input = new Vector2(horizontal, vertical);
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothInputSpeed,1);
        Vector3 direction = new Vector3(currentInputVector.x, currentInputVector.y, 0f);
        Vector3 position = transform.position;
        position += direction * speedMultiplier * Time.deltaTime;
        
        position = new Vector3(
            Mathf.Clamp(position.x, -right, right),
            Mathf.Clamp(position.y, -right, right),
            position.z);
        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if (PowerUpsManager.instance.playerInvincible)
            {
                OnPlayerInivincibleHit?.Invoke();
                EnemiesManager.instance.Destroy(other.transform);
            }
            else
            {
                RunManager.instance.OnPlayerTakeDamage?.Invoke();
            }
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
