using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private CharacterController controller;
    private Animator animator;
    private float xRotation;
    public float Speed;
    public float FowardMultiplier = 2.0f;
    public float mouseXSensitivity;

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
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
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
