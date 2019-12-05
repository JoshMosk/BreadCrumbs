using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;
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
        //[SerializeField] LightSettings lightSettings;
        //[SerializeField] LightSettings darkSettings;
        //[SerializeField] NavMeshSurface navmesh;

        [SerializeField] PostProcessVolume volume;
		[SerializeField]ColorGrading colGrad;
		[Range(-100, 100)]
		[SerializeField] float uncorruptTint;
		[Range(-100, 100)]
		[SerializeField] float corruptTint;
		[Range(-100, 100)]
		[SerializeField] float uncorruptTemperature;
		[Range(-100, 100)]
		[SerializeField] float corruptTemperature;
		[SerializeField] float volTransitionTime = 1f;
		float volProgress;

		//[SerializeField] VolumeProfile lightVolume;
		//[SerializeField] VolumeProfile darkVolume;

		//public NavMeshSurface uncorruptNav;
		//public NavMeshSurface corruptNav;

		//public NavMeshData uncorruptData;
		//public NavMeshData corruptData;

		List<LightTransition> m_transitionLights = new List<LightTransition>();

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
			Application.targetFrameRate = 120;

			//Linear intensity must be active if you're modifying light color temps
			// GraphicsSettings.lightsUseLinearIntensity = true;
			// GraphicsSettings.lightsUseColorTemperature = true;

			m_transitionLights.AddRange(FindObjectsOfType<LightTransition>());

            //OnCorrupted.AddListener(RebakeNavMesh);
            //OnNormal.AddListener(RebakeNavMesh);

            HandleLightTransitions();
            HandleParticleEffects();

			volume.profile.TryGetSettings(out colGrad);
			//foreach (GameObject g in GameObject.FindObjectsOfType(typeof(LightTransition)))
			//	m_transitionLights.Add(g.GetComponent<LightTransition>());
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
                //HandleLightTransitions();
                HandleParticleEffects();
                HandleDialogue();
				RebakeNavMesh();
            }
			HandleLightTransitions();		//this ones here cause it lerps
        }

        void RebakeNavMesh()
        {
            //navmesh.BuildNavMesh();
			//need to just have swap between two nav meshes

			//JUST STOP JOSH :'( I HATE NAVMESH, JUST USE NAV MESH OBSTACLES IN REAL TIME CAUSE THIS SHIT IS SO FUCKING STUPID
            //to josh, the above message was written by yourself dont act suprised now

			//if(isCorrupted)
			//{
			//	uncorruptNav.enabled = false;
			//	corruptNav.enabled = true;

			//	//corruptNav.navMeshData = corruptData;
			//}
			//if(!isCorrupted)
			//{
			//	//corruptNav.navMeshData = uncorruptData;

			//	corruptNav.enabled = false;
			//	corruptNav.enabled = true;
			//}

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
				volProgress += Time.deltaTime / volTransitionTime;

				foreach (LightTransition l in m_transitionLights)
					l.m_isCorrupt = true;
			}
			else
			{
				volProgress -= Time.deltaTime / volTransitionTime;

				foreach (LightTransition l in m_transitionLights)
					l.m_isCorrupt = false;
			}

			if(volProgress > 1)
			{
				volProgress = 1;
			}
			else if(volProgress < 0)
			{
				volProgress = 0;
			}

			if (volume != null)
			{
				volume.profile.TryGetSettings(out colGrad);

				colGrad.tint.value = Mathf.Lerp(uncorruptTint, corruptTint, volProgress);
				colGrad.temperature.value = Mathf.Lerp(uncorruptTemperature, corruptTemperature, volProgress);
				
				colGrad.temperature.value = uncorruptTemperature;
			}
			else
				Debug.LogError("hey idiot set your volume bitch");

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
