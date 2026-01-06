using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(menuName ="Guitar/Single Note")]
public class Note : ScriptableObject
{
    public AudioClip pluck;
    public AudioClip major;
    public AudioClip minor;
    public AudioClip power;

    public string ToString()
    {
        char octaveChar = name[0];
        string outputName = "";

        // iterates through 1 or 2 letters (C or C#)
        for (int i = 1; i < name.Length; i++)
        {
            outputName += name[i];
        }
        return outputName + octaveChar;
    }
}

