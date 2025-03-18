using System.Collections;
using UnityEngine;

public class PlayerBlinkController : MonoBehaviour
{
    [SerializeField] Renderer modelRenderer;  // Assign your model's Renderer in the inspector
    [SerializeField] float blinkDuration = 0.2f;
    private Material originalMaterial;
    private Color originalColor;

    void Start()
    {
        if (modelRenderer == null)
        {
            modelRenderer = GetComponent<Renderer>();
        }

        originalMaterial = modelRenderer.material;
        originalColor = originalMaterial.color;
    }

    public void TriggerBlink(float duration, Color color)
    {
        float blinkCount = duration/(2*blinkDuration);
        StartCoroutine(BlinkCoroutine(blinkCount, color));
    }

    private IEnumerator BlinkCoroutine(float blinkCount, Color color)
    {
        modelRenderer.material.color = color;  // Switch to red
        for (int i = 0; i < blinkCount; i++)
        {
            modelRenderer.material.color = color;
            yield return new WaitForSeconds(blinkDuration);

            modelRenderer.material.color = originalColor;
            yield return new WaitForSeconds(blinkDuration);
        }
        modelRenderer.material.color = originalColor;  // Return to original color
    }
}
