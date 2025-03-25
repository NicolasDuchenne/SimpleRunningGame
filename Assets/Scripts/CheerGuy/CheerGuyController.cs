
using UnityEngine;

public class CheerGuyController : MonoBehaviour
{
    [SerializeField] float lagSpeed = 1;
    [SerializeField] float rotationSpeed = 1;
    [SerializeField] Animator animator;
    [SerializeField] float lifeExpectancy = 10f;
    private Transform activeTransform;
    private CheerGuyGlobalController cheerGuyGlobalController;
    private bool move = false;
    private bool isRight = false;
    public int currentLane {get; private set;}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activeTransform = GameController.Instance.getCheerGuyTransform();
        cheerGuyGlobalController = activeTransform.GetComponent<CheerGuyGlobalController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (activeTransform.childCount == 0 & GameController.Instance.playerInLevel != GameController.Levels.Level1)
        if (activeTransform.childCount == 0 & cheerGuyGlobalController.startCheerGuy)
        {
            transform.SetParent(activeTransform, true);
            move = true;
            Destroy(gameObject, lifeExpectancy);
            cheerGuyGlobalController.SetStartCheerGuy(false);
        }
        if (move)
        {
            if (GameController.Instance.PlayerController.transform.position.z > transform.position.z)
            {

                Move();
            }
        }

    }

    private void Move()
    {
        animator.SetBool("Running", true);
        if((GameController.Instance.PlayerController.transform.position.z-transform.position.z)*lagSpeed <1)
        {
            lagSpeed = lagSpeed+0.01f;
        }
        float tmpZ = Mathf.Lerp(
                transform.position.z,
                GameController.Instance.PlayerController.transform.position.z,
                lagSpeed * GameController.Instance.PlayerController.getForwardSpeed() * Time.deltaTime
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
        float tmpX = Mathf.Lerp(transform.position.x, targetLaneX, lagSpeed * Time.deltaTime);
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
        currentLane = (int)((tmpX+Mathf.Sign(tmpX)*GameController.Instance.laneWidth/2)/GameController.Instance.laneWidth);
    }

}
