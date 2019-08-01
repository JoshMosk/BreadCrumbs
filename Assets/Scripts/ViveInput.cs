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

        public bool pointAndClickDown => SteamVR_Actions._default.PointAndClick.GetLastStateDown(SteamVR_Input_Sources.Any);

        public bool swapDimensionDown => SteamVR_Actions._default.SwapDimension.GetLastStateDown(SteamVR_Input_Sources.Any);

        public bool pointAndClick => SteamVR_Actions._default.PointAndClick.GetLastStateDown(SteamVR_Input_Sources.Any);

        public bool swapDimension => SteamVR_Actions._default.SwapDimension.GetLastStateDown(SteamVR_Input_Sources.Any);
    }
}