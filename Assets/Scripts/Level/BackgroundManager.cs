using System.Collections.Generic;
using UnityEngine;
public class BackgroundManager : MonoBehaviour
{
    private string backgroundPath = "Prefabs/Background";
    private GameObject Player;
    private List<GameObject> listBackgroundToSpawn;

    private GameObject[] backgroundOnStage;
    [SerializeField] int numberOfbackground = 100;
    private float fullBackgroundLength = 0;

    void Start()
    {
        Player = GameObject.Find("Player");
        listBackgroundToSpawn = GameController.Instance.LoadPrefabs(backgroundPath);
        RemoveOldBackground();
        InitBackground();
    }

    private void RemoveOldBackground()
    {
        foreach (GameObject child in transform)
        {
            Destroy(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBackground();
    }

    private void InitBackground()
    {
        backgroundOnStage = new GameObject[numberOfbackground];
        float? pos=null;
        for (int i = 0; i < numberOfbackground; i++)
        {
            pos = SpawnBackground(pos, i);
        }
    }

    private float? SpawnBackground(float? pos, int i)
    {
        GameObject background;
        int n = Random.Range(0, listBackgroundToSpawn.Count);
        background = listBackgroundToSpawn[n];
        backgroundOnStage[i] = Instantiate(background, transform);
        float meshWidth = GetBrackgroundWidth(background);
        if (pos is null)
        {
            pos = Player.transform.position.z - 2*meshWidth;
        }
        else
        {
            pos += meshWidth;
        }
        fullBackgroundLength +=meshWidth;
        background.transform.position = new Vector3(background.transform.position.x, background.transform.position.y, (float)pos);
        return pos;
    }

    private float GetBrackgroundWidth(GameObject background)
    {
        return 40;
    }
    private void UpdateBackground()
    {
        for (int i =numberOfbackground-1; i>=0; i--)
        {
            GameObject background = backgroundOnStage[i];
            float meshWidth = GetBrackgroundWidth(background);
            if(background.transform.position.z + 4*meshWidth<Player.transform.position.z)
            {
                fullBackgroundLength-=meshWidth;
                float oldZ = background.transform.position.z;
                float? pos = oldZ + fullBackgroundLength;
                Destroy(background);
                SpawnBackground(pos, i);
                break;
            }      
        }
    }


}
