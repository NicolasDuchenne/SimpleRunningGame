using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;
public class RoadsController : MonoBehaviour
{
    
    [SerializeField] private Transform serumParentObject;
    [SerializeField] private Transform catalyseursParentObject;
    [SerializeField] private Transform bonusParentObject;

    [SerializeField] GameObject backGroundPrefab;
    private GameObject door;
    public bool active {get; private set;}= true;

    public GameController.Levels level {get; private set;} = GameController.Levels.Level1;

    private float roadLength = 80f;
    public float backgroundLength {get; private set;}= 40f;

    [SerializeField] private Transform backgroundTransform;


    void Start()
    {
        SetRoadLength(roadLength);
        if (serumParentObject == null | catalyseursParentObject == null | bonusParentObject == null)
        {
            throw new System.Exception("Parent object non assigné !");
        }
        InstantiateBonusMalus();
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

    public void InstantiateCatalyseurs(List<GameObject> prefabsToSpawn)
    {
        foreach (Transform child in catalyseursParentObject)
        {
            int random = Random.Range(0, prefabsToSpawn.Count);
            GameObject randomPrefab = prefabsToSpawn[random];
            Instantiate(randomPrefab, child.position, Quaternion.identity, child);
        }
    }

    public void InstantiateBonusMalus()
    {
        foreach(Transform child in bonusParentObject)
        {
            int random = Random.Range(0, 2);
            List<GameObject> prefabs = random==0?PrefabLoader.bonusPrefabsToSpawn: PrefabLoader.malusPrefabsToSpawn;

            random = Random.Range(0, prefabs.Count);
            GameObject randomPrefab = prefabs[random];
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
