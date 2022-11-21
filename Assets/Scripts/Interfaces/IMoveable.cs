using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    public float speed { get; set; }
    public bool isMoving { get; set; }
    public event Action onCheckPointReach;
}
