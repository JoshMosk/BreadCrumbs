using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Events;
namespace MajorVRProj
{
    //Handles transitioning between light and dark mode
    public class CorruptTransitioner : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] bool debug = false;
        [SerializeField] List<LightSettings> dbLightSettings;
        int currentLSIdx = 0;   //Current light setting index

        [Header("The Corrupt")]
        [SerializeField] TransitionLight transitionLight;
        [SerializeField] LightSettings lightSettings;
        [SerializeField] LightSettings darkSettings;
        [SerializeField] NavMeshSurface navmesh;
        bool isCorrupted = false;

        [SerializeField] UnityEvent OnCorrupted, OnNormal;

        IInput input;

        void Awake()
        {
            input = GetComponent<IInput>();

            //Make sure there is atleast one light setting
            if (dbLightSettings.Count <= 0)
                throw new System.NotImplementedException();
        }

        void Start()
        {
            //Linear intensity must be active if you're modifying light color temps
            // GraphicsSettings.lightsUseLinearIntensity = true;
            // GraphicsSettings.lightsUseColorTemperature = true;

            //Init light with first light settings
            transitionLight.Transition(dbLightSettings[0]);

            OnCorrupted.AddListener(RebakeNavMesh);
            OnNormal.AddListener(RebakeNavMesh);
        }

        void Update()
        {
            if (input.useDown)
            {
                if (debug) {
                    RunDebug(); 
                    return; }

                ToggleCorruption();
                HandleLightTransitions();
            }
        }

        void RebakeNavMesh()
        {
            navmesh.BuildNavMesh();
        }

        void ToggleCorruption()
        {
            isCorrupted = !isCorrupted;
            if (isCorrupted)
                OnCorrupted.Invoke();
            else
                OnNormal.Invoke();
        }

        void HandleLightTransitions()
        {
            if (isCorrupted)
                transitionLight.Transition(darkSettings);
            else
                transitionLight.Transition(lightSettings);
        }

        //-------------- DEBUG --------------------
        void RunDebug()
        {
            currentLSIdx = (currentLSIdx < dbLightSettings.Count - 1) ? ++currentLSIdx : currentLSIdx = 0;    //i++ is different from ++i!!!
            transitionLight.Transition(dbLightSettings[currentLSIdx]);
        }
    }
}
