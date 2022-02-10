
using UnityEngine;
using System;

public interface IGridEntity 
{
    event Action<IGridEntity> OnMove;

    Vector3 Position { get; set; }
}
