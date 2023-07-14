using System;
using UnityEngine;

public abstract class BaseScene: MonoBehaviour
{
   [HideInInspector] public InputController inputController;
   
   public Action SceneWon;
   public Action SceneLost;
   
   public float SceneDuration { get; protected set; }
   public float TimeElapsedInScene;

   public virtual void Init()
   {
      // Made sure we find the inputController even if we forget to assign it
      if (!inputController)
      {
         inputController = FindObjectOfType<InputController>();
      }
      
      inputController.KeyboardInputAction += OnKeyboardInputPressed;
      inputController.MouseLeftClickAction += OnMouseLeftClick;
      inputController.MouseRightClickAction += OnMouseRightClick;
      inputController.MouseMoveAction += OnMouseMove;
      
      this.gameObject.SetActive(true);
      
      Debug.Log("Loaded Scene with name: " + gameObject.name);
   }
   
   public virtual void Unload()
   {
      inputController.KeyboardInputAction -= OnKeyboardInputPressed;
      inputController.MouseLeftClickAction -= OnMouseLeftClick;
      inputController.MouseRightClickAction -= OnMouseRightClick;
      inputController.MouseMoveAction -= OnMouseMove;
      
      this.gameObject.SetActive(false);

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
