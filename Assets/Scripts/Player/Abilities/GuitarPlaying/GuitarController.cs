using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarController : MonoBehaviour
{
    KeyCode majorChord = KeyCode.M;
    KeyCode minorChord = KeyCode.N;
    KeyCode powerChord = KeyCode.P;

    AudioSource audioSource;

    public Scale[] scales;
    public int scaleIndex = 0;
    public Note[] allNotes; // storage of all Note data
    public int rootNoteIndex = 0; // determines what note the scale is base on in allNotes (0=E, 1=F, 2=F#)
    // max rootNoteIndex should be 11
    
    public Note[] activeButtons = new Note[10]; // assignment of Notes depending on scale

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AssignScale();
    }

    // Update is called once per frame
    void Update()
    {
        int noteToPlayIndex = ReadAlphaNumberInput() - 1; // subtract 1 to make index
        if (noteToPlayIndex != -1)
        {
            // notes have already been assigned to a scale

            // find what chord type to play and set clip
            if (Input.GetKey(majorChord))
                audioSource.clip = activeButtons[noteToPlayIndex].major;
            else
                if (Input.GetKey(minorChord))
                    audioSource.clip = activeButtons[noteToPlayIndex].minor;
            else
                if (Input.GetKey(powerChord))
                    audioSource.clip = activeButtons[noteToPlayIndex].power;
            else
                audioSource.clip = activeButtons[noteToPlayIndex].pluck;

            // sustain
            // Play() will be interrupted by next Play() while PlayOneShot() will not be interrupted
            if (Input.GetKey(KeyCode.Space))
                audioSource.PlayOneShot(audioSource.clip);
            else
                audioSource.Play();
        }
    }

    int ReadAlphaNumberInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            return 1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            return 2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            return 3;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            return 4;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            return 5;
        if (Input.GetKeyDown(KeyCode.Alpha6))
            return 6;
        if (Input.GetKeyDown(KeyCode.Alpha7))
            return 7;
        if (Input.GetKeyDown(KeyCode.Alpha8))
            return 8;
        if (Input.GetKeyDown(KeyCode.Alpha9))
            return 9;
        if (Input.GetKeyDown(KeyCode.Alpha0))
            return 10;
        return 0;
    }

    void AssignScale()
    {
        // assign first button (Alpha1) to root note
        int runningAllNotesIndex = rootNoteIndex;
        activeButtons[0] = allNotes[runningAllNotesIndex];

        // need to assign Note to all 9 remaining number keys
        for (int i=1; i<10; i++)
        {
            // out of allNotes, increment index by scale spacing. Select scale using scaleIndex (0=major)
            // add runningAllNotesIndex to itself to cumulatively increment index
            runningAllNotesIndex = scales[scaleIndex].spacings[i - 1] + runningAllNotesIndex;
            activeButtons[i] = allNotes[runningAllNotesIndex];
        }
    }

}
