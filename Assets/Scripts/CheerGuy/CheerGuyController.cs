
using System;
using UnityEngine;

public class CheerGuyController : MonoBehaviour
{
    [SerializeField] float startSpeed = 1;
    private float speed;
    [SerializeField] float rotationSpeed = 1;
    [SerializeField] Animator animator;
    [SerializeField] float lifeExpectancy = 10f;
    [SerializeField] float slowDownRate = 0.1f;
    [SerializeField] GameObject runningStance;
    private Transform activeTransform;
    private CheerGuyGlobalController cheerGuyGlobalController;
    private bool move = false;
    private bool isRight = false;
    public int? currentLane {get; private set;}
    private float slowDownTime;
    private float runTime=0;
    private bool slowingDown=false;
    [SerializeField] AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activeTransform = GameController.Instance.getCheerGuyTransform();
        cheerGuyGlobalController = activeTransform.GetComponent<CheerGuyGlobalController>();
        speed = startSpeed;
        slowDownTime = lifeExpectancy -2;
    }

    public void SetMove(bool value)
    {
        move = value;
        audioSource.Play();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameController.Instance.PlayerController.transform.position.z > transform.position.z & activeTransform.childCount == 0 & cheerGuyGlobalController.startCheerGuy & GameController.Instance.playerInLevel != GameController.Levels.Level1)
        {
            GameObject RunningStance = Instantiate(runningStance, transform.position, Quaternion.identity, activeTransform); // We do it like this because if we switch anim from chear to run, or character does not always look forward
            //transform.SetParent(activeTransform, true);
            RunningStance.GetComponent<CheerGuyController>().SetMove(true);
            Destroy(RunningStance, lifeExpectancy);
            cheerGuyGlobalController.SetStartCheerGuy(false);
            Destroy(gameObject);
            
        }
        if (move)
        {
            if (GameController.Instance.PlayerController.transform.position.z > transform.position.z)
            {
                runTime+=Time.deltaTime;
                Move();

            }
        }

    }

    private void Move()
    {
        animator.SetBool("Running", true);
        if(runTime<slowDownTime) 
        {
            if((GameController.Instance.PlayerController.transform.position.z-transform.position.z)*speed <1)
            {
                speed = speed+0.01f;
            }
        }
        else
        {
            slowingDown = true;
            speed = speed - slowDownRate;
        }
            
        float tmpZ = Mathf.Lerp(
                transform.position.z,
                GameController.Instance.PlayerController.transform.position.z,
                speed * GameController.Instance.PlayerController.getForwardSpeed() * Time.deltaTime
            );
        float targetLaneX;
        if (transform.position.x < GameController.Instance.PlayerController.transform.position.x)
        {
            isRight = false;
            targetLaneX = GameController.Instance.PlayerController.transform.position.x - GameController.Instance.laneWidth;
        }
        else
        {
            isRight = true;
            targetLaneX = GameController.Instance.PlayerController.transform.position.x + GameController.Instance.laneWidth;
        }
        float tmpX = Mathf.Lerp(transform.position.x, targetLaneX, speed * Time.deltaTime);
        if ((isRight & tmpX < transform.position.x) | (isRight == false & tmpX > transform.position.x))
        {
        }
        else
        {
            tmpX = transform.position.x;

        }
        Vector3 direction = new Vector3(tmpX - transform.position.x, 0, tmpZ - transform.position.z);
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.position = new Vector3(tmpX, transform.position.y, tmpZ);
        if(slowingDown == false)
        {
            currentLane = (int)((tmpX+Mathf.Sign(tmpX)*GameController.Instance.laneWidth/2)/GameController.Instance.laneWidth);
        }
        else if (GameController.Instance.PlayerController.transform.position.z-transform.position.z>2)
        {
            currentLane = null;        
        }    
    }

}
