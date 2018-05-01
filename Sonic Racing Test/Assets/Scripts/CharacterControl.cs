using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    private float gravity;
    private float YVel;
    private float jumpSpeed;
    private bool canDoubleJump;

    private Vector3 moveDirection = Vector3.zero;

    private float speed;
    private float maxSpeed;
    private float accel;
    [SerializeField]
    private float rotateValue;
    private float rotAdd;

    CharacterController controller;
    AudioSource jumpSound;
    AudioSource skidSound;

    Animator anim;

    void Start ()
    {
        gravity = 30;
        jumpSpeed = 15;

        speed = 0;
        maxSpeed = 30;
        accel = 0.1f;

        AudioSource[] sounds = GetComponents<AudioSource>();
        jumpSound = sounds[0];
        skidSound = sounds[1];
        anim = transform.Find("SonicModel").GetComponent<Animator>();
        //Gets the CharacterController component
        controller = GetComponent<CharacterController>();
    }

    /*This is wrapped around every input to allow for 2 player input
        if (name == "P1Player" || name == "SinglePlayer")
        {

        }
        else if (name == "P2Player")
        {

        }
    */
	void Update ()
    {
        //Accelerates if the accelerate button is pressed
        if (name == "P1Player" || name == "SinglePlayer")
        {
            if (Input.GetButton("P1 Accel"))
            {
                speed += accel;
            }
        }
        else if (name == "P2Player")
        {
            if (Input.GetButton("P2 Accel"))
            {
                speed += accel;
            }
        }

        //Clamps the speed so you can't go indefinitely fast
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);

        //Adds extra steering when going faster
        if (speed > 10)
        {
            rotAdd = (speed / 2);
            rotAdd = Mathf.Clamp(rotAdd, 5, 15);
        }
        else if (speed > 1)
        {
            rotAdd = 5;
        }
        else
        {
            rotAdd = 0;
        }

        //Gets input from the horizontal axis to steer
        if (name == "P1Player" || name == "SinglePlayer")
        {
            rotateValue = Input.GetAxis("P1 Horizontal") * (Time.deltaTime * 25 + (rotAdd / 35));
        }
        else if (name == "P2Player")
        {
            rotateValue = Input.GetAxis("P2 Horizontal") * (Time.deltaTime * 25 + (rotAdd / 35));
        }

        //Gets and handles the leaning inputs
        if (name == "P1Player" || name == "SinglePlayer")
        {
            if (Input.GetButton("P1 Lean Left") && (Input.GetAxis("P1 Horizontal") <= 0) && (controller.isGrounded))
            {
                rotateValue -= ((rotAdd / 50) + 0.5f);
            }
            else if (Input.GetButton("P1 Lean Right") && (Input.GetAxis("P1 Horizontal") >= 0) && (controller.isGrounded))
            {
                rotateValue += ((rotAdd / 50) + 0.5f);
            }
        }
        else if (name == "P2Player")
        {
            if (Input.GetButton("P2 Lean Left") && (Input.GetAxis("P2 Horizontal") <= 0) && (controller.isGrounded))
            {
                rotateValue -= ((rotAdd / 50) + 0.5f);
            }
            else if (Input.GetButton("P2 Lean Right") && (Input.GetAxis("P2 Horizontal") >= 0) && (controller.isGrounded))
            {
                rotateValue += ((rotAdd / 50) + 0.5f);
            }
        }
        
        //Plays the skid sound if the turning arc is tight enough
        if (((rotateValue > 1.5) || (rotateValue < -1.5)) && (controller.isGrounded))
        {
            skidSound.mute = false;
        }
        if (!controller.isGrounded || ((rotateValue > -1.5) && (rotateValue < 1.5)))
        {
            skidSound.mute = true;
        }

        //Resets the speed to 0 when hitting a wall
        if (Physics.Raycast(transform.position, transform.forward, 1))
        {
            speed = 0;
        }

        //Allows the player to jump
        if (controller.isGrounded)
        {
            YVel = 0;
            canDoubleJump = true;
            anim.SetBool("jump", false);
            if (name == "P1Player" || name == "SinglePlayer")
            {
                if (Input.GetButtonDown("P1 Jump"))
                {
                    YVel = jumpSpeed;
                    jumpSound.Play();
                    anim.SetBool("jump", true);
                    canDoubleJump = true;
                }
            }
            else if (name == "P2Player")
            {
                if (Input.GetButtonDown("P2 Jump"))
                {
                    YVel = jumpSpeed;
                    jumpSound.Play();
                    anim.SetBool("jump", true);
                    canDoubleJump = true;
                }
            }    
        }
        //handles double jumping
        if ((canDoubleJump) && (!controller.isGrounded))
        {
            if (name == "P1Player" || name == "SinglePlayer")
            {
                if (Input.GetButtonDown("P1 Jump"))
                {
                    YVel = jumpSpeed;
                    jumpSound.Play();
                    anim.SetBool("jump", true);
                    canDoubleJump = false;
                }
            }
            else if (name == "P2Player")
            {
                if (Input.GetButtonDown("P2 Jump"))
                {
                    YVel = jumpSpeed;
                    jumpSound.Play();
                    anim.SetBool("jump", true);
                    canDoubleJump = false;
                }
            }
        }
        //Decelerates when the button is not held
        if (name == "P1Player" || name == "SinglePlayer")
        {
            if (!Input.GetButton("P1 Accel"))
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
        }
        else if (name == "P2Player")
        {
            if (!Input.GetButton("P2 Accel"))
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
        }
        

        //Sets speed parameter for Sonic's animation
        anim.SetFloat("speed", speed);

        //Rotates the player itself based on the rotation input and calculated
        transform.Rotate(0, rotateValue, 0);

        //Sets the move direction
        moveDirection = transform.forward * speed;

        //Handles gravity
        YVel -= gravity * Time.deltaTime;
        moveDirection.y = YVel;

        //Moves the player forward based on the speed
        controller.Move(moveDirection * Time.deltaTime);
    }
}
