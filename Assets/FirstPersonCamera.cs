using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour {
    public GameObject player;
    public Vector3 offset = new Vector3(0.0f, 1.3f, 0.0f);
    private YLook playerYLook;
    // Use this for initialization
    void Start () {
        playerYLook = player.GetComponent<YLook>();
    }

    // Update is called once per frame
    void Update () {
        transform.localPosition = player.transform.localPosition + offset;
        transform.localRotation = player.transform.localRotation * playerYLook.rotation;
    }
}
