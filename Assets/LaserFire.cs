using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFire : MonoBehaviour {
    public GameObject lightPrefab;
    public bool manual = false;
    public bool isFiring;
    public GameObject currentLight { get; private set; }
    public Vector3 autoPosition;
    public const float MAX_DISTANCE = 1000.0f;

    // Use this for initialization
    void Start () {

    }

    public bool ShootBeam(Vector3 fromPosition, Vector3 direction, int depth = 0) {
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
            newLaser.GetComponent<LightMesh>().Hit(hit.collider.gameObject);
            currentLight = newLaser;
            // TODO: hit mirror
            return true;
        } else {
            return false;
        }
    }


    // Update is called once per frame
    void Update () {
        if (manual) return;
        if (isFiring && currentLight == null) {
            ShootBeam(transform.TransformPoint(autoPosition), transform.rotation * Vector3.forward);
        } else if (!isFiring && currentLight) {
            currentLight.GetComponent<LightMesh>().Kill();
            currentLight = null;
        }
        if (isFiring) {
            Debug.DrawRay(transform.TransformPoint(autoPosition), transform.rotation * Vector3.forward, Color.red, 0.0f, false);
        }
        isFiring = false;
    }
}
