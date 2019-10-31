using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DebugBounds))]
public class DragMovement : MonoBehaviour
{
    IInput m_input;

    public Transform m_dragPoint;
    public Vector3 m_prevPoint;
    public float m_sensitivity = 30;

    public Vector2 m_maxBounds = new Vector2(100, 100);
    public Vector2 m_minBounds = new Vector2(-100, -100);
    Vector3 m_prevPos;

    void Start()
    {
        m_input = GetComponent<IInput>();

    }

    void Update()
    {
        Vector3 pos = transform.position;


        if (m_input.dragMovement)
        {
            Vector3 pointDiff = m_dragPoint.localPosition - m_prevPoint;

            pointDiff.y = 0;

            transform.position -= pointDiff * m_sensitivity;
        }

        //keeps player within world bounds
        if (transform.position.x > m_maxBounds.x)
        {
            transform.position = new Vector3(m_prevPos.x, pos.y, pos.z);
        }
        if (transform.position.x < m_minBounds.x)
        {
            transform.position = new Vector3(m_prevPos.x, pos.y, pos.z);
        }
        if (transform.position.z > m_maxBounds.y)
        {
            transform.position = new Vector3(pos.x, pos.y, m_prevPos.z);
        }
        if (transform.position.z < m_minBounds.y)
        {
            transform.position = new Vector3(pos.x, pos.y, m_prevPos.z);
        }

        m_prevPoint = m_dragPoint.localPosition;
        m_prevPos = transform.position;
    }
}
