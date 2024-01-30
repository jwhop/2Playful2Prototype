using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FIMSpace.FTail;

public class TailTest : MonoBehaviour
{
    private Rigidbody rb;
    //public float pushForce;
    //public Transform sprite3D;
    private TailAnimator2 tailAnimator;
    // Start is called before the first frame update
    void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        tailAnimator = GetComponent<TailAnimator2>();
    }

    // Update is called once per frame
    void Update()
    {
        //sprite3D.position = transform.position;
    }

    private void LateUpdate()
    {
        if (tailAnimator.isActiveAndEnabled)
        {
            tailAnimator.ManualUpdate();
            tailAnimator.ManualUpdate2();
        }
    }

    private void FixedUpdate()
    {
        //rb.AddForce(Input.GetAxis("Horizontal") * pushForce * Vector3.right, ForceMode.Force);
    }
}
