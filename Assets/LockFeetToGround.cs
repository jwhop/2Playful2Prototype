using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockFeetToGround : MonoBehaviour
{
    public bool currentlyLockingFeet = true;
    public float lockedYValue = 0f;
    public Transform bodyReference;
    public float angleBeforeFootGoesUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (bodyReference.eulerAngles.z < angleBeforeFootGoesUp)
        {
            Debug.Log("unlocking foot");
            currentlyLockingFeet = true;
        }
        else currentlyLockingFeet = true;
        if (currentlyLockingFeet)
        {
            //Debug.Log(gameObject.name + transform.position);
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
    }
}
