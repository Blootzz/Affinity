using UnityEngine;

[CreateAssetMenu(menuName = "Guitar/Scale")]
public class Scale : ScriptableObject
{
    public string nameDesignation;
    [Header("1 = half step, 2 = whole step")]
    public int[] spacings = new int[10]; // 1s and 2s for half and whole steps, respectively
}



public enum ScaleType
{
    // uses underscore to prevent autocapitalization in the editor
    Scale_I,
    Scale_ii,
    Scale_iii,
    Scale_IV,
    Scale_V,
    Scale_vi,
    Scale_vii
}