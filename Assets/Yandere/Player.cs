using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private CharacterController controller;
    private Animator animator;
    private float xRotation;
    public float Speed = 1.0f;
    public float FowardMultiplier = 2.0f;
    public float mouseXSensitivity = 500.0f;
    public float jumpVelocity = 10.0f;
    public float gravity = 1.0f;
    private float yVelocity;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update() {
        float speed;
        speed = Vector3.Dot(controller.velocity, transform.localRotation * Vector3.forward);
        animator.SetFloat("Speed", speed);
        bool jump = false;
        if (controller.isGrounded)
        {
            yVelocity = 0.0f;
            if (Input.GetButton("Jump"))
            {
                yVelocity = jumpVelocity;
                jump = true;
            }
        }
        else
        {
            yVelocity -= gravity * Time.deltaTime;
        }
        animator.SetBool("Jump", jump);
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), yVelocity, Input.GetAxis("Vertical"));
        if (move.z > 0) {
            move.Scale(new Vector3(1.0f, 1.0f, FowardMultiplier));
        }
        controller.Move(transform.localRotation * move * Time.deltaTime * Speed);
        xRotation += Input.GetAxis("Mouse X") * mouseXSensitivity * Time.deltaTime;
        if (xRotation < -360.0f) {
            xRotation += 360.0f;
        } else if (xRotation > 360.0f) {
            xRotation -= 360.0f;
        }
        Quaternion q = Quaternion.AngleAxis(xRotation, Vector3.up);
        transform.localRotation = q;
	}
}
