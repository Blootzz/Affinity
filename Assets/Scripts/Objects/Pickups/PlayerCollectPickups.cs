using UnityEngine;

public class PlayerCollectPickups : MonoBehaviour
{
    [SerializeField] string ItemTagString;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(ItemTagString))
        {
            print("Found pickup");
        }
    }
}
