using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PermanentCanvasReferences : MonoBehaviour
{
    [SerializeField] GameObject permanentCanvas;
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] Image timeFillerBar;
    [Space(10f)]
    [SerializeField] GameObject puertaAlcalaPivot;
    [SerializeField] GameObject puertaAlcalaDoor;
    [SerializeField] TextMeshProUGUI scoreText;
}
