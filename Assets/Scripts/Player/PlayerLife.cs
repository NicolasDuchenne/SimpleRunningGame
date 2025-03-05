using Unity.VisualScripting;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] LifeBarController[] lifeBars;
    [SerializeField] float lifeLoseRate = 1f;

    public bool isDead { get; private set; } = false;
    void Start()
    {
        ActivateLifeBar(Serum.SerumType.alpha);
        ActivateLifeBar(Serum.SerumType.beta);
    }

    // Update is called once per frame
    void Update()
    {
        ApplyConstantDamage();
    }

    private void ApplyConstantDamage()
    {
        foreach (LifeBarController lifeBarController in lifeBars)
        {
            TakeDamage(lifeBarController, lifeLoseRate * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ProcessSerumTrigger(other);
    }

    private void ProcessSerumTrigger(Collider other)
    {
        if(other.transform.parent.gameObject.CompareTag("Serum"))
        {
            Serum serum = other.transform.parent.gameObject.GetComponent<Serum>();
            Serum.SerumType otherSerumType = serum.GetSerumType();
            foreach (LifeBarController lifeBarController in lifeBars)
            {
                Serum.SerumType lifeBarSerumType = lifeBarController.GetSerumType();
                if (lifeBarSerumType == otherSerumType)
                {
                    bool isCatalyseur = serum.IsCatalyseur();
                    float healthGain = serum.GetHealthGain();
                    if (isCatalyseur==false)
                        lifeBarController.Heal(healthGain);
                    else
                        TakeDamage(lifeBarController,healthGain);
                }
            }
            serum.Destroy();
        }
    }

    public void ActivateLifeBar(Serum.SerumType serumType)
    {
        foreach (LifeBarController lifebarController in lifeBars)
        {
            Serum.SerumType lifebarSerumType = lifebarController.GetSerumType();
            if (lifebarSerumType == serumType)
            {
                lifebarController.SetActive(true);
            }
        }
    }

    private void TakeDamage(LifeBarController lifeBarController, float damage)
    {
        bool tmpIsDead = lifeBarController.TakeDamage(damage);
        if (tmpIsDead)
        {
            isDead = true;
        }
    }

}
