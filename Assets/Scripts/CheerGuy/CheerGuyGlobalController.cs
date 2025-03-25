using System.Collections;
using UnityEngine;

public class CheerGuyGlobalController : MonoBehaviour
{
    [SerializeField] private float cheerGuyDelay =20f;

    public int? cheerGuyLane=null;
    public bool startCheerGuy {get; private set;}= false;
    private bool startDelay=true;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckCheerGuyLane();
    }
    private void CheckCheerGuyLane()
    {
        cheerGuyLane = null;
        foreach(Transform cheerGuy in transform)
        {
            cheerGuyLane = cheerGuy.GetComponent<CheerGuyController>().currentLane;
        }
        if(startCheerGuy == false & startDelay)
        {
            float delay = cheerGuyDelay + Random.Range(0f, 15f); // 20s + délai aléatoire
            StartCoroutine(InvokeWithDelay(delay));
        }
    }

    IEnumerator InvokeWithDelay(float delay)
    {
        startDelay = false;
        yield return new WaitForSeconds(delay);
        SetStartCheerGuy(true);
        startDelay = true;
    }
    public void SetStartCheerGuy(bool start)
    {
        startCheerGuy=start;
    }
}


