using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeetleController : MonoBehaviour
{
    [SerializeField] Rigidbody rbLeftMandible, rbRightMandible, rbAbdomen, rbThorax;
    [SerializeField] GameObject sprite3DAbdomen, sprite3DThorax, sprite3DLeftMandible, sprite3DRightMandible;
    [SerializeField] GameObject colliderAbdomen, colliderThorax, colliderLeftMandible, colliderRightMandible;
    [SerializeField] MandiblesController leftMandible, rightMandible;
    private Rigidbody rbBody;
    private Gamepad gamepad;
    public bool isLeft;
    public float pincerForce, moveForceHorizontal, moveForceHorizontalWhileGrabbing, moveForceVertical, moveForceVerticalWhileGrabbing, gravityWhileGrabbingForce;
    private float baselineMoveForceHorizontal, baselineMoveForceVerticle;
    private Vector3 diffRotationThroax, diffPositionThorax, diffRotationAbdomen, diffPositionAbdomen, diffRotationLeftMandible, diffRotationRightMandible;
    public BeetleController opponentBeetle;
    private bool isGrabbed;
    public bool isGrabbingLeft, isGrabbingRight;
    // Start is called before the first frame update
    void Start()
    {
        gamepad = isLeft? Gamepad.all[0] : Gamepad.all[1];
        rbBody = GetComponent<Rigidbody>();
        diffRotationThroax = colliderThorax.transform.eulerAngles - sprite3DThorax.transform.eulerAngles;
        diffPositionThorax = colliderThorax.transform.position - sprite3DThorax.transform.position;
        diffPositionThorax = sprite3DThorax.transform.InverseTransformDirection(diffPositionThorax);

        diffRotationAbdomen = colliderAbdomen.transform.eulerAngles - sprite3DAbdomen.transform.eulerAngles;
        diffPositionAbdomen = colliderAbdomen.transform.position - sprite3DAbdomen.transform.position;
        diffPositionAbdomen = sprite3DAbdomen.transform.InverseTransformDirection(diffPositionAbdomen);

        diffRotationLeftMandible = colliderLeftMandible.transform.eulerAngles - sprite3DLeftMandible.transform.eulerAngles;
        diffRotationRightMandible = colliderRightMandible.transform.eulerAngles - sprite3DRightMandible.transform.eulerAngles;

        //Debug.Log(sprite3DRightMandible.transform.eulerAngles);
        baselineMoveForceHorizontal = moveForceHorizontal;
        baselineMoveForceVerticle = moveForceVertical;
    }
    private void Update()
    {
        sprite3DAbdomen.transform.position = colliderAbdomen.transform.position - sprite3DAbdomen.transform.TransformDirection(diffPositionAbdomen); 
        sprite3DAbdomen.transform.eulerAngles = colliderAbdomen.transform.eulerAngles;
        sprite3DThorax.transform.position = colliderThorax.transform.position - sprite3DThorax.transform.TransformDirection(diffPositionThorax);
        sprite3DThorax.transform.eulerAngles = colliderThorax.transform.eulerAngles - diffRotationThroax;
        sprite3DLeftMandible.transform.eulerAngles = colliderLeftMandible.transform.eulerAngles;
        sprite3DRightMandible.transform.eulerAngles = colliderRightMandible.transform.eulerAngles;

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(gamepad.leftStick.value.x);
        if (isGrabbingLeft || isGrabbingRight)
        {
            rbAbdomen.AddForceAtPosition(Vector3.down * gravityWhileGrabbingForce, rbAbdomen.transform.position + rbAbdomen.transform.forward/1f);
            rbAbdomen.AddForceAtPosition(Vector3.up * gravityWhileGrabbingForce + (isLeft? 1f : -1f) *-Vector3.right * gravityWhileGrabbingForce, rbAbdomen.transform.position - rbAbdomen.transform.forward / 1f);

            if (isGrabbingLeft && (gamepad.leftTrigger.value > 0 || gamepad.rightTrigger.value == 0))
            {
                leftMandible.LetGo();
            }
            if (isGrabbingRight && (gamepad.leftTrigger.value > 0 || gamepad.rightTrigger.value == 0))
            {
                rightMandible.LetGo();
            }
        }
        else
        {
            if (rbThorax.transform.eulerAngles.z < 25f && !isGrabbed)
            {
                Debug.Log("forcing down" + gameObject.name);
                rbAbdomen.AddForceAtPosition(-rbAbdomen.transform.up * gravityWhileGrabbingForce * 2f, rbAbdomen.transform.position - rbAbdomen.transform.forward);
                rbThorax.AddForce(-rbThorax.transform.right * gravityWhileGrabbingForce * 2f);

            }
        }
        
        rbLeftMandible.AddForceAtPosition(gamepad.leftTrigger.value * rbLeftMandible.transform.right * pincerForce + gamepad.rightTrigger.value * -rbLeftMandible.transform.right * pincerForce, rbLeftMandible.transform.position + rbLeftMandible.transform.up);
        rbRightMandible.AddForceAtPosition(gamepad.leftTrigger.value * -rbRightMandible.transform.right * pincerForce + gamepad.rightTrigger.value * rbRightMandible.transform.right * pincerForce, rbRightMandible.transform.position + rbRightMandible.transform.up);
        
        if (Mathf.Abs(gamepad.leftStick.value.x) >= 0.2f)
        {
            rbAbdomen.AddForce(gamepad.leftStick.value.x * Vector3.right * moveForceHorizontal);
            if (Mathf.Abs(rbAbdomen.velocity.x) < 1f && gamepad.leftStick.value.x > 0.6)
            {
                moveForceHorizontal += 5f;
            }
            else
            {
                moveForceHorizontal = baselineMoveForceHorizontal;
            }
        }

        if (Mathf.Abs(gamepad.leftStick.value.x) >= 0.2f || Mathf.Abs(gamepad.leftStick.value.y) >= 0.2f)
        {
            if (isGrabbingRight || isGrabbingLeft)
            {
                moveForceVertical = moveForceVerticalWhileGrabbing;
                moveForceHorizontal = moveForceHorizontalWhileGrabbing;
            }
            else
            {
                moveForceVertical = baselineMoveForceVerticle;
                moveForceHorizontal = baselineMoveForceHorizontal;

            }
            if (rbThorax.transform.eulerAngles.z < 30f)
            {
                moveForceVertical = 0f;
            }
            else if (Mathf.Abs(rbThorax.velocity.y) < 1f)
            {
                moveForceVertical += 5f;
            }
            else
            {
                moveForceVertical = baselineMoveForceVerticle;
            }
            rbThorax.AddForce(gamepad.leftStick.value.x * Vector3.right * moveForceHorizontal + gamepad.leftStick.value.y * rbThorax.transform.right * moveForceVertical);
            if (isGrabbingLeft || isGrabbingRight)
            {
                rbThorax.AddForce(-rbThorax.transform.up * moveForceHorizontal / 1f + gamepad.leftStick.value.y * rbThorax.transform.right * moveForceVertical);

            }
        }
    }

    public void StartBeingGrabbed(Rigidbody mandible)
    {
        Debug.Log("im being grabbed, aaaaah!");
        isGrabbed = true;
        FixedJoint[] currentJoints = rbAbdomen.GetComponents<FixedJoint>();
        for (int i = 0; i < currentJoints.Length; i++)
        {
            if (currentJoints[i].connectedBody == mandible) return;
        }

        if(currentJoints.Length < 2)
        {
            FixedJoint _f = rbAbdomen.gameObject.AddComponent<FixedJoint>();
            _f.connectedBody = mandible.GetComponent<Rigidbody>();
        }
    }

    public void StopBeingGrabbed(Rigidbody mandible)
    {
        Debug.Log("no longer grabbed");
        FixedJoint[] currentJoints = rbAbdomen.GetComponents<FixedJoint>();
        if (currentJoints.Length == 1) isGrabbed = false;
        for (int i = 0; i < currentJoints.Length; i++)
        {
            if(currentJoints[i].connectedBody == mandible)
            {
                Destroy(currentJoints[i]);
                break;
            }
        }
    }

    public void StartGrabbing(bool isLeft)
    {
        if (isLeft)
        {
            isGrabbingLeft = true;
        }
        else
        {
            isGrabbingRight = true;
        }
    }

    public void StopGrabbing(bool isLeft)
    {
        if (isLeft)
        {
            isGrabbingLeft = false;
        }
        else
        {
            isGrabbingRight = false;
        }
    }
}
