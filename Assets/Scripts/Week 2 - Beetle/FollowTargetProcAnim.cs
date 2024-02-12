using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetProcAnim : MonoBehaviour
{
    [SerializeField] GameObject targetToFollow;
    [SerializeField] float distance;
    [SerializeField] List<FollowTargetProcAnim> limbsToCheck;
    private float originalDistance;
    public bool _isMoving, _willMove;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(targetToFollow.transform.position.x, 0f, targetToFollow.transform.position.z);
        originalDistance = distance;
        _willMove = true;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (_willMove)
        {
            if (_isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetToFollow.transform.position.x, targetToFollow.transform.position.y, targetToFollow.transform.position.z), 0.16f);

                //transform.position = Vector3.MoveTowards(transform.position, targetToFollow.transform.position, 0.2f);
                if (Mathf.Abs(transform.position.x - targetToFollow.transform.position.x) < 0.02f &&
                    Mathf.Abs(transform.position.y - targetToFollow.transform.position.y) < 0.02f &&
                    Mathf.Abs(transform.position.z - targetToFollow.transform.position.z) < 0.02f)
                {
                    _isMoving = false;
                }
            }
            else
            {
                float d = Vector3.Distance(targetToFollow.transform.position, transform.position);
                if (d > distance && !AreOtherLimbsMoving())
                {
                    _isMoving = true;
                }
            }
            
        }
        //_willMove = !_willMove;
    }
    public bool AreOtherLimbsMoving()
    {
        bool _AreOtherLimbsMoving = false;
        for (int i = 0; i < limbsToCheck.Count; i++)
        {
            if (limbsToCheck[i]._isMoving)
            {
                _AreOtherLimbsMoving = true;
                break;
            }
        }
        return _AreOtherLimbsMoving;
    }
}
