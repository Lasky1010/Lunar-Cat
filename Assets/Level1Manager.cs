using UnityEngine;
using System.Collections;

public class Level1Manager : MonoBehaviour
{
    [Header("Ссылки на менеджеры")]
    [Tooltip("Ссылка на MonologController (UI Panel + Text)")]
    [SerializeField] private MonologController MonologController;

    // Флаг: первый кристалл ещё не собран
    private bool firstCrystalCollected = false;

    // Флаг: стена ещё не активирована
    private bool wallActivatedFirstTime = false;

    // Флаг: первое изменение физики ещё не произошло
    private bool firstPhysicalChange = false;

    private void Start()
    {
        // 1. При запуске Level1 показываем монолог (пункт 1)
        MonologController.textParts = new System.Collections.Generic.List<string>
        {
           "Looeny: I need to collect all the crystal shards as soon as possible," +
           "I just see the first one, need to get to it." +
           "Just don't fall into a trap!!!" +
           "But the boxes will be able to help me to climb higher.\n" +
           "(tap to continue)"
        };

        MonologController.StartMonolog();
    }

    /// <summary>
    /// Метод, который вызывается из местного скрипта кристалла, 
    /// когда игрок собирает какой-либо кристалл.
    /// </summary>
    public void OnCrystalCollected(int crystalIndex)
    {
        if (!firstCrystalCollected && crystalIndex == 0)
        {
            firstCrystalCollected = true;

            // 2. Первый кристалл собран – показываем второй текст (пункт 2)
            MonologController.textParts = new System.Collections.Generic.List<string>
            {
               "Looeny: The first crystal has been collected, perfectly!" +
               "It remains to find the rest. If only I didn't get caught by the monsters," +
               "they wouldn't let me complete the mission, for them I'm just a pest.\n" +
               "(tap to continue)"
            };
            MonologController.StartMonolog();
        }
    }

    /// <summary>
    /// Метод, вызываемый из логики стены, когда стена начинает двигаться.
    /// </summary>
    public void OnWallStartMoving()
    {
        if (!wallActivatedFirstTime)
        {
            wallActivatedFirstTime = true;

            // 3. Стена активировалась – показываем текст, трясём камеру, меняем музыку (пункт 3)
            MonologController.textParts = new System.Collections.Generic.List<string>
            {
                "Looeny: Oh no, what's going on, it looks like we need to run away soon!\n" +
                "(tap to continue)"
            };

            MonologController.StartMonolog();

            // во время диалога игра уже стоит на паузе (Time.timeScale = 0), 
            // но нам нужно тряску и музыку запустить именно сразу, 
            // чтобы они сработали, пока идёт диалог. 
            // Поэтому используем WaitForSecondsRealtime для задержек, если нужно.
            //cameraShake.Shake(1.5f, 0.2f); // трясём камеру 1.5 сек с амплитудой 0.2 (пример)

            // Меняем музыку: фон выкл, музыка стены вкл
            //audioManager.PlayWallMusic();
        }
    }

    /// <summary>
    /// Метод, вызываемый при каждом изменении физических свойств игрока (эффекта),
    /// например, в скрипте RandomEffectController, когда выбран первый эффект.
    /// </summary>
    public void OnFirstPhysicalEffect()
    {
        if (!firstPhysicalChange)
        {
            firstPhysicalChange = true;

            // 4. Первое изменение физики – показываем текст (пункт 4)
            MonologController.textParts = new System.Collections.Generic.List<string>
            {
                "Looeny: Oh, no, something is happening to me, probably because of the instability of the moon\n" +
                "(tap to continue)",
                "Due to the instability of the moon, the physical properties of the moon are changing, from above you can see what state it is in now.\n" +
                "(tap to continue)"
            };
            MonologController.StartMonolog();
        }
    }
}
