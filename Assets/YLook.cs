using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YLook : MonoBehaviour {
    public float yRotation = 0.0f;
    public float sensitivity = 300.0f;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        yRotation += Time.deltaTime * sensitivity * Input.GetAxis("Mouse Y");
        if (yRotation < -90.0f) { yRotation = -90.0f; }
        if (yRotation >  90.0f) { yRotation =  90.0f; }
    }

    public Quaternion rotation {
        get {
            return Quaternion.AngleAxis(yRotation, Vector3.left);
        }
    }
}
