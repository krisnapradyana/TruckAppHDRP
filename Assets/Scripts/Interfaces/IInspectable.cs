using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public interface IInspectable
{
    public EventTrigger trigger { get; set; }
    public CinemachineVirtualCamera vCam { get; set; }
}
