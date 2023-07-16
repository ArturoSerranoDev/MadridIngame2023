using System;
using System.Collections;
using System.Collections.Generic;
using DevLocker.Utils;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Analytics;
using Unity.VisualScripting;

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
    [SerializeField] private GameObject finalScorePanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    [SerializeField, Space] private GameObject puertaAlcalaPivot;
    [SerializeField] private GameObject puertaAlcalaDoor;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI sceneTitleText;
    [SerializeField] private GameObject playButtonGO;

    [SerializeField, Space] private GameState gameState = GameState.MainMenu;
    [SerializeField] private List<BaseScene> gameScenes;
    
    
    private Dictionary<BaseScene, bool > successfullGames = new Dictionary<BaseScene, bool>();
    

    private int _currentSceneIndex = 0;
    private int _currentScore = 0;
    
    private bool _hasGameStarted = false;

    void OnEnable()
    {
        inputController.EscPressAction += RestartGame;
    }

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
        playButtonGO.SetActive(false);
        // INTRO DEL JUEGO
        
        MusicController.Instance.PlaySound(MusicController.Instance.gameplayMusic);
        StartCoroutine(IntroSequence());
    }

    [SerializeField] private Image GreyFloorImage;
    [SerializeField] private Image OsoImage;
    [SerializeField] private Image SchweppsImage;
    [SerializeField] private Image AlcalaImage;
    [SerializeField] private Image GameTitleImage;
    
    public IEnumerator IntroSequence()
    {
        // OsoImage.transform.doro(OsoImage.transform.localEulerAngles,1f).SetLoops(-1, LoopType.Incremental);
        OsoImage.transform.DOShakeRotation(3f, 10f, 20, 90f, true);
        SchweppsImage.transform.DOShakeRotation(3f, 10f, 20, 90f, true);
        AlcalaImage.transform.DOShakeRotation(3f, 10f, 20, 90f, true);
        
        GameTitleImage.transform.DOScale(12f, 3f);
        GameTitleImage.transform.DOLocalMoveY(GameTitleImage.transform.localPosition.y - 200f, 3f);
        

        yield return new WaitForSeconds(3f);
 
        GreyFloorImage.transform.DOScale(17f, 1f);
        GreyFloorImage.transform.DOLocalMoveY(-1900f, 1f);
        
        OsoImage.transform.DOLocalMoveX(1541f, 1f);
        OsoImage.transform.DOLocalMoveY(700f, 1f);
        
        SchweppsImage.transform.DOLocalMoveY(1000f, 1f);
        SchweppsImage.transform.DOLocalMoveX(-1180f, 1f);
        
        AlcalaImage.transform.DOLocalMoveY(AlcalaImage.transform.localPosition.y + 2000f, 1f);
        
        yield return new WaitForSeconds(2f);

        GameTitleImage.transform.DOLocalMoveY(GameTitleImage.transform.localPosition.y + 1000f, 2f);
        
        // Aparecer a la vez la puerta de alcala

        
        InitMinigame(0);
        
        StartCoroutine(TransitionInMinigameCoroutine(firstTime: true));
        yield return null;
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
        // gameScenes[_currentSceneIndex].sceneCamera.gameObject.SetActive(true);
        
        sceneTitleText.gameObject.SetActive(true);
        sceneTitleText.transform.DOScale(1f, 1f).From(5f);
        yield return new WaitForSeconds(1f);

        scoreText.DOFade(0f, 0.5f).From(1f);
        puertaAlcalaPivot.transform.DOScale(50, 1f).From(1f);
        
        gameScenes[_currentSceneIndex].sceneCamera.gameObject.SetActive(true);
        gameScenes[_currentSceneIndex].sceneCamera.GetComponent<FadeCamera>().FadeOut(1f);
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
        gameScenes[_currentSceneIndex].sceneCamera.gameObject.SetActive(true);
        gameScenes[_currentSceneIndex].sceneCamera.GetComponent<FadeCamera>().FadeIn(1f);
        yield return new WaitForSeconds(1f);
        
        puertaAlcalaPivot.SetActive(true);
        puertaAlcalaPivot.transform.DOScale(1f, 1f).From(50f);
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
        successfullGames[gameScenes[_currentSceneIndex]] = true;
            
        AudioManager.Instance.PlaySound(AudioManager.Instance.WinRoundClip);
       OnSceneEnded();
    }
    
    private void OnSceneLost()
    {
        successfullGames[gameScenes[_currentSceneIndex]] = false;

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
            //Debug.Log("Unloaded Level Asynchronously with name " + gameScenes[_currentSceneIndex].UnitySceneReference.SceneName);
        };
        
        _currentSceneIndex++;
        
        Debug.Log("CurrentSceneIndex " + _currentSceneIndex); 
        Debug.Log( "GameScenes.Count " + gameScenes.Count);
        // Load the next scene
        if (_currentSceneIndex >= gameScenes.Count)
        {
            // GAME OVER
            MusicController.Instance.PlaySound(MusicController.Instance.creditsMusic);
            // update final score text
            finalScoreText.text = "�Conseguiste superar " + _currentScore + "/" + gameScenes.Count + " pruebas!\r\n\r\nEsc - Jugar de nuevo";
            // show final score panel
            finalScorePanel.transform.DOScale(1f, 1f).SetEase(Ease.OutBounce);
            
           
            yield break;
        }

        InitMinigame(_currentSceneIndex);
        
        StartCoroutine(TransitionInMinigameCoroutine());
        
        Debug.Log("CurrentSceneIndex " + _currentSceneIndex);
    }

    public void OnPlayAgainPressed()
    {
        // Remove from the list the scenes that were not completed
        for (int i = gameScenes.Count - 1; i >= 0; i--)
        {
            if (!successfullGames[gameScenes[i]])
            {
                gameScenes.RemoveAt(i);
            }
        }

        _currentScore = 0;
        _currentSceneIndex = 0;
    }

    void RestartGame()
    {
        if (!_hasGameStarted)
            Application.Quit();

        SceneManager.LoadScene(0);
    }
}
