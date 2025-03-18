using UnityEngine;

public class Serum : MonoBehaviour
{
    [SerializeField] float rotationSpeed =1f;
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

    public void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

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
