using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Intro,
    TransitionIn,
    Playing,
    Paused,
    TransitionOut,
    GameOver
}

public class MainController : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    [SerializeField] private GameObject mainCanvas;
    
    [SerializeField] private GameState gameState = GameState.MainMenu;
    [SerializeField] private List<BaseScene> gameScenes;

    [SerializeField] private BaseScene testScene;

    private int _currentSceneIndex = 0;
    
    
    public void OnPlayButtonPressed()
    {
        mainCanvas.gameObject.SetActive(false);

        StartMinigame(0);
    }
    public void StartMinigame(int sceneIndex)
    {
        _currentSceneIndex = sceneIndex;
        gameState = GameState.Playing;
        
        gameScenes[_currentSceneIndex].Init();
        
        gameScenes[_currentSceneIndex].SceneWon += OnSceneWon;
        gameScenes[_currentSceneIndex].SceneLost += OnSceneLost;
    }
    
    private void OnSceneWon()
    {
        // Unload the current scene
        gameScenes[_currentSceneIndex].Unload();
        
        gameScenes[_currentSceneIndex].SceneWon -= OnSceneWon;
        gameScenes[_currentSceneIndex].SceneLost -= OnSceneLost;
        
        // Transition out
        
        _currentSceneIndex++;
        
        // Load the next scene
        if (_currentSceneIndex >= gameScenes.Count)
        {
            Debug.Log("Congratulations, you've finished all the levels!");
            return;
        }
        
        
        StartMinigame(_currentSceneIndex);
        
        Debug.Log("CurrentSceneIndex " + _currentSceneIndex);
    }
    
    private void OnSceneLost()
    {
        // Unload the current scene
        
        // Transition out
    }
}
