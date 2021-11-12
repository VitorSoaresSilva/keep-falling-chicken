using System;
using  System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnalogic : MonoBehaviour
{
    private float vertical, horizontal;
    public float speed = 1;
    private CharacterController characterController;

    private Rigidbody rb;
    // Start is called before the first frame update
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");


        Vector3 move = Vector3.right * horizontal + Vector3.up * vertical;
        characterController.Move(move * speed * Time.deltaTime);
        //transform.position =  Vector3.MoveTowards(transform.position, transform.position + new Vector3(horizontal, vertical), speed); // muito ruim

        // rb.AddForce(new Vector3(horizontal,vertical) * Time.deltaTime * speed,ForceMode.Acceleration) ; // mais ou menos
        // transform.position.
    }
}
