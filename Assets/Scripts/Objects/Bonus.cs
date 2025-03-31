using System;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public enum BonusType
    {
        Adrenaline,
        AugmentedPhysiology,
        SuperSerum,
        BlackEnergie,
        EmptyStreak,
        SecondaryEffect
    }
    [SerializeField] BonusType bonusType;
    [SerializeField] float rotationSpeed =50f;
    [SerializeField] float bonusDuration = 5f;
    [SerializeField] float malusDuration = 5f;

    Action<GameObject> GetOperation(BonusType bonusType) => bonusType switch
        {
            BonusType.Adrenaline  => AdrenalineBonus,
            BonusType.AugmentedPhysiology  => AugmentedPhysiologyBonus,
            BonusType.SuperSerum  => SuperSerumBonus,
            BonusType.BlackEnergie => BlackEnergieMalus,
            BonusType.EmptyStreak => EmptyStreakMalus,
            BonusType.SecondaryEffect => SecondaryEffectMalus,
            _ => throw new ArgumentException("Opération non valide")
        };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject player = other.transform.parent.gameObject;
        if (player.CompareTag("Player"))
        {
            GetOperation(bonusType)(player);
        }
        Destroy(gameObject);
        
    }

    private void AdrenalineBonus(GameObject player)
    {
        player.GetComponent<PlayerLife>().StartAdrenaline(bonusDuration);
        player.GetComponent<PlayerBlinkController>().TriggerBlink(bonusDuration, Color.green);
        player.GetComponent<SoundController>().PlayBonus();
    }
    private void AugmentedPhysiologyBonus(GameObject player)
    {
        player.GetComponent<PlayerLife>().StartAugmentedPhysiology(bonusDuration);
        player.GetComponent<PlayerBlinkController>().TriggerBlink(bonusDuration, Color.yellow);
        player.GetComponent<SoundController>().PlayBonus();
    }
    private void SuperSerumBonus(GameObject player)
    {
        player.GetComponent<PlayerLife>().StartSuperSerum(bonusDuration);
        player.GetComponent<PlayerBlinkController>().TriggerBlink(bonusDuration, Color.blue);
        player.GetComponent<SoundController>().PlayBonus();
    }
    private void BlackEnergieMalus(GameObject player)
    {
        player.GetComponent<PlayerLife>().StartBlackEnergie(malusDuration);
        player.GetComponent<PlayerBlinkController>().TriggerBlink(bonusDuration, Color.black);
        player.GetComponent<SoundController>().PlayMalus();
    }
    private void EmptyStreakMalus(GameObject player)
    {
        GameController.Instance.StartEmptyStreak(malusDuration);
        player.GetComponent<PlayerBlinkController>().TriggerBlink(bonusDuration, Color.gray);
        player.GetComponent<SoundController>().PlayMalus();
    }
    private void SecondaryEffectMalus(GameObject player)
    {
        player.GetComponent<PlayerLife>().StartSecondaryEffect(malusDuration);
        player.GetComponent<PlayerBlinkController>().TriggerBlink(bonusDuration, Color.red);
        player.GetComponent<SoundController>().PlayMalus();
    }

    
}
