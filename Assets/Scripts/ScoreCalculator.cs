using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ScoreCalculator : MonoBehaviour
{
    private string CurrentConsumptionUrl = "http://20.15.114.131:8080/api/power-consumption/current/view";
    private string SpecificMonthConsumptionUrl = "http://20.15.114.131:8080/api/power-consumption/month/view";
    private string DailyConsumptionUrl = "http://20.15.114.131:8080/api/power-consumption/current-month/daily/view";

    public Text score;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    private double prevUnits;
    private float Score;
    public Text resultUnits;
    public float decreasingConst = 1.0f;
    public double consumptionRate;

    // Get the current date
    System.DateTime theTime = System.DateTime.Now;
    int prevDay;

    public void Start()
    {
        // Start by initializing prevUnits
        StartCoroutine(InitializePrevUnits());
    }

    private IEnumerator InitializePrevUnits()
    {
        string jwtToken = PlayerPrefs.GetString("JWTToken");

        IEnumerator getCurrentUnits = AuthenticationManager.GetUnits(CurrentConsumptionUrl, jwtToken, "CurrentState");
        yield return StartCoroutine(getCurrentUnits);
        string CurrentUnits = getCurrentUnits.Current as string;

        if (CurrentUnits != null)
        {
            CurrentConsumptionUnits currentConsumptionUnits = JsonUtility.FromJson<CurrentConsumptionUnits>(CurrentUnits);
            if (currentConsumptionUnits != null)
            {
                prevUnits = currentConsumptionUnits.currentConsumption / 1000;
            }
        }

        // After initializing prevUnits, start invoking GetCurrentConsumption every 10 seconds
        InvokeRepeating("GetCurrentConsumption", 1, 10);
    }

    public void GetCurrentConsumption()
    {
        // Fetch the current and average daily consumption units to calculate the score
        StartCoroutine(AverageDailyConsumption());
    }

    private IEnumerator AverageDailyConsumption()
    {
        // Get the JWT token from the player preferences
        string jwtToken = PlayerPrefs.GetString("JWTToken");
        Score = PlayerPrefs.GetFloat("TotalScore");

        string Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(theTime.Month).ToUpper();

        // Get the current consumption units
        IEnumerator getCurrentUnits = AuthenticationManager.GetUnits(CurrentConsumptionUrl, jwtToken, "CurrentState");
        yield return StartCoroutine(getCurrentUnits);
        string CurrentUnits = getCurrentUnits.Current as string;

        // Get the average consumption units for the specific month
        IEnumerator getPrvMonthUnits = AuthenticationManager.GetUnits(SpecificMonthConsumptionUrl, jwtToken, "SpecificMonth", theTime.Year, Month);
        yield return StartCoroutine(getPrvMonthUnits);
        string PrevMonthUnits = getPrvMonthUnits.Current as string;

        // Get the daily consumption units for the current month
        IEnumerator getDailyConsumption = AuthenticationManager.GetUnits(DailyConsumptionUrl, jwtToken, "CurrentState");
        yield return StartCoroutine(getDailyConsumption);
        string DailyConsumption = getDailyConsumption.Current as string;

        // Calculate the score
        if (PrevMonthUnits != null && CurrentUnits != null && DailyConsumption != null)
        {
            ComputeScore(PrevMonthUnits, CurrentUnits, DailyConsumption);
        }
        else
        {
            Debug.LogError("Error fetching JSON data for consumption.");
        }
    }

    private void ComputeScore(string PrevMonthUnits, string CurrentUnits, string DailyConsumption)
    {
        try
        {
            MonthlyPowerConsumption monthlyPowerConsumption = JsonUtility.FromJson<MonthlyPowerConsumption>(PrevMonthUnits);
            DailyPowerConsumption dailyPowerConsumption = JsonConvert.DeserializeObject<DailyPowerConsumption>(DailyConsumption);
            CurrentConsumptionUnits currentConsumptionUnits = JsonUtility.FromJson<CurrentConsumptionUnits>(CurrentUnits);

            if (currentConsumptionUnits != null && monthlyPowerConsumption != null && dailyPowerConsumption != null)
            {
                double avgDailyConsumption = monthlyPowerConsumption.monthlyPowerConsumptionView.units / 30.0;
                double currentDayUnits = currentConsumptionUnits.currentConsumption / 1000;

                prevDay = theTime.Day == 1 ? 30 : theTime.Day - 1;

                dailyPowerConsumption.dailyPowerConsumptionView.dailyUnits.TryGetValue(prevDay, out float dailyUnitsAmount);

                // Algorithm to calculate the consumption rate
                double consumptionForTenSec = currentDayUnits - prevUnits; // consumption for 10 seconds by getting 10s gap values
                double averageConsumptionForTenSec = dailyUnitsAmount / (24 * 60 * 6); // average consumption for 10 seconds by considering yesterday's consumption

                consumptionRate = consumptionForTenSec / averageConsumptionForTenSec;

                string dataList = $"consumptionRate = {consumptionRate}\n" +
                                  $"consumptionForTenSec = {consumptionForTenSec}\n" +
                                  $"AverageConsumptionForTenSec = {averageConsumptionForTenSec}\n" +
                                  $"Current Consumption = {currentDayUnits}\n" +
                                  $"Previous Consumption = {prevUnits}\n" +
                                  $"Decreasing Constant = {decreasingConst}\n" +
                                  $"Daily Consumption = {dailyUnitsAmount}\n";

                // Store the current consumption units for the next calculation
                prevUnits = currentDayUnits;

                dataList += $"Score = {Score}\n";
                resultUnits.text = dataList;

                Score -= (float)(consumptionRate * decreasingConst);
                if (Score <= 0)
                {
                    // gameOver.SetActive(true);
                    // Pause the game
                    Time.timeScale = 0;
                    //yield return new WaitForSecondsRealtime(2);
                    SceneManager.LoadScene("Mission Failed");
                }
                else
                {
                    PlayerPrefs.SetFloat("TotalScore", Score);
                    SetScoreValue();
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Exception in processing consumption data: {ex.Message}");
        }
    }

    public void SetDecreasingConst(float decVal)
    {
        decreasingConst = decVal;
    }

    public float GetDecreasingConst()
    {
        return decreasingConst;
    }

    public void UpdateDecreasingConst(float divider)
    {
        decreasingConst /= divider;
    }

    public void ScoreIncrement(int value_add)
    {
        if ((Score + value_add) > 1000)
        {
            Score = 1000;
        }
        else
        {
            Score += value_add;
        }

        PlayerPrefs.SetFloat("TotalScore", Score);
        SetScoreValue();
    }

    public void SetScoreValue()
    {
        score.text = Score.ToString();
        slider.value = Score;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    // Get the current consumption units
    [System.Serializable]
    public class CurrentConsumptionUnits
    {
        public double currentConsumption;
    }

    // Get the average consumption units for the specific month
    [System.Serializable]
    public class MonthlyPowerConsumption
    {
        public UnitsData monthlyPowerConsumptionView;
    }

    [System.Serializable]
    public class UnitsData
    {
        public int year;
        public int month;
        public int units;
    }

    // Get the daily consumption units for the specific month
    [System.Serializable]
    public class DailyPowerConsumption
    {
        public DailyUnitData dailyPowerConsumptionView;
    }

    [System.Serializable]
    public class DailyUnitData
    {
        public int year;
        public int month;
        public Dictionary<int, float> dailyUnits;
    }
}