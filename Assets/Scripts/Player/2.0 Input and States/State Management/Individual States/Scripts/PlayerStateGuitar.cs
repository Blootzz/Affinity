using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Guitar")]
public class PlayerStateGuitar : PlayerBaseState
{
    //public PlayerStateGuitar(PlayerStateManager newStateManager) : base(newStateManager)
    //{
    //}

    public override void OnEnter()
    {
        stateManager.SwitchActionMap("Guitar");
        stateManager.playerAnimationManager.PlayAnimationFromString("GuitarBaseLayer");
    }

    public override void OnExit()
    {
        stateManager.SwitchActionMap("Basic");
    }

    public override void PlayNote(int note)
    {
        stateManager.guitarController.EnterNoteInput(note);
    }

    public override void ApplyChord(ChordType chordNum)
    {
        stateManager.guitarController.ApplyChordModifier(chordNum);
    }
}
