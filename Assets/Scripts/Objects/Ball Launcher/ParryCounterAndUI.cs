using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class ParryCounterAndUI : MonoBehaviour
{
    [SerializeField] int parryCount;
    [SerializeField] TextMeshProUGUI displayText;

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
    }
}
