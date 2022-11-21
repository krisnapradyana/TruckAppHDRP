using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILookCamera
{
    public Transform defaultLook { get; set; }
    public Transform targetLook { get; set; }
    public float lookDelay { get; set; }
}
