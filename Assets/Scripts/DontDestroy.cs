using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static DontDestroy holi;
    public int Score;
    public ScoreSystem _ScoreSystem;

    void Awake()
    {
        if (holi == null)
        {
            holi = this;
            DontDestroyOnLoad(holi);
        }
        else
        {
            Destroy(gameObject);
        }
      
    }
    private void Start()
    {
        EventManager.Subscribe("GiveMeScore", GiveMeScore);
    }

    void GiveMeScore(params object[] parameteres)
    {
        Score = _ScoreSystem.Score;
    }
}
