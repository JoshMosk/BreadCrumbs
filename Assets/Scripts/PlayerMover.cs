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
        public GameObject meshLylah;
        public float speed = 1f;
        public float lookspeed = 1f;

        void Start()
        {
            input = GetComponent<IInput>();
            agent = GetComponent<NavMeshAgent>();
            cam = FindObjectOfType<Camera>();
            m_line = GetComponent<LineRenderer>();

			if(m_line != null)
			{
				m_line.enabled = false;
			}
        }

        void Update()
        {
            RayCastMove();

            

            ////////////////////////////////////// temp code lol
          //  var targetRotation = Quaternion.LookRotation(agent.destination - transform.position);
           // targetRotation.x = 0f;
           // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookspeed * Time.deltaTime);


            float score = meshLylah.GetComponent<Animator>().GetFloat("Walking");
            float scoreTo = agent.velocity.normalized.magnitude;
            score = Mathf.Lerp(score, scoreTo, Time.deltaTime / speed);
            ///////////// max wuz here

            /////////////// yolo


            meshLylah.GetComponent<Animator>().SetFloat("Walking", score);
        }

        void RayCastMove()
        {
			if(input.pointAndClickDown)
			{
				m_line.enabled = true;
			}
            //Move player to clicked position
            if (input.pointAndClick)
            {
                //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                //if (Physics.Raycast(ray, out RaycastHit hit))
                //{
                //    agent.SetDestination(hit.point);
                //}

                Ray ray = new Ray(m_rayPoint.position, m_rayPoint.forward);
                    //Debug.Log("heyoooooo");
                    Debug.DrawRay(ray.origin, ray.GetPoint(300));

                if(Physics.Raycast(ray, out RaycastHit hit, 500.0f))
                {
                    agent.SetDestination(hit.point);
                    m_line.SetPosition(0, m_rayPoint.position);
                    m_line.SetPosition(1, hit.point);
                    
                }

            }
			if(input.pointAndClickUp)
			{
				m_line.enabled = false;
			}
        }
    }
}
