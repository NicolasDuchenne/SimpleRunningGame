using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] float lifeLoseRate = 1f;

    [SerializeField] LifeBarController[] lifeBars;
    public static Controller Instance;

    public bool playerDead = false;
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

    void Update()
    {
        ApplyConstantDamage();
    }

    private void ApplyConstantDamage()
    {
        foreach (LifeBarController lifebarController in lifeBars)
        {
            playerDead = lifebarController.TakeDamage(lifeLoseRate*Time.deltaTime);
        }
    }
    
}
