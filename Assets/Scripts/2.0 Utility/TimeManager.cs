using UnityEngine;

public class TimeManager : MonoBehaviour
{
    float initialFixedDeltaTime;

    private void Start()
    {
        initialFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void SetTimeRegular()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = initialFixedDeltaTime;
    }
    public void SetTimeQuarter()
    {
        Time.timeScale = 0.25f;
        Time.fixedDeltaTime = initialFixedDeltaTime / Time.timeScale;
    }

    public void SetFixedTimeScale(float timeScale)
    {
        //Time.timeScale = timeScale;
        Time.fixedDeltaTime = initialFixedDeltaTime / timeScale;
    }
}
