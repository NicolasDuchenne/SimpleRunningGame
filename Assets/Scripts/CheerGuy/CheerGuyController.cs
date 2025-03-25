using UnityEngine;

public class CheerGuyController : MonoBehaviour
{
    [SerializeField] float lagSpeed = 5;
    [SerializeField] float rotationSpeed = 1;
    [SerializeField] Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameController.Instance.PlayerController.transform.position.z > transform.position.z)
        {
            animator.SetBool("Running", true);
            float tmpZ= Mathf.Lerp(transform.position.z, GameController.Instance.PlayerController.transform.position.z, lagSpeed*Time.deltaTime);
            float targetLaneX;
            if(transform.position.x<GameController.Instance.PlayerController.transform.position.x)
            {
                targetLaneX = GameController.Instance.PlayerController.transform.position.x-GameController.Instance.laneWidth;
            }
            else
            {
                targetLaneX = GameController.Instance.PlayerController.transform.position.x+GameController.Instance.laneWidth;
            }
            float tmpX= Mathf.Lerp(transform.position.x, targetLaneX, lagSpeed*Time.deltaTime);
            Vector3 direction = new Vector3(tmpX-transform.position.x, 0, tmpZ-transform.position.z);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            transform.position = new Vector3(tmpX, transform.position.y, tmpZ);


        }
    }

}
