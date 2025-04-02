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
        SetMaterialColor(color);  // Switch to red
        for (int i = 0; i < blinkCount; i++)
        {
            SetMaterialColor(color);
            yield return new WaitForSeconds(blinkDuration);

            SetDefaultColor();
            yield return new WaitForSeconds(blinkDuration);
        }
        SetDefaultColor();  // Return to original color
    }

    public void SetMaterialColor(Color color)
    {
        modelRenderer.material.color = color;
    }
    public void SetDefaultColor()
    {
        modelRenderer.material.color = originalColor;
    }
    public Color GetCurrentColor()
    {
        return modelRenderer.material.color;
    }
}
