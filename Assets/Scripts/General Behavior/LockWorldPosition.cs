using UnityEngine;
using UnityEngine.UIElements;

public class LockWorldPosition : MonoBehaviour
{
    Vector2 fixedWorldPos;
    Vector2 initialOffset;

    private void Awake()
    {
        initialOffset = transform.localPosition;
    }

    private void OnEnable()
    {
        // reset to where it should be relative to parent
        transform.localPosition = initialOffset;

        // assign locked world position
        fixedWorldPos = transform.position;
        Debug.LogWarning("Starting pos: "+transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        print("Updating pos to: " + fixedWorldPos);
        transform.position = fixedWorldPos;
    }
}
