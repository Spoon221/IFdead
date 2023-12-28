using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameCanvas : MonoBehaviour
{
    [SerializeField] private static RectTransform keyRect;
    [SerializeField] private static Image progressBar;
    [SerializeField] private static GameObject canvas;

    [SerializeField] private RectTransform _keyRect;
    [SerializeField] private Image _progressBar;

    public static Image ProgressBar => progressBar;

    public static RectTransform KeyRect => keyRect;

    public static GameObject Canvas => canvas;

    private void Awake()
    {
        keyRect = _keyRect;
        progressBar = _progressBar;

        canvas = this.gameObject;
        canvas.SetActive(false);
    }
}
