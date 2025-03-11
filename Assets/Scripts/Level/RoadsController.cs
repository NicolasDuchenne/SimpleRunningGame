using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
public class RoadsController : MonoBehaviour
{
    
    [SerializeField] private Transform serumParentObject;
    [SerializeField] private Transform catalyseursParentObject;

    public bool active {get; private set;}= true;



    public GameController.Levels level {get; private set;} = GameController.Levels.level1;


    void Start()
    {
        if (serumParentObject == null | catalyseursParentObject == null)
        {
            throw new System.Exception("Parent object non assigné !");
        }
    }

    public void setLevel(GameController.Levels level)
    {
        this.level = level;
    }
    public void setInactive()
    {
        active = false;
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

    public void InstantiateCatalyseurs(List<GameObject> prefabsToSpawn)
    {
        foreach (Transform child in catalyseursParentObject)
        {
            int random = Random.Range(0, prefabsToSpawn.Count);
            GameObject randomPrefab = prefabsToSpawn[random];
            Instantiate(randomPrefab, child.position, Quaternion.identity, child);
        }
    }

    public void SetRoadLength(float length)
    {
        foreach(Transform plane in transform.Find("Planes").transform)
        {
            plane.localScale = new Vector3(plane.transform.localScale.x, plane.transform.localScale.y, length);
        }
    }


}
