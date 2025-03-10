using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float minzRangeProjection = 0.8f;
    [SerializeField] float maxzRangeProjection = 1f;
    [SerializeField] float minxRangeProjection = 0.2f;
    [SerializeField] float maxxRangeProjection = 0.5f;
    [SerializeField] float minStrenghtCollision = 10f;
    [SerializeField] float maxStrenghtCollision = 15f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void ProcessCollision(float speedMult)
    {
        float sign = 1;
        if(Math.Abs(transform.position.x) < 0.1f)
        {
            float random = Random.Range(0f,1f);
            if (random < 0.5)
                sign = -1;
        }
        else
        {
            sign = Math.Sign(transform.position.x);
        }
        float xRandomRange = sign*Random.Range(minxRangeProjection, maxxRangeProjection);
        float yRandomRange = Random.Range(minxRangeProjection, maxxRangeProjection);
        float zRandomRange = Random.Range(minzRangeProjection, maxzRangeProjection);
        float strenghtCollision = Random.Range(minStrenghtCollision, maxStrenghtCollision);
        rb.AddForce(
            new Vector3(xRandomRange, yRandomRange, zRandomRange) * speedMult * strenghtCollision, ForceMode.Impulse);
    }
}
