using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimController : MonoBehaviour
{
	public Animator m_animator;
	public IInput m_input;

	public float transitionTime = 0.3f;

	float pointProgress;

	private void Update()
	{
		//m_animator.SetTrigger();

		if(m_input.pointAndClick)
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
