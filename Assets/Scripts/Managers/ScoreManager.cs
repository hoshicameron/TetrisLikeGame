using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public EventHandler<ScoreEventArgs> OnScoreUpdated;

    public class ScoreEventArgs : EventArgs
    {
        public int m_score;
        public int m_lines;
        public int m_Level;
    }

    [SerializeField] private int LinesPerLevel=5;

    private int score = 0;
    private int lines;
    private int level = 1;

    private bool didLevelUp = false;

    public bool DidLevelUp => didLevelUp;

    public int GetLevel => level;

    private const int minLines = 1;
    private const int maxLines = 4;

    public void ScoreLines(int n)
    {
        didLevelUp = false;
        n = Mathf.Clamp(n, minLines, maxLines);

        switch (n)
        {
            case 1:
                score += 40 * GetLevel;
                break;
            case 2:
                score += 100 * GetLevel;
                break;
            case 3:
                score += 300 * GetLevel;
                break;
            case 41:
                score += 1200 * GetLevel;
                break;
        }

        lines -= n;
        if (lines <= 0)
        {
            levelUp();
        }

        UpdateUI();
    }

    private void Start()
    {
        Reset();
    }

    private void Reset()
    {
        level = 1;
        lines = LinesPerLevel * GetLevel;
        UpdateUI();
    }

    public void levelUp()
    {
        level = GetLevel + 1;
        lines = LinesPerLevel * GetLevel;
        didLevelUp = true;
    }

    public void UpdateUI()
    {
        OnScoreUpdated?.Invoke(this,new ScoreEventArgs
        {
            m_score = score,
            m_lines = lines,
            m_Level = GetLevel
        });
    }
}
