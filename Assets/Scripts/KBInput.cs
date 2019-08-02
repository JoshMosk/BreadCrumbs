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

        public bool pointAndClickDown => Input.GetKeyDown(fireKey);
        public bool pointAndClick => Input.GetKey(fireKey);
        public bool pointAndClickUp => Input.GetKeyUp(fireKey);


        [SerializeField] KeyCode useKey = KeyCode.Space;
        public bool swapDimensionDown => Input.GetKeyDown(useKey);
        public bool swapDimension => Input.GetKey(useKey);
        public bool swapDimensionUp => Input.GetKeyDown(useKey);

        [SerializeField] KeyCode moveKey = KeyCode.UpArrow;
        public bool dragMovementDown => Input.GetKey(useKey);
        public bool dragMovement => Input.GetKeyDown(useKey);
        public bool dragMovementUp => Input.GetKeyDown(useKey);
    }
}
