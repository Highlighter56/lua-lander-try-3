using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_CollisionFlash2D : MonoBehaviour
{
    // A small helper structure to group a Tag string with a Color in the Inspector
    [System.Serializable]
    public struct TagColorConfig
    {
        public string targetTag;
        public Color flashColor;
    }

    [Header("Default Settings")]
    [SerializeField] private Color defaultFlashColor = Color.red;
    [SerializeField] private float flashDuration = 0.2f;

    [Header("Custom Tag Configurations")]
    [Tooltip("Add tags here to customize colors for landing pads, powerups, etc.")]
    [SerializeField] private List<TagColorConfig> customTagColors;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine flashCoroutine;

    private void Start()
    {
		/*
			Because its good practice to seperate the visuals (sprites) from the logic (I/O,...), 
			SpriteRenderer's are often located on child objects. 
		*/
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

	// This is a Built in Unity Method that gets called when 2D collisiosn occur
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Determine what color we should flash based on the tag
        Color colorToFlash = GetColorForCollision(collision.gameObject);

        // 2. Trigger the coroutine flash using that color
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(SpriteFlashRoutine(colorToFlash));
    }

    private Color GetColorForCollision(GameObject collidedObject)
    {
        // Loop through our list of custom tags to see if the collided object matches one
        foreach (var config in customTagColors)
        {
            // if the collided objects tag is found in the list of custom color flashing tags, get the associated color
            if (collidedObject.CompareTag(config.targetTag))
            {
                return config.flashColor;
            }
        }

        // Fallback color if the object is "Untagged" or not in our custom list
        return defaultFlashColor;
    }

    private IEnumerator SpriteFlashRoutine(Color flashColor)
    {
        if (spriteRenderer == null) yield break;

        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }
}