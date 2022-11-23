using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

namespace MainControl
{
    [RequireComponent(typeof(EventTrigger))]
    public class InspectableObject : MonoBehaviour, IInspectable
    {
        [field : SerializeField]
        public CinemachineVirtualCamera vCam { get; set; }
        [field: SerializeField]
        public EventTrigger trigger { get; set; }
        [SerializeField] ItemDataScriptable itemData;
        [SerializeField] GameObject pointerAnimation;

        public event Action<GameObject> onClick;

        public void SelectThisCamera()
        {
            vCam.Priority = 1;
        }
        
        public ItemDataScriptable GetItemData()
        {
            return itemData;
        }

        public void SetPointerVisibility(bool state)
        {
            pointerAnimation.SetActive(state);
        }
    }
}
