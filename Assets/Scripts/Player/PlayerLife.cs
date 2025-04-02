using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] LifeBarController[] lifeBars;
    [SerializeField] float damagePerRoad = 5f;
    [SerializeField] float malusDamageMult = 2f;
    private float currentDamagePerRoad;
    private float lifeLoseRate;

    public bool isDead { get; private set; }
    public List<Serum.SerumType> activeSerums  {get; private set;}= new List<Serum.SerumType>();
    private PlayerController playerController;
    private bool adrenalineActive = false;
    private bool augmentedPhysiologyActive = false;
    private bool SuperSerum = false;
    private bool blackEnergieActive = false;
    private bool lightningActive = false;
    
    private bool SecondaryEffectActive = false;

    private SoundController soundController;
    void Start()
    {
        currentDamagePerRoad = damagePerRoad;
        isDead = false;
        ActivateLifeBar(Serum.SerumType.alpha);
        ActivateLifeBar(Serum.SerumType.beta);
        playerController = GetComponent<PlayerController>();
        soundController = GetComponent<SoundController>();
    }
    public void IncreaseDamagePerRoad(float value)
    {
        currentDamagePerRoad += value;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyConstantDamage();
        ProcessLifeLoseRate();
    }

    private void ProcessLifeLoseRate()
    {
        //Dynamically compute life lose rate so that we insure that the player the same amount of life per road finished. This way if you go faster you lose more life
        lifeLoseRate = currentDamagePerRoad/activeSerums.Count*playerController.forwardSpeed/GameController.Instance.roadLength; 
        // Total damage is always the same, you get less damage per serum if you have more serum
    }

    private void ApplyConstantDamage()
    {
        float damage = lifeLoseRate * Time.deltaTime;
        if (adrenalineActive)
        {
            damage = 0;
        }
        if (blackEnergieActive)
        {
            damage = damage * malusDamageMult;
        }
        if (lightningActive)
        {
            damage = damage * malusDamageMult;
        }
        foreach (LifeBarController lifeBarController in lifeBars)
        {
            TakeDamage(lifeBarController, damage);
        }
        Score.Instance.IncrementDamage(damage);
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
                    {
                        if(SuperSerum)
                        {
                            healthGain = healthGain * 2;
                        }
                        if(SecondaryEffectActive)
                        {
                            TakeCatalyseurDamage(lifeBarController, healthGain);
                        }
                        else
                        {
                            lifeBarController.Heal(healthGain);
                            Score.Instance.IncreaseScore(otherSerumType);
                        }
                        soundController.PlaySerum();
                    }
                    else
                    {
                        TakeCatalyseurDamage(lifeBarController, healthGain);
                        soundController.PlayCatalyseur();
                    }     
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
                if (!activeSerums.Contains(serumType))
                {
                    activeSerums.Add(serumType);
                }
            }
        }
    }

    private void TakeCatalyseurDamage(LifeBarController lifeBarController, float damage)
    {
        if (augmentedPhysiologyActive == false)
        {
            TakeDamage(lifeBarController,damage);
            playerController.LoseSpeed();
            Score.Instance.ResetMult();
        } 
    }

    private void TakeDamage(LifeBarController lifeBarController, float damage)
    {
        if (isDead == false)
        {
            bool tmpIsDead = lifeBarController.TakeDamage(damage);
            
            if (tmpIsDead)
            {
                isDead = true;
            }
        }  
    }

    public List<Serum.SerumType> getActiveSerumRatio()
    {
        // Gives more chance to get serum where you are missing life
        List<Serum.SerumType> activeSerumsWithRatio = new List<Serum.SerumType>();
        foreach (var lifeBar in lifeBars)
        {
            if (activeSerums.Contains(lifeBar.getSerumType()))
            {
                int ratio=0;
                if (lifeBar.life >= 0)
                {
                    ratio = (int)((1-lifeBar.life/lifeBar.maxLife)*10);
                }
                for(int i=0; i<Math.Max(ratio,1);i++)
                {
                    activeSerumsWithRatio.Add(lifeBar.getSerumType());
                }
            }
                
        }
        return activeSerumsWithRatio;
    }

    public void StartAdrenaline(float duration)
    {
        adrenalineActive = true;
        Invoke("StopAdrenaline", duration);
    }
    private void StopAdrenaline()
    {
        adrenalineActive = false;
    }

    public void StartAugmentedPhysiology(float duration)
    {
        augmentedPhysiologyActive = true;
        Invoke("StopAugmentedPhysiology", duration);
    }
    private void StopAugmentedPhysiology()
    {
        augmentedPhysiologyActive = false;
    }

    public void StartSuperSerum(float duration)
    {
        SuperSerum = true;
        Invoke("StopSuperSerum", duration);
    }
    private void StopSuperSerum()
    {
        SuperSerum = false;
    }

    public void StartBlackEnergie(float duration)
    {
        blackEnergieActive = true;
        Invoke("StopBlackEnergie", duration);
    }

    private void StopBlackEnergie()
    {
        adrenalineActive = false;
    }
    public void StartSecondaryEffect(float duration)
    {
        SecondaryEffectActive = true;
        Invoke("StopSecondaryEffect", duration);
    }

    private void StopSecondaryEffect()
    {
        SecondaryEffectActive = false;
    }

    public void StartLightningEffect()
    {
        lightningActive = true;
    }
    public void StopLightningEffect()
    {
        lightningActive = false;
    }

}
