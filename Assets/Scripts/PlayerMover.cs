using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMover : MonoBehaviour
    {
		public static PlayerMover instance;
		void Awake()
		{
			if (instance != null) return;
			instance = this;
		}

		[SerializeField]
        IInput input;
        NavMeshAgent agent;

        public Transform m_rayPoint;
		public string m_rayPointName = "JNT_VRHands_IndexFinger_004_R";
        public LineRenderer m_line;
        public GameObject meshLylah;
        public float speed = 1f;
        public float lookspeed = 1f;

		public GameObject target;
		public HandAnimController HandAnim;

        void Start()
        {
            input = GetComponent<IInput>();
            agent = GetComponent<NavMeshAgent>();

			if(m_line != null)
			{
				m_line.enabled = false;
			}
        }

        void Update()
        {
			if(DialogueManager.instance.inConversation == false)		//if in convo dont fucking move
			{
				RayCastMove();
			}

            

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

			//Debug.Log(agent.velocity.magnitude);
			if (agent.velocity.magnitude == 0)
			{
				target.SetActive(false);
			}
            //TODO:find out where the corners end, does it include player pos and does it include the dest, does it update or 
            //do i have to force it using agent.CalculatePath?
            else
            {
                Vector3 startPoint;
                Vector3 endPoint;

                Debug.DrawLine(agent.transform.position, agent.path.corners[0], Color.blue);

                for (int i = 0; i < agent.path.corners.Length - 1; i++)
                {
                    startPoint = agent.path.corners[i];
                    endPoint = agent.path.corners[i + 1];

                    Debug.DrawLine(startPoint, endPoint, Color.red);
                }

                Debug.DrawLine(agent.pathEndPosition, agent.destination, Color.blue);
                
            }
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
				//Debug.Log("heyoooooo");
				Debug.DrawRay(ray.origin, ray.GetPoint(300));

                if(Physics.Raycast(ray, out RaycastHit hit, (float)500.0f))
                {
					if (HandAnim.pointProgress > 0.95f)		//point only when pointer finger has extended
					{
						agent.SetDestination(hit.point);		//set target destination

						m_line.SetPosition(0, m_rayPoint.position);		//set line rend
						m_line.SetPosition(1, hit.point);		//set line rend
						m_line.enabled = true;      //enable line rend

                        target.transform.position = agent.destination;//hit.point;		//set target visual pos
						target.SetActive(true);		//enable target visual
					}
                }

            }
			if(input.pointAndClickUp)
			{
				m_line.enabled = false;
			}
        }
    }
