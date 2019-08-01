using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace MajorVRProj
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField]
        IInput input;
        NavMeshAgent agent;
        Camera cam;
        public Transform m_rayPoint;
        LineRenderer m_line;

        void Start()
        {
            input = GetComponent<IInput>();
            agent = GetComponent<NavMeshAgent>();
            cam = FindObjectOfType<Camera>();
            m_line = GetComponent<LineRenderer>();
        }

        void Update()
        {
            RayCastMove();
        }

        void RayCastMove()
        {
            //Move player to clicked position
            if (input.pointAndClick)
            {
                //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                //if (Physics.Raycast(ray, out RaycastHit hit))
                //{
                //    agent.SetDestination(hit.point);
                //}

                Ray ray = new Ray(m_rayPoint.position, m_rayPoint.forward);
                    Debug.Log("heyoooooo");
                    Debug.DrawRay(ray.origin, ray.GetPoint(300));

                if(Physics.Raycast(ray, out RaycastHit hit, 500.0f))
                {
                    agent.SetDestination(hit.point);
                    m_line.SetPosition(0, m_rayPoint.position);
                    m_line.SetPosition(1, hit.point);
                    
                }

            }
        }
    }
}
