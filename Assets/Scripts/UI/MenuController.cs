using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject ScoreDisplayPrefab;
    [SerializeField] Transform scoreDisplayAnchor;
    private GameObject highScoreDisplay;
    void Start()
    {
        highScoreDisplay = Instantiate(ScoreDisplayPrefab, scoreDisplayAnchor);
    }
    public void Play()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Quit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Arrête le jeu dans l'éditeur
        #endif
    }

    public void DeleteSave()
    {
        HighScoreManager.Instance.ClearAllScores();
        Destroy(highScoreDisplay);
        highScoreDisplay = Instantiate(ScoreDisplayPrefab, scoreDisplayAnchor);
    }
}
