using DG.Tweening;
using MainControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TruckBehaviour : MonoBehaviour, IMoveable
{
    [Header("Main Reference")]
    [SerializeField] MainController controller;

    [Header("Main Attributes")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _wheelRteMltpr;
    [SerializeField] Transform _targetPos;
    [SerializeField] GameObject[] _wheels;
 
    [Header("Debug Values")]
    [SerializeField] bool _isMoving;
    // Start is called before the first frame update
    public Action<Action> moveTruck;

    public event Action onCheckPointReach;

    public float speed { get => _moveSpeed; set => value = _moveSpeed; }
    public bool isMoving { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == string.Format("checkpoint_{0}", controller._checkpointCount))
        {
            onCheckPointReach.GetInvocationList()[controller._checkpointCount].DynamicInvoke();
            controller._checkpointCount++;
        }
    }

    public void MoveVehicle()
    {
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, new Vector3( _targetPos.position.x, transform.position.y, _targetPos.position.z), step);

        // Check if the position of the cube and sphere are approximately equal.
        foreach (var item in _wheels)
        {
            item.transform.localEulerAngles = new Vector3(_wheelRteMltpr * item.transform.position.x, item.transform.localRotation.y, item.transform.localRotation.z);
        }
    }
}
