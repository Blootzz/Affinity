using System.Collections;
using UnityEngine;

public class ColorFlash : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] float singleFlashSeconds = 0.2f;
    [SerializeField] float repeatingFlashInterval = 0.2f;

    Coroutine cancellableCoroutine;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartSingleWhiteFlash()
    {
        StartCoroutine(SingleWhiteFlash());
    }

    IEnumerator SingleWhiteFlash()
    {
        spriteRenderer.material.SetInt("_flashToggle", 1);
        spriteRenderer.material.SetInt("_useBlueInsteadOfWhite", 0);
        yield return new WaitForSeconds(singleFlashSeconds);
        spriteRenderer.material.SetInt("_flashToggle", 0);
    }

    public void StartRepeatingBlueFlash()
    {
        cancellableCoroutine = StartCoroutine(RepeatingBlueFlash());
    }
    IEnumerator RepeatingBlueFlash()
    {
        // set to blue
        spriteRenderer.material.SetInt("_useBlueInsteadOfWhite", 1);

        // infinite loop until coroutine is cancelled
        while (true)
        {
            print("Doing blue while loop");
            spriteRenderer.material.SetInt("_flashToggle", 1);
            yield return new WaitForSeconds(repeatingFlashInterval);
            spriteRenderer.material.SetInt("_flashToggle", 0);
            yield return new WaitForSeconds(repeatingFlashInterval);
        }
    }

    public void EndBlueFlash()
    {
        StopCoroutine(cancellableCoroutine);
    }
}