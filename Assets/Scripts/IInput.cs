using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IInput
{
    bool pointAndClick { get; }
    bool pointAndClickDown { get; }
    bool pointAndClickUp { get; }

    bool swapDimension { get; }
    bool swapDimensionDown { get; }
    bool swapDimensionUp { get; }
}
