using UnityEngine;

public class LightManager : MonoBehaviour
{
    public Light directionalLight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        directionalLight = GetComponent<Light>();
        directionalLight.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.Instance.playerInLevel == GameController.Levels.Level3)
        {
            directionalLight.intensity = 1;
        }
    }
}
