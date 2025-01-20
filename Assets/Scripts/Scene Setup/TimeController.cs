using UnityEngine;

public class TimeController
{
    private readonly int updateFrequency; // number of fixedUpdate calls between each adjustment of Time.timeScale
    private int updateCounter;

    public bool controllerActive = false;

    // Start is called before the first frame update
    public TimeController()
    {
        updateCounter = 0;
        updateFrequency = 10; // updates every nth cycle through fixedUpdate
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        UpdateFixedDeltaTime();
        controllerActive = true;
    }

    public void FixedUpdate() // Called by player, not monobehaviour - speeds up time back to normal. Not built to slow time back to normal
    {
        if (updateCounter % updateFrequency == 0)
        {
            if (Time.timeScale > 0.95)
            {
                Time.timeScale = 1;
                controllerActive = false;
            }
            else
            {
                if (updateCounter % updateFrequency == 0)
                {
                    Time.timeScale += .1f;
                }
            }
        }
        UpdateFixedDeltaTime();
        updateCounter += 1;
    }

    public void UpdateFixedDeltaTime()
    {
        //Debug.Log("setting fixedDeltaTime: " + Time.fixedDeltaTime);
        Time.fixedDeltaTime = Time.timeScale / 60;
        //Debug.Log("new: " + Time.fixedDeltaTime);
    }
}
