using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MajorVRProj
{
    public static class Extensions
    {
        //Floating point value approx equality test
        public static bool NearlyEqual(this float value, float other, float epsilon = 0.000001f)
        {
            return Mathf.Abs(value - other) <= epsilon;
        }
    }
}
