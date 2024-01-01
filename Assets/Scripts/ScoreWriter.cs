using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreWriter : MonoBehaviour
{
    public static ScoreWriter Instance;
    public int Score;
    public List<int> HighScore = new List<int>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!Directory.Exists(Application.persistentDataPath+"/Highscore"))
        {
            Directory.CreateDirectory(Application.persistentDataPath+"/Highscore");
            //string HighScoreText = "";
            //HighScore.Clear();
            //for(int i = 0; i < 10; i++)
            //{
            //    HighScore.Add(0);
            //}
            //foreach(int score in HighScore)
            //{
            //    HighScoreText += score.ToString()+",";
            //}
            //HighScoreText = HighScoreText.TrimEnd(',');
            //File.WriteAllText(Application.persistentDataPath+"/Highscore/Highscore.txt",HighScoreText);
            ResetScore();
        }
        else
        {
            HighScore.Clear();
            string HighScoreTextRaw = File.ReadAllText(Application.persistentDataPath + "/Highscore/Highscore.txt");
            string[] strings = HighScoreTextRaw.Split(',');
            foreach(string str in strings)
            {
                HighScore.Add(System.Convert.ToInt32(str));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.C))
        //{
        //    ScreenCapture.CaptureScreenshot(Application.dataPath+"/Capture-"+DateTime.Now.ToString("yyyy.MM.dd HH-mm-ss")+".png");
        //}
    }
    public void AppendHighScore()
    {
        HighScore.Add(Score);
        HighScore.Sort();
        HighScore.Reverse();
        HighScore.RemoveAt(HighScore.Count - 1);

        string HighScoreText = "";
        foreach (int score in HighScore)
        {
            HighScoreText += score.ToString() + ",";
        }
        HighScoreText = HighScoreText.TrimEnd(',');
        File.WriteAllText(Application.persistentDataPath + "/Highscore/Highscore.txt", HighScoreText);
    }
    public void ResetScore()
    {
        HighScore = new List<int>() {0,0,0,0,0,0,0,0,0,0};
        string HighScoreText = "0,0,0,0,0,0,0,0,0,0";
        File.WriteAllText(Application.persistentDataPath + "/Highscore/Highscore.txt", HighScoreText);
    }
}
