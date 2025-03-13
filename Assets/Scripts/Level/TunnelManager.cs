using System.Collections.Generic;
using UnityEngine;

public class TunnelManager : MonoBehaviour
{

    void Start()
    {
        ChoseWalls();
        ChoseRoof();
        ChoseFloor();
    }

    private GameObject InstantiateRandomPrefab(List<GameObject>listPrefab, Transform transform, float probability = 100)
    {
        GameObject prefab = null;
        if(listPrefab is null)
        {
            throw new System.Exception("listPrefab is empty");
        }
        if(CheckIfSpawn(probability) )
        {
            int numberOfPrefabs = listPrefab.Count;
            if (numberOfPrefabs>0)
            {
                int n = Random.Range(0, listPrefab.Count);
                prefab = listPrefab[n];
                return Instantiate(prefab, transform);
            }
        }
        
        return prefab;
        
        
    }

    private bool CheckIfSpawn(float probality)
    {
        int randomNumber = Random.Range(0, 101);
        if (randomNumber > probality)
        {
            return false;
        }
        return true;
    }
    private void SpawnObject(Transform transform, List<GameObject> listPrefabs, float probability)
    {
        Transform objects = transform.Find("Objects");
        foreach(Transform child in objects)
        {
            InstantiateRandomPrefab(listPrefabs, child, probability);
        }
    }

    private void ChoseWalls()
    {
        Transform leftWallTransform = transform.Find("Walls").transform.Find("LeftWall");
        GameObject prefab = InstantiateRandomPrefab(PrefabLoader.listWallsPrefabs, leftWallTransform);
        SpawnObject(prefab.transform, PrefabLoader.listWallObjectsPrefabs,GameController.Instance.GetWallObjectSpawnRate());

        Transform rightWallTransform = transform.Find("Walls").transform.Find("RightWall");
        prefab = InstantiateRandomPrefab(PrefabLoader.listWallsPrefabs, rightWallTransform);
        SpawnObject(prefab.transform, PrefabLoader.listWallObjectsPrefabs,GameController.Instance.GetWallObjectSpawnRate());
    }
    private void ChoseRoof()
    {
        Transform roofTransform = transform.Find("Roof");
        GameObject prefab =InstantiateRandomPrefab(PrefabLoader.listRoofsPrefabs, roofTransform);
        SpawnObject(prefab.transform, PrefabLoader.listRoofObjectsPrefabs,GameController.Instance.GetRoofObjectSpawnRate());
    }

    private void ChoseFloor()
    {
        Transform floorTransform = transform.Find("Floor");
        GameObject prefab =InstantiateRandomPrefab(PrefabLoader.listFloorsPrefabs, floorTransform);
        SpawnObject(prefab.transform, PrefabLoader.listFloorObjectsPrefabs,GameController.Instance.GetFloorObjectSpawnRate());
    }

    
}
