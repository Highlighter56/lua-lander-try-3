using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class M_IdentifyLandingPad : MonoBehaviour
{
	// Attributes 
	[SerializeField] private int scoreMultiplyer = 1;


	// Attributes should always be private, so best to make a public getter. Most other methods should be private
    public int getScoreMultiplyer()
    {
        return scoreMultiplyer;
    }
}
