using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{

    public enum Levels
    {
        level1,
        level2,
        level3
    }
    private Levels level;
    private Levels playerInLevel;
    public static GameController Instance;

    private GameObject Player;
    private PlayerLife PlayerLife;
    private PlayerController PlayerController;

    [SerializeField] public float laneWidth = 3f;
    [SerializeField] float level2StartSec = 5f;
    [SerializeField] float level3StartSec = 10f;
    public int minLane {get; private set;} = -1;
    public int maxLane {get; private set;} = 1;

    public bool playerDead {get; private set;} = false;

    private string serumsFolderPath = "Prefabs/Serums/Serums";
    private string catalyseursFolderPath = "Prefabs/Serums/Catalyseurs";
    private string roadsLevel1Path = "Prefabs/Road/RoadsLevel1";
    private string roadsLevel2Path = "Prefabs/Road/RoadsLevel2";
    private string roadsLevel3Path = "Prefabs/Road/RoadsLevel3";

    private List<GameObject> serumPrefabsToSpawn;
    private List<GameObject> serumPrefabsToSpawnFiltered;
    private List<GameObject> catalyseurPrefabsToSpawn;
    private List<GameObject> catalyseurPrefabsToSpawnFiltered;

    [SerializeField] GameObject firstRoad;
    [SerializeField] GameObject endOfRoadPlane;
    private List<GameObject> roads;
    private List<GameObject> roadsLevel1;
    private List<GameObject> roadsLevel2;
    private List<GameObject> roadsLevel3;

    private GameObject[] roadsOnStage;
    [SerializeField] int numberOfRoads = 1;
    public float roadLength {get; private set;}
    private float minRoadLength = 55;
    private float maxRoadLength = 85;
    [SerializeField] Transform roadParent;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
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
        LoadSerums();
        LoadCatalyseurs();
        LoadRoads();
        roads = roadsLevel1;
        level = Levels.level1;
        playerInLevel = Levels.level1;
        FilterSerumAndCatalyseurs();
        InitRoads();
        Score.Instance.initScore();
    }

    void Update()
    {
        CheckPlayerIsDead();
        UpdateRoadLength();
        UpdateRoad();
    }

    private void UpdateRoadLength()
    {
        float speedIncreasePercentage = (PlayerController.totalIncreasePercent-100)/(PlayerController.maxIncreasePercent-100);
        roadLength = (int)Mathf.Lerp(minRoadLength, maxRoadLength,speedIncreasePercentage);
    }

    private void InitRoads()
    {
        roadsOnStage = new GameObject[numberOfRoads];
        for (int i = 0; i < numberOfRoads; i++)
        {
            GameObject road;
            if (i==0)
            {
                // This way we can chose a first road with no serum or catalyseur
                road = firstRoad;
            }
            else
            {
                int n = Random.Range(0, roads.Count);
                road = roads[n];
            }
            
            roadsOnStage[i] = Instantiate(road, roadParent);
        }
        roadLength = getRoadLength(roadsOnStage[0]);
        float pos = Player.transform.position.z - roadLength/4;
        foreach (var road in roadsOnStage)
        {
            road.GetComponent<RoadsController>().setLevel(level);
            road.GetComponent<RoadsController>().SetRoadLength(minRoadLength);
            float tmpRoadLength = getRoadLength(road);
            road.transform.position = new Vector3(0, 0, pos);
            pos +=tmpRoadLength;
            SpawnObjectsOnRoad(road);
        }
        endOfRoadPlane.transform.position = new Vector3(0, 0, pos+roadLength/2);
    }

    private float getRoadLength(GameObject road)
    {
        float tmpRoadLength = road.transform.Find("Planes").transform.Find("PlaneCenter").localScale.z;
        return tmpRoadLength;
    }

    private void UpdateRoad()
    {
        float totalRoadLength = 0;
        int indexRoadToProcess=-1;
        for (int i =numberOfRoads-1; i>=0; i--)
        {
            GameObject road = roadsOnStage[i];
            if(Player.transform.position.z > (road.transform.position.z-roadLength/2) & playerInLevel!=road.GetComponent<RoadsController>().level)
            {
                if (road.GetComponent<RoadsController>().active)
                {
                    // added because if not there is a bug when switching from level 2 to level3, level 2 road still exists so you go back and forth between 2 and 3
                    SetLevel(road.GetComponent<RoadsController>().level);
                    road.GetComponent<RoadsController>().setInactive();
                }
                
            }
            if(road.transform.position.z + roadLength/2<Player.transform.position.z-6f)
            {
                indexRoadToProcess = i;
            }
            else
            {
                totalRoadLength+=getRoadLength(road);
            }
        }
        if (indexRoadToProcess>=0)
        {
            GameObject road = roadsOnStage[indexRoadToProcess];
            float z = road.transform.position.z;
            float oldRoadLength = getRoadLength(road);
            Destroy(road);
            int n = Random.Range(0, roads.Count);
            road = Instantiate(roads[n], roadParent);
            road.GetComponent<RoadsController>().setLevel(level);
            road.GetComponent<RoadsController>().SetRoadLength(roadLength);
            float newRoadLength = getRoadLength(road);
            float pos = z + totalRoadLength+(oldRoadLength+newRoadLength)/2;
            road.transform.position = new Vector3(0, 0, pos);
            SpawnObjectsOnRoad(road);
            roadsOnStage[indexRoadToProcess] = road;
            Score.Instance.EndOfRoad();
            endOfRoadPlane.transform.position = new Vector3(0, 0, pos+roadLength/2);
        }
        
    }


    private void SpawnObjectsOnRoad(GameObject road)
    {
        FilterSerumAndCatalyseurs();
        road.GetComponent<RoadsController>().InstantiateSerums(serumPrefabsToSpawnFiltered);
        road.GetComponent<RoadsController>().InstantiateCatalyseurs(catalyseurPrefabsToSpawnFiltered);
    }

    private List<GameObject> filterSerums(List<GameObject> inputSerums)
    {
        List<GameObject> outputSerums = new List<GameObject>();
        foreach(var serum in inputSerums)
        {
            Serum.SerumType prefabSerumType = serum.GetComponent<Serum>().GetSerumType();
            List<Serum.SerumType> activeSerumsWithRatio = PlayerLife.getActiveSerumRatio();
            foreach(var serumType in activeSerumsWithRatio)
            {
                if(prefabSerumType==serumType)
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
        level = Levels.level2;
        roads = roadsLevel2;    
    }

    private void ChangeToLevel3()
    {
        level = Levels.level3;
        roads = roadsLevel3;
    }
    private void SetLevel(Levels level)
    {
        switch (level)
        {
            case Levels.level2:
            {
                StartLevel2();
                break;
            }
            case Levels.level3:
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
        serumPrefabsToSpawnFiltered = filterSerums(serumPrefabsToSpawn);
        catalyseurPrefabsToSpawnFiltered = filterSerums(catalyseurPrefabsToSpawn);
    }

    private void StartLevel2()
    {
        PlayerLife.ActivateLifeBar(Serum.SerumType.gamma);
        maxLane = 2; 
        playerInLevel = Levels.level2;    
    }

    private void StartLevel3()
    {
        PlayerLife.ActivateLifeBar(Serum.SerumType.iota);
        minLane = -2;  
        playerInLevel = Levels.level3;
    }

    public List<GameObject> LoadPrefabs(string folderPath)
    {
        List<GameObject> prefabs = new List<GameObject>();
        GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>(folderPath);
        prefabs.AddRange(loadedPrefabs);
        return prefabs;
    }

    public void LoadSerums()
    {
        serumPrefabsToSpawn = LoadPrefabs(serumsFolderPath);
    }

    public void LoadCatalyseurs()
    {
        catalyseurPrefabsToSpawn = LoadPrefabs(catalyseursFolderPath);
    }
    public void LoadRoads()
    {
        roadsLevel1 = LoadPrefabs(roadsLevel1Path);
        roadsLevel2 = LoadPrefabs(roadsLevel2Path);
        roadsLevel3 = LoadPrefabs(roadsLevel3Path);
    }
    
}
