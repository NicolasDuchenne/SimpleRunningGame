using System.Collections;
using UnityEngine;

public class CheerGuyGlobalController : MonoBehaviour
{
    [SerializeField] float delay =20f;
    [SerializeField] float delayVariation = 15f;

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
            float realDelay = delay + Random.Range(0f, delayVariation); // 20s + délai aléatoire
            StartCoroutine(InvokeWithDelay(realDelay));
        }
    }

    IEnumerator InvokeWithDelay(float time)
    {
        startDelay = false;
        yield return new WaitForSeconds(time);
        SetStartCheerGuy(true);
        startDelay = true;
    }
    public void SetStartCheerGuy(bool start)
    {
        startCheerGuy=start;
    }
}


