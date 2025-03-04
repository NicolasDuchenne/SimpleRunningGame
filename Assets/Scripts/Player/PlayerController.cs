using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float forwardSpeed = 10f;
    [SerializeField] float lateralSpeed = 20f;
    private bool isChangingLane = false;
    [SerializeField] float laneWidth = 2f;
    private int lane = 0;
    private int minLane = -2;
    private int maxLane = 2;
    private float targetX;
    private int numberOfLaneChange = 1;
    private int realNumberOfLaneChange = 1;
    private bool doubleLaneChange => realNumberOfLaneChange==1? false : true;
    private GameObject Models;
    private GameObject Colliders;
    private bool hasDisappeared = false;

    void Start()
    {
        Models = transform.Find("Models").gameObject;
        Colliders = transform.Find("Colliders").gameObject;
    }

    
    void Update()
    {
        ProcessForwardMovement();
        ProcessLane();
    }

    private void ProcessForwardMovement()
    {
        transform.position = transform.position + Vector3.forward * forwardSpeed * Time.deltaTime;
    }
    private void ProcessLane()
    {
        ProcessNumberOfLaneChange();
        StartLaneChange();
        ProcessLaneChange();
    }

    private void ProcessNumberOfLaneChange()
    {
        numberOfLaneChange = 1;
        if(Input.GetKey(KeyCode.Space))
        {
            numberOfLaneChange = 2;
        }
    }

    private void StartLaneChange()
    {
        if (isChangingLane == false)
        {
            float horizontalInput =  Input.GetAxis("Horizontal");
            if (Math.Abs(horizontalInput) > 0.2f)
            {
                int newLane = Math.Clamp(lane + Math.Sign(horizontalInput) * numberOfLaneChange, minLane, maxLane);
                if (lane != newLane)
                {
                    realNumberOfLaneChange = Math.Abs(lane-newLane);
                    
                    targetX = transform.position.x + Math.Sign(horizontalInput) * laneWidth * realNumberOfLaneChange;
                    isChangingLane = true;
                    lane = newLane;
                }
            }
        }       
    }

    private void ProcessLaneChange()
    {
        if (isChangingLane)
        {
            float tmpX = Mathf.Lerp(transform.position.x, targetX, lateralSpeed * Time.deltaTime);
            float distance = Math.Abs(tmpX-targetX);
            if (distance<0.05)
            {
                tmpX = targetX;
                isChangingLane = false;
            }
            else if (distance<laneWidth*0.2)
            {
                if (doubleLaneChange)
                {
                    Appear();
                }
            }
            else if (distance<laneWidth*0.8)
            {
                if(doubleLaneChange)
                {
                    Disappear();
                }
            }
            
            
            transform.position = new Vector3(tmpX, transform.position.y, transform.position.z);
            
        }
    }

    private void Disappear()
    {
        if (hasDisappeared == false)
        {
            Models.SetActive(false);
            Colliders.SetActive(false);
            hasDisappeared = true;
        }
        
    }

    private void Appear()
    {
        if (hasDisappeared)
        {
            Models.SetActive(true);
            Colliders.SetActive(true);
            hasDisappeared = false;
        } 
    }
}
