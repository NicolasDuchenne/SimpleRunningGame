using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
public class RoadsController : MonoBehaviour
{
    
    [SerializeField] private Transform serumParentObject;
    [SerializeField] private Transform catalyseursParentObject;



    public GameController.Levels level {get; private set;} = GameController.Levels.level1;


    void Start()
    {
    }

    public void setLevel(GameController.Levels level)
    {
        this.level = level;
    }


    public void InstantiateObjects(Transform parentObject, List<GameObject> prefabsToSpawn)
    {
        if (parentObject == null)
        {
            throw new System.Exception("Parent object non assigné !");
        }
        int i = 0;
        // Récupère tous les enfants du parentObject
        foreach (Transform child in parentObject)
        {
            int random = Random.Range(0, prefabsToSpawn.Count);
            GameObject randomPrefab = prefabsToSpawn[random];
            Instantiate(randomPrefab, child.position, Quaternion.identity, child);
            i++;
        }
    }

    public void InstantiateSerums(List<GameObject> prefabsToSpawn)
    {
        InstantiateObjects(serumParentObject, prefabsToSpawn);
    }

    public void InstantiateCatalyseurs(List<GameObject> prefabsToSpawn)
    {
        InstantiateObjects(catalyseursParentObject, prefabsToSpawn);
    }


}
