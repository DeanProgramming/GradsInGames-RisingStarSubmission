using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class FileSaving 
{
    // Gets file if it exists it will read from the file and splitting the file content using colon it then will read the second half of the content by trimming off the quotation
    // And then converting it to a int and return the int 
    public static int GetScoreFromFile()
    {
        if (File.Exists(Application.dataPath + "/Score.txt"))
        {
            string[] scoreFile = File.ReadAllText(Application.dataPath + "/Score.txt").Split(new[] { ":" }, System.StringSplitOptions.None);
            string score = "";

            for (int i = 1; i < scoreFile[1].ToString().Length - 2; i++)
            {
                score += scoreFile[1][i];
            }

            return int.Parse(score);
        }
        return -1;
    }

    // Overwrites the file with the new highscore
    // When SetScoreToFile is called the file has already been confirmed to exist from GetScoreFromFile
    public static void SetScoreToFile(int Score)
    {
        SaveScore save = new SaveScore
        {
            score = Score.ToString()
        };
        File.WriteAllText(Application.dataPath + "/Score.txt", JsonUtility.ToJson(save));
    }

    private class SaveScore
    {
        public string score;
    }
}
