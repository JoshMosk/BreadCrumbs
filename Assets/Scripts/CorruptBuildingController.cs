using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MajorVRProj
{
    //this keeps track of all buildings and calls the transition on them
    public class CorruptBuildingController : MonoBehaviour
    {
        [SerializeField]
        public List<CorruptBuilding> corruptBuildings;

        void Awake()
        {
			corruptBuildings.AddRange(FindObjectsOfType<CorruptBuilding>());
        }

        public void Corrupt()
        {
            foreach (var b in corruptBuildings) {
                b.TransitionIn();
            }
        }

        public void UnCorrupt()
        {
            foreach (var b in corruptBuildings) {
                b.TransitionOut();
            }
        }
    }
}
