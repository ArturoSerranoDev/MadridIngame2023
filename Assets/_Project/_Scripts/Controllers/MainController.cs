using System;
using System.Collections;
using System.Collections.Generic;
using DevLocker.Utils;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    
    [SerializeField, Space] private GameObject permanentCanvas;
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private Image timerFillBar;

    [SerializeField, Space] private GameObject puertaAlcalaPivot;
    [SerializeField] private GameObject puertaAlcalaDoor;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI sceneTitleText;

    [SerializeField, Space] private GameState gameState = GameState.MainMenu;
    [SerializeField] private List<BaseScene> gameScenes;
    
    private int _currentSceneIndex = 0;
    private int _currentScore = 0;
    
    private bool _hasGameStarted = false;
    
    
    private void Start()
    {
        scoreText.gameObject.SetActive(false);
        sceneTitleText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_hasGameStarted)
        {
            timerFillBar.fillAmount -= Time.deltaTime / gameScenes[_currentSceneIndex].sceneDuration;
        }
        else
        {
            timerFillBar.fillAmount = 0f;
        }
    }

    public void OnPlayButtonPressed()
    {
        // INTRO DEL JUEGO
        mainMenuCanvas.gameObject.SetActive(false);
        
        InitMinigame(0);
        
        StartCoroutine(TransitionInMinigameCoroutine(firstTime: true));
    }
    
    private IEnumerator TransitionInMinigameCoroutine(bool firstTime = false)
    {
        permanentCanvas.SetActive(true);
        scoreText.gameObject.SetActive(false);
        sceneTitleText.gameObject.SetActive(false);

        scoreText.text = _currentScore.ToString();

        if (firstTime)
        {
            mainCamera.GetComponent<FadeCamera>().FadeOut(2f);
            yield return new WaitForSeconds(2f);

            // Puerta abre (in black)
            puertaAlcalaDoor.transform.DOLocalMoveY(2, 1f);     
            yield return new WaitForSeconds(1f);
        }

        // Fade in Controllers si no se han explicado
        
        // Fade out Controllers
        
        // Animar Score, win or lose
        scoreText.gameObject.SetActive(true);
        scoreText.DOFade(1f, 0.5f).From(0f);
        yield return new WaitForSeconds(1.5f);

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
        // permanentCanvas.SetActive(false);
        sceneTitleText.gameObject.SetActive(false);
            
        // EMPIEZA JUEGO
        gameScenes[_currentSceneIndex].StartGame();
        timerFillBar.fillAmount = 1f;
        _hasGameStarted = true;
        
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator TransitionOutMinigameCoroutine()
    {
        gameScenes[_currentSceneIndex].SceneCamera.gameObject.SetActive(true);
        gameScenes[_currentSceneIndex].SceneCamera.GetComponent<FadeCamera>().FadeIn(1f);
        yield return new WaitForSeconds(1f);
        
        puertaAlcalaPivot.SetActive(true);
        puertaAlcalaPivot.transform.DOScale(1f, 1f).From(10f);
        yield return new WaitForSeconds(1f);

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
        _currentScore++; // add 1 to score
        
       OnSceneEnded();
    }
    
    private void OnSceneLost()
    {
        OnSceneEnded();
    }

    private void OnSceneEnded()
    {
        _hasGameStarted = false;
        
        scoreText.text = _currentScore.ToString();

        // Unload the current scene
        gameScenes[_currentSceneIndex].Unload();
        
        gameScenes[_currentSceneIndex].SceneWon -= OnSceneWon;
        gameScenes[_currentSceneIndex].SceneLost -= OnSceneLost;
        

        StartCoroutine(OnScenePassedCoroutine());
    }

    public IEnumerator OnScenePassedCoroutine()
    {
        // Transition out
        yield return StartCoroutine(TransitionOutMinigameCoroutine());
        
        // Load Scene in background
        var loadSceneOperation = SceneManager.UnloadSceneAsync(gameScenes[_currentSceneIndex].UnitySceneReference.SceneName);
        loadSceneOperation.completed += (x) => 
        {
            Debug.Log("Unloaded Level Asynchronously with name " + gameScenes[_currentSceneIndex].UnitySceneReference.SceneName);
        };
        
        _currentSceneIndex++;
        
        // Load the next scene
        if (_currentSceneIndex >= gameScenes.Count)
        {
            Debug.Log("Congratulations, you've finished all the levels!");
            yield break;
        }
        
        InitMinigame(_currentSceneIndex);
        
        StartCoroutine(TransitionInMinigameCoroutine());
        
        Debug.Log("CurrentSceneIndex " + _currentSceneIndex);
    }
}
