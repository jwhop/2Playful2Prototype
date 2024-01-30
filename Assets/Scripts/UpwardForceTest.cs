using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpwardForceTest : MonoBehaviour
{
    private Rigidbody rb;
    public HingeJoint hj1, hj2;
    public float forceAmount;
    public GameObject arm;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        
        if (Input.GetKey(KeyCode.Space))
        {
            //hj1.autoConfigureConnectedAnchor = false;
            //hj2.autoConfigureConnectedAnchor = false;
            
            Vector3 dir = (arm.transform.position - transform.position).normalized;
            dir.z = 0;
            Debug.Log(dir);
            hj1.anchor = new Vector3(hj1.anchor.x, hj1.anchor.y - Time.deltaTime * forceAmount, hj1.anchor.z);
            //hj2.anchor = new Vector3(hj2.anchor.x, hj2.anchor.y - Time.deltaTime * forceAmount, hj2.anchor.z);
            //rb.MovePosition(transform.position + (Vector3.up * Time.deltaTime * forceAmount)); 
            rb.AddForce(Vector3.up * forceAmount * Time.deltaTime, ForceMode.Force);
            //hj1.autoConfigureConnectedAnchor = true;
            //hj2.autoConfigureConnectedAnchor = true;
        }
    }

    private void LateUpdate()
    {
        //hj.autoConfigureConnectedAnchor = true;
    }
    
}
