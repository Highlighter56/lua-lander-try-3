using System.Collections;
using UnityEngine;

public class M_CollisionFlash2D : MonoBehaviour
{
    [Header("Flash Settings")]
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.2f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine flashCoroutine;

    private void Start()
    {
        // Grab the 2D Sprite Renderer component attached to the ship
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Fallback check if the sprite renderer is on a child object
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        // Store the original color of your spaceship sprite
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    // Called automaticly when 2D objects collide
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If we are already flashing, stop it so we can restart the timer fresh
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        
        // Start our pause-able coroutine
        flashCoroutine = StartCoroutine(SpriteFlashRoutine());
    }

    private IEnumerator SpriteFlashRoutine()
    {
        if (spriteRenderer == null) yield break;

        // Tint the spaceship sprite red
        spriteRenderer.color = flashColor;

        // Pause right here for 0.2 seconds, letting the game keep running
        yield return new WaitForSeconds(flashDuration);

        // Reset back to the ship's normal sprite color
        spriteRenderer.color = originalColor;
    }
}