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

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    bool ShootBeam(Vector3 fromPosition, Vector3 direction, int depth = 0) {
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
            Mesh mesh = newLaser.GetComponent<MeshFilter>().mesh;
            mesh.Clear();
            Vector3[] verts = new Vector3[] {
                new Vector3(-0.5f, 0.0f, 0.0f),
                new Vector3(-0.5f, 0.0f, hitDistance),
                new Vector3( 0.5f, 0.0f, 0.0f),
                new Vector3( 0.5f, 0.0f, hitDistance),
                new Vector3(-0.5f, 0.0f, 0.0f),
                new Vector3(-0.5f, 0.0f, hitDistance),
                new Vector3( 0.5f, 0.0f, 0.0f),
                new Vector3( 0.5f, 0.0f, hitDistance),
            };
            Vector2[] uvs = new Vector2[] {
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, hitDistance),
                new Vector2(1.0f, 0.0f),
                new Vector2(1.0f, hitDistance),
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, hitDistance),
                new Vector2(1.0f, 0.0f),
                new Vector2(1.0f, hitDistance),
            };
            Vector3[] norms = new Vector3[] {
                Vector3.up, Vector3.up, Vector3.up, Vector3.up,
                Vector3.down, Vector3.down, Vector3.down, Vector3.down,
            };
            int[] tris = new int[] {
                0, 1, 2, 3, 2, 1,
                6, 5, 4, 5, 6, 7,
            };
            mesh.vertices  = verts;
            mesh.uv        = uvs;
            mesh.normals   = norms;
            mesh.triangles = tris;
            newLaser.GetComponent<MeshCollider>().sharedMesh = mesh;
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
            // TODO: look forwards
            ShootBeam(transform.position, transform.localRotation * Vector3.forward);
        }
    }
}
