using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        public int coinsCounter = 0;
        [SerializeField] private GameObject _endImage;
        [SerializeField] private GameObject _losePanel;

        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioClip _endGameClip;
        [SerializeField] private AudioSource _soundSource;
        [SerializeField] private SceneSaveLogics _sceneSaveLogics;

        public GameObject playerGameObject;
        private PlayerController player;
        public GameObject deathPlayerPrefab;
        public Text coinText;

        void Start()
        {
            if(_sceneSaveLogics._enableMusic == false)
                _musicSource.enabled = false;
            if(_sceneSaveLogics._enableSound == false)
                _soundSource.enabled = false;

            player = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        void Update()
        {
            if(coinText != null)
                coinText.text = coinsCounter.ToString();
            if (player != null && player.deathState == true)
            {
                playerGameObject.SetActive(false);
                GameObject deathPlayer = (GameObject)Instantiate(deathPlayerPrefab, playerGameObject.transform.position, playerGameObject.transform.rotation);
                deathPlayer.transform.localScale = new Vector3(playerGameObject.transform.localScale.x, playerGameObject.transform.localScale.y, playerGameObject.transform.localScale.z);
                player.deathState = false;
                ChangeMusic(_endGameClip);
                _endImage.SetActive(true);
                Invoke("ReloadLevel", 2);
            }
        }
        public void ChangeMusic(AudioClip clip)
        {
            _musicSource.clip = null;
            _soundSource.PlayOneShot(clip);
        }
        public void RestartLevel()
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        private void ReloadLevel()
        {
            _losePanel.SetActive(true);
        }
    }
}
