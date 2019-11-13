using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxTransition : MonoBehaviour
{
	//need to grab the two skyboxes then lerp thing
	public Material unCorruptSkybox;
	public Material corruptSkybox;

	public Color colBright;
	public Color colDark;

	public float transitionTime = 0.5f;
	[SerializeField]float progress;

	CorruptPlayer isCorrupt;

	private void Start()
	{
		isCorrupt = FindObjectOfType<CorruptPlayer>();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.T))
		{
			RenderSettings.skybox = unCorruptSkybox;
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
			RenderSettings.skybox = corruptSkybox;
		}

		if (isCorrupt.m_isCorrupt == true)
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

		//JM:StartHere, need to have colour lerp to colDark, swap tex, colour lerp to colBright 

		//RenderSettings.skybox.color = 
	}

}
