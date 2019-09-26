using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LylahTransition : MonoBehaviour
{
	Renderer rend;
	CorruptPlayer corrupt;

	public float m_transitionTime = 1.0f;
	float progress;		//corrupt == 1

    // Start is called before the first frame update
    void Start()
    {
		corrupt = FindObjectOfType<CorruptPlayer>();
		rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(corrupt.m_isCorrupt)
		{
			progress += Time.deltaTime / m_transitionTime;
		}
		else
		{
			progress -= Time.deltaTime / m_transitionTime;
		}

		if(progress < 0)
		{
			progress = 0;
		}
		else if(progress > 1)
		{
			progress = 1;
		}

		rend.material.SetFloat("_DissolveSlider", progress);
    }
}
