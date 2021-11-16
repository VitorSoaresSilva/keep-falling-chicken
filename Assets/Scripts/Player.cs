using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // public float velMovimento;
    public  float right;
    [SerializeField] private Camera camera;
    public AnimationCurve speedCurve;

    public float time;
    public float speedMultiplier;


    private Vector2 smoothInput;

    [SerializeField] private float smoothInputSpeed = .2f;
    [SerializeField] private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    private void Start()
    {
        camera = SceneDataHolder.instance.mainCamera;
        right = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.transform.position.z)).x;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
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
            RunManager.instance.OnPlayerTakeDamage?.Invoke();
        }else if (other.TryGetComponent(out CoinComponent coinComponent))
        {
            RunManager.instance.CollectGold(coinComponent.gold);
            //TODO: use a pool here
            Destroy(other.gameObject);
        }else if (other.TryGetComponent(out PowerUpComponent powerUpComponent))
        {
            
            Destroy(other.gameObject);
        }
    }
}
