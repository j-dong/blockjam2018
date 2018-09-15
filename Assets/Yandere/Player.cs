using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private CharacterController controller;
    private float xRotation;
    public float Speed;
    public float mouseXSensitivity;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(transform.localRotation * move * Time.deltaTime * -1.0f * Speed);
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
