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
        stateManager.guitarController.gameObject.SetActive(true);
        stateManager.playerAnimationManager.PlayAnimationFromString("GuitarBaseLayer");
    }

    public override void OnExit()
    {
        stateManager.SwitchActionMap("Basic");
        stateManager.guitarController.gameObject.SetActive(false);
    }

    public override void PlayNote(int note)
    {
        stateManager.guitarController.EnterNoteInput(note);
    }

    public override void ApplyChord(ChordType chordNum)
    {
        stateManager.guitarController.ApplyChordModifier(chordNum);
    }

    public override void UseSustain(bool useSustain)
    {
        stateManager.guitarController.SetSustain(useSustain);
    }

    public override void IncrementGuitarSprite(bool forward)
    {
        stateManager.guitarController.ProcessGuitarSpriteCycleInput(forward);
    }

    public override void Exit()
    {
        stateManager.SwitchState(stateManager.playerStateIdle);
    }
}
