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

        if (!stateManager.faceRight)
            stateManager.ForceFlip();
    }

    public override void OnExit()
    {
        stateManager.SwitchActionMap("Basic");
        stateManager.guitarController.gameObject.SetActive(false);
    }

    public void PlayNote(int note, bool buttonDown)
    {
        stateManager.guitarController.EnterNoteInput(note, buttonDown);
    }

    public void ApplyChord(ChordType chordNum, bool buttonDown)
    {
        stateManager.guitarController.ApplyChordModifier(chordNum, buttonDown);
    }
    public void UseSustain(bool useSustain)
    {
        stateManager.guitarController.SetSustain(useSustain);
    }

    public void IncrementGuitarSprite(bool forward)
    {
        stateManager.guitarController.ProcessGuitarSpriteCycleInput(forward);
    }

    public void CycleKey(bool forward, bool buttonDown)
    {
        stateManager.guitarController.ProcessCycleKeyInput(forward, buttonDown);
    }
    public void CycleScale(bool forward, bool buttonDown)
    {
        stateManager.guitarController.ProcessCycleScaleInput(forward, buttonDown);
    }

    public void Bend(bool useHalfStep, bool buttonDown)
    {
        stateManager.guitarController.ProcessBend(useHalfStep, buttonDown);
    }

    public override void Exit()
    {
        stateManager.SwitchState(stateManager.playerStateIdle);
    }
}
