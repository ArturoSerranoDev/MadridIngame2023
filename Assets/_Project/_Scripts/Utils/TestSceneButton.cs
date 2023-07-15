using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSceneButton : MonoBehaviour
{
    [SerializeField] Button testSceneButton;
    
    private void Start()
    {
        testSceneButton.onClick.AddListener(OnTestSceneButtonPressed);
    }
    private void OnTestSceneButtonPressed()
    {
        TestBaseScene testBaseScene = FindObjectOfType<TestBaseScene>();
        if (testBaseScene != null)
        {
            testBaseScene.Win();
        }
    }
}
