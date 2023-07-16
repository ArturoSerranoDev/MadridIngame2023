using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EndlessRunnerScene : BaseScene
{
    private enum PlayerPositionType
    {
        Left,
        Center,
        Right
    }
    
    public int trashCleaned = 0;
    
    private PlayerPositionType _playerPositionType = PlayerPositionType.Center;
    private EndlessRunnerRefs _endlessRunnerRefs;

    private bool isJumping = false;
    
    public override void StartGame()
    {
        base.StartGame();
        
        _endlessRunnerRefs.playerRunner.playerAnimator.SetBool("Run", true);

        _endlessRunnerRefs.torresKio.transform.DOMoveY(_endlessRunnerRefs.torresKio.transform.position.y + 18, 6);

    }
    public override void Init(InputController inputControllerRef)
    {
        _endlessRunnerRefs = FindObjectOfType<EndlessRunnerRefs>();
        sceneCamera = _endlessRunnerRefs.runnerCamera;

        _endlessRunnerRefs.playerRunner.endlessRunnerScene = this; // DONT DO THIS AT HOME
        
        base.Init(inputControllerRef);
    }

    public override void Update()
    {
        base.Update();
        
        if (HasGameStarted)
        {
            _endlessRunnerRefs.rotatingWorld.transform.RotateAround(_endlessRunnerRefs.rotatingWorld.transform.position, Vector3.forward, 25f * Time.deltaTime);
        }
    }

    // Method used whenever time ends. It may yield a positive or negative result
    // depending on Scene Logic
    protected override void TimeoutEnded()
    {
        if (_endlessRunnerRefs.playerRunner.trashCleaned >= 3)
        {
            Win();
        }
        else
        {
            Lose();
        }
    }

    public override void Unload()
    {
        base.Unload();
    }

    public void OnPlayerTriggerEnter(Collider other)
    {
        if (!isJumping)
        {
            _endlessRunnerRefs.playerRunner.playerAnimator.SetBool("Crash", true);
            AudioManager.Instance.PlaySound(AudioManager.Instance.hitGarbageClip);
            Lose();
        }
        else
        {
            _endlessRunnerRefs.playerRunner.trashCleaned++;
            AudioManager.Instance.PlaySound(AudioManager.Instance.CollectGarbageClip);
            _endlessRunnerRefs.playerRunner.playerScore.text = _endlessRunnerRefs.playerRunner.trashCleaned.ToString();
            Destroy(other.gameObject);
        }
    }

    protected override void OnMouseLeftClick()
    {
        _endlessRunnerRefs.playerRunner.playerAnimator.SetBool("Jump", true);
        isJumping = true;
        
        AudioManager.Instance.PlaySound(AudioManager.Instance.jumpSoundClip);

        Invoke( "SetJumpToFalse", 1.28f);
    }
    
    public void SetJumpToFalse()
    {
        _endlessRunnerRefs.playerRunner.playerAnimator.SetBool("Jump", false);
        isJumping = false;
    }

    protected override void OnMouseRightClick()
    {
    }

    protected override void OnMouseMove(Vector2 obj)
    {
    }

    protected override void OnKeyboardInputPressed(KeyCode keyPressed)
    {
        if(!HasGameStarted)
            return;
        
        if (keyPressed == KeyCode.D)
        {
            if (_playerPositionType == PlayerPositionType.Left)
                return;
            
            if(_playerPositionType == PlayerPositionType.Center)
            {
                _playerPositionType = PlayerPositionType.Left;
            }
            else if (_playerPositionType == PlayerPositionType.Right)
            {
                _playerPositionType = PlayerPositionType.Center;
            }
            
            _endlessRunnerRefs.playerCharacter.transform.localPosition -= new Vector3(0, 0, 0.5f);
        }
        else if (keyPressed == KeyCode.A)
        {
            if (_playerPositionType == PlayerPositionType.Right)
                return;
            
            if(_playerPositionType == PlayerPositionType.Center)
            {
                _playerPositionType = PlayerPositionType.Right;
            }
            else if (_playerPositionType == PlayerPositionType.Left)
            {
                _playerPositionType = PlayerPositionType.Center;
            }
            
            _endlessRunnerRefs.playerCharacter.transform.localPosition += new Vector3(0, 0, 0.5f);
        }
    }

    public override void Win()
    {
        SceneWon?.Invoke();
    }

    protected override void Lose()
    {
        SceneLost?.Invoke();
    }
}
