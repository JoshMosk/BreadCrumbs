﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Events;
namespace MajorVRProj
{
    //Handles transitioning between light and dark mode using unity events
    public class CorruptTransitioner : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] bool debug = false;
        [SerializeField] List<LightSettings> dbLightSettings;
        int currentLSIdx = 0;   //Current light setting index

        [Header("The Corrupt")]
        [SerializeField] LightSettings lightSettings;
        [SerializeField] LightSettings darkSettings;
        [SerializeField] NavMeshSurface navmesh;

        [SerializeField] Volume volume;
        [SerializeField] VolumeProfile lightVolume;
        [SerializeField] VolumeProfile darkVolume;

		public NavMeshSurface uncorruptNav;
		public NavMeshSurface corruptNav;

		List<LightTransition> m_transitionLights;

        [SerializeField] bool isCorrupted = false;

        [SerializeField] UnityEvent OnCorrupted, OnNormal;

        IInput input;

        [SerializeField] ParticleSystem cityCorruptParticle;

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


            OnCorrupted.AddListener(RebakeNavMesh);
            OnNormal.AddListener(RebakeNavMesh);

            HandleLightTransitions();
            HandleParticleEffects();

			foreach (GameObject g in GameObject.FindObjectsOfType(typeof(LightTransition)))
				m_transitionLights.Add(g.GetComponent<LightTransition>());
        }

        void Update()
        {
            if (input.swapDimensionDown)
            {
                if (debug)
                {
                    RunDebug();
                    return;
                }

                ToggleCorruption();
                HandleLightTransitions();
                HandleParticleEffects();
                HandleDialogue();
            }
        }

        void RebakeNavMesh()
        {
            //navmesh.BuildNavMesh();
			//need to just have swap between two nav meshes
			if(isCorrupted)
			{
				uncorruptNav.enabled = false;
				corruptNav.enabled = true;
			}
			if(!isCorrupted)
			{
				corruptNav.enabled = false;
				corruptNav.enabled = true;
			}

        }

        void ToggleCorruption()
        {
            isCorrupted = !isCorrupted;
            if (isCorrupted)
            {
                OnCorrupted.Invoke();
            }
            else
            {
                OnNormal.Invoke();
            }
        }

        void HandleLightTransitions()
        {
            if (isCorrupted)
            {
                volume.profile = darkVolume;
				foreach (LightTransition l in m_transitionLights)
					l.m_isCorrupt = true;
            }
            else
            {
                volume.profile = lightVolume;
				foreach (LightTransition l in m_transitionLights)
					l.m_isCorrupt = false;
			}
        }

        void HandleDialogue()
        {
            if (isCorrupted)
            {
                DialogueManager.instance.SetBlackboardVariable("Global", "isCorrupt", true);
            }
            else
            {
                DialogueManager.instance.SetBlackboardVariable("Global", "isCorrupt", false);
            }
        }

        void HandleParticleEffects()
        {
            if (isCorrupted)
            {
                cityCorruptParticle.Play();
            }
            else
            {
                cityCorruptParticle.Stop();
                cityCorruptParticle.Clear();
            }
        }

        //-------------- DEBUG --------------------
        void RunDebug()
        {
            currentLSIdx = (currentLSIdx < dbLightSettings.Count - 1) ? ++currentLSIdx : currentLSIdx = 0;    //i++ is different from ++i!!!
        }
    }
}
