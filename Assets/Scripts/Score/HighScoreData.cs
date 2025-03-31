using System;
using System.Collections.Generic;

[Serializable]
public class HighScoreEntry
{
    public string playerName;
    public int score;
    public float duration;
}

[Serializable]
public class HighScoreData
{
    public List<HighScoreEntry> highScores = new List<HighScoreEntry>();
}