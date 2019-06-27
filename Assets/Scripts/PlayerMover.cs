using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace MajorVRProj
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMover : MonoBehaviour
    {
        IInput input;
        NavMeshAgent agent;
        Camera cam;

        void Start()
        {
            input = GetComponent<IInput>();
            agent = GetComponent<NavMeshAgent>();
            cam = FindObjectOfType<Camera>();
        }

        void Update()
        {
            RayCastMove();
        }

        void RayCastMove()
        {
            //Move player to clicked position
            if (input.clicked)
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    agent.SetDestination(hit.point);
                }
            }
        }
    }
}
