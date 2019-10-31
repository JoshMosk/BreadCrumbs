using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MajorVRProj
{
    public class TransitionLight : MonoBehaviour
    {
        [SerializeField] float transitionTime = 2f;

        new Light light;

        void Awake()
        {
            light = GetComponent<Light>();
        }

        public void Transition(LightSettings newSettings)
        {
            StartCoroutine(TransitionRoutine(newSettings));
        }

        IEnumerator TransitionRoutine(LightSettings newSettings)
        {
            for (float t = 0f; t < transitionTime; t += Time.deltaTime)
            {
                //light.colorTemperature = Mathf.Lerp(light.colorTemperature, newSettings.colorTemperature, t / lerpTime);
                light.color = Color.Lerp(light.color, newSettings.color, t / transitionTime);
                light.intensity = Mathf.Lerp(light.intensity, newSettings.intensity, t / transitionTime);
                yield return null;
            }
        }
    }
}