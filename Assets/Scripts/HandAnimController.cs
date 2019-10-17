using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class HandAnimController : MonoBehaviour
{
	public Animator m_animator;
	public IInput m_input;

	public SteamVR_Input_Sources m_inputSources;

	public float transitionTime = 0.2f;

	public float pointProgress;

	private void Start()
	{
		m_input = GetComponent<IInput>();

		if (m_inputSources == SteamVR_Input_Sources.RightHand)
		{
			FindObjectOfType<MajorVRProj.PlayerMover>().HandAnim = this;
		}
	}

	private void Update()
	{
		//m_animator.SetTrigger();

		//if(m_input.pointAndClick)
		if (SteamVR_Actions._default.PointAndClick.GetState(m_inputSources) == true)
		{
			pointProgress += Time.deltaTime / transitionTime;
		}
		else
		{
			pointProgress -= Time.deltaTime / transitionTime;
		}

		if(pointProgress < 0)
		{
			pointProgress = 0;
		}
		else if(pointProgress > 1)
		{
			pointProgress = 1;
		}

		m_animator.SetFloat("pointFloat", pointProgress);
	}
}
