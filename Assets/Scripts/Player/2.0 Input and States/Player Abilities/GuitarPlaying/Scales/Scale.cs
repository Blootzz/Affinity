using UnityEngine;

[CreateAssetMenu(menuName = "Guitar/Scale")]
public class Scale : ScriptableObject
{
    public string nameDesignation;
    [Header("1 = half step, 2 = whole step")]
    public int[] spacings = new int[10]; // 1s and 2s for half and whole steps, respectively
}
