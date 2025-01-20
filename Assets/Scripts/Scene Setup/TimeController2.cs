using UnityEngine;

public class TimeController2 : MonoBehaviour
{
    public float slowDownFactor = 0.05f;
    public float slowDownDuration = 2f;
    public float originalFixedDeltaTime;
    private bool active = false;

    private void Start()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        if (active)
        {
            // speed up the game by adding (elapsed time / constant duration value)
            Time.timeScale += Time.unscaledDeltaTime / slowDownDuration;
            // slow down up physics calculations because fixedDeltaTime gets set very low by ZA_WARUDO
            Time.fixedDeltaTime += Time.unscaledDeltaTime / slowDownDuration * originalFixedDeltaTime;

            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            Time.fixedDeltaTime = Mathf.Clamp(Time.fixedDeltaTime, 0f, originalFixedDeltaTime);

            if (Time.timeScale == 1 && Time.fixedDeltaTime == originalFixedDeltaTime)
            {
                active = false;
            }
        }// most of code provided by commenter Infinityplays on Brackeys YT video
    }// Update

    public void ZA_WARUDO()
    {
        active = true;
        Time.timeScale = slowDownFactor; // slows down everything (unless it uses unscaled time)
        Time.fixedDeltaTime *= slowDownFactor; // increases the frequency of physics calculations
    }
}
