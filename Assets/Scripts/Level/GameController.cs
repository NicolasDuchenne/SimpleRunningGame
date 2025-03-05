using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private PlayerLife PlayerLife;

    [SerializeField] public float laneWidth = 3f;
    [SerializeField] float level2StartSec = 5f;
    [SerializeField] float level3StartSec = 10f;
    public int minLane {get; private set;} = -1;
    public int maxLane {get; private set;} = 1;

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
        Invoke("StartLevel2", level2StartSec);
        Invoke("StartLevel3", level3StartSec);
    }

    void Update()
    {
        CheckPlayerIsDead();
    }

    private void CheckPlayerIsDead()
    {
        playerDead = PlayerLife.isDead; 
    }

    private void StartLevel2()
    {
        PlayerLife.ActivateLifeBar(Serum.SerumType.gamma);
        maxLane = 2;
    }

    private void StartLevel3()
    {
        PlayerLife.ActivateLifeBar(Serum.SerumType.iota);
        minLane = -2;
    }

    
    
}
