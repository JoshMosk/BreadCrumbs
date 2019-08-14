using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRotate : MonoBehaviour
{
    public Camera m_mainCamera;
    public float m_lerpSpeed = 0.2f;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, m_mainCamera.transform.rotation.eulerAngles.y, 0), m_lerpSpeed);
        transform.position = Vector3.Lerp(transform.position, m_mainCamera.transform.position, m_lerpSpeed);
    }
}
