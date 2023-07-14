using System;
using UnityEngine;

public abstract class BaseScene: MonoBehaviour
{
   public Action SceneWon;
   public Action SceneLost;

   public Camera SceneCamera;
   
   public float sceneDuration = 5f;
   public string sceneTitle = "Base Scene";
   
   private InputController inputController;
   
   private float timeElapsedInScene = 0f;
   public bool IsSceneInit = false;
   
   public void Update()
   {
      if (IsSceneInit)
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
      
      inputController.KeyboardInputAction += OnKeyboardInputPressed;
      inputController.MouseLeftClickAction += OnMouseLeftClick;
      inputController.MouseRightClickAction += OnMouseRightClick;
      inputController.MouseMoveAction += OnMouseMove;
      
      this.gameObject.SetActive(true);
      
      IsSceneInit = true;
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
      IsSceneInit = false;
      
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
