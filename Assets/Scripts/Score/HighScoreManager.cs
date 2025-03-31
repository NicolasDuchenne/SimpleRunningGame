using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance;
    private string savePath;
    public int maxScores = 10;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre les scènes
            savePath = Path.Combine(Application.persistentDataPath, "highscores.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddNewScore(string playerName, int score, float duration)
    {
        HighScoreData data = LoadScores();

        data.highScores.Add(new HighScoreEntry
        {
            playerName = playerName,
            score = score,
            duration = duration
        });

        data.highScores.Sort((a, b) => b.score.CompareTo(a.score)); // Tri décroissant
        if (data.highScores.Count > maxScores)
            data.highScores.RemoveRange(maxScores, data.highScores.Count - maxScores);

        SaveScores(data);
    }

    public bool IsHighScore(int score)
    {
        HighScoreData data = LoadScores();

        // S'il y a encore de la place dans le top
        if (data.highScores.Count < maxScores)
            return true;

        // Sinon on regarde si ton score est meilleur que le pire dans le top
        int lowestScore = data.highScores[data.highScores.Count - 1].score;
        return score > lowestScore;
    }

    public HighScoreData LoadScores()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<HighScoreData>(json);
        }
        return new HighScoreData();
    }

    public void SaveScores(HighScoreData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }
    public void ClearAllScores()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }
}
