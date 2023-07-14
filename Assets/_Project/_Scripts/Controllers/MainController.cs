using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    Transitioning,
    GameOver
}

public class MainController : MonoBehaviour
{
    [SerializeField] private InputController inputController;
    
    [SerializeField] private GameState gameState = GameState.MainMenu;
    [SerializeField] private List<BaseScene> gameScenes;

    [SerializeField] private BaseScene testScene;

    private void Start()
    {
        testScene.Init();
        
        testScene.SceneWon += OnSceneWon;
        testScene.SceneLost += OnSceneLost;
    }
    
    private void OnSceneWon()
    {
        testScene.Unload();
        
        testScene.SceneWon -= OnSceneWon;
        testScene.SceneLost -= OnSceneLost;
    }
    
    private void OnSceneLost()
    {
        
    }
}
