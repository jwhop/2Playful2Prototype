using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MonkeyManager : MonoBehaviour
{
    [Header("WHICH MONKEY ARE YOU")]
    public bool isLeft;
    [Header("MESH STUFF")]
    public GameObject sprite3D; // actual mesh
    public GameObject monkeyCollider, monkeyHeadCollider; //has rb and collider 
    public GameObject LeftHandTargetObject, RightHandTargetObject; //targets for IK
    public GameObject reachTargetFront, reachTargetBack;
    public Rigidbody featherBody;
    private Vector3 originalLeftHandPos, originalRightHandPos; // local positions of both hands to revert to when not reaching
    private Vector3 originalStartingPosBody, originalStartingPosHead;
    private Quaternion originalStartingRotationBody, originalStartingRotationHead;
    private HandTargets LeftHandTarget, RightHandTarget;
    private HingeJoint LeftHandJoint, RightHandJoint;
    public GameObject startingGround;
    public float AttachRungForce;
    private Gamepad gamepad; //controller
    public GameObject LeftHandIndicator, RightHandIndicator;
    [Header("FORCES")]
    public float forceAmount;
    public float swingAmount;
    public float featherMoveAmount; //amount to raise hands
    public GameObject featherObject;
    private GameObject LeftHandRung, RightHandRung; //currently attached rungs
    [Header("UI OBJECT")]
    public GameObject LifeHolder;
    private Rigidbody rb;
    private bool isDying = false;
    public float damageForce = 0f;
    private float laughTimer = .25f;
    // Start is called before the first frame update
    void Start()
    {
        gamepad = isLeft? Gamepad.all[0] : Gamepad.all[1];
        LeftHandRung = null;
        LeftHandJoint = null;
        RightHandRung = null;
        RightHandJoint = null;
        LeftHandTarget = LeftHandTargetObject.GetComponent<HandTargets>();
        RightHandTarget = RightHandTargetObject.GetComponent<HandTargets>();
        originalLeftHandPos = LeftHandTargetObject.transform.localPosition;
        originalRightHandPos = RightHandTargetObject.transform.localPosition;
        //monkeyCollider.GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, 3f, 0f);
        rb = GetComponent<Rigidbody>();
        originalStartingPosBody = monkeyCollider.transform.position;
        originalStartingRotationBody = monkeyCollider.transform.rotation;

        originalStartingPosHead = monkeyHeadCollider.transform.position;
        originalStartingRotationHead = monkeyHeadCollider.transform.rotation;
        InvokeRepeating("Laugh", .25f, .25f);
    }
    void Laugh()
    {
        monkeyCollider.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f) * damageForce * 60f, ForceMode.Impulse);
    }
    // Update is called once per frame
    void Update()
    {
        if (gamepad.aButton.wasReleasedThisFrame) Respawn();
        sprite3D.transform.position = monkeyCollider.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f) * damageForce;
        sprite3D.transform.eulerAngles = monkeyCollider.transform.eulerAngles;
    }

    private void FixedUpdate()
    {
        if (StartTimer.Instance.canMove)
        {
            //Debug.Log(gamepad.leftTrigger.value);
            if (gamepad.leftTrigger.value < 1 && gamepad.leftShoulder.value == 0 && LeftHandTarget.isAttached) LeftHandTarget.DetachRung();
            if (gamepad.rightTrigger.value < 1 && gamepad.rightShoulder.value == 0 && RightHandTarget.isAttached) RightHandTarget.DetachRung();

            //if (LeftHandTarget.isAttached || RightHandTarget.isAttached) //this doesnt work with two hinge joints for some reason 
            {
                monkeyCollider.GetComponent<Rigidbody>().AddForce(Vector3.right * gamepad.leftStick.value.x * swingAmount);
            }

            if (gamepad.leftTrigger.value > 0 && !LeftHandTarget.isAttached)
            {
                LeftHandTargetObject.transform.position = Vector3.MoveTowards(LeftHandTargetObject.transform.position, new Vector3(reachTargetFront.transform.position.x, reachTargetFront.transform.position.y, LeftHandTargetObject.transform.position.z), 0.25f);
            }
            else if (gamepad.leftShoulder.value > 0 && !LeftHandTarget.isAttached)
            {
                LeftHandTargetObject.transform.position = Vector3.MoveTowards(LeftHandTargetObject.transform.position, new Vector3(reachTargetBack.transform.position.x, reachTargetBack.transform.position.y, LeftHandTargetObject.transform.position.z), 0.25f);
            }
            else if (gamepad.leftTrigger.value == 0 && gamepad.leftShoulder.value == 0)
            {
                LeftHandTargetObject.transform.localPosition = Vector3.MoveTowards(LeftHandTargetObject.transform.localPosition, originalLeftHandPos, 0.05f);
            }
            if (gamepad.rightTrigger.value > 0 && !RightHandTarget.isAttached)
            {
                RightHandTargetObject.transform.position = Vector3.MoveTowards(RightHandTargetObject.transform.position, new Vector3(reachTargetFront.transform.position.x, reachTargetFront.transform.position.y, RightHandTargetObject.transform.position.z), 0.25f);
            }
            else if (gamepad.rightShoulder.value > 0 && !RightHandTarget.isAttached)
            {
                RightHandTargetObject.transform.position = Vector3.MoveTowards(RightHandTargetObject.transform.position, new Vector3(reachTargetBack.transform.position.x, reachTargetBack.transform.position.y, RightHandTargetObject.transform.position.z), 0.25f);
            }
            else if (gamepad.rightTrigger.value == 0 && gamepad.rightShoulder.value == 0)
            {
                RightHandTargetObject.transform.localPosition = Vector3.MoveTowards(RightHandTargetObject.transform.localPosition, originalRightHandPos, 0.05f);
            }

            //if(featherBody != null && featherObject != null)
            {
                if (Mathf.Abs(gamepad.rightStick.value.x) < 0.1f && Mathf.Abs(gamepad.rightStick.value.y) < 0.1f)
                {
                    if (!isLeft) Debug.Log("retreating tail");
                    featherBody.transform.position = Vector3.MoveTowards(featherBody.transform.position, featherObject.transform.position, 0.05f);
                }
                else if (Mathf.Abs(Vector3.Distance(featherBody.transform.position, featherObject.transform.position)) <= 4f)
                {
                    if (!isLeft) Debug.Log("translating along input");
                    featherBody.transform.Translate(Vector3.forward * gamepad.rightStick.value.y * featherMoveAmount + Vector3.up * (isLeft ? gamepad.rightStick.value.x : -gamepad.rightStick.value.x) * featherMoveAmount);
                }
                else
                {
                    if (!isLeft) Debug.Log("translating along circle");
                    float angle = Mathf.Atan2(gamepad.rightStick.value.y, isLeft ? gamepad.rightStick.value.x : -gamepad.rightStick.value.x);
                    featherBody.transform.position = featherObject.transform.position + (4f * Mathf.Cos(angle) * Vector3.left) + (4f * Mathf.Sin(angle) * Vector3.up);
                }
            }

            //LeftHandTargetObject.GetComponent<Rigidbody>().AddForce(LeftHandTargetObject.transform.forward * gamepad.leftTrigger.value * forceAmount, ForceMode.Force);
            //RightHandTargetObject.GetComponent<Rigidbody>().AddForce(Vector3.up * gamepad.rightTrigger.value * forceAmount, ForceMode.Force);

        }

        //if (gamepad.leftShoulder.isPressed) monkeyCollider.GetComponent<Rigidbody>().AddForce((LeftHandRung.transform.position - monkeyCollider.transform.position).normalized);
    }
    public void Respawn()
    {
        if(!isDying) StartCoroutine(RespawnMonkeyRoutine());
        
        
    }
    IEnumerator RespawnMonkeyRoutine()
    {
        isDying = true;
        EnableStartingGround();
        damageForce = 0f;
        if(LifeHolder != null) LifeHolder.GetComponent<LifeHolder>().Died();
        yield return new WaitForSeconds(1f);
        monkeyCollider.GetComponent<Rigidbody>().velocity = Vector3.zero;
        monkeyCollider.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        monkeyCollider.transform.position = originalStartingPosBody;
        monkeyCollider.transform.rotation = originalStartingRotationBody;

        monkeyHeadCollider.GetComponent<Rigidbody>().velocity = Vector3.zero;
        monkeyHeadCollider.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        monkeyHeadCollider.transform.position = originalStartingPosHead;
        monkeyHeadCollider.transform.rotation = originalStartingRotationHead;
        isDying = false;
    }
    public void EnableStartingGround()
    {
        startingGround.SetActive(true);
    }

    public void DisableStartingGround()
    {
        startingGround.SetActive(false);
    }
    public void AttachToRung(GameObject g, bool isLeft)
    {
        if (startingGround.activeSelf) DisableStartingGround();
        //monkeyCollider.GetComponent<Rigidbody>().AddForce((transform.position - g.transform.position).normalized * AttachRungForce, ForceMode.Impulse );
        HingeJoint _hj = monkeyHeadCollider.AddComponent<HingeJoint>();

        Vector3 _localSpaceRungPoint = monkeyHeadCollider.transform.InverseTransformPoint(isLeft? LeftHandTarget.transform.position : RightHandTarget.transform.position);
        Debug.Log(LeftHandTarget.transform.position);
        _hj.anchor = new Vector3(_localSpaceRungPoint.x, _localSpaceRungPoint.y, _localSpaceRungPoint.z); //no idea why i have to transpose x and z (can i swizzle this?)
        _hj.axis = new Vector3(0f, 0f, 1f);
        _hj.connectedBody = g.GetComponent<Rigidbody>();

        JointLimits _jl = _hj.limits;
        _jl.max = 110;
        _jl.min = 0;
        _hj.limits = _jl;
        //_hj.useLimits = true;
        //JointMotor _jm = _hj.motor;
        //_jm.targetVelocity = 100f;
        //_jm.force = 100f;
        //_hj.motor = _jm;
        if (isLeft)
        {
            LeftHandRung = g;
            LeftHandJoint = _hj;
            LeftHandIndicator.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            RightHandRung = g;
            RightHandJoint = _hj;
            RightHandIndicator.GetComponent<MeshRenderer>().enabled = true;
        }
    }
    public void TakeDamage()
    {
        damageForce += Random.Range(.05f, .25f);
    }
    public void DetachRung(bool isLeft)
    {
        if (isLeft)
        {
            LeftHandRung = null;
            Destroy(LeftHandJoint);
            LeftHandJoint = null;
            LeftHandIndicator.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            RightHandRung = null;
            Destroy(RightHandJoint);
            RightHandJoint = null; 
            RightHandIndicator.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public bool IsHandFree(bool isLeft)
    {
        return isLeft ? LeftHandRung == null : RightHandRung == null;
    }
}
