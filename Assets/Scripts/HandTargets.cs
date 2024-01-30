using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTargets : MonoBehaviour
{
    public MonkeyManager monkey;
    public bool isLeft;
    public bool isAttached = false;
    private Vector3 attachPoint;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("is hand free?" + monkey.IsHandFree(isLeft));
        if (other.CompareTag("Rung") && monkey.IsHandFree(isLeft))
        {
            Debug.Log("attaching");
            isAttached = true;
            attachPoint = other.ClosestPoint(transform.position);
            //transform.parent = other.gameObject.transform;
            monkey.AttachToRung(other.gameObject, isLeft);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Rung") && !monkey.IsHandFree(isLeft))
        //{
        //    monkey.DetachRung(isLeft);
        //}
    }

    public void DetachRung()
    {
        monkey.DetachRung(isLeft);
        isAttached = false;
    }

    private void FixedUpdate()
    {
        if (isAttached)
        {
            transform.position = attachPoint;
        }
    }
}
