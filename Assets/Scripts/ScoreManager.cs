using TMPro;
public static class ScoreManager
{
    private static int slowScore = 0;
    private static int finalScore = 0;
    private static int multiplier = 1;

    public static int GetScore()
    {
        return finalScore;
    }
    public static int GetMultiplier()
    {
        return multiplier;
    }

    public static void ResetScoreManager(TextMeshProUGUI multiplierUI)
    {
        finalScore = 0; 
        multiplier = 1;
        multiplierUI.text = "X" + multiplier.ToString();
    }
    public static void IncreaseScore(int scoreIncrease)
    {
        finalScore += scoreIncrease * multiplier;
    }

    public static void IncreaseMultipler(int value, TextMeshProUGUI multiplierUI)
    {
        multiplier += value;
        multiplierUI.text = "X" + multiplier.ToString();
    }

    public static void UpdateScoreBoard(TextMeshProUGUI textUi)
    {
        if (finalScore != slowScore)
        {
            int increaseValue = 10;

            if (slowScore < finalScore - 10000)
            {
                increaseValue = 10000;                
            } 
            else if (slowScore < finalScore - 1000)
            {
                increaseValue = 1000;
            }
            else if (slowScore < finalScore - 500)
            {
                increaseValue = 100;
            }
            else if (slowScore < finalScore - 250)
            {
                increaseValue = 50;
            }
            else if (slowScore < finalScore - 100)
            {
                increaseValue = 30;
            }
            else if (slowScore < finalScore - 50)
            {
                increaseValue = 20;
            }

            slowScore += increaseValue;
        }

        if (finalScore < slowScore)
        {
            slowScore = finalScore;
        }
        textUi.text = slowScore.ToString();
    }
}
