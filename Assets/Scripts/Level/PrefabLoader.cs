using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrefabLoader : MonoBehaviour
{

    public static List<GameObject> listRoofsPrefabs;
    public static List<GameObject> listWallsPrefabs;
    public static List<GameObject> listFloorsPrefabs;

    public static List<GameObject> listRoofObjectsPrefabs;
    public static List<GameObject> listWallObjectsPrefabs;
    public static List<GameObject> listFloorObjectsPrefabs;

    private static string serumsFolderPath = "Prefabs/Serums/Serums";
    private static string catalyseursFolderPath = "Prefabs/Serums/Catalyseurs";
    
    public static List<GameObject> serumPrefabsToSpawn;
    public static List<GameObject> catalyseurPrefabsToSpawn;
    public static List<GameObject> roads;

    public static GameController.Levels ObjectLevel;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public static void LoadBackgroundPrefab(GameController.Levels level)
    {
        ObjectLevel = level;
        string roofsPath = $"Prefabs/Background/{level}/Roofs";
        string wallsPath = $"Prefabs/Background/{level}/Walls";
        string floorsPath = $"Prefabs/Background/{level}/Floors";

        string roofObjectsPath = $"Prefabs/Objects/{level}/Roof";
        string wallObjectsPath = $"Prefabs/Objects/{level}/Wall";        
        string floorObjectsPath = $"Prefabs/Objects/{level}/Floor";
        listRoofsPrefabs = LoadPrefabs(roofsPath);
        listWallsPrefabs = LoadPrefabs(wallsPath);
        listFloorsPrefabs = LoadPrefabs(floorsPath);
        listWallObjectsPrefabs = LoadPrefabs(wallObjectsPath);
        listRoofObjectsPrefabs = LoadPrefabs(roofObjectsPath);
        listFloorObjectsPrefabs = LoadPrefabs(floorObjectsPath);
    }

    public static void LoadCollectables()
    {
        LoadSerums();
        LoadCatalyseurs();
    }

    public static void LoadSerums()
    {
        serumPrefabsToSpawn = LoadPrefabs(serumsFolderPath);
    }

    public static void LoadCatalyseurs()
    {
        catalyseurPrefabsToSpawn = LoadPrefabs(catalyseursFolderPath);
    }
    public static void LoadRoad(GameController.Levels level)
    {
        string roadsPath = $"Prefabs/Road/Roads{level}";
        roads = LoadPrefabs(roadsPath);
    }

    public static List<GameObject> LoadPrefabs(string folderPath)
    {
        List<GameObject> prefabs = new List<GameObject>();
        GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>(folderPath);
        prefabs.AddRange(loadedPrefabs);
        return prefabs;
    }

}
