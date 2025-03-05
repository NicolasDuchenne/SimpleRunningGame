using UnityEngine;

public class Serum : MonoBehaviour
{
    public enum SerumType
    {
        alpha,
        beta,
        gamma,
        iota
    }

    [SerializeField] SerumType serumType;

    [SerializeField] float healthGain = 10f;
    [SerializeField] bool isCatalyseur = false;

    public SerumType GetSerumType()
    {
        return serumType;
    }

    public float GetHealthGain()
    {
        return  healthGain;
    }

    public bool IsCatalyseur()
    {
        return isCatalyseur;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }   

}
