using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{

    public enum Levels
    {
        Level1,
        Level2,
        Level3
    }
    private Levels level;
    public Levels playerInLevel { get; private set; }
    public static GameController Instance;

    private GameObject Player;
    private PlayerLife PlayerLife;
    public PlayerController PlayerController { get; private set; }
    [Header("Level Configuration")]
    [SerializeField] public float laneWidth = 3f;
    [SerializeField] float level2StartSec = 5f;
    [SerializeField] float level3StartSec = 10f;
    [SerializeField] float difficultyIncrease1Sec = 60f;
    [SerializeField] float difficultyIncrease2Sec = 120f;
    [SerializeField] float difficultyIncrease3Sec = 180f;
    [SerializeField] float difficultyIncrease4Sec = 240f;
    [SerializeField] float difficultyIncrease5Sec = 300;
    [SerializeField] int timeBetweenDamageIncrease = 10;
    [SerializeField] int damageIncrease = 1;
    public int minLane { get; private set; } = -1;
    public int maxLane { get; private set; } = 1;

    public bool playerDead { get; private set; } = false;


    private List<GameObject> serumPrefabsToSpawnFiltered;

    private List<GameObject> catalyseurPrefabsToSpawnFiltered;




    private GameObject[] roadsOnStage;
    [Header("Road")]
    [SerializeField] GameObject firstRoad;
    [SerializeField] GameObject endOfRoadPlane;
    [SerializeField] int numberOfRoads = 1;
    [SerializeField] Transform roadParent;
    public float roadLength { get; private set; }

    [Header("Object Spawn Rate")]
    [SerializeField] private float WallObjectSpawnRate = 50f;
    [SerializeField] private float RoofObjectSpawnRate = 50f;
    [SerializeField] private float FloorObjectSpawnRate = 50f;
    [Header("Light")]
    [SerializeField] private Light directionalLight;
    [Header("CheerGuy")]
    [SerializeField] private Transform cheerGuyTransform;


    private int catalyseurProbabilty = 25;
    public int bonusProbability { get; private set; } = 10; // bonus probabilty and malus probability must not sum over 100
    public int malusProbability { get; private set; } = 0;



    private bool spawnDoor = false;
    public int malusRoadCount { get; set; } = 0;



    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public float GetWallObjectSpawnRate()
    {
        return WallObjectSpawnRate;
    }
    public float GetRoofObjectSpawnRate()
    {
        return RoofObjectSpawnRate;
    }
    public float GetFloorObjectSpawnRate()
    {
        return FloorObjectSpawnRate;
    }


    private void OnDestroy()
    {
        Instance = null;
    }

    void Start()
    {
        if (roadParent == null)
        {
            throw new System.Exception("Road Parent object non assigné !");
        }
        Player = GameObject.Find("Player");
        PlayerLife = Player.GetComponent<PlayerLife>();
        PlayerController = Player.GetComponent<PlayerController>();
        Invoke("ChangeToLevel2", level2StartSec);
        Invoke("ChangeToLevel3", level3StartSec);
        Invoke("DifficultyIncrease1", difficultyIncrease1Sec);
        Invoke("DifficultyIncrease2", difficultyIncrease2Sec);
        Invoke("DifficultyIncrease3", difficultyIncrease3Sec);
        Invoke("DifficultyIncrease4", difficultyIncrease4Sec);
        Invoke("DifficultyIncrease5", difficultyIncrease5Sec);
        PrefabLoader.LoadRoad(level);
        level = Levels.Level1;
        playerInLevel = Levels.Level1;
        PrefabLoader.LoadBackgroundPrefab(level);
        PrefabLoader.LoadCollectables();
        FilterSerumAndCatalyseurs();
        InitRoads();
        Score.Instance.initScore();
        directionalLight.intensity = 0;
    }

    void Update()
    {
        CheckPlayerIsDead();
        UpdateRoad();
    }


    private void InitRoads()
    {
        roadsOnStage = new GameObject[numberOfRoads];
        for (int i = 0; i < numberOfRoads; i++)
        {
            GameObject road;
            if (i == 0)
            {
                // This way we can chose a first road with no serum or catalyseur
                road = firstRoad;
            }
            else
            {
                int n = Random.Range(0, PrefabLoader.roads.Count);
                road = PrefabLoader.roads[n];
            }

            roadsOnStage[i] = Instantiate(road, roadParent);
        }
        roadLength = getRoadLength(roadsOnStage[0]);
        float pos = Player.transform.position.z - roadLength / 4;
        foreach (var road in roadsOnStage)
        {
            RoadsController roadsController = road.GetComponent<RoadsController>();
            roadsController.setLevel(level);
            float tmpRoadLength = getRoadLength(road);
            road.transform.position = new Vector3(0, 0, pos);
            roadsController.SpawnBackgrounds();
            pos += tmpRoadLength;
            SpawnObjectsOnRoad(road);

        }
        endOfRoadPlane.transform.position = new Vector3(0, 0, pos + roadLength / 2);
    }

    private float getRoadLength(GameObject road)
    {
        float tmpRoadLength = road.transform.Find("Planes").transform.Find("PlaneCenter").localScale.z;
        return tmpRoadLength;
    }

    private void UpdateRoad()
    {
        float totalRoadLength = 0;
        int indexRoadToProcess = -1;
        for (int i = numberOfRoads - 1; i >= 0; i--)
        {
            GameObject road = roadsOnStage[i];
            RoadsController roadsController = road.GetComponent<RoadsController>();
            if (Player.transform.position.z > (road.transform.position.z - roadLength / 2) & playerInLevel != roadsController.level)
            {
                if (roadsController.active)
                {
                    // added because if not there is a bug when switching from level 2 to level3, level 2 road still exists so you go back and forth between 2 and 3
                    SetLevel(road.GetComponent<RoadsController>().level);
                    roadsController.setInactive();
                }

            }
            if (road.transform.position.z + roadLength / 2 < Player.transform.position.z - 6f)
            {
                indexRoadToProcess = i;
            }
            else
            {
                totalRoadLength += getRoadLength(road);
            }
        }
        if (indexRoadToProcess >= 0)
        {
            GameObject road = roadsOnStage[indexRoadToProcess];
            float z = road.transform.position.z;
            float oldRoadLength = getRoadLength(road);
            Destroy(road);
            int n = Random.Range(0, PrefabLoader.roads.Count);
            road = Instantiate(PrefabLoader.roads[n], roadParent);
            RoadsController roadsController = road.GetComponent<RoadsController>();
            roadsController.setLevel(level);
            if (spawnDoor)
            {
                roadsController.ActivateDoor(roadLength);
                spawnDoor = false;
            }
            float newRoadLength = getRoadLength(road);
            float pos = z + totalRoadLength + (oldRoadLength + newRoadLength) / 2;
            road.transform.position = new Vector3(0, 0, pos);
            roadsController.SpawnBackgrounds();
            SpawnObjectsOnRoad(road);
            roadsOnStage[indexRoadToProcess] = road;
            Score.Instance.EndOfRoad();
            endOfRoadPlane.transform.position = new Vector3(0, 0, pos + roadLength / 2);
        }

    }


    private void SpawnObjectsOnRoad(GameObject road)
    {
        FilterSerumAndCatalyseurs();
        road.GetComponent<RoadsController>().InstantiateSerums(serumPrefabsToSpawnFiltered);
        road.GetComponent<RoadsController>().InstantiateCatalyseurs(catalyseurPrefabsToSpawnFiltered, catalyseurProbabilty);
    }

    private List<GameObject> filterSerums(List<GameObject> inputSerums)
    {
        List<GameObject> outputSerums = new List<GameObject>();
        foreach (var serum in inputSerums)
        {
            Serum.SerumType prefabSerumType = serum.GetComponent<Serum>().GetSerumType();
            List<Serum.SerumType> activeSerumsWithRatio = PlayerLife.getActiveSerumRatio();
            foreach (var serumType in activeSerumsWithRatio)
            {
                if (prefabSerumType == serumType)
                {
                    outputSerums.Add(serum);
                }
            }

        }
        return outputSerums;
    }

    private void CheckPlayerIsDead()
    {
        playerDead = PlayerLife.isDead;
    }

    private void ChangeToLevel2()
    {
        level = Levels.Level2;
        PrefabLoader.LoadRoad(level);
        PrefabLoader.LoadBackgroundPrefab(level);
        spawnDoor = true;
    }

    private void ChangeToLevel3()
    {
        level = Levels.Level3;
        PrefabLoader.LoadRoad(level);
        PrefabLoader.LoadBackgroundPrefab(level);
        spawnDoor = true;
    }
    private void SetLevel(Levels level)
    {
        switch (level)
        {
            case Levels.Level2:
                {
                    StartLevel2();
                    break;
                }
            case Levels.Level3:
                {
                    StartLevel3();
                    break;
                }
            default:
                break;
        }
    }
    private void FilterSerumAndCatalyseurs()
    {
        serumPrefabsToSpawnFiltered = filterSerums(PrefabLoader.serumPrefabsToSpawn);
        catalyseurPrefabsToSpawnFiltered = filterSerums(PrefabLoader.catalyseurPrefabsToSpawn);
    }

    private void StartLevel2()
    {
        PlayerLife.ActivateLifeBar(Serum.SerumType.gamma);
        maxLane = 2;
        playerInLevel = Levels.Level2;
    }

    private void StartLevel3()
    {
        PlayerLife.ActivateLifeBar(Serum.SerumType.iota);
        minLane = -2;
        playerInLevel = Levels.Level3;
        directionalLight.intensity = 1;
    }
    private void DifficultyIncrease1()
    {
        catalyseurProbabilty += 25;
        bonusProbability += 20;
        malusProbability += 10;
    }
    private void DifficultyIncrease2()
    {
        catalyseurProbabilty += 15;
        bonusProbability += 20;
        malusProbability += 10;
    }

    private void DifficultyIncrease3()
    {
        catalyseurProbabilty += 15;
        //bonusProbability = 40;
        malusProbability += 20;
    }

    private void DifficultyIncrease4()
    {
        catalyseurProbabilty += 20;
        bonusProbability -= 10;
        malusProbability += 30;
    }
    private void DifficultyIncrease5()
    {
        IncreaseDamagePerRoad();
    }

    public void IncreaseDamagePerRoad()
    {
        Debug.Log("damage increase");
        PlayerLife.IncreaseDamagePerRoad(damageIncrease);
        Invoke("IncreaseDamagePerRoad", timeBetweenDamageIncrease);
    }


    private void toggleAllSerums(bool active)
    {
        foreach (GameObject road in roadsOnStage)
        {
            road.GetComponent<RoadsController>().toggleSerums(active);
        }
    }

    public void StartEmptyStreak(float duration)
    {
        toggleAllSerums(false);
        Invoke("StopBlackEnergie", duration);
    }

    private void StopBlackEnergie()
    {
        toggleAllSerums(true);
    }

    public Transform getCheerGuyTransform()
    {
        return cheerGuyTransform;
    }





}
