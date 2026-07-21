using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode; 

public class CollisionFlash2D : NetworkBehaviour 
{
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
    [SerializeField] private List<TagColorConfig> customTagColors;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine flashCoroutine;

    private void Start()
    {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // So color flash only applies to a given players ship
        if (!IsOwner)
        {
            // Debug.Log("NOT the Owner");
            return;
        } 
        // Debug.Log("I AM the Owner");

        Color colorToFlash = GetColorForCollision(collision.gameObject);

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(SpriteFlashRoutine(colorToFlash));
    }

    private Color GetColorForCollision(GameObject collidedObject)
    {
        foreach (var config in customTagColors)
        {
            if (collidedObject.CompareTag(config.targetTag))
            {
                return config.flashColor;
            }
        }
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