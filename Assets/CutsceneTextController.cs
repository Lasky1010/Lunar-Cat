using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneTextController : MonoBehaviour
{

    [Header("UI Components")]
    [Tooltip("UI Text, отображающий текст на экране")]
    [SerializeField] private TMPro.TextMeshProUGUI uiText;

    [Tooltip("Фон для катсцены")]
    [SerializeField] private GameObject background;

    [Header("Текст катсцены")]
    [TextArea(3, 6), Tooltip("Части текста, которые появятся последовательно")]
    [SerializeField] private string[] textParts;

    [Header("Настройки эффекта \"печатание текста\"")]
    [Tooltip("Интервал между символами (в секундах) при появлении текста")]
    [SerializeField] private float typeSpeed = 0.04f;

    // Индекс текущей части диалога
    private int currentPartIndex = 0;
    // Флаг, что текст полностью показан и ждём нажатия
    private bool waitingForClick = false;

    public void StartCutScene()
    {
        Debug.Log("StartCutScene() был вызван");
        // Убедимся, что чёрный фон и текст включены
        if (background != null) background.SetActive(true);
        if (uiText != null) uiText.text = "";

        // Если массив пуст или отсутствует UI Text — сразу выходим
        if (textParts == null || textParts.Length == 0 || uiText == null)
        {
            Debug.LogWarning("CutsceneTextController: Нет текста или UI Text не назначен.");
            EndCutscene();
            return;
        }

        // Запускаем показ первой части текста
        StartCoroutine(TypeTextCoroutine(textParts[currentPartIndex]));
    }

    private void Update()
    {
        if (waitingForClick)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                OnScreenClicked();
            }

            if (Input.GetMouseButtonDown(0))
            {
                OnScreenClicked();
            }
        }
    }

    /// <summary>
    /// Корутин для эффекта «печатание текста» (typewriter effect).
    /// Постепенно выводит символы строки с задержкой typeSpeed.
    /// По завершении устанавливает waitingForClick = true.
    /// </summary>
    private IEnumerator TypeTextCoroutine(string fullText)
    {
        uiText.text = "";
        waitingForClick = false;

        foreach (char c in fullText)
        {
            uiText.text += c;
            Debug.Log(uiText.text);
            yield return new WaitForSecondsRealtime(typeSpeed);
        }

        waitingForClick = true;
    }

    /// <summary>
    /// Вызывается при первом клике после завершения показа текста.
    /// Если есть ещё части, запускаем следующий корутин, иначе завершаем катсцену.
    /// </summary>
    private void OnScreenClicked()
    {
        waitingForClick = false;

        currentPartIndex++;

        if (currentPartIndex < textParts.Length)
        {
            StartCoroutine(TypeTextCoroutine(textParts[currentPartIndex]));
        }
        else
        {
            EndCutscene();
        }
    }

    /// <summary>
    /// Логика завершения катсцены: скрываем фон и текст, запускаем следующий уровень
    /// или возвращаем управление игроку.
    /// </summary>
    private void EndCutscene()
    {
        SceneManager.LoadScene("Level1");
    }
}
