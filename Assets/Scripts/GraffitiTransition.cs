using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraffitiTransition : MonoBehaviour
{
	[SerializeField] float transitionTime = 1f;

	float progress;		//1 == corrupt

	Renderer rend;

	CorruptPlayer corrupt;

	//use dissolve slider

	// Start is called before the first frame update
	void Start()
    {
		rend = GetComponent<Renderer>();

		corrupt = GameObject.FindObjectOfType<CorruptPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(corrupt.m_isCorrupt)
		{
			progress += Time.deltaTime / transitionTime;
			if (progress < 0)
				progress = 0;
		}
		else
		{
			progress -= Time.deltaTime / transitionTime;
			if (progress < 1)
				progress = 1;
		}

		rend.material.SetFloat("_DissolveSlider", progress);
	}
}
