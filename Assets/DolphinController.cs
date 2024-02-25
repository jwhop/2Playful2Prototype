using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DolphinController : MonoBehaviour
{
    [SerializeField] Rigidbody bodyRb;
    private Gamepad gamepad;
    [SerializeField] GameObject sprite3D, bodyCollider;
    // Start is called before the first frame update
    void Start()
    {
        gamepad = Gamepad.current;
    }

    // Update is called once per frame
    void Update()
    {
        sprite3D.transform.position = bodyCollider.transform.position;
        sprite3D.transform.eulerAngles = bodyCollider.transform.eulerAngles;
        if (gamepad.aButton.wasReleasedThisFrame)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void FixedUpdate()
    {
        //bodyRb.AddForceAtPosition(gamepad.leftStick.value.x * bodyCollider.transform.right, bodyCollider.transform.position + bodyCollider.transform.forward);
        bodyRb.AddTorque(gamepad.leftStick.value.x * bodyCollider.transform.up + gamepad.leftStick.value.y * bodyCollider.transform.forward);
        bodyRb.AddForce(gamepad.rightTrigger.value * -bodyCollider.transform.right * 2.5f);
    }
}
