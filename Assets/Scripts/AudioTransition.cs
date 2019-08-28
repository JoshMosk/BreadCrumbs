using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTransition : MonoBehaviour
{

	public float m_unCorruptVolume;
	public float m_corruptVolume;

	public float m_transitionTime;
	float m_progress;       //progress is 0 at  full uncorrupt, 1 at full corrupt

	public bool m_isCorrupt;

	AudioSource m_aud;

	// Start is called before the first frame update
	void Start()
    {
		m_aud = GetComponent<AudioSource>();
    }

	public void StartTransition()
	{
		if (m_isCorrupt)
		{
			m_progress += Time.deltaTime / m_transitionTime;

			m_aud.volume = Mathf.Lerp(m_aud.volume, m_corruptVolume, m_progress);
			
		}
		else
		{
			m_progress -= Time.deltaTime / m_transitionTime;
			m_aud.volume = Mathf.Lerp(m_aud.volume, m_unCorruptVolume, m_progress);
			
		}
	}
}
