using UnityEngine;

public class DeleteOnButton : MonoBehaviour
{
    public KeyCode deleteButton;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(deleteButton))
            Destroy(this.gameObject);
    }
}
