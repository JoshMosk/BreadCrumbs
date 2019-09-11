using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTransition : MonoBehaviour
{

	public float m_unCorruptIntensity;
	public float m_corruptIntensity;

	public float m_transitionTime;
	float m_progress;       //progress is 0 at  full uncorrupt, 1 at full corrupt
	float m_unprogress = 1;

	public bool m_isCorrupt;


	Light m_light;

	private void Start()
	{
		m_light = GetComponent<Light>();
	}

	private void Update()
	{
		StartTransition();
	}

	public void StartTransition()
	{

		if(m_isCorrupt)
		{
			m_progress += Time.deltaTime / m_transitionTime;
			m_unprogress -= Time.deltaTime / m_transitionTime;

			m_light.intensity = Mathf.Lerp(m_light.intensity, m_corruptIntensity, m_progress);
		}
		else
		{
			m_unprogress += Time.deltaTime / m_transitionTime;
			m_progress -= Time.deltaTime / m_transitionTime;

			m_light.intensity = Mathf.Lerp(m_light.intensity, m_unCorruptIntensity, m_unprogress);
		}

		if(m_progress > 1)
		{
			m_progress = 1;
			m_unprogress = 0;
		}
		else if(m_progress < 0)
		{
			m_progress = 0;
			m_unprogress = 1;
		}
	}
}
