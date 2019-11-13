using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxTransition : MonoBehaviour
{
	//need to grab the two skyboxes then lerp thing
	Material unCorruptSkybox;
	Material corruptSkybox;

	public float transitionTime = 0.5f;
	float progress;

	CorruptPlayer isCorrupt;

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

		RenderSettings.skybox.Lerp(unCorruptSkybox, corruptSkybox, progress);
	}

}
