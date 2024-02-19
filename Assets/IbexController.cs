using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IbexController : MonoBehaviour
{
    [SerializeField] Collider2D torsoCollider, rearCollider, frontLegsCollider, backLegsCollider, singleBodyCollider;
    [SerializeField] Rigidbody2D torsoRb, rearRb, frontLegsRb, backLegsRb, singleBodyRb;
    [SerializeField] float upForce, horizontalForce, sphereCastRadius, sphereCastMaxDistance, dotCoefficient, distanceToFrontForcePosition, distanceToBackForcePosition, distanceToBackForceHorizontalPosition, legForce;
    [SerializeField] LayerMask environment;
    private Gamepad gamepad;
    public bool isFrontGrounded, isBackGrounded;
    public bool useDotFront, useDotBack;
    // Start is called before the first frame update
    void Start()
    {
        gamepad = Gamepad.current;
        isFrontGrounded = true;
        isBackGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        //CalculateCapsuleBottom(bodyCollider, out var bottom);
        isFrontGrounded = Physics.SphereCast(torsoCollider.transform.position - torsoCollider.transform.up * distanceToBackForceHorizontalPosition + Vector3.up * sphereCastRadius, sphereCastRadius, Vector3.down, out RaycastHit hit, sphereCastMaxDistance,
        environment);
        isBackGrounded = Physics.SphereCast(rearCollider.transform.position - rearCollider.transform.up * distanceToBackForceHorizontalPosition + Vector3.up * sphereCastRadius, sphereCastRadius, Vector3.down, out RaycastHit hit2, sphereCastMaxDistance,
        environment);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isFrontGrounded? Color.green : Color.red;
        Gizmos.DrawSphere(singleBodyCollider.transform.position + singleBodyCollider.transform.right * distanceToFrontForcePosition, 0.1f);
        Gizmos.color = isBackGrounded? Color.green: Color.red;
        Gizmos.DrawSphere(singleBodyCollider.transform.position - singleBodyCollider.transform.right * distanceToBackForcePosition, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(rearCollider.transform.position - rearCollider.transform.up * distanceToBackForceHorizontalPosition, 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(torsoCollider.transform.position - torsoCollider.transform.up * distanceToBackForceHorizontalPosition, 0.1f);
    }

    private void FixedUpdate()
    {
        //torsoRb.AddForce(gamepad.leftStick.value.y * Vector3.up * upForce);

        Debug.Log(Vector3.Dot(Vector3.up, torsoCollider.transform.up));


        //if (gamepad.leftStick.value.y > 0.1f)
        //{
        //    if (isBackGrounded)
        //    {
        //        if (useDotFront)
        //        {
        //            torsoRb.AddForceAtPosition(gamepad.leftStick.value.y * torsoCollider.transform.up * upForce * dotCoefficient * Vector3.Dot(Vector3.up, torsoCollider.transform.up), torsoCollider.transform.position + torsoCollider.transform.right * distanceToFrontForcePosition);
        //        }
        //        else
        //        {
        //            torsoRb.AddForceAtPosition(gamepad.leftStick.value.y * torsoCollider.transform.up * upForce, torsoCollider.transform.position + torsoCollider.transform.right * distanceToFrontForcePosition);

        //        }
        //    }
        //    else if (isFrontGrounded)
        //    {
        //        rearRb.AddForceAtPosition(-gamepad.leftStick.value.y * rearCollider.transform.up * upForce * dotCoefficient * Vector3.Dot(Vector3.up, rearCollider.transform.up), rearCollider.transform.position - rearCollider.transform.right * distanceToBackForcePosition);
        //    }
        //}
        //else if (gamepad.leftStick.value.y < -0.1f)
        //{
        //    if (isFrontGrounded)
        //    {
        //        rearRb.AddForceAtPosition(-gamepad.leftStick.value.y * rearCollider.transform.up * upForce * dotCoefficient * Vector3.Dot(Vector3.up, rearCollider.transform.up), rearCollider.transform.position - rearCollider.transform.right * distanceToBackForcePosition);
        //    }
        //    if (isBackGrounded)
        //    {
        //        torsoRb.AddForceAtPosition(gamepad.leftStick.value.y * torsoCollider.transform.up * upForce * dotCoefficient * Vector3.Dot(Vector3.up, torsoCollider.transform.up), torsoCollider.transform.position + torsoCollider.transform.right * distanceToFrontForcePosition);
        //    }
        //}
        
        if (isBackGrounded)
        {
            rearRb.AddForceAtPosition(gamepad.rightStick.value.x * horizontalForce * Vector3.right, rearCollider.transform.position - rearCollider.transform.up * distanceToBackForceHorizontalPosition);
            if (gamepad.leftStick.value.y > 0.1f)
            {
                if (useDotFront)
                {
                    torsoRb.AddForceAtPosition(gamepad.leftStick.value.y * torsoCollider.transform.up * upForce * dotCoefficient * Vector3.Dot(Vector3.up, torsoCollider.transform.up), torsoCollider.transform.position + torsoCollider.transform.right * distanceToFrontForcePosition);
                }
                else
                {
                    torsoRb.AddForceAtPosition(gamepad.leftStick.value.y * torsoCollider.transform.up * upForce, torsoCollider.transform.position + torsoCollider.transform.right * distanceToFrontForcePosition);

                }
            }
            else if (gamepad.leftStick.value.y < -0.1f)
            {
                {
                    torsoRb.AddForceAtPosition(gamepad.leftStick.value.y * torsoCollider.transform.up * upForce, torsoCollider.transform.position + torsoCollider.transform.right * distanceToFrontForcePosition);
                }
            }
        }
        
        if (isFrontGrounded)
        {
            torsoRb.AddForceAtPosition(gamepad.rightStick.value.x * horizontalForce * Vector3.right, torsoCollider.transform.position - torsoCollider.transform.up * distanceToBackForceHorizontalPosition);
            if(gamepad.rightStick.value.y > 0.1f)
            {
                if (useDotBack)
                {
                    rearRb.AddForceAtPosition(gamepad.rightStick.value.y * rearCollider.transform.up * upForce * dotCoefficient * Vector3.Dot(Vector3.up, rearCollider.transform.up), rearCollider.transform.position - rearCollider.transform.right * distanceToBackForcePosition);
                }
                else
                {
                    rearRb.AddForceAtPosition(gamepad.rightStick.value.y * rearCollider.transform.up * upForce, rearCollider.transform.position - rearCollider.transform.right * distanceToBackForcePosition);
                }
            }
            else if (gamepad.rightStick.value.y < -0.1f)
            {
                {
                    rearRb.AddForceAtPosition(gamepad.rightStick.value.y * rearCollider.transform.up * upForce, rearCollider.transform.position - rearCollider.transform.right * distanceToBackForcePosition);
                }
            }
        }

        frontLegsRb.AddForceAtPosition(((frontLegsCollider.transform.right * gamepad.rightTrigger.value) + (-frontLegsCollider.transform.right * gamepad.leftTrigger.value)) * legForce, frontLegsCollider.transform.position - frontLegsCollider.transform.up);
    

        //single body 
        //if (isBackGrounded)
        //{
        //    singleBodyRb.AddForceAtPosition(gamepad.leftStick.value.y * upForce * singleBodyCollider.transform.up * dotCoefficient * Vector3.Dot(Vector3.up, singleBodyCollider.transform.up), singleBodyCollider.transform.position + singleBodyCollider.transform.right * distanceToFrontForcePosition);
        //}

        //if (isFrontGrounded)
        //{
        //    singleBodyRb.AddForceAtPosition(gamepad.rightStick.value.y * upForce * singleBodyCollider.transform.up * dotCoefficient * Vector3.Dot(Vector3.up, singleBodyCollider.transform.up), singleBodyCollider.transform.position - singleBodyCollider.transform.right * distanceToBackForcePosition);
        //}

        //if(isBackGrounded || isFrontGrounded)
        //{
        //    singleBodyRb.AddForce(gamepad.leftStick.value.x * horizontalForce * singleBodyCollider.transform.right * dotCoefficient * Vector3.Dot(Vector3.up, singleBodyCollider.transform.up));
        //}
    }
}
