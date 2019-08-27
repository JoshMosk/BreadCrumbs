using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    public float weight = 1f;
    public Vector3 offset;

    void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position + offset, "vic.png", true);
    }

}
