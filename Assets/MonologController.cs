using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonologController : MonoBehaviour
{
    [Header("Настройка UI")]
    [Tooltip("Panel, которая содержит фон и MonologText (UI → Text)")]
    [SerializeField] private GameObject monologPanel;

    [Tooltip("Text-компонент внутри monologPanel, который показывает сам текст")]
    [SerializeField] private TMPro.TextMeshProUGUI monologText;

    [Header("Монолог героя (строки)")]
    [TextArea(1, 5), Tooltip("Последовательность строк: каждая строка появится по нажатию игрока")]
    [SerializeField] public List<string> textParts = new List<string>();

    [Header("Настройки эффекта \"печатание текста\"")]
    [Tooltip("Интервал между символами (в секундах) при появлении текста")]
    [SerializeField] private float typeSpeed = 0.04f;

    // Индекс текущей части диалога
    private int currentPartIndex = 0;
    // Флаг, что текст полностью показан и ждём нажатия
    private bool waitingForInput = false;

    // Флаг, что мы сейчас показываем диалог (игра должна быть на паузе)
    private bool isMonologActive = false;

    public void StartMonolog()
    {
        int currentPartIndex = 0;
        if (monologPanel == null || monologText == null)
        {
            Debug.LogError("MonologController: MonologPanel или MonologText не настроены в инспекторе!");
            return;
        }

        if (textParts == null || textParts.Count == 0)
        {
            Debug.LogWarning("MonologController: MonologLines пуст или не заданы.");
            return;
        }

        // Показываем панель и ставим игру на паузу
        monologPanel.SetActive(true);
        Time.timeScale = 0f; // Полная пауза игры
        isMonologActive = true;
        StartCoroutine(TypeTextCoroutine(textParts[currentPartIndex]));
    }

    private IEnumerator TypeTextCoroutine(string textLine)
    {
        waitingForInput = false;
        monologText.text = "";

        for (int i = 0; i < textLine.Length; i++)
        {
            monologText.text += textLine[i];
            yield return new WaitForSecondsRealtime(typeSpeed);
        }

        waitingForInput = true;
    }

    private void Update()
    {
        if (!isMonologActive) return;
        if (!waitingForInput) return;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            OnScreenClicked();
        }
        if (Input.GetMouseButtonDown(0))
        {
            OnScreenClicked();
        }
    }

    private void OnScreenClicked()
    {
        waitingForInput = false;
        currentPartIndex++;

        if (currentPartIndex < textParts.Count)
        {
            StartCoroutine(TypeTextCoroutine(textParts[currentPartIndex]));
        }
        else
        {
            EndMonolog();
        }
    }

    private void EndMonolog()
    {
        monologPanel.SetActive(false);
        Time.timeScale = 1f;
        isMonologActive = false;
    }
}
