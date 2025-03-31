using UnityEngine;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{
    [SerializeField] GameObject scoreEntryPrefab; // Prefab avec un TextMeshProUGUI pour afficher une ligne

    void Start()
    {
        CreateScoreDisplay();
    }

    public void CreateScoreDisplay()
    {
        var data = HighScoreManager.Instance.LoadScores();
        int i = 0;
        foreach (var entry in data.highScores)
        {
            var go = Instantiate(scoreEntryPrefab, transform);
            go.GetComponent<TextMeshProUGUI>().text = $"{i+1} - {entry.playerName} - {entry.score} pts - {entry.duration:0.0}s";
            i++;
        }
    }
}