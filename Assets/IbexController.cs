using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IbexController : MonoBehaviour
{
    [SerializeField] Collider2D torsoCollider, rearCollider, frontLegsCollider, backLegsCollider, singleBodyCollider;
    [SerializeField] Rigidbody2D torsoRb, rearRb, frontLegsRb, backLegsRb, singleBodyRb;
    [SerializeField] float upForce, horizontalForce, sphereCastRadius, sphereCastMaxDistance, dotCoefficient, distanceToFrontForcePosition, distanceToBackForcePosition, distanceToBackForceHorizontalPosition, legForce;
    [SerializeField] float distanceFromBackLegForGroundCheck, distanceFromFrontLegForGroundCheck;
    [SerializeField] LayerMask environment;
    [SerializeField] GameObject rearSprite3D, torsoSprite3D, rearLeg1Sprite3D, rearLeg2Sprite3D, frontLeg1Sprite3D, frontLeg2Sprite3D, headSprite3D;
    private Gamepad gamepad;
    public bool isFrontGrounded, isBackGrounded;
    public bool useDotFront, useDotBack;
    private Vector3 diffRotationRear, diffPositionRear, diffRotationTorso;
    private float diffPositionFrontLeg1, diffPositionFrontLeg2, diffPositionBackLeg1, diffPositionBackLeg2;
    public bool isLeft;
    private UnityEngine.InputSystem.Controls.StickControl frontLegs, backLegs;
    // Start is called before the first frame update
    void Start()
    {
        gamepad = isLeft? Gamepad.all[0] : Gamepad.all[1];
        frontLegs = isLeft ? gamepad.rightStick : gamepad.leftStick;
        backLegs = isLeft ? gamepad.leftStick : gamepad.rightStick;
        isFrontGrounded = true;
        isBackGrounded = true;
        diffRotationRear = rearCollider.transform.eulerAngles - rearSprite3D.transform.eulerAngles;
        Debug.Log(frontLegsCollider.transform.eulerAngles);
        Debug.Log(frontLeg1Sprite3D.transform.eulerAngles);
        //diffRotationRear = rearSprite3D.transform.InverseTransformDirection(diffRotationRear);
        diffPositionRear = rearCollider.transform.position - rearSprite3D.transform.position;
        diffPositionRear = rearSprite3D.transform.InverseTransformDirection(diffPositionRear);
        diffRotationTorso = torsoCollider.transform.eulerAngles - torsoSprite3D.transform.eulerAngles;
        //diffRotationFrontLegs = frontLegsCollider.transform.eulerAngles - frontLeg1Sprite3D.transform.eulerAngles;
        diffPositionFrontLeg1 = frontLegsCollider.transform.position.y - frontLeg1Sprite3D.transform.position.y;
        diffPositionFrontLeg2 = frontLegsCollider.transform.position.y - frontLeg2Sprite3D.transform.position.y;

        diffPositionBackLeg1 = backLegsCollider.transform.position.y - rearLeg1Sprite3D.transform.position.y;
        diffPositionBackLeg2 = backLegsCollider.transform.position.y - rearLeg2Sprite3D.transform.position.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        //CalculateCapsuleBottom(bodyCollider, out var bottom);
        isFrontGrounded = Physics.SphereCast(torsoCollider.transform.position - torsoCollider.transform.up * distanceToBackForceHorizontalPosition + Vector3.up * sphereCastRadius, sphereCastRadius, Vector3.down, out RaycastHit hit, sphereCastMaxDistance,
        environment) || Physics.SphereCast(torsoCollider.transform.position + Vector3.up * sphereCastRadius, sphereCastRadius, Vector3.down, out RaycastHit hit3, sphereCastMaxDistance);
        isBackGrounded = Physics.SphereCast(rearCollider.transform.position - rearCollider.transform.up * distanceToBackForceHorizontalPosition + Vector3.up * sphereCastRadius, sphereCastRadius, Vector3.down, out RaycastHit hit2, sphereCastMaxDistance,
        environment) || Physics.SphereCast(rearCollider.transform.position + Vector3.up * sphereCastRadius, sphereCastRadius, Vector3.down, out RaycastHit hit4, sphereCastMaxDistance); ;

        rearSprite3D.transform.position = rearCollider.transform.position - rearSprite3D.transform.TransformDirection(diffPositionRear);
        rearSprite3D.transform.eulerAngles = rearCollider.transform.eulerAngles - diffRotationRear;

        torsoSprite3D.transform.position = torsoCollider.transform.position;
        torsoSprite3D.transform.eulerAngles = torsoCollider.transform.eulerAngles - diffRotationTorso;

        frontLeg1Sprite3D.transform.position = new Vector3(frontLegsCollider.transform.position.x, frontLegsCollider.transform.position.y - diffPositionFrontLeg1, frontLeg1Sprite3D.transform.position.z);
        frontLeg2Sprite3D.transform.position = new Vector3(frontLegsCollider.transform.position.x, frontLegsCollider.transform.position.y - diffPositionFrontLeg2, frontLeg2Sprite3D.transform.position.z);
        //frontLeg1Sprite3D.transform.eulerAngles = new Vector3(frontLeg1Sprite3D.transform.eulerAngles.x, frontLeg1Sprite3D.transform.eulerAngles.y, frontLeg1Sprite3D.transform.eulerAngles.z + frontLegsCollider.transform.eulerAngles.z);
        //frontLeg2Sprite3D.transform.eulerAngles = new Vector3(frontLeg1Sprite3D.transform.eulerAngles.x, frontLeg1Sprite3D.transform.eulerAngles.y, frontLeg1Sprite3D.transform.eulerAngles.z + frontLegsCollider.transform.eulerAngles.z);

        rearLeg1Sprite3D.transform.position = new Vector3(backLegsCollider.transform.position.x, backLegsCollider.transform.position.y - diffPositionBackLeg1, rearLeg1Sprite3D.transform.position.z);
        rearLeg2Sprite3D.transform.position = new Vector3(backLegsCollider.transform.position.x, backLegsCollider.transform.position.y - diffPositionBackLeg2, rearLeg2Sprite3D.transform.position.z);
        if (gamepad.aButton.wasReleasedThisFrame)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
            isLeft = !isLeft;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isFrontGrounded? Color.green : Color.red;
        Gizmos.DrawSphere(singleBodyCollider.transform.position + singleBodyCollider.transform.right * distanceToFrontForcePosition, 0.1f);
        Gizmos.color = isBackGrounded? Color.green: Color.red;
        Gizmos.DrawSphere(rearCollider.transform.position + rearCollider.transform.right * distanceFromBackLegForGroundCheck - rearCollider.transform.up * distanceToBackForceHorizontalPosition, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(rearCollider.transform.position - rearCollider.transform.up * distanceToBackForceHorizontalPosition, 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(torsoCollider.transform.position - torsoCollider.transform.up * distanceToBackForceHorizontalPosition, 0.1f);
    }

    private void FixedUpdate()
    {
        //torsoRb.AddForce(gamepad.leftStick.value.y * Vector3.up * upForce);

        //Debug.Log(Vector3.Dot(Vector3.up, torsoCollider.transform.up));


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
            rearRb.AddForceAtPosition(backLegs.value.x * horizontalForce * (rearCollider.transform.right + Vector3.right).normalized, rearCollider.transform.position - rearCollider.transform.up * distanceToBackForceHorizontalPosition);
            if (frontLegs.value.y > 0.1f)
            {
                if (useDotFront)
                {
                    torsoRb.AddForceAtPosition(frontLegs.value.y * torsoCollider.transform.up * upForce * dotCoefficient * Vector3.Dot(Vector3.up, torsoCollider.transform.up), torsoCollider.transform.position + torsoCollider.transform.right * distanceToFrontForcePosition);
                }
                else
                {
                    torsoRb.AddForceAtPosition(frontLegs.value.y * torsoCollider.transform.up * upForce, torsoCollider.transform.position + torsoCollider.transform.right * distanceToFrontForcePosition);

                }
            }
            else if (frontLegs.value.y < -0.1f)
            {
                {
                    torsoRb.AddForceAtPosition(frontLegs.value.y * torsoCollider.transform.up * upForce, torsoCollider.transform.position + torsoCollider.transform.right * distanceToFrontForcePosition);
                }
            }
        }
        
        if (isFrontGrounded)
        {
            torsoRb.AddForceAtPosition(frontLegs.value.x * horizontalForce * (torsoCollider.transform.right + Vector3.right).normalized, torsoCollider.transform.position - torsoCollider.transform.up * distanceToBackForceHorizontalPosition);
            if(backLegs.value.y > 0.1f)
            {
                if (useDotBack)
                {
                    rearRb.AddForceAtPosition(backLegs.value.y * rearCollider.transform.up * upForce * dotCoefficient * Vector3.Dot(Vector3.up, rearCollider.transform.up), rearCollider.transform.position - rearCollider.transform.right * distanceToBackForcePosition);
                }
                else
                {
                    rearRb.AddForceAtPosition(backLegs.value.y * rearCollider.transform.up * upForce, rearCollider.transform.position - rearCollider.transform.right * distanceToBackForcePosition);
                }
            }
            else if (backLegs.value.y < -0.1f)
            {
                {
                    rearRb.AddForceAtPosition(backLegs.value.y * rearCollider.transform.up * upForce, rearCollider.transform.position - rearCollider.transform.right * distanceToBackForcePosition);
                }
            }
        }

        //frontLegsRb.AddForceAtPosition(((frontLegsCollider.transform.right * gamepad.rightTrigger.value) + (-frontLegsCollider.transform.right * gamepad.leftTrigger.value)) * legForce, frontLegsCollider.transform.position - frontLegsCollider.transform.up);
    

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
