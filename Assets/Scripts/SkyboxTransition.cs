using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxTransition : MonoBehaviour
{
	//need to grab the two skyboxes then lerp thing
	public Material uncorruptSkybox;
	public Material corruptSkybox;

	public Color colBright;
	public Color colDark;

	public float transitionTime = 0.5f;

	[SerializeField] float uncorruptProgress;
	[SerializeField] float corruptProgress;     //corrupt progress = 1 when fully corrupt

	CorruptPlayer isCorrupt;

	private void Start()
	{
		isCorrupt = FindObjectOfType<CorruptPlayer>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			RenderSettings.skybox = uncorruptSkybox;
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
			RenderSettings.skybox = corruptSkybox;
		}

		if (isCorrupt.m_isCorrupt == true)
		{
			corruptProgress += Time.deltaTime / transitionTime;
			uncorruptProgress -= Time.deltaTime / transitionTime;
		}
		else
		{
			corruptProgress -= Time.deltaTime / transitionTime;
			uncorruptProgress += Time.deltaTime / transitionTime;
		}

		if (corruptProgress > 1)
		{
			corruptProgress = 1;
			uncorruptProgress = -1;
		}
		else if (corruptProgress < -1)
		{
			corruptProgress = -1;
			uncorruptProgress = 1;
		}

		if (corruptProgress < 0)//if mat is diff and in neg then swap
		{
			if (RenderSettings.skybox != corruptSkybox)
			{
				RenderSettings.skybox = corruptSkybox;
			}
			RenderSettings.skybox.color = Color.Lerp(colBright, colDark, uncorruptProgress);
		}
		else
		{
			if (RenderSettings.skybox != uncorruptSkybox)
			{
				RenderSettings.skybox = uncorruptSkybox;
			}
			RenderSettings.skybox.color = Color.Lerp(colBright, colDark, corruptProgress);
		}
	}


	//RenderSettings.skybox.Lerp(unCorruptSkybox, corruptSkybox, progress);

	//JM:StartHere, need to have colour lerp to colDark, swap tex, colour lerp to colBright 

	//RenderSettings.skybox.color = 
}


