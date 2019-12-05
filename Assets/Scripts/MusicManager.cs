using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	public AudioSource uncorruptSource;
	public AudioSource corruptSource;

	CorruptPlayer isCorrupt;

	public float transitionTime = 1f;

	public float uncorruptVolume;
	public float corruptVolume;

	float progress;

	private void Start()
	{
		isCorrupt = FindObjectOfType<CorruptPlayer>();
	}

	private void Update()
	{
		if(isCorrupt.m_isCorrupt)
		{
			progress += Time.deltaTime / transitionTime;
		}
		else
		{
			progress -= Time.deltaTime / transitionTime;
		}

		if(progress > 1)
		{
			progress = 1;
		}
		else if(progress < 0)
		{
			progress = 0;
		}

		uncorruptSource.volume = Mathf.Lerp(uncorruptVolume, 0f, progress);
		corruptSource.volume = Mathf.Lerp(0f, corruptVolume, progress);
	}
}
