using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MajorVRProj
{
    //this script goes on the building and turns it transparent
    public class CorruptBuilding : MonoBehaviour
    {
        /*How a building transitions to corrupt
        1. Alphas to zero
        2. Colliders get disabled
        */
        [SerializeField] float transitionTime = 0.5f;

        Material[] materials;
        [SerializeField]
        Renderer[] renderers;
        MeshRenderer[] mr;
        Collider col;   //Should only need one collider
        void Awake()
        {
            //materials = GetComponentsInChildren<Material>();
            renderers = GetComponentsInChildren<Renderer>();
            mr = GetComponentsInChildren<MeshRenderer>();
            col = GetComponentInChildren<Collider>();
        }

        public void TransitionIn()      //transition into corrupt
        {
            StartCoroutine(TransitionRoutine(true));
            //foreach (var o in mr)
            //{
            //    o.enabled = false;
            //}
            //col.enabled = false;
        }

        public void TransitionOut()     //transition into normal
        {
            StartCoroutine(TransitionRoutine(false));
            //foreach (var o in mr)
            //{
            //    o.enabled = true;
            //}
            //col.enabled = true;
        }

        //Transition coroutine
        //"forward" = true means it transition TO the corrupt. False means it transitions TO normal
        IEnumerator TransitionRoutine(bool direction)
        {
            //Modify the alphas
            for (float t = 0f; t < transitionTime; t += Time.deltaTime)
            {
                
                if (direction)
                {
                    foreach(Renderer r in renderers)
                    {
                        r.material.SetFloat("_DissolveSlider", transitionTime - t * 2.1f);
                    }

                    //foreach (Material m in materials)
                    //{
                    //    m.SetFloat("_DissolveSlider", t);
                    //}
                    //light.colorTemperature = Mathf.Lerp(light.colorTemperature, newSettings.colorTemperature, t / lerpTime);
                    //light.color = Color.Lerp(light.color, newSettings.color, t / transitionTime);
                    //light.intensity = Mathf.Lerp(light.intensity, newSettings.intensity, t / transitionTime);
                }
                else
                {
                    foreach(Renderer r in renderers)
                    {
                        r.material.SetFloat("_DissolveSlider", t * 2.1f);
                    }
                }
                yield return null;
            }
        }

    }
}
