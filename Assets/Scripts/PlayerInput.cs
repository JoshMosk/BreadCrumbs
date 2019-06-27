using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MajorVRProj
{
    public class PlayerInput : MonoBehaviour, IInput
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

        [SerializeField] KeyCode clickKey = KeyCode.Mouse0;
        public bool click => Input.GetKey(clickKey);

        public bool clicked => Input.GetKeyDown(clickKey);
    }
}
