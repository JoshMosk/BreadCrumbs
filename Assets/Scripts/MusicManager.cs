using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	//need two audio source 
	//progress
	//trans time
	//max vol
	//ref to corr


	public AudioSource uncorruptSource;
	public AudioSource corruptSource;

	public float uncorruptVolume;
	public float corruptVolume;

	public float transitionTime = 1f;

	float progress;

	CorruptPlayer isCorrupt;

	private void Start()
	{
		isCorrupt = FindObjectOfType<CorruptPlayer>();
	}

	private void Update()
	{
		//profress
		//if corr
		//check 0-1
		//apply

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

		uncorruptSource.volume = Mathf.Lerp(uncorruptVolume, 0, progress);
		corruptSource.volume = Mathf.Lerp(0, corruptVolume, progress);
	}
}
