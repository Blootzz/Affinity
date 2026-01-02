using UnityEngine;

[CreateAssetMenu(menuName ="Guitar/Note")]
public class Note : ScriptableObject
{
    public AudioClip pluck;
    public AudioClip major;
    public AudioClip minor;
    public AudioClip power;
}
