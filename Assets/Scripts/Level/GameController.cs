using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

    [SerializeField] public float laneWidth = 3f;
    [SerializeField] float level2StartSec = 5f;
    [SerializeField] float level3StartSec = 10f;
    public int minLane {get; private set;} = -1;
    public int maxLane {get; private set;} = 1;

    public bool playerDead {get; private set;} = false;

    [SerializeField] private string serumsFolderPath = "Assets/Prefabs/Serums/Serums";
    [SerializeField] private string catalyseursFolderPath = "Assets/Prefabs/Serums/Catalyseurs";
    private string roadsLevel1Path = "Assets/Prefabs/Road/RoadsLevel1";
    private string roadsLevel2Path = "Assets/Prefabs/Road/RoadsLevel2";
    private string roadsLevel3Path = "Assets/Prefabs/Road/RoadsLevel3";

    private List<GameObject> serumPrefabsToSpawn;
    private List<GameObject> serumPrefabsToSpawnFiltered;
    private List<GameObject> catalyseurPrefabsToSpawn;
    private List<GameObject> catalyseurPrefabsToSpawnFiltered;

    private List<GameObject> roads;
    private List<GameObject> roadsLevel1;
    private List<GameObject> roadsLevel2;
    private List<GameObject> roadsLevel3;

    private GameObject[] roadsOnStage;
    [SerializeField] int numberOfRoads = 1;
    private float roadLength;
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
            int n = Random.Range(0, roads.Count);
            roadsOnStage[i] = Instantiate(roads[n], roadParent);
        }
        roadLength = roadsOnStage[0].transform.Find("Planes").transform.Find("PlaneCenter").localScale.z;
        float pos = Player.transform.position.z + roadLength/4;
        foreach (var road in roadsOnStage)
        {
            road.GetComponent<RoadsController>().setLevel(level);
            road.transform.position = new Vector3(0, 0, pos);
            pos +=roadLength;
            SpawnObjectsOnRoad(road);
        }
    }

    private void UpdateRoad()
    {
        for (int i =numberOfRoads-1; i>=0; i--)
        {
            GameObject road = roadsOnStage[i];
            if(Player.transform.position.z > (road.transform.position.z-roadLength/2) & playerInLevel!=road.GetComponent<RoadsController>().level)
            {
                SetLevel(road.GetComponent<RoadsController>().level);
            }
            if(road.transform.position.z + roadLength/2<Player.transform.position.z-6f)
            {
                float z = road.transform.position.z;
                Destroy(road);
                int n = Random.Range(0, roads.Count);
                road = Instantiate(roads[n], roadParent);
                road.GetComponent<RoadsController>().setLevel(level);
                road.transform.position = new Vector3(0, 0,z + roadLength * numberOfRoads);
                SpawnObjectsOnRoad(road);
                roadsOnStage[i] = road;
            }
        }
    }

    private void SpawnObjectsOnRoad(GameObject road)
    {
        road.GetComponent<RoadsController>().InstantiateSerums(serumPrefabsToSpawnFiltered);
        road.GetComponent<RoadsController>().InstantiateCatalyseurs(catalyseurPrefabsToSpawnFiltered);
    }

    private List<GameObject> filterSerums(List<GameObject> inputSerums)
    {
        List<GameObject> outputSerums = new List<GameObject>();
        foreach(var serum in inputSerums)
        {
            Serum.SerumType prefabSerumType = serum.GetComponent<Serum>().GetSerumType();
            foreach(var serumType in PlayerLife.activeSerums)
            {
                if(prefabSerumType==serumType)
                {
                    //Debug.Log(serumType);
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
        FilterSerumAndCatalyseurs();
    }

    private void StartLevel3()
    {
        PlayerLife.ActivateLifeBar(Serum.SerumType.iota);
        minLane = -2;  
        playerInLevel = Levels.level3;
        FilterSerumAndCatalyseurs();
    }

    public List<GameObject> LoadPrefabs(string folderPath)
    {
        List<GameObject> prefabs = new List<GameObject>();
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
        foreach (string guid in prefabGUIDs)
        {  
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            prefabs.Add(AssetDatabase.LoadAssetAtPath<GameObject>(assetPath));
        }
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
