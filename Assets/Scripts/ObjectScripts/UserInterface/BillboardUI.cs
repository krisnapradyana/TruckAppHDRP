using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform.position, -Vector3.up);
    }
}