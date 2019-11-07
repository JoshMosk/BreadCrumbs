using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area1ChildCorrect : MonoBehaviour
{
	[SerializeField]
	bool childCorrect;

	public float transitionTime = 0.5f;
	float progress;

	Material mat;

    // Start is called before the first frame update
    void Start()
    {
		mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if(childCorrect)
		{
			progress += Time.deltaTime / transitionTime;
		}
		else
		{
			progress -= Time.deltaTime / transitionTime;
		}

		if(progress > 1.0f)
		{
			progress = 1.0f;
		}
		else if(progress < 0.0f)
		{
			progress = 0.0f;
		}

		mat.SetFloat("_DissolveSlider" ,Mathf.Lerp(0, 1, progress));
    }

	public void ChildCorrect()
	{
		childCorrect = true;
	}

	public void ChildIncorrect()
	{
		childCorrect = false;
	}
}
