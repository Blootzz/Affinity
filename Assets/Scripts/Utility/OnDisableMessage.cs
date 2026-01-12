using UnityEngine;

public class OnDisableMessage : MonoBehaviour
{
    private void OnDisable()
    {
        print(name + " Disabling");
    }
}
