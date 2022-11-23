using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

namespace MainControl
{
    public class MainController : MonoBehaviour
    {
        [Header("Positional")]
        public Transform _truckTargetPosition;
        public Transform _cameraStartMovePos;

        [Header("Main Components")]
        [SerializeField] TruckBehaviour _truckBehaviour;
        [SerializeField] UIControl _uiController; 

        [Header("Inspectable Objects")]
        [SerializeField] InspectableObject[] _inspectableObjects;

        [Header("Cameras")]
        [SerializeField] CinemachineFreeLook _freeLookCam1;
        [SerializeField] CinemachineVirtualCamera _vCam1;
        [SerializeField] CinemachineVirtualCamera[] _vCamCameras;

        [Header("MockupUI")]
        [SerializeField] GameObject _mockupUI;

        public int _checkpointCount;
        private float _xAxisMaxSpeed;
        private float _yAxisMaxSpeed;
        InspectableObject _currentInspected;
        bool _isAllowInspect;
        bool _isInspecting;
        // Start is called before the first frame update
        void Start()
        {
            _xAxisMaxSpeed = _freeLookCam1.m_XAxis.m_MaxSpeed;
            _yAxisMaxSpeed = _freeLookCam1.m_YAxis.m_MaxSpeed;
            _isAllowInspect = false;

            _uiController._backButton.onClick.AddListener(() => CancelSelect());
            RegisterInvocation();
            LockMouseOnStart();
        }

        // Update is called once per frame
        void Update()
        {
            _truckBehaviour.MoveVehicle();
            ActivateMouseOrbit();

            if(Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Application.Quit();
            }
        }


        //Warning, subscribe event in order
        void RegisterInvocation()
        {
            _truckBehaviour.onCheckPointReach += SwitchCameras;
            _truckBehaviour.onCheckPointReach += InitiateInteraction;
        }

        private void SwitchCameras()
        {
            Debug.LogFormat("Checkpoint {0}", _checkpointCount);
            _vCam1.Priority = 0;
            _freeLookCam1.Priority = 1;
        }

        private void CancelSelect()
        {
            foreach (var item in _vCamCameras)
            {
                item.Priority = 0;
            }

            foreach (var item in _inspectableObjects)
            {
                item.SetPointerVisibility(false);
            }
            _freeLookCam1.Priority = 1;
            _isInspecting = false;
            _isAllowInspect = true;
            _mockupUI.SetActive(false);
            _uiController.ToggleBack(false);
            _uiController.ToggleShowWindow(false);
        }

        private void ActivateMouseOrbit()
        {
            // No input unless right mouse is down
            if (Mouse.current.rightButton.isPressed)
            {
                Debug.Log("Mouse has pressed");
                _freeLookCam1.m_XAxis.m_MaxSpeed = _xAxisMaxSpeed;
                _freeLookCam1.m_YAxis.m_MaxSpeed = _yAxisMaxSpeed;
            }

            if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                Debug.Log("Mouse has released");
                _freeLookCam1.m_XAxis.m_MaxSpeed = 0;
                _freeLookCam1.m_YAxis.m_MaxSpeed = 0;
            }
        }

        //Enable all selection object and disabled all camera priority
        private void InitiateInteraction()
        {
            Debug.LogFormat("Checkpoint {0}", _checkpointCount);
            StartCoroutine(_uiController.ShowTutorialWithTimer());
            _isAllowInspect = true;
            _uiController.Show360UI();

            foreach (var item in _inspectableObjects)
            {
                item.trigger.AddEvent(EventTriggerType.PointerEnter, (data) => 
                {
                    if (!_isAllowInspect || _isInspecting)
                    {
                        return;
                    }
                    _uiController._inspectText.text = item.gameObject.name;
                    item.GetComponent<InspectableObject>().SetPointerVisibility(true);
                });
                item.trigger.AddEvent(EventTriggerType.PointerExit, (data) => 
                {
                    if (_isInspecting)
                    {
                        return;
                    }
                    _uiController._inspectText.text = "Hover to Inspect";
                    item.GetComponent<InspectableObject>().SetPointerVisibility(false);
                });

            }

            _uiController._exitButton.onClick.AddListener(() => Application.Quit());

            for (int i = 0; i < _inspectableObjects.Length; i++)
            {
                var currentInspected = _inspectableObjects[i];
                _inspectableObjects[i].trigger.AddEvent(EventTriggerType.PointerClick, (data) =>
                {
                    if (!(data.button == PointerEventData.InputButton.Left))
                    {
                        return;
                    }

                    if (_isInspecting)
                    {
                        return;
                    }

                    if (!_isAllowInspect)
                    {
                        return;
                    }

                    _currentInspected = currentInspected;
                    _freeLookCam1.Priority = 0;
                    foreach (var item in _inspectableObjects)
                    {
                        item.vCam.Priority = 0;
                    }
                    _currentInspected.gameObject.layer = LayerMask.NameToLayer("Default");
                    _currentInspected.SelectThisCamera();
                    _uiController.ToggleBack(true);
                    _mockupUI.SetActive(true);
                    //_uiController.ToggleShowWindow(true);
                    //_uiController.SetTexts(_currentInspected.GetItemData()._title, _currentInspected.GetItemData()._description);
                    _isInspecting = true;
                    _isAllowInspect = false;
                });
            }
        }

        private void LockMouseOnStart()
        {
            _freeLookCam1.m_XAxis.m_MaxSpeed = 0;
            _freeLookCam1.m_YAxis.m_MaxSpeed = 0;
        }
    }
}
