using UnityEngine;
using TMPro;

public class DeathController : MonoBehaviour
{
    [SerializeField] private GameObject deathMenu; // Assign this in the Inspector
    [SerializeField] TMP_InputField nameInputField;

    [SerializeField] GameObject ScoreDisplayPrefab;
    [SerializeField] Transform scoreDisplayAnchor;
    private GameObject highScoreDisplay;
    private bool gameRestarting = false;
    void Start()
    {
        gameRestarting = false;
        if (deathMenu != null)
            deathMenu.SetActive(false);
        nameInputField.transform.parent.gameObject.SetActive(false);
        highScoreDisplay = Instantiate(ScoreDisplayPrefab, scoreDisplayAnchor);
    }

    // Update is called once per frame
    void Update()
    {
        if ((GameController.Instance.playerDead) & (deathMenu != null) & (!gameRestarting))
        {
            Time.timeScale = 0; 
            deathMenu.SetActive(true);
            if(HighScoreManager.Instance.IsHighScore((int)Score.Instance.score))
            {
                nameInputField.transform.parent.gameObject.SetActive(true);
            }
            
            gameRestarting = true;
            
        }  
    }

    public void SaveHighScore()
    {
        string playerName = nameInputField.text;
        if (!string.IsNullOrWhiteSpace(playerName)) // if a name if filled
        {
            playerName = playerName.Length >= 5 ? playerName.Substring(0, 6) : playerName;
            HighScoreManager.Instance.AddNewScore(playerName, (int)Score.Instance.score, Score.Instance.time);
            nameInputField.transform.parent.gameObject.SetActive(false);
            Destroy(highScoreDisplay);
            highScoreDisplay = Instantiate(ScoreDisplayPrefab, scoreDisplayAnchor);
        }
        
    }


}
