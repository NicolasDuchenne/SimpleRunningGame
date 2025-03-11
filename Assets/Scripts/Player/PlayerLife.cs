using System;
using System.Collections.Generic;
using Unity.IntegerTime;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] LifeBarController[] lifeBars;


    [SerializeField] float damagePerRoad = 5f;
    private float lifeLoseRate;

    public bool isDead { get; private set; }
    public List<Serum.SerumType> activeSerums  {get; private set;}= new List<Serum.SerumType>();
    private PlayerController playerController;
    void Start()
    {
        isDead = false;
        ActivateLifeBar(Serum.SerumType.alpha);
        ActivateLifeBar(Serum.SerumType.beta);
        playerController = GetComponent<PlayerController>();
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
        lifeLoseRate = damagePerRoad*playerController.forwardSpeed/GameController.Instance.roadLength;
    }

    private void ApplyConstantDamage()
    {
        float damage = lifeLoseRate * Time.deltaTime;
        foreach (LifeBarController lifeBarController in lifeBars)
        {
            TakeDamage(lifeBarController, lifeLoseRate * Time.deltaTime);
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
                        lifeBarController.Heal(healthGain);
                        Score.Instance.IncreaseScore(1);
                    }
                    else
                    {
                        TakeDamage(lifeBarController,healthGain);
                        playerController.LoseSpeed();
                        Score.Instance.ResetMult();
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

}
