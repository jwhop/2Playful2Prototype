using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MandiblesController : MonoBehaviour
{
    public BeetleController myBeetle, opponentBeetle;
    public bool isLeft;
    private Rigidbody rb;
    private bool isGrabbing, canGrab;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        canGrab = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Abdomen") && canGrab)
        {
            opponentBeetle.StartBeingGrabbed(rb);
            myBeetle.StartGrabbing(isLeft);
            isGrabbing = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Abdomen") && !isGrabbing && canGrab)
        {
            opponentBeetle.StartBeingGrabbed(rb);
            myBeetle.StartGrabbing(isLeft);
            isGrabbing = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Abdomen"))
        {
            opponentBeetle.StopBeingGrabbed(rb);
            myBeetle.StopGrabbing(isLeft);
            isGrabbing = false;
        }
    }

    public void LetGo()
    {
        opponentBeetle.StopBeingGrabbed(rb);
        myBeetle.StopGrabbing(isLeft);
        isGrabbing = false;
        StartCoroutine(GrabCooldownTimer());
    }

    IEnumerator GrabCooldownTimer()
    {
        canGrab = false;
        yield return new WaitForSeconds(0.1f);
        canGrab = true;
    }
}
