using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
    public bool isLeft;
    public Collider ownFeather, headCollider;
    private void Start()
    {
        Physics.IgnoreCollision(ownFeather, GetComponent<BoxCollider>());
        Physics.IgnoreCollision(ownFeather, headCollider);
    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("StartButton"))
        {
            Debug.Log("collided with start");
            collision.gameObject.transform.parent.GetComponent<StartButtons>().ReadyPlayer();
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            //respawn monkey
            transform.parent.GetComponent<MonkeyManager>().Respawn();
        }
        else if (collision.gameObject.CompareTag("Feather"))
        {
            transform.parent.GetComponent<MonkeyManager>().TakeDamage();
        }
    }
}
