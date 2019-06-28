using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MajorVRProj
{
    //Handles transitioning between light and dark mode
    public class CorruptTransitioner : MonoBehaviour
    { 
        [SerializeField] TransitionLight transitionLight;

        [SerializeField] List<LightSettings> lightSettings = new List<LightSettings>();
        int currentLSIdx = 0;   //Current light setting index


        IInput input;

        void Awake()
        {
            input = GetComponent<IInput>();

            //Make sure there is atleast one light setting
            if (lightSettings.Count <= 0)
                throw new System.NotImplementedException();
        }

        void Start()
        {
            //Init light with first light settings
            transitionLight.Transition(lightSettings[0]);
        }

        void Update()
        {
            HandleTransition();
        }

        void HandleTransition()      
        {
            if (input.useDown)
            {
                currentLSIdx++;
                if (currentLSIdx >= lightSettings.Count)
                {
                    currentLSIdx = 0;
                }
                transitionLight.Transition(lightSettings[currentLSIdx]);
            }
        }
    }
}
