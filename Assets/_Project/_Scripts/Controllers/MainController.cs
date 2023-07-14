using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

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
    
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject marcoAlrededorEscena;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI sceneTitleText;
    
    [SerializeField] private GameObject testPuertaIzquierda;
    [SerializeField] private GameObject testPuertaDerecha;
    
    [SerializeField] private GameState gameState = GameState.MainMenu;
    [SerializeField] private List<BaseScene> gameScenes;
    
    [Header("Parameters")]
    
    private int _currentSceneIndex = 0;
    
    private void Start()
    {
        scoreText.gameObject.SetActive(false);
        sceneTitleText.gameObject.SetActive(false);
    }
    
    public void OnPlayButtonPressed()
    {
        // INTRO DEL JUEGO
        mainCanvas.gameObject.SetActive(false);
        
        InitMinigame(0);
        
        StartCoroutine(TransitionInMinigameCoroutine());
    }
    private IEnumerator TransitionInMinigameCoroutine()
    {
        // Fade in Puerta cerrada
        marcoAlrededorEscena.gameObject.SetActive(true);
        foreach (Transform child in marcoAlrededorEscena.transform)
        {
            child.GetComponent<Renderer>().material.DOFade(1f, 0.5f).From(0f);
        }
        yield return new WaitForSeconds(0.5f);
        
        // Puerta abre (in black)
        testPuertaIzquierda.transform.DOMoveX(-6f, 1f).SetEase(Ease.InOutSine);
        testPuertaDerecha.transform.DOMoveX(6f, 1f).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(1f);

        // Fade in Controllers si no se han explicado
        
        // Fade out Controllers
        
        // Animar Score, win or lose
        scoreText.gameObject.SetActive(true);
        scoreText.DOFade(1f, 0.5f).From(0f);
        
        // Fade in juego y texto a la vez
        sceneTitleText.transform.DOScale(1f, 1f).From(5f);
        
        
        // Texto empieza en grande y lerpea hasta tamaño pequeño
        
        // Cuando el texto termina de lerpear, se agranda la view hasta dejar 100% view de la camara de la escena

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
        
        gameScenes[_currentSceneIndex].Init(inputController);
        
        gameScenes[_currentSceneIndex].SceneWon += OnSceneWon;
        gameScenes[_currentSceneIndex].SceneLost += OnSceneLost;
        
        sceneTitleText.text = gameScenes[_currentSceneIndex].sceneTitle;
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
