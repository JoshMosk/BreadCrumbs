using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadInMesh : MonoBehaviour
{
    
    public Renderer mat;

    public float transitionTime = 0.2f;
    float progress = 1.0f;

    bool inWall;

	private void OnCollisionEnter(Collision collision)
	{
		inWall = true;
	}

	private void OnCollisionExit(Collision collision)
	{
		inWall = false;
	}

	//private void OnTriggerEnter(Collider other)
 //   {
 //       inWall = true;
 //   }

 //   private void OnTriggerExit(Collider other)
 //   {
 //       inWall = false;
 //   }

    private void Start()
    {
        if (mat == null)
            mat = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (inWall)
        {
            progress -= Time.deltaTime / transitionTime;
        }
        else
        {
            progress += Time.deltaTime / transitionTime;
        }

        if(progress > 1.0f)
        {
            progress = 1;
        }
        else if(progress < 0.0f)
        {
            progress = 0;
        }

        mat.material.SetFloat("_DissolveSlider", progress);
    }
}
