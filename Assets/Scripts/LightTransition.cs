using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTransition : MonoBehaviour
{
    public Color m_uncorruptCol;
    public Color m_corruptCol;

    public float m_uncorruptIntensity;
	public float m_corruptIntensity;

	public float m_transitionTime;


	float m_progress;       //progress is 0 at  full uncorrupt, 1 at full corrupt

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
		}
		else
		{
			m_progress -= Time.deltaTime / m_transitionTime;
		}

        if (m_progress > 1)
		{
			m_progress = 1;
		}
		else if(m_progress < 0)
		{
			m_progress = 0;
		}

        m_light.color = Color.Lerp(m_uncorruptCol, m_corruptCol, m_progress);
        m_light.intensity = Mathf.Lerp(m_uncorruptIntensity, m_corruptIntensity, m_progress);
    }
}
