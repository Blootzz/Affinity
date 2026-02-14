using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.Events;

public class ParryCounterAndUI : MonoBehaviour
{
    public UnityEvent OnGoalAchievedEvent;

    [SerializeField] int parryCount;
    [SerializeField] int goalNum = 10;

    [SerializeField] TextMeshProUGUI displayText;
    [SerializeField] Color32 successColor;

    // subscribed in UnityEvent ZoneParryDetect.ParryCountedEvent
    public void LISTENER_Add1()
    {
        parryCount++;
        UpdateNumberDisplay();
    }

    
    public void LISTENER_Reset()
    {
        parryCount = 0;
        UpdateNumberDisplay();
    }

    void UpdateNumberDisplay()
    {
        displayText.text = parryCount.ToString();

        // evaluate
        if (parryCount == goalNum)
        {
            DoVisualEffect();
            OnGoalAchievedEvent?.Invoke();
        }
    }

    void DoVisualEffect()
    {
        displayText.text = "<b>" + parryCount.ToString() + "</b>";
        //displayText.vertexCo
    }
}
