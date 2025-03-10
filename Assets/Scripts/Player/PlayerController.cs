using System;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float startForwardSpeed = 10f;
    private float forwardSpeed;
    [SerializeField] float catchupSpeed = 1f;
    [SerializeField] float speedLose = 0.4f;
    private float totalIncreasePercent=100;
    [SerializeField] float minIncreasePercent = 80;

    public float speedMult=1;
    private float temporaryIncreasePercent;
    [SerializeField] float lateralSpeed = 20f;
    private bool isChangingLane = false;
    [SerializeField] float increaseSpeedDelaySec= 3f;
    [SerializeField] float increasePercent = 1f;
    private float maxIncreasePercent = 300f; // need to change player blend animation if this is changed
    private int lane = 0;
    private float targetX;
    private int numberOfLaneChange = 1;
    private int realNumberOfLaneChange = 1;
    private bool doubleLaneChange => realNumberOfLaneChange==1? false : true;
    private GameObject Models;
    private GameObject Colliders;
    private bool hasDisappeared = false;
    private PlayerInputs playerInputs;

    private Rigidbody rb;

    private Vector3 targetPosition;

    private Animator animator;




    void Start()
    {
        animator = transform.Find("Models").transform.Find("PlayerModel").GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        Models = transform.Find("Models").transform.Find("PlayerModel").transform.Find("head_low").gameObject;
        Colliders = transform.Find("Colliders").gameObject;
        playerInputs = GetComponent<PlayerInputs>();
        forwardSpeed = startForwardSpeed;
        temporaryIncreasePercent = totalIncreasePercent;
        InvokeRepeating("IncreaseSpeed", increaseSpeedDelaySec, increaseSpeedDelaySec); // Call `ChangeValue` every 2 seconds    
        SetAnimationBlendSpeed();
    }

    private void IncreaseSpeed()
    {
        totalIncreasePercent+=increasePercent;
        totalIncreasePercent = Math.Min(maxIncreasePercent, totalIncreasePercent);
        
    }

    private void SetAnimationBlendSpeed()
    {
        animator.SetFloat("Blend", speedMult);
    }


    void FixedUpdate()
    {
        
        targetPosition = transform.position;
        ProcessForwardMovement();
        ProcessLane();
        ProcessSpeedPercent();
        rb.MovePosition(targetPosition);
        
    }

    private void ProcessSpeedPercent()
    {
        temporaryIncreasePercent = Mathf.Lerp(temporaryIncreasePercent, totalIncreasePercent,catchupSpeed*Time.deltaTime);
        speedMult = temporaryIncreasePercent/100;
        forwardSpeed = startForwardSpeed * speedMult;
        SetAnimationBlendSpeed();
    }

    private void ProcessForwardMovement()
    {
        targetPosition += Vector3.forward * forwardSpeed * Time.deltaTime;
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
        if(playerInputs.disappear)
        {
            numberOfLaneChange = 2;
        }
    }

    private void StartLaneChange()
    {
        if (isChangingLane == false)
        {
            float horizontalInput = playerInputs.horizontalInput;
            if (Math.Abs(horizontalInput) > 0.2f)
            {
                int newLane = Math.Clamp(lane + Math.Sign(horizontalInput) * numberOfLaneChange, GameController.Instance.minLane, GameController.Instance.maxLane);
                if (lane != newLane)
                {
                    realNumberOfLaneChange = Math.Abs(lane-newLane);
                    
                    targetX = transform.position.x + Math.Sign(horizontalInput) * GameController.Instance.laneWidth * realNumberOfLaneChange;
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
                if (doubleLaneChange)
                {
                    Appear();
                }
            }
            else if (distance<numberOfLaneChange*GameController.Instance.laneWidth*0.2)
            {
                if (doubleLaneChange)
                {
                    Appear();
                }
            }
            else if (distance<numberOfLaneChange*GameController.Instance.laneWidth*0.8)
            {
                
                if(doubleLaneChange)
                {
                    Disappear();
                }
            }
            
            targetPosition += new Vector3(tmpX-transform.position.x, 0, 0);        
        }
    }


    private void Disappear()
    {
        if (hasDisappeared == false)
        {
            Models.SetActive(false);
            //Colliders.GetComponent<BoxCollider>().enabled= false;
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
            //Colliders.GetComponent<BoxCollider>().enabled= true;
            hasDisappeared = false;
        } 
    }

    public void LoseSpeed()
    {
        temporaryIncreasePercent = Math.Max(minIncreasePercent,(1-speedLose)*totalIncreasePercent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent.gameObject.CompareTag("Obstacle"))
        {
            other.transform.parent.GetComponent<ObstacleController>().ProcessCollision(speedMult);
            LoseSpeed();
        }
    }
}
