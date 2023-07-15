using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndlessRunnerPlayer : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private TextMeshProUGUI playerScore;
    
    public int trashCleaned = 0;
    private void OnTriggerEnter(Collider other)
    {
        // Play success SFX 
        
        // Play Jump Animation
        playerAnimator.SetBool("Jump", true);
        
        // Remove object
        Destroy(other.gameObject);

        trashCleaned++;
        playerScore.text = trashCleaned.ToString();
        
        Invoke("SetJumpFalse", 0.5f);
    }
    
    private void SetJumpFalse()
    {
        playerAnimator.SetBool("Jump", false);
    }
}
