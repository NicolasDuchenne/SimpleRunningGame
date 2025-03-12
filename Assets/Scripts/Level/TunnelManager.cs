using System.Collections.Generic;
using UnityEngine;

public class TunnelManager : MonoBehaviour
{
    private string roofsPath = "Prefabs/Background/Roofs";
    private List<GameObject> listRoofsPrefabs;
    private string wallsPath = "Prefabs/Background/Walls";
    private List<GameObject> listWallsPrefabs;
    private string floorsPath = "Prefabs/Background/Floors";
    private List<GameObject> listFloorsPrefabs;
    void Start()
    {
        listRoofsPrefabs = GameController.Instance.LoadPrefabs(roofsPath);
        listWallsPrefabs = GameController.Instance.LoadPrefabs(wallsPath);
        listFloorsPrefabs = GameController.Instance.LoadPrefabs(floorsPath);
        ChoseWalls();
        ChoseRoof();
        ChoseFloor();
    }

    private void InstantiatePrefab(List<GameObject>listPrefab, Transform transform)
    {
        int n = Random.Range(0, listPrefab.Count);
        GameObject prefab = listPrefab[n];
        Instantiate(prefab, transform);
    }

    private void ChoseWalls()
    {
        Transform leftWallTransform = transform.Find("Walls").transform.Find("LeftWall");
        InstantiatePrefab(listWallsPrefabs, leftWallTransform);

        Transform rightWallTransform = transform.Find("Walls").transform.Find("RightWall");
        InstantiatePrefab(listWallsPrefabs, rightWallTransform);
    }
    private void ChoseRoof()
    {
        Transform roofTransform = transform.Find("Roof");
        InstantiatePrefab(listRoofsPrefabs, roofTransform);
    }

    private void ChoseFloor()
    {
        Transform floorTransform = transform.Find("Floor");
        InstantiatePrefab(listFloorsPrefabs, floorTransform);
    }

    
}
