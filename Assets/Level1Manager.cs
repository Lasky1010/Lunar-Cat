using UnityEngine;
using System.Collections;

public class Level1Manager : MonoBehaviour
{
    [Header("������ �� ���������")]
    [Tooltip("������ �� MonologController (UI Panel + Text)")]
    [SerializeField] private MonologController MonologController;

    // ����: ������ �������� ��� �� ������
    private bool firstCrystalCollected = false;

    // ����: ����� ��� �� ������������
    private bool wallActivatedFirstTime = false;

    // ����: ������ ��������� ������ ��� �� ���������
    private bool firstPhysicalChange = false;

    private void Start()
    {
        // 1. ��� ������� Level1 ���������� ������� (����� 1)
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
    /// �����, ������� ���������� �� �������� ������� ���������, 
    /// ����� ����� �������� �����-���� ��������.
    /// </summary>
    public void OnCrystalCollected(int crystalIndex)
    {
        if (!firstCrystalCollected && crystalIndex == 0)
        {
            firstCrystalCollected = true;

            // 2. ������ �������� ������ � ���������� ������ ����� (����� 2)
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
    /// �����, ���������� �� ������ �����, ����� ����� �������� ���������.
    /// </summary>
    public void OnWallStartMoving()
    {
        if (!wallActivatedFirstTime)
        {
            wallActivatedFirstTime = true;

            // 3. ����� �������������� � ���������� �����, ����� ������, ������ ������ (����� 3)
            MonologController.textParts = new System.Collections.Generic.List<string>
            {
                "Looeny: Oh no, what's going on, it looks like we need to run away soon!\n" +
                "(tap to continue)"
            };

            MonologController.StartMonolog();

            // �� ����� ������� ���� ��� ����� �� ����� (Time.timeScale = 0), 
            // �� ��� ����� ������ � ������ ��������� ������ �����, 
            // ����� ��� ���������, ���� ��� ������. 
            // ������� ���������� WaitForSecondsRealtime ��� ��������, ���� �����.
            //cameraShake.Shake(1.5f, 0.2f); // ����� ������ 1.5 ��� � ���������� 0.2 (������)

            // ������ ������: ��� ����, ������ ����� ���
            //audioManager.PlayWallMusic();
        }
    }

    /// <summary>
    /// �����, ���������� ��� ������ ��������� ���������� ������� ������ (�������),
    /// ��������, � ������� RandomEffectController, ����� ������ ������ ������.
    /// </summary>
    public void OnFirstPhysicalEffect()
    {
        if (!firstPhysicalChange)
        {
            firstPhysicalChange = true;

            // 4. ������ ��������� ������ � ���������� ����� (����� 4)
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
