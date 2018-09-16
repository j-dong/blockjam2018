using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour {
    public GameObject player;
    public Vector3 offset = new Vector3(0.0f, 1.3f, 0.0f);
    public float mouseYSensitivity = 500.0f;
    private float yRotation;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = player.transform.localPosition + offset;
        transform.localRotation = player.transform.localRotation * Quaternion.AngleAxis(yRotation, Vector3.left);
        yRotation += Time.deltaTime * mouseYSensitivity * Input.GetAxis("Mouse Y");
        if (yRotation < -90.0f) { yRotation = -90.0f; }
        if (yRotation >  90.0f) { yRotation =  90.0f; }
	}
}
