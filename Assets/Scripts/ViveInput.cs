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
        public bool pointAndClickDown => SteamVR_Actions._default.PointAndClick.GetStateDown(SteamVR_Input_Sources.Any);
        public bool pointAndClick => SteamVR_Actions._default.PointAndClick.GetState(SteamVR_Input_Sources.Any);
        public bool pointAndClickUp => SteamVR_Actions._default.PointAndClick.GetStateUp(SteamVR_Input_Sources.Any);


        //swap dimension
        public bool swapDimensionDown => SteamVR_Actions._default.SwapDimension.GetStateDown(SteamVR_Input_Sources.Any);
        public bool swapDimension => SteamVR_Actions._default.SwapDimension.GetState(SteamVR_Input_Sources.Any);
        public bool swapDimensionUp => SteamVR_Actions._default.SwapDimension.GetStateUp(SteamVR_Input_Sources.Any);


        //drag movement
        public bool dragMovementDown => SteamVR_Actions._default.DragWorld.GetStateDown(SteamVR_Input_Sources.Any);
        public bool dragMovement => SteamVR_Actions._default.DragWorld.GetState(SteamVR_Input_Sources.Any);
        public bool dragMovementUp => SteamVR_Actions._default.DragWorld.GetStateUp(SteamVR_Input_Sources.Any);


        public bool NPCInteractDown => SteamVR_Actions._default.InteractNPC.GetStateDown(SteamVR_Input_Sources.Any);
        public bool NPCInteract => SteamVR_Actions._default.InteractNPC.GetState(SteamVR_Input_Sources.Any);
        public bool NPCInteractUp => SteamVR_Actions._default.InteractNPC.GetStateUp(SteamVR_Input_Sources.Any);

		public Vector2 TouchPadPos => SteamVR_Actions._default.MultipleChoiceInput.GetLastAxis(SteamVR_Input_Sources.Any);

		//need to add in touch down and up (so the button aint pressed but you finger on the trigger (ooo rhyme) (how does this rhyme you madman, you are actually losing it lol) )
    }
}