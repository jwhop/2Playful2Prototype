using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionUpdate : MonoBehaviour
{
    public float timerLength;
    [SerializeField] RenderTexture renderTexture;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.clearFlags = CameraClearFlags.Nothing;
        InvokeRepeating("UpdateCameraPosition", 0.1f, timerLength);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(Input.GetAxis("Vertical") *  transform.forward);
    }

    void UpdateCameraPosition()
    {
        Debug.Log("updating camera position");
        Shader.SetGlobalVector("_CamPosition", transform.position);
        Shader.SetGlobalVector("_CamDirection", transform.forward);
        //RenderTexture.active = renderTexture;
        StartCoroutine(FixCameraClearing());
    }
    IEnumerator FixCameraClearing()
    {
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        yield return new WaitForSeconds(0.1f);
        Camera.main.clearFlags = CameraClearFlags.Nothing;
    }
}
