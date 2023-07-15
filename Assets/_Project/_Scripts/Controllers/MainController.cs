using System;
using System.Collections;
using System.Collections.Generic;
using DevLocker.Utils;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

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
    [Header("Dependencies")]
    [SerializeField] private InputController inputController;
    
    [SerializeField] private Camera mainCamera;
    
    [SerializeField] private GameObject fullCanvas;
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject puertaAlcalaPivot;
    [SerializeField] private GameObject puertaAlcalaDoor;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI sceneTitleText;

    [SerializeField] private GameState gameState = GameState.MainMenu;
    [SerializeField] private List<BaseScene> gameScenes;
    
    [Header("Parameters")]
    
    private int _currentSceneIndex = 0;
    private BaseScene _currentScene;
    
    private void Start()
    {
        scoreText.gameObject.SetActive(false);
        sceneTitleText.gameObject.SetActive(false);
    }
    
    public void OnPlayButtonPressed()
    {
        // INTRO DEL JUEGO
        mainMenuCanvas.gameObject.SetActive(false);
        
        InitMinigame(0);
        
        StartCoroutine(TransitionInMinigameCoroutine());
    }
    
    private IEnumerator TransitionInMinigameCoroutine()
    {
        mainCamera.GetComponent<FadeCamera>().FadeOut(2f);
        yield return new WaitForSeconds(2f);

        // Puerta abre (in black)
        puertaAlcalaDoor.transform.DOLocalMoveY(2, 1f).From(0);     
        yield return new WaitForSeconds(1f);

        // Fade in Controllers si no se han explicado
        
        // Fade out Controllers
        
        // Animar Score, win or lose
        scoreText.gameObject.SetActive(true);
        scoreText.DOFade(1f, 0.5f).From(0f);
        yield return new WaitForSeconds(0.5f);

        // Fade in juego y texto a la vez
        gameScenes[_currentSceneIndex].SceneCamera.gameObject.SetActive(true);
        
        sceneTitleText.gameObject.SetActive(true);
        sceneTitleText.transform.DOScale(1f, 1f).From(5f);
        yield return new WaitForSeconds(1f);

        scoreText.DOFade(0f, 0.5f).From(1f);
        puertaAlcalaPivot.transform.DOScale(10, 1f).From(1f);
        
        gameScenes[_currentSceneIndex].SceneCamera.gameObject.SetActive(true);
        gameScenes[_currentSceneIndex].SceneCamera.GetComponent<FadeCamera>().FadeOut(1f);
        yield return new WaitForSeconds(1f);
        
        puertaAlcalaPivot.SetActive(false);
        
        yield return new WaitForSeconds(1f);
        fullCanvas.SetActive(false);
        
        gameScenes[_currentSceneIndex].StartGame();

        
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator TransitionOutMinigameCoroutine()
    {
        // Empezar a echar para atras camara
        
        // En 0.1f segundos, Fade to black
        
        // Update Score
        
        // Transicionar a otro juego
        
        yield return new WaitForEndOfFrame();
    }

    public void InitMinigame(int sceneIndex)
    {
        _currentSceneIndex = sceneIndex;
        
        // Load Scene in background
        var loadSceneOperation = SceneManager.LoadSceneAsync(gameScenes[_currentSceneIndex].UnitySceneReference.SceneName, LoadSceneMode.Additive);
        loadSceneOperation.completed += (x) => 
        {
            Debug.Log("Loaded Level Asynchronously with name " + gameScenes[_currentSceneIndex].UnitySceneReference.SceneName);
            
            gameScenes[_currentSceneIndex].Init(inputController);
        
            gameScenes[_currentSceneIndex].SceneWon += OnSceneWon;
            gameScenes[_currentSceneIndex].SceneLost += OnSceneLost;
        
            sceneTitleText.text = gameScenes[_currentSceneIndex].sceneTitle;
        };
    }
    
    private void OnSceneWon()
    {
        // Unload the current scene
        gameScenes[_currentSceneIndex].Unload();
        
        gameScenes[_currentSceneIndex].SceneWon -= OnSceneWon;
        gameScenes[_currentSceneIndex].SceneLost -= OnSceneLost;
        
        // Transition out
        StartCoroutine(TransitionOutMinigameCoroutine());

        _currentSceneIndex++;
        
        // Load the next scene
        if (_currentSceneIndex >= gameScenes.Count)
        {
            Debug.Log("Congratulations, you've finished all the levels!");
            return;
        }
        
        InitMinigame(_currentSceneIndex);
        
        Debug.Log("CurrentSceneIndex " + _currentSceneIndex);
    }
    
    private void OnSceneLost()
    {
        // Unload the current scene
        
        // Transition out
    }
}
