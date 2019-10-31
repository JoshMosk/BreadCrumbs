using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DebugBounds : MonoBehaviour
{
    DragMovement drag;
    // Start is called before the first frame update
    void Start()
    {
        drag = GetComponent<DragMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(new Vector3(drag.m_minBounds.x, 10, drag.m_maxBounds.y), new Vector3(drag.m_maxBounds.x, 10, drag.m_maxBounds.y));//line from top left to top right
        Debug.DrawLine(new Vector3(drag.m_maxBounds.x, 10, drag.m_maxBounds.y), new Vector3(drag.m_maxBounds.x, 10, drag.m_minBounds.y));//line from top left to bottom right
        Debug.DrawLine(new Vector3(drag.m_maxBounds.x, 10, drag.m_minBounds.y), new Vector3(drag.m_minBounds.x, 10, drag.m_minBounds.y));//line from bottom right to bottom left
        Debug.DrawLine(new Vector3(drag.m_minBounds.x, 10, drag.m_minBounds.y), new Vector3(drag.m_minBounds.x, 10, drag.m_maxBounds.y));//line from bottom left to top left
    }
}
