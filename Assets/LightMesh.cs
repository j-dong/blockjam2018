using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMesh : MonoBehaviour {
    private Mesh mesh;
    private Material material;
    public float length = 10.0f;
    public float lengthProgress = 0.01f;
    public float opacity = 0.0f;
    public GameObject hitObject { get; private set; }

    public void Hit(GameObject target) { hitObject = target; }

    void RecreateMesh() {
        float len = length * lengthProgress;
        mesh.Clear();
        Vector3[] verts = new Vector3[] {
            new Vector3(-0.5f, 0.0f, 0.0f),
            new Vector3(-0.5f, 0.0f, len),
            new Vector3( 0.5f, 0.0f, 0.0f),
            new Vector3( 0.5f, 0.0f, len),
            new Vector3(-0.5f, 0.0f, 0.0f),
            new Vector3(-0.5f, 0.0f, len),
            new Vector3( 0.5f, 0.0f, 0.0f),
            new Vector3( 0.5f, 0.0f, len),
        };
        Vector2[] uvs = new Vector2[] {
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, len),
            new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, len),
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, len),
            new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, len),
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
    }

    public void Kill() {
        GetComponent<Animator>().SetBool("Dead", true);
    }

    void AnimationFinished() {
        Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        mesh = new Mesh();
        material = GetComponent<Renderer>().material;
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshCollider>().enabled = false;
        RecreateMesh();
        material.SetColor("_EmissionColor", Color.black);
    }

    // Update is called once per frame
    void Update () {
        Vector3[] verts = mesh.vertices;
        Vector2[] uvs   = mesh.uv;
        RecreateMesh();
        material.SetColor("_EmissionColor", new Color(opacity, opacity, opacity, 1.0f));
        GetComponent<MeshCollider>().enabled = opacity > 0.99f;
    }

    void LateUpdate() {
        if (hitObject != null && opacity > 0.99f) {
            LaserFire laser = hitObject.GetComponent<LaserFire>();
            if (laser != null) {
                laser.isFiring = true;
            }
        }
    }
}
