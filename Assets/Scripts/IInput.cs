using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IInput
{
    //point and click character movement
    bool pointAndClick { get; }
    bool pointAndClickDown { get; }
    bool pointAndClickUp { get; }

    //swap dimension
    bool swapDimensionDown { get; }
    bool swapDimension { get; }
    bool swapDimensionUp { get; }

    //player drag movement
    bool dragMovementDown { get; }
    bool dragMovement { get; }
    bool dragMovementUp { get; }

    bool NPCInteractDown { get; }
    bool NPCInteract { get; }
    bool NPCInteractUp { get; }

	Vector2 TouchPadPos { get; }
}
