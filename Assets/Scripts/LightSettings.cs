using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MajorVRProj
{
    [CreateAssetMenu(menuName = "MajorVRProj/Light Settings")]
    public class LightSettings : ScriptableObject
    {
        public Color color = Color.white;
        public float colorTemperature = 5200f;
        public float intensity = 10f;
    }
}