using System;
using TMPro;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float startForwardSpeed = 10f;
    [SerializeField] float rotationSpeed = 250f;
    public float forwardSpeed {get; private set;}
    [SerializeField] float catchupSpeed = 1f;
    [SerializeField] float speedLose = 0.4f;
    public float totalIncreasePercent{get; private set;}=100;
    [SerializeField] float minIncreasePercent = 80;

    private float speedMult=1;
    private float temporaryIncreasePercent;
    [SerializeField] float startCrossLaneTime = 0.3f;
    private float crossLaneTime;
    private bool isChangingLane = false;
    [SerializeField] float increaseSpeedDelaySec= 3f;
    [SerializeField] float increasePercent = 1f;
    public float maxIncreasePercent {get; private set;} = 300f ; // need to change player blend animation if this is changed
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

    [SerializeField] Animator animator;

    private float velocity = 0.0f;

    [SerializeField] GameObject smokeBombPrefab; 
    private CheerGuyGlobalController cheerGuyLevelController;
    [SerializeField] GameObject CheerGuy;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cheerGuyLevelController = CheerGuy.transform.GetComponent<CheerGuyGlobalController>();
        Models = transform.Find("Models").transform.Find("PlayerModel").transform.Find("head_low").gameObject;
        Colliders = transform.Find("Colliders").gameObject;
        playerInputs = GetComponent<PlayerInputs>();
        forwardSpeed = startForwardSpeed;
        crossLaneTime = startCrossLaneTime;
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
    
    public float getForwardSpeed()
    {
        return startForwardSpeed*speedMult;
    }


    void FixedUpdate()
    {
        targetPosition = transform.position;
        ProcessForwardMovement();
        ProcessLane();
        ProcessSpeedPercent();

        Vector3 direction = new Vector3(targetPosition.x-transform.position.x, 0, targetPosition.z-transform.position.z);
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        rb.MovePosition(targetPosition); // pas nécéssaire car le rigid body est kinematic.
        
    }


    private void ProcessSpeedPercent()
    {
        temporaryIncreasePercent = Mathf.Lerp(temporaryIncreasePercent, totalIncreasePercent,catchupSpeed*Time.deltaTime);
        speedMult = temporaryIncreasePercent/100;
        forwardSpeed = getForwardSpeed();
        crossLaneTime = startCrossLaneTime / speedMult;
        SetAnimationBlendSpeed();
    }

    private void ProcessForwardMovement()
    {
        targetPosition.z += forwardSpeed * Time.deltaTime;
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
        if (isChangingLane == false & animator.GetBool("Stumble") == false)
        {
            float horizontalInput = playerInputs.horizontalInput;
            if (Math.Abs(horizontalInput) > 0.2f)
            {
                int newLane = Math.Clamp(lane + Math.Sign(horizontalInput) * numberOfLaneChange, GameController.Instance.minLane, GameController.Instance.maxLane);

                if (lane != newLane & cheerGuyLevelController.cheerGuyLane!=newLane)
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
            float tmpX;
            tmpX = Mathf.SmoothDamp(transform.position.x, targetX, ref velocity, crossLaneTime);
            float distance = Math.Abs(tmpX-targetX);
            if (distance<0.01)
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
            
            targetPosition.x += tmpX-transform.position.x;        
        }
    }


    private void Disappear()
    {
        if (hasDisappeared == false)
        {
            Models.SetActive(false);
            Colliders.SetActive(false);
            hasDisappeared = true;
            SpawnSmokeBomb();
        }
        
    }

    private void Appear()
    {
        if (hasDisappeared)
        {
            Models.SetActive(true);
            Colliders.SetActive(true);
            hasDisappeared = false;
            //SpawnSmokeBomb();
        } 
    }

    public void LoseSpeed()
    {
        temporaryIncreasePercent = Math.Max(minIncreasePercent,(1-speedLose)*totalIncreasePercent);
        StartStumble();
    }

    public void StartStumble()
    {
        animator.SetBool("Stumble", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent.gameObject.CompareTag("Obstacle"))
        {
            other.transform.parent.GetComponent<ObstacleController>().ProcessCollision(speedMult);
            LoseSpeed();
        }
    }

    private void SpawnSmokeBomb()
    {
        // Instancier le prefab à la position du joueur
        GameObject smokeBomb = Instantiate(smokeBombPrefab, transform.position, Quaternion.identity);

        // Récupérer le système de particules et jouer l'effet
        ParticleSystem particleSystem = smokeBomb.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
            Destroy(smokeBomb, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
        }
    }
}
