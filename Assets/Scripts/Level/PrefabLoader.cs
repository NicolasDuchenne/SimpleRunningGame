using System.Collections.Generic;
using UnityEngine;

public class PrefabLoader : MonoBehaviour
{

    private string roofsPath = "Prefabs/Background/Roofs";
    public static List<GameObject> listRoofsPrefabs;
    private string wallsPath = "Prefabs/Background/Walls";
    public static List<GameObject> listWallsPrefabs;
    private string floorsPath = "Prefabs/Background/Floors";
    public static List<GameObject> listFloorsPrefabs;

    private string roofObjectsPath = "Prefabs/Objects/Roof";
    public static List<GameObject> listRoofObjectsPrefabs;
    private string wallObjectsPath = "Prefabs/Objects/Wall";
    public static List<GameObject> listWallObjectsPrefabs;
    private string floorObjectsPath = "Prefabs/Objects/Floor";
    public static List<GameObject> listFloorObjectsPrefabs;

    private string serumsFolderPath = "Prefabs/Serums/Serums";
    private string catalyseursFolderPath = "Prefabs/Serums/Catalyseurs";
    private string roadsLevel1Path = "Prefabs/Road/RoadsLevel1";
    private string roadsLevel2Path = "Prefabs/Road/RoadsLevel2";
    private string roadsLevel3Path = "Prefabs/Road/RoadsLevel3";

    public static List<GameObject> serumPrefabsToSpawn;
    public static List<GameObject> catalyseurPrefabsToSpawn;
    public static List<GameObject> roadsLevel1;
    public static List<GameObject> roadsLevel2;
    public static List<GameObject> roadsLevel3;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        listRoofsPrefabs = LoadPrefabs(roofsPath);
        listWallsPrefabs = LoadPrefabs(wallsPath);
        listFloorsPrefabs = LoadPrefabs(floorsPath);
        listWallObjectsPrefabs = LoadPrefabs(wallObjectsPath);
        listRoofObjectsPrefabs = LoadPrefabs(roofObjectsPath);
        listFloorObjectsPrefabs = LoadPrefabs(floorObjectsPath);
        LoadSerums();
        LoadCatalyseurs();
        LoadRoads();
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

    public static List<GameObject> LoadPrefabs(string folderPath)
    {
        List<GameObject> prefabs = new List<GameObject>();
        GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>(folderPath);
        prefabs.AddRange(loadedPrefabs);
        return prefabs;
    }

}
