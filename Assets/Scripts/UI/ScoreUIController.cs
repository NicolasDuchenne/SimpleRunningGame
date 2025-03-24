using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI  TimeText;
    [SerializeField] TextMeshProUGUI  ScoreText;
    [SerializeField] TextMeshProUGUI  MultText;
    [SerializeField] TextMeshProUGUI  LastSerumText;
    [SerializeField] TextMeshProUGUI  TimePerRoadText;
    [SerializeField] TextMeshProUGUI  DamagePerRoadText;
    [SerializeField] TextMeshProUGUI  FPSText;

    Dictionary<Serum.SerumType, Color> mappingColor =  new Dictionary<Serum.SerumType, Color>{
        {Serum.SerumType.alpha, Color.blue},
        {Serum.SerumType.beta, Color.green},
        {Serum.SerumType.gamma, Color.yellow},
        {Serum.SerumType.iota, new Color(160, 32, 240)},
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeText.text = $"{formatTime(Score.Instance.time)}";
        ScoreText.text = $"Score: {Score.Instance.score}";
        MultText.text = $"Mult * {Score.Instance.mult}";
        MultText.color = mappingColor[Score.Instance.lastSerum];
        LastSerumText.text = $"{Score.Instance.lastSerum}";
        LastSerumText.color = mappingColor[Score.Instance.lastSerum];
        TimePerRoadText.text = $"TPR : {Score.Instance.timePerRoad}";
        DamagePerRoadText.text = $"DPR : {Score.Instance.damagePerRoad}";
        FPSText.text = $"FPS : {Score.Instance.FPS}";
    }
    private string formatTime(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string formattedTime = $"{timeSpan.Seconds}s";
        if (timeSpan.Minutes > 0)
        {
            formattedTime = $"{timeSpan.Minutes}m:" + formattedTime;
        }
        if (timeSpan.Hours > 0)
        {
            formattedTime = $"{timeSpan.Hours}h:" + formattedTime;
        }
        
        return formattedTime;
    }
}
