using Platformer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalLogic : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] public GameObject _wall;
    [SerializeField] private int _needCoins;
    [SerializeField] private int _wallMovedCoins;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _endImageAnim;
    [SerializeField] private AudioClip _portalClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_gameManager.coinsCounter >= _needCoins)
            {
                _gameManager.ChangeMusic(_portalClip);
                _endImageAnim.SetActive(true);
                _playerController.gameObject.SetActive(false);
                if (SceneManager.sceneCountInBuildSettings - 1 == SceneManager.GetActiveScene().buildIndex)
                    Invoke(nameof(ChangeLevelZero), 8f);
                else
                    Invoke(nameof(ChangeNextLevel), 8f);
            }
        }
    }
    private void ChangeLevelZero()
    {
        SceneManager.LoadScene(0);
    }
    private void ChangeNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CoinChecker()
    {
        if (_gameManager.coinsCounter == _needCoins - _wallMovedCoins)
        {
            Debug.Log("MoveWall");
            _wall.SetActive(true);
        }
    }
}
