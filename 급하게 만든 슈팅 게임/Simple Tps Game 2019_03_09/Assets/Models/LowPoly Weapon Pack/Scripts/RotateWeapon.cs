using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeapon : MonoBehaviour {

    float rotSpeed = 10;

    private void OnMouseDrag()
    {
        float y = -Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;
        transform.RotateAround(Vector3.right, y);
        float x = -Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        transform.RotateAround(Vector3.up, x);
    }
}
