using System;
using DevLocker.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseScene: MonoBehaviour
{
   public Action SceneWon;
   public Action SceneLost;

   public SceneReference UnitySceneReference;
   
   public Camera SceneCamera;
   public string CameraTag;
   
   public float sceneDuration = 5f;
   public string sceneTitle = "Base Scene";
   
   private InputController inputController;
   
   private float timeElapsedInScene = 0f;
   
   public bool HasGameStarted = false;
   
  
   
   public void Update()
   {
      if (HasGameStarted)
      {
         timeElapsedInScene += Time.deltaTime;
      }
      
      if (timeElapsedInScene >= sceneDuration)
      {
         // END SCENE
         Debug.Log("Scene Ended from timeout");

         TimeoutEnded();
      }
   }

   public void StartGame()
   {
      HasGameStarted = true;
   }
   
   public virtual void Init(InputController inputControllerRef)
   {
      // Made sure we find the inputController even if we forget to assign it
      if (!inputControllerRef)
      {
         inputController = FindObjectOfType<InputController>();
      }
      else
      {
         inputController = inputControllerRef;
      }
      
      SceneCamera = GameObject.FindWithTag(CameraTag).GetComponent<Camera>();
      SceneCamera.gameObject.SetActive(false);
      
      inputController.KeyboardInputAction += OnKeyboardInputPressed;
      inputController.MouseLeftClickAction += OnMouseLeftClick;
      inputController.MouseRightClickAction += OnMouseRightClick;
      inputController.MouseMoveAction += OnMouseMove;
      
      this.gameObject.SetActive(true);
      
      Debug.Log("Loaded Scene with name: " + gameObject.name);
   }
   
   // Method used whenever time ends. It may yield a positive or negative result
   // depending on Scene Logic
   protected virtual void TimeoutEnded()
   {
      
   }
   
   public virtual void Unload()
   {
      inputController.KeyboardInputAction -= OnKeyboardInputPressed;
      inputController.MouseLeftClickAction -= OnMouseLeftClick;
      inputController.MouseRightClickAction -= OnMouseRightClick;
      inputController.MouseMoveAction -= OnMouseMove;
      
      this.gameObject.SetActive(false);
      HasGameStarted = false;
      
      Debug.Log("Unloaded Scene with name: " + gameObject.name);
   }
   
   protected virtual void OnMouseLeftClick()
   {
   }
   
   protected virtual void OnMouseRightClick()
   {
   }
   
   protected virtual void OnMouseMove(Vector2 obj)
   {
   }

   protected virtual void OnKeyboardInputPressed(KeyCode keyPressed)
   {
   }
   
   public virtual void Win()
   {
      SceneWon?.Invoke();
   }

   protected virtual void Lose()
   {
      SceneLost?.Invoke();
   }
}
