using UnityEngine;

public class EnemyHitboxManager : MonoBehaviour
{
    /// <summary>
    /// Iterates through all children and disables Collider2D
    /// </summary>
    public void SetEnableAllHitboxes(bool enableAll)
    {
        Collider2D collider2D;
        SpriteRenderer spriteRenderer;
        foreach (Transform child in transform)
        {
            // change collider enabled
            if (child.TryGetComponent(out collider2D))
                collider2D.enabled = enableAll;
            else
                Debug.LogError("No Collider2D found in child no. " + child.GetSiblingIndex() + " of EnemyHitboxManager");

            // change color
            if (child.TryGetComponent(out spriteRenderer))
                spriteRenderer.enabled = enableAll;
            else
                Debug.LogWarning("No SpriteRenderer found in child no. " + child.GetSiblingIndex() + " of EnemyHitboxManager");
        }
    }

    public void SetHitboxAttackFaceRight(bool faceRight)
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<EnemyHitbox>().SetAttackFaceRight(faceRight);
        }
    }
}
