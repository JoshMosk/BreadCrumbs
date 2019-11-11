using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadInMesh : MonoBehaviour
{

    Renderer mat;

    public float transitionTime = 0.2f;
    float progress;

    bool inWall;

    private void OnTriggerEnter(Collider other)
    {
        inWall = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inWall = false;
    }

    private void Start()
    {
        mat = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (inWall)
        {
            progress += Time.deltaTime / transitionTime;
        }
        else
        {
            progress -= Time.deltaTime / transitionTime;
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
