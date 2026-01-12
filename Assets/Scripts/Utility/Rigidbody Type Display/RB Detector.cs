using UnityEngine;
using TMPro;

public class RBDetector : MonoBehaviour
{
    Rigidbody2D rb;
    TMP_Text displayText;
    RigidbodyType2D oldType;

    private void Awake()
    {
        if (!transform.parent.TryGetComponent<Rigidbody2D>(out rb))
            Debug.LogWarning("RBDetector could not find RigidBody2D");

        displayText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateDisplayText();
    }

    private void Update()
    {
        if (rb.bodyType != oldType)
            UpdateDisplayText();
    }
    
    void UpdateDisplayText()
    {
        oldType = rb.bodyType;
        displayText.text = "RB = " + oldType.ToString();
    }

}
