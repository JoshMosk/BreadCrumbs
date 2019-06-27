using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IInput
{
    Vector2 axis { get; }

    bool click { get; }
    bool clicked { get; }
}
