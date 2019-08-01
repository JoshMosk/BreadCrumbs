using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace MajorVRProj
{
    public class KBInput : MonoBehaviour, IInput
    {

        //public Vector2 axis
        //{
        //    get
        //    {
        //        Vector2 result;
        //        result.x = Input.GetAxis("Horizontal");
        //        result.y = Input.GetAxis("Vertical");
        //        return result;
        //    }
        //}

        [SerializeField] KeyCode fireKey = KeyCode.Mouse0;

        public bool pointAndClick => Input.GetKey(fireKey);
        public bool pointAndClickDown => Input.GetKeyDown(fireKey);

        public bool pointAndClickUp => Input.GetKeyUp(fireKey);


        [SerializeField] KeyCode useKey = KeyCode.Space;
        public bool swapDimension => Input.GetKey(useKey);
        public bool swapDimensionDown => Input.GetKeyDown(useKey);
        public bool swapDimensionUp => Input.GetKeyDown(useKey);
    }
}
