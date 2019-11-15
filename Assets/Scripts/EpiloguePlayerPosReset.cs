using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpiloguePlayerPosReset : MonoBehaviour
{
	GameObject player;

	Material playerFadeEffect;

	float progress = 1.0f;
	public float transitionTime = 0.5f;

	//reset raypoint in player mover and target in player mover
    void Start()
    {
		player = GameObject.Find("Player");

		playerFadeEffect = GameObject.Find("SceneFadeEffect").GetComponent<Material>();

		FindObjectOfType<LinePointAutoSetter>().DoTheThing();
		HandAnimController[] hand = FindObjectsOfType<HandAnimController>();
		foreach(HandAnimController h in hand)
		{
			h.DoTheThing();
		}

		player.transform.position = this.transform.position;

		DialogueManager.instance.LoadEpilogue();
	}

    void Update()
    {
        if(progress >= 0)
        {
            progress -= Time.deltaTime / transitionTime;
            playerFadeEffect.SetFloat("_DissolveSlider", progress);
        }
    }
}
