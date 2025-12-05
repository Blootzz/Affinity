using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public void SetTimeRegular()
    {
        Time.timeScale = 1;
    }
    public void SetTimeQuarter()
    {
        Time.timeScale = 0.25f;
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
}
