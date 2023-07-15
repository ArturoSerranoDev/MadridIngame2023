using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndlessRunnerPlayer : MonoBehaviour
{
    [SerializeField] public Animator playerAnimator;
    [SerializeField] public TextMeshProUGUI playerScore;
    
    public EndlessRunnerScene endlessRunnerScene;
    public int trashCleaned = 0;
    public bool isJumping = false;
    
    private void OnTriggerEnter(Collider other)
    {
        endlessRunnerScene.OnPlayerTriggerEnter(other);
    }
}
