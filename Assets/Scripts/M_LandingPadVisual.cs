using TMPro;
using UnityEngine;

public class M_LandingPadVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreMultiplyerTextMesh;

    // Awake should be used to set up all local references
    private void Awake()
    {
        M_IdentifyLandingPad landingPadScript = GetComponent<M_IdentifyLandingPad>();
        scoreMultiplyerTextMesh.text = $"x{landingPadScript.getScoreMultiplyer()}";
    }
}
