using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EpilogueSceneLoad : MonoBehaviour
{
	public PuzzleComplete puzzle1Complete;
	public PuzzleComplete puzzle2Complete;
	public PuzzleComplete puzzle3Complete;
	public PuzzleComplete puzzle4Complete;

	public MeshRenderer portalMesh;

	public Renderer playerFadeEffect;
	bool sceneLoadEnabled;

	public float transitionTime = 1.5f;
	float progress;
    bool loadStarted;

	private void Start()
	{
        StartCoroutine(SetPlayerFade());

		portalMesh.enabled = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Lylah")
		{
			if (puzzle1Complete.m_puzzleComplete == true || puzzle2Complete.m_puzzleComplete == true || puzzle3Complete.m_puzzleComplete == true || puzzle4Complete.m_puzzleComplete == true)
			{
				sceneLoadEnabled = true;
			}
		}


	}

	private void Update()
	{
		//when triggered start anim, once anim complete load scene, then un anim once loaded
		if (portalMesh.enabled == false)
		{
			if (puzzle1Complete.m_puzzleComplete == true || puzzle2Complete.m_puzzleComplete == true || puzzle3Complete.m_puzzleComplete == true || puzzle4Complete.m_puzzleComplete == true)
			{
				portalMesh.enabled = true;
			}
		}


			if (sceneLoadEnabled == true)
		{
			progress -= Time.deltaTime / transitionTime;
		}
		else
		{
			progress += Time.deltaTime / transitionTime;
		}

		if(progress > 1)
		{
			progress = 1;
		}
		else if(progress < 0)
		{
			progress = 0;
		}

        //if (playerFadeEffect != null)
		playerFadeEffect.material.SetFloat("_DissolveSlider", progress);

		if (progress == 0.0f)
		{
            if (loadStarted == false)
            {
                SceneManager.LoadSceneAsync(1);
                loadStarted = true;
            }
		}

	}

    IEnumerator SetPlayerFade()
    {
        while (playerFadeEffect == null)
        {
            playerFadeEffect = GameObject.Find("SceneFadeEffect").GetComponent<Renderer>();

            yield return new WaitForSeconds(5.0f);
        }

    }
}
