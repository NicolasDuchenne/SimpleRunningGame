using Unity.VisualScripting;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] LifeBarController[] lifeBars;
    [SerializeField] float lifeLoseRate = 1f;

    public bool isDead { get; private set; } = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ApplyConstantDamage();
    }

    private void ApplyConstantDamage()
    {
        foreach (LifeBarController lifebarController in lifeBars)
        {
            isDead = lifebarController.TakeDamage(lifeLoseRate * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ProcessSerumTrigger(other);
    }

    private void ProcessSerumTrigger(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Serum"))
        {
            Serum serum = other.transform.parent.gameObject.GetComponent<Serum>();
            Serum.SerumType otherSerumType = serum.GetSerumType();
            foreach (LifeBarController lifebarController in lifeBars)
            {
                Serum.SerumType lifebarSerumType = lifebarController.GetSerumType();
                if (lifebarSerumType == otherSerumType)
                {
                    bool isCatalyseur = serum.IsCatalyseur();
                    float healthGain = serum.GetHealthGain();
                    if (isCatalyseur==false)
                        lifebarController.Heal(healthGain);
                    else
                        isDead = lifebarController.TakeDamage(healthGain);
                    serum.Destroy();
                }
            }
        }
    }

}
