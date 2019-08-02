using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace MajorVRProj
{
    public class ViveInput : MonoBehaviour, IInput
    {
        public SteamVR_ActionSet m_actionSet;

        //public SteamVR_Action_Boolean m_pointAndClickAction;
        //public SteamVR_Action_Boolean m_swapDimension;

        //private void Awake()
        //{
        //    m_pointAndClickAction[SteamVR_Input_Sources.Any].onStateDown += PointAndClick;
        //    m_swapDimension[SteamVR_Input_Sources.Any].onStateDown += SwapDimension;
        //}


        //private void Start()
        //{
        //    m_actionSet.Activate(SteamVR_Input_Sources.Any, 0, true);
        //}

        //private void Update()
        //{
        //    if (SteamVR_Actions._default.PointAndClick.GetLastStateDown(SteamVR_Input_Sources.Any))
        //    {
        //        print("point down");
        //    }
        //}

        //public void PointAndClick(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
        //{
            
        //}

        //public void SwapDimension(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
        //{

        //}


        //point and click movement
        public bool pointAndClickDown => SteamVR_Actions._default.PointAndClick.GetLastStateDown(SteamVR_Input_Sources.LeftHand);
        public bool pointAndClick => SteamVR_Actions._default.PointAndClick.GetLastState(SteamVR_Input_Sources.LeftHand);
        public bool pointAndClickUp => SteamVR_Actions._default.PointAndClick.GetLastStateUp(SteamVR_Input_Sources.LeftHand);


        //swap dimension
        public bool swapDimensionDown => SteamVR_Actions._default.SwapDimension.GetLastStateDown(SteamVR_Input_Sources.LeftHand);
        public bool swapDimension => SteamVR_Actions._default.SwapDimension.GetLastState(SteamVR_Input_Sources.LeftHand);
        public bool swapDimensionUp => SteamVR_Actions._default.SwapDimension.GetLastStateUp(SteamVR_Input_Sources.LeftHand);


        //drag movement
        public bool dragMovementDown => SteamVR_Actions._default.DragWorld.GetLastStateDown(SteamVR_Input_Sources.RightHand);
        public bool dragMovement => SteamVR_Actions._default.DragWorld.GetLastStateDown(SteamVR_Input_Sources.RightHand);
        public bool dragMovementUp => SteamVR_Actions._default.DragWorld.GetLastStateDown(SteamVR_Input_Sources.RightHand);
    }
}