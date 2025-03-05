using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private PlayerLife PlayerLife;

    public bool playerDead {get; private set;} = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    void Start()
    {
        PlayerLife = GameObject.Find("Player").GetComponent<PlayerLife>();
    }

    void Update()
    {
        CheckPlayerIsDead();
    }

    private void CheckPlayerIsDead()
    {
        playerDead = PlayerLife.isDead; 
    }

    
    
}
