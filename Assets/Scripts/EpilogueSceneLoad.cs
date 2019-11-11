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

	Renderer playerFadeEffect;
	bool startSceneLoad;

	public float transitionTime = 1.5f;
	float progress;

	private void Start()
	{
        StartCoroutine(SetPlayerFade());
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Lylah")
		{
			if (puzzle1Complete.m_puzzleComplete == true || puzzle2Complete.m_puzzleComplete == true || puzzle3Complete.m_puzzleComplete == true || puzzle4Complete.m_puzzleComplete == true)
			{
				startSceneLoad = true;
			}
		}


	}

	private void Update()
	{
		//when triggered start anim, once anim complete load scene, then un anim once loaded

		if (startSceneLoad == true)
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

        //if (playerFadeEffect != null)
		playerFadeEffect.material.SetFloat("_DissolveSlider", progress);

		if (progress == 1.0f)
		{
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
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
