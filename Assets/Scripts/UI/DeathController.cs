using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathController : MonoBehaviour
{
    [SerializeField] private GameObject deathMenu; // Assign this in the Inspector
    private bool gameRestarting = false;
    void Start()
    {
        gameRestarting = false;
        if (deathMenu != null)
            deathMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if ((GameController.Instance.playerDead) & (deathMenu != null) & (!gameRestarting))
        {
            Debug.Log(GameController.Instance.playerDead);
            Time.timeScale = 0; 
            deathMenu.SetActive(true);
            gameRestarting = true;
        }
            
    }
}
