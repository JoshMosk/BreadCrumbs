using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MajorVRProj
{
    public class KBInput : MonoBehaviour, IInput
    {
        public Vector2 axis
        {
            get
            {
                Vector2 result;
                result.x = Input.GetAxis("Horizontal");
                result.y = Input.GetAxis("Vertical");
                return result;
            }
        }

        [SerializeField] KeyCode fireKey = KeyCode.Mouse0;
        public bool fire => Input.GetKey(fireKey);
        public bool fireDown => Input.GetKeyDown(fireKey);


        [SerializeField] KeyCode useKey = KeyCode.Space;
        public bool use => Input.GetKey(useKey);
        public bool useDown => Input.GetKeyDown(useKey);

    }
}
