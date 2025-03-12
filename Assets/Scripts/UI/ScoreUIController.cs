using System;
using TMPro;
using UnityEngine;

public class ScoreUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI  TimeText;
    [SerializeField] TextMeshProUGUI  ScoreText;
    [SerializeField] TextMeshProUGUI  MultText;
    [SerializeField] TextMeshProUGUI  TimePerRoadText;
    [SerializeField] TextMeshProUGUI  DamagePerRoadText;
    [SerializeField] TextMeshProUGUI  FPSText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeText.text = $"Time : {Math.Round(Score.Instance.time,1)}";
        ScoreText.text = $"Score: {Score.Instance.score}";
        MultText.text = $"Mult : {Score.Instance.mult}\nMultFact: {Score.Instance.scoreWithoutDamage}";
        TimePerRoadText.text = $"TPR : {Score.Instance.timePerRoad}";
        DamagePerRoadText.text = $"DPR : {Score.Instance.damagePerRoad}";
        FPSText.text = $"FPS : {Score.Instance.FPS}";
    }
}
