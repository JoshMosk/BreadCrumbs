using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IInput
{
    Vector2 axis { get; }

    bool fire { get; }
    bool fireDown { get; }

    bool use { get; }
    bool useDown { get; }
}
