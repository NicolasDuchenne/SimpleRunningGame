using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class RoadsController : MonoBehaviour
{
    
    [SerializeField] private Transform serumParentObject;
    [SerializeField] private Transform catalyseursParentObject;
    [SerializeField] private Transform bonusParentObject;

    [SerializeField] GameObject backGroundPrefab;
    [SerializeField] int minRoadsBonus;
    private GameObject door;
    public bool active {get; private set;}= true;

    public GameController.Levels level {get; private set;} = GameController.Levels.Level1;

    private float roadLength = 80f;
    public float backgroundLength {get; private set;}= 40f;

    [SerializeField] private Transform backgroundTransform;
    [SerializeField] private GameObject LightingEffect;
    [SerializeField] private Transform LightingEffectTransform;


    void Start()
    {
        SetRoadLength(roadLength);
        if (serumParentObject == null | catalyseursParentObject == null | bonusParentObject == null)
        {
            throw new System.Exception("Parent object non assigné !");
        }
        InstantiateBonusMalus();
        if(GameController.Instance.startLightning)
        {
            SpawnLightingEffect();
        }
        
    }
    public void SpawnLightingEffect()
    {
        int numberOfLane = GetNumberOflane();
        int minLane = -Mathf.CeilToInt(numberOfLane/2f)+1;
        int maxLane = Mathf.FloorToInt(numberOfLane/2f);
        int lane = Random.Range(minLane, maxLane+1);
        GameObject lighting = Instantiate(LightingEffect,LightingEffectTransform);
        //GameObject lighting = Instantiate(LightingEffect, LightingEffectTransform.position, Quaternion.identity, LightingEffectTransform);
        lighting.transform.position = new Vector3(lane*GameController.Instance.laneWidth,lighting.transform.position.y, lighting.transform.position.z);
    }
    private int GetNumberOflane()
    {
        int numberOfLane = transform.Find("Planes").childCount;
        return numberOfLane;
    }

    public void setLevel(GameController.Levels level)
    {
        this.level = level;
    }
    public void setInactive()
    {
        active = false;
    }

    public void ActivateDoor(float length)
    {
        door = transform.Find("Door").GameObject();
        door.SetActive(true);
        door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y, -length/2);
    }


    public void InstantiateSerums(List<GameObject> prefabsToSpawn)
    {
        foreach (Transform child in serumParentObject)
        {
            int random = Random.Range(0, prefabsToSpawn.Count);
            GameObject randomPrefab = prefabsToSpawn[random];
            foreach(Transform grandChild in child)
            {
                Instantiate(randomPrefab, grandChild.position, Quaternion.identity, grandChild);
            }
        }
    }


    public void toggleSerums(bool active)
    {
        foreach (Transform child in serumParentObject)
        {
            child.transform.gameObject.SetActive(active);
        }
    }
    public void InstantiateCatalyseurs(List<GameObject> prefabsToSpawn, int probability)
    {
        foreach (Transform child in catalyseursParentObject)
        {
            if(probability > Random.Range(0,101))
            {
                int random = Random.Range(0, prefabsToSpawn.Count);
                GameObject randomPrefab = prefabsToSpawn[random];
                Instantiate(randomPrefab, child.position, Quaternion.identity, child);
            }
        }
    }
    

    public void InstantiateBonusMalus()
    {
        // We insure that we have an optimal repartition of bonus and malus
        if(GameController.Instance.malusRoadCount >=minRoadsBonus) // spawn malus only if two roads have passed, this way we insure that we have 5 seconds between bonus or malus
        {
            List<GameObject> bonusPrefabs = PrefabLoader.ClonePrefabs(PrefabLoader.bonusPrefabsToSpawn);
            List<GameObject> malusPrefabs = PrefabLoader.ClonePrefabs(PrefabLoader.malusPrefabsToSpawn);
            foreach(Transform child in bonusParentObject)
            {
                List<GameObject> prefabs = new List<GameObject>();
                int random = Random.Range(0, 101);
                if(GameController.Instance.bonusProbability > random)
                {
                    InstantiateAndRemoveBonusMalus(bonusPrefabs, child);
                    if(bonusPrefabs.Count == 0)
                    {
                        bonusPrefabs = PrefabLoader.ClonePrefabs(PrefabLoader.bonusPrefabsToSpawn);
                    }
                }
                else if(GameController.Instance.bonusProbability+GameController.Instance.malusProbability > random)
                {
                    InstantiateAndRemoveBonusMalus(malusPrefabs, child);
                    if(malusPrefabs.Count == 0)
                    {
                        malusPrefabs = PrefabLoader.ClonePrefabs(PrefabLoader.malusPrefabsToSpawn);
                    }
                }
                
            }
        }
        GameController.Instance.malusRoadCount++;
    }
    private void InstantiateAndRemoveBonusMalus(List<GameObject> prefabs, Transform child)
    {
        if (prefabs.Count > 0)
        {
            int random = Random.Range(0, prefabs.Count);
            GameObject randomPrefab = prefabs[random];
            prefabs.RemoveAt(random);
            Instantiate(randomPrefab, child.position, Quaternion.identity, child);
            GameController.Instance.malusRoadCount = 0;
        }
        
    }

    public void SetRoadLength(float length)
    {
        foreach(Transform plane in transform.Find("Planes").transform)
        {
            plane.localScale = new Vector3(plane.transform.localScale.x, plane.transform.localScale.y, length);
        }
    }

    public void SpawnBackgrounds()
    {
        foreach(Transform child in backgroundTransform)
        {
            GameObject background;
            background = backGroundPrefab;
            Instantiate(background, child);
        }
    }


}
