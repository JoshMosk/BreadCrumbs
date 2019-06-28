using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MajorVRProj
{
    public class CorruptBuilding : MonoBehaviour
    {
        /*How a building transitions to corrupt
        1. Alphas to zero
        2. Colliders get disabled
        */
        [SerializeField] float transitionTime = 0.5f;

        Material[] materials;
        MeshRenderer[] mr;
        Collider col;   //Should only need one collider
        void Awake()
        {
            // materials = GetComponentsInChildren<Material>();
            mr = GetComponentsInChildren<MeshRenderer>();
            col = GetComponentInChildren<Collider>();
        }

        public void TransitionIn()
        {
            // StartCoroutine(TransitionRoutine(true));
            foreach (var o in mr)
            {
                o.enabled = false;
            }
            col.enabled = false;
        }

        public void TransitionOut()
        {
            // StartCoroutine(TransitionRoutine(false));
            foreach (var o in mr)
            {
                o.enabled = true;
            }
            col.enabled = true;
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
                    // materials.SetFloat("")
                    // //light.colorTemperature = Mathf.Lerp(light.colorTemperature, newSettings.colorTemperature, t / lerpTime);
                    // light.color = Color.Lerp(light.color, newSettings.color, t / transitionTime);
                    // light.intensity = Mathf.Lerp(light.intensity, newSettings.intensity, t / transitionTime);
                }
                else
                {

                }
                yield return null;
            }
        }

    }
}
