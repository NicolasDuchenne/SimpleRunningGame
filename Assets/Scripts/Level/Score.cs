using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance;

    public float time {get; private set;}
    public float score {get; private set;}
    public int mult {get; private set;}
    public float timePerRoad {get; private set;}

    private float tmpTimePerRoad;

    public float damagePerRoad {get; private set;}
    private float tmpDamagePerRoad;
    public float scoreSameSerum {get; private set;} = 0;
    [SerializeField] int multIncreaseRate = 10;
    public float FPS;
    public Serum.SerumType lastSerum {get; private set;}

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

    public void initScore()
    {
        time = 0;
        score = 0;
        mult = 1;
        timePerRoad = 0;
        tmpTimePerRoad = 0;
        damagePerRoad = 0;
        tmpDamagePerRoad = 0;
    }

    void Update()
    {
        time+=Time.deltaTime;
        tmpTimePerRoad+=Time.deltaTime;
        FPS = 1/Time.deltaTime;
    }

    public void IncreaseScore(Serum.SerumType serum)
    {
        if (serum == lastSerum)
        {
            scoreSameSerum+=1;
            if ((scoreSameSerum % multIncreaseRate) == 0)
            {
                IncreaseMult();
            }
        }
        else
        {
            ResetMult();
        }
        lastSerum = serum;
        score +=mult;
    }

    public void IncreaseMult()
    {
        mult+=1;
    }
    public void ResetMult()
    {
        mult = 1;
        scoreSameSerum = 0;
    }

    public void IncrementDamage(float damage)
    {
        tmpDamagePerRoad += damage;
    }
    
    public void EndOfRoad()
    {
        timePerRoad = tmpTimePerRoad;
        damagePerRoad = tmpDamagePerRoad;
        tmpTimePerRoad = 0;
        tmpDamagePerRoad = 0;
    }
}
