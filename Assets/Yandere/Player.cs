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
    public const float MAX_DISTANCE = 1000.0f;
    public GameObject lightPrefab;
    private GameObject currentLight;
    private YLook yLook;

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        yLook = GetComponent<YLook>();
    }

    bool ShootBeam(Vector3 fromPosition, Vector3 direction, int depth = 0) {
        if (currentLight != null) {
            currentLight.GetComponent<LightMesh>().Kill();
            currentLight = null;
        }
        if (depth > 20) {
            return false;
        }
        RaycastHit hit;
        // exclude player and ground
        int mask = ~(1 << 9 | 1 << 10);
        if (Physics.Raycast(fromPosition, direction, out hit, MAX_DISTANCE, mask)) {
            // Vector3 hitPoint = direction * hit.distance;
            GameObject newLaser = (GameObject) Instantiate(lightPrefab, fromPosition, Quaternion.LookRotation(direction));
            float hitDistance = hit.distance * direction.magnitude;
            newLaser.GetComponent<LightMesh>().length = hitDistance;
            currentLight = newLaser;
            // TODO: hit mirror
            return true;
        } else {
            return false;
        }
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
        if (Input.GetButtonDown("Fire1")) {
            ShootBeam(transform.position, transform.localRotation * yLook.rotation * Vector3.forward);
        }
    }
}
