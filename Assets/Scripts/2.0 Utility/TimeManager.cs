using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public void SetTimeRegular()
    {
        Time.timeScale = 1;
    }
    public void SetTimeSlow()
    {
        Time.timeScale = 0.25f;
    }
}
