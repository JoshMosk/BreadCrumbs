using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMovement : MonoBehaviour
{
    IInput m_input;

    public Transform m_dragPoint;
    Vector3 m_prevPoint;        //last frames point
    public float m_sensitivity = 30;

    void Start()
    {
        m_input = GetComponent<IInput>();

    }

    void Update()
    {
        if(m_input.dragMovement)
        {
            //Vector3 newPos = new Vector3( m_prevPoint.x - m_dragPoint.position.x, m_dragPoint.position.y, m_prevPoint.y - m_dragPoint.position.z );
            //transform.position = newPos;
            Vector3 pointDiff = m_dragPoint.localPosition - m_prevPoint;

            pointDiff.y = 0;

            transform.position -= pointDiff * m_sensitivity;
            //transform.position = m_dragPoint.localPosition;

            //Debug.Log(m_dragPoint.localPosition);
            //Debug.Log(m_dragPoint.position);

            //Debug.Log(pointDiff * 1000);
        }

        m_prevPoint = m_dragPoint.localPosition;
    }
}
