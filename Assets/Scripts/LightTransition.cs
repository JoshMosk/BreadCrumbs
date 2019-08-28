using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTransition : MonoBehaviour
{

	public float m_unCorruptIntensity;
	public float m_corruptIntensity;

	public float m_transitionTime;
	float m_progress;		//progress is 0 at  full uncorrupt, 1 at full corrupt

	public bool m_isCorrupt;


	Light m_light;

	private void Start()
	{
		m_light = GetComponent<Light>();
	}

	public void StartTransition()
	{
		if(m_isCorrupt)
		{
			m_progress += Time.deltaTime / m_transitionTime;

			m_light.intensity = Mathf.Lerp(m_light.intensity, m_corruptIntensity, m_progress);
		}
		else
		{
			m_progress -= Time.deltaTime / m_transitionTime;
			m_light.intensity = Mathf.Lerp(m_light.intensity, m_unCorruptIntensity, m_progress);
		}
	}
}
