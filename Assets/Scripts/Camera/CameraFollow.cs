using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Should be one of these on the camera in each scene
    [SerializeField]
    float minX;
    [SerializeField]
    float minY;
    [SerializeField]
    float maxX;
    [SerializeField]
    float maxY;

    [SerializeField]
    private Transform target;

    [SerializeField]
    public Vector3 cameraOffset;

    [SerializeField]
    private float followSpeed = 10f;

    [SerializeField]
    private float xMinClamp = 0f;

    private Vector3 velocity = Vector3.zero;
    private float panSpeed = 0.05f;
    private Vector3 panVector;
    public bool isPanning;

    [Header("Panning To Other Objects")]
    private bool isFollowingPlayer = true;
    private Vector3 otherTarget;
    private float maxDistDelta = .01f;
    public bool panToPlayer = false;

    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {
        if (!isFollowingPlayer)
        {
            // Check if the object has reached the target
            if (!(Vector2.Distance(transform.position, otherTarget) < maxDistDelta))
            {
                // Move the object towards the target position
                transform.position = Vector3.MoveTowards(transform.position, otherTarget, panSpeed);
            }
        }
        else if (panToPlayer)
        {
            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            else
            {
                //If far away from player, pan to them instead
                Vector3 targetPos = target.position + cameraOffset;
                Vector3 tryVec = new Vector3(targetPos.x, targetPos.y, -10);
                if (tryVec.y > maxY)
                {
                    //print("hitting maxY");
                    tryVec.y = maxY;
                }
                if (tryVec.y < minY)
                {
                    //print("hitting minY");
                    tryVec.y = minY;
                }
                if (tryVec.x > maxX)
                {
                    //print("hitting maxX");
                    tryVec.x = maxX;
                }
                if (tryVec.x < minX)
                {
                    //print("hitting minX");
                    tryVec.x = minX;
                }
                transform.position = Vector3.MoveTowards(transform.position, tryVec, panSpeed);
                if (Vector2.Distance(transform.position, target.position) < maxDistDelta)
                {
                    panToPlayer = false;
                }
            }
        }
        else
        {
            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            if (target != null && !isPanning)
            {
                Vector3 targetPos = target.position + cameraOffset;
                Vector3 clampedPos = new Vector3(Mathf.Clamp(targetPos.x, xMinClamp, float.MaxValue), targetPos.y, -10);
                Vector3 smoothPos = Vector3.SmoothDamp(transform.position, clampedPos, ref velocity, followSpeed * Time.fixedDeltaTime);
                Vector3 tryVec = new Vector3(targetPos.x, targetPos.y, -10);
                //TODO: Have this stop moving in corners
                //Idea on how to do it: each scene has a max and min x and y for the camera
                if (tryVec.y > maxY)
                {
                    //print("hitting maxY");
                    tryVec.y = maxY;
                }
                if (tryVec.y < minY)
                {
                    //print("hitting minY");
                    tryVec.y = minY;
                }
                if (tryVec.x > maxX)
                {
                    //print("hitting maxX");
                    tryVec.x = maxX;
                }
                if (tryVec.x < minX)
                {
                    //print("hitting minX");
                    tryVec.x = minX;
                }
                transform.position = tryVec;
            }
            if (isPanning)
            {
                transform.Translate(panVector);
                if (panVector.y == 0)
                {
                    //Horizontal
                    if (transform.position.x > minX && panVector.x > 0 ||
                        transform.position.x < maxX && panVector.x < 0)
                    {
                        isPanning = false;
                    }
                }
                else
                {
                    if (transform.position.y > minY && panVector.y > 0 ||
                        transform.position.y < minY && panVector.y < 0)
                    {
                        isPanning = false;
                    }
                }
            }
        }
    }

    public void setBounds(float xmin, float ymin, float xmax, float ymax, bool isHorizontal, bool shouldPan)
    {

        isPanning = shouldPan;
        if (shouldPan)
        {
            if (isHorizontal)
            {
                if (minX < xmin)
                {
                    panVector = new Vector3(panSpeed, 0, 0);
                }
                else
                {
                    panVector = new Vector3(-1 * panSpeed, 0, 0);
                }
            }
            else
            {
                if (minY < ymin)
                {
                    panVector = new Vector3(0, panSpeed, 0);
                }
                else
                {
                    panVector = new Vector3(0, -1 * panSpeed, 0);
                }
            }
        }
        minX = xmin;
        minY = ymin;
        maxX = xmax;
        maxY = ymax;
    }

    //Starts the Camera panning towards something else
    public void holdCamera(Vector3 towards)
    {
        //Debug.Log("Holding Camera");
        isFollowingPlayer = false;
        otherTarget = new Vector3(towards.x, towards.y, -10);
    }

    public void releaseCamera(bool shouldPanToPlayer)
    {
        panToPlayer = shouldPanToPlayer;
        isFollowingPlayer = true;
    }

    public Vector3 getPos()
    {
        Vector3 targetPos = target.position + cameraOffset;
        Vector3 clampedPos = new Vector3(Mathf.Clamp(targetPos.x, xMinClamp, float.MaxValue), targetPos.y, -10);
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, clampedPos, ref velocity, followSpeed * Time.fixedDeltaTime);
        Vector3 tryVec = new Vector3(targetPos.x, targetPos.y, -10);
        //TODO: Have this stop moving in corners
        //Idea on how to do it: each scene has a max and min x and y for the camera
        if (tryVec.y > maxY)
        {
            //print("hitting maxY");
            tryVec.y = maxY;
        }
        if (tryVec.y < minY)
        {
            //print("hitting minY");
            tryVec.y = minY;
        }
        if (tryVec.x > maxX)
        {
            //print("hitting maxX");
            tryVec.x = maxX;
        }
        if (tryVec.x < minX)
        {
            //print("hitting minX");
            tryVec.x = minX;
        }
        return tryVec;
    }

    public Vector3 getClampedPos(Vector3 basePos)
    {
        Vector3 clampedPos = new Vector3(Mathf.Clamp(basePos.x, xMinClamp, float.MaxValue), basePos.y, -10);
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, clampedPos, ref velocity, followSpeed * Time.fixedDeltaTime);
        Vector3 tryVec = new Vector3(basePos.x, basePos.y, -10);
        //TODO: Have this stop moving in corners
        //Idea on how to do it: each scene has a max and min x and y for the camera
        if (tryVec.y > maxY)
        {
            //print("hitting maxY");
            tryVec.y = maxY;
        }
        if (tryVec.y < minY)
        {
            //print("hitting minY");
            tryVec.y = minY;
        }
        if (tryVec.x > maxX)
        {
            //print("hitting maxX");
            tryVec.x = maxX;
        }
        if (tryVec.x < minX)
        {
            //print("hitting minX");
            tryVec.x = minX;
        }
        return tryVec;
    }
}
