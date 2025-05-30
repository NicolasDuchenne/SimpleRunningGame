using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarController : MonoBehaviour
{

    [SerializeField] public float maxLife {get; private set;}= 100f;
    private Slider healthBar;
    [SerializeField] Serum.SerumType serumType;
    public float life { get; private set; }
    private bool activated = false;
    private bool startWasLaunched = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        life = maxLife;
        healthBar = GetComponent<Slider>();
        healthBar.minValue = 0;
        healthBar.maxValue = maxLife;
        UpdateLifeBar();
        startWasLaunched = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLifeBar();
    }

    public Serum.SerumType getSerumType()
    {
        return serumType;
    }


    public bool TakeDamage(float damage)
    {
        if(activated & startWasLaunched)
        {
            if (damage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(damage), "Value must be positive.");
            }
                
            life -= damage;
            life = Math.Max(life, 0);
            if (life == 0)
            {
                return true;
            }
        }
        return false;
    }

    public void Heal(float heal)
    {
        if (activated & startWasLaunched)
        {
            if (heal < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(heal), "Value must be positive.");
            }
            
            life += heal;
            life = Math.Min(life, maxLife);
        }
    }

    public Serum.SerumType GetSerumType()
    {
        return serumType;
    }

    private void UpdateLifeBar()
    {
        healthBar.value = life;
    }

    public void SetActive(bool setActive)
    {
        activated = setActive;
        gameObject.SetActive(setActive);
    }
}

