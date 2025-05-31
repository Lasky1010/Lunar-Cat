using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSaveLogics : MonoBehaviour
{
    [SerializeField] private int Level = -1;

    [SerializeField] public bool _enableSound = true;
    [SerializeField] public bool _enableMusic = true;

    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Sprite _enableMusicImg;
    [SerializeField] private Sprite _disableMusicImg;
    [SerializeField] private Sprite _enableSoundImg;
    [SerializeField] private Sprite _disableSoundImg;
    void Awake()
    {
        int i = 1;
        i = PlayerPrefs.GetInt("MusicState", 1);
        if (i == 0)
        {
            _enableMusic = false;
            if(_musicButton != null)
                _musicButton.image.sprite = _disableMusicImg;
        }
        int i2 = 1;
        i2 = PlayerPrefs.GetInt("SoundState", 1);
        if (i2 == 0)
        {
            _enableSound = false;
            if (_soundButton != null)
                _soundButton.image.sprite = _disableSoundImg;
        }

        if (Level >= 0)
        {
            PlayerPrefs.SetInt("LevelNum", Level);
        }
    }
    public void ChangeStateMusic()
    {
        int i = 1;
        _enableMusic = !_enableMusic;
        if (_enableMusic == false)
        {
            i = 0;
            _musicButton.image.sprite = _disableMusicImg;
        }
        else
            _musicButton.image.sprite = _enableMusicImg;
        PlayerPrefs.SetInt("MusicState", i);
    }
    public void ChangeStateSound()
    {
        _enableSound = !_enableSound;
        int i = 1;
        if (_enableSound == false)
        {
            i = 0;
            _soundButton.image.sprite = _disableSoundImg;
        }
        else
            _soundButton.image.sprite = _enableSoundImg;
        PlayerPrefs.SetInt("SoundState", i);
    }
    public void NewGame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("LevelNum",0) + 1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
