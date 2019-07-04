using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MajorVRProj
{
    public class CorruptBuildingController : MonoBehaviour
    {
        [SerializeField]
        CorruptBuilding[] corruptBuildings;

        void Awake()
        {
            corruptBuildings = FindObjectsOfType<CorruptBuilding>();
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
