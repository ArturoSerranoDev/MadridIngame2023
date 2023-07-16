using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FadeCamera : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup cameraCanvasGroup;
    
    [SerializeField] private Material fadeMaterial;
    
    public CanvasGroup CameraCanvasGroup => cameraCanvasGroup;
    
    public void FadeIn(float duration)
    {
        // cameraCanvasGroup.DOFade(1f, duration);
        fadeMaterial.DOFade(1f, duration);
    }
    
    public void FadeOut(float duration)
    {
        // cameraCanvasGroup.DOFade(0f, duration).From(1f);
        fadeMaterial.DOFade(0f, duration).From(1f);

    }
}
