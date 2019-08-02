using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMovement : MonoBehaviour
{
    IInput m_input;

    Transform m_dragPoint;
    Vector2 m_prevPoint;        //last frames point

    void Start()
    {
        m_input = GetComponent<IInput>();
    }

    void Update()
    {
        if(m_input.dragMovement)
        {
            Vector3 newPos = new Vector3( m_prevPoint.x - m_dragPoint.position.x, m_dragPoint.position.y, m_prevPoint.y - m_dragPoint.position.z );

            transform.position = newPos;
        }
        m_prevPoint = m_dragPoint.position;
    }
}
