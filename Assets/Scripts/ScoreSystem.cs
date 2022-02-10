
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{

    public Text scoreText;
    public int Score = 0;
    public DontDestroy _dontdestroy;

    void Awake()
    {
        Score = 0;
        EventManager.Subscribe("SendScore", SendScore);

    }

    void SendScore(params object[] parameters)
    {
        var points = (int)parameters[0];
        Score += points;
        scoreText.text = Score.ToString();
        EventManager.Trigger("GiveMeScore");

    }
    public void Save()
    {
        var data = new ScoreData();
        data.score = Score;

        BinarySerializer.SaveBinary(data, $"{Application.dataPath}\\ScoreData.dat");
    }
    public void Load()
    {
        var data = BinarySerializer.LoadBinary<ScoreData>($"{Application.dataPath}\\ScoreData.dat");
        Score = data.score;
        scoreText.text = Score.ToString();
    }

}
