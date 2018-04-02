using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    private float gravity;
    private float YVel;
    private float jumpSpeed;

    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float accel;

    private float rotateValue;
    [SerializeField]
    private float rotAdd;

    CharacterController controller;
    public GameObject player;
    AudioSource jumpSound;

    // Use this for initialization
    void Start ()
    {
        gravity = 30;
        jumpSpeed = 15;

        speed = 0;
        maxSpeed = 25;
        accel = 0.1f;

        jumpSound = transform.Find("Sonic_Jam_01").GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        controller = GetComponent<CharacterController>();

        if (Input.GetButton("Accel"))
        {
            speed += accel;
        }

        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);

        if (speed > 12)
        {
            rotAdd = speed;
        }
        else
        {
            rotAdd = 0;
        }

        rotateValue = Input.GetAxis("Horizontal") * (Time.deltaTime * 25 + (rotAdd / 35));

        if (Physics.Raycast(transform.position, transform.forward, 1))
        {
            speed = 0;
        }

        if (controller.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                YVel = jumpSpeed;
                jumpSound.Play();
            }
        }
        if (!Input.GetButton("Accel"))
        {
            if (speed > accel)
            {
                speed -= (accel * 2);
            }
            if (speed < -accel)
            {
                speed += (accel * 2);
            }
            if (speed < 0.000001)
            {
                speed = 0;
            }
        }

        transform.Rotate(0, rotateValue, 0);

        moveDirection = transform.forward * speed;

        YVel -= gravity * Time.deltaTime;
        moveDirection.y = YVel;

        controller.Move(moveDirection * Time.deltaTime);
    }
}
