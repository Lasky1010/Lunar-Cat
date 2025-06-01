using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;


[RequireComponent(typeof(Platformer.PlayerController))]
public class RandomEffectController : MonoBehaviour
{
    [SerializeField] private float minInterval = 1f;
    [SerializeField] private float maxInterval = 3f;
    [SerializeField] private float effectDuration = 7f;
    [SerializeField] private float speedMultiplier = 2f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private float scaleMultiplier = 1.3f;

    [SerializeField] private Text gravityStatus;
    [SerializeField] private Text speedStatus;

    private Platformer.PlayerController playerController;
    private Rigidbody2D playerRigidbody;

    private float originalSpeed;
    private float originalGravity;

    private void Awake()
    {
        playerController = GetComponent<Platformer.PlayerController>();
        playerRigidbody = GetComponent<Rigidbody2D>();

        originalSpeed = playerController.movingSpeed;
        originalGravity = playerRigidbody.gravityScale;
    }

    private void Start()
    {
        StartCoroutine(RandomEffectLoop());
    }

    public enum Effect
    {
        NONE, SLOW, FAST, GRAVITY_LOW, GRAVITY_HIGH
    }

    private IEnumerator RandomEffectLoop()
    {
        while (true)
        {
            float waitTime = UnityEngine.Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            Array values = Enum.GetValues(typeof(Effect));
            System.Random random = new System.Random();
            Effect randomEffect = (Effect)values.GetValue(random.Next(1, 4));

            if (SceneManager.GetActiveScene().name.Equals("Level1")) {
                FindFirstObjectByType<Level1Manager>().OnFirstPhysicalEffect();
            }

            switch (randomEffect)
            {
                case Effect.FAST:
                    Debug.Log("Fast");
                    playerController.movingSpeed = originalSpeed * speedMultiplier;
                    transform.localScale = new Vector3(
                        transform.localScale.x / scaleMultiplier,
                        transform.localScale.y / scaleMultiplier,
                        transform.localScale.z);
                    UpdateStatus(Effect.FAST);
                    break;
                case Effect.SLOW:
                    Debug.Log("Slow");
                    playerController.movingSpeed = originalSpeed / speedMultiplier;
                    transform.localScale = new Vector3(
                        transform.localScale.x * scaleMultiplier,
                        transform.localScale.y * scaleMultiplier,
                        transform.localScale.z);
                    UpdateStatus(Effect.SLOW);
                    break;
                case Effect.GRAVITY_HIGH:
                    Debug.Log("Gravity high");
                    playerRigidbody.gravityScale = originalGravity * gravityMultiplier;
                    UpdateStatus(Effect.GRAVITY_HIGH);
                    break;
                case Effect.GRAVITY_LOW:
                    Debug.Log("Gravity low");
                    playerRigidbody.gravityScale = originalGravity / gravityMultiplier;
                    UpdateStatus(Effect.GRAVITY_LOW);
                    break;
            }

            yield return new WaitForSeconds(effectDuration);

            playerController.movingSpeed = originalSpeed;
            playerRigidbody.gravityScale = originalGravity;
            if (randomEffect == Effect.FAST)
            {
                transform.localScale = new Vector3(
                transform.localScale.x * scaleMultiplier,
                transform.localScale.y * scaleMultiplier,
                transform.localScale.z);
            }
            else if (randomEffect == Effect.SLOW)
            {
                transform.localScale = new Vector3(
                transform.localScale.x / scaleMultiplier,
                transform.localScale.y / scaleMultiplier,
                transform.localScale.z);
            }
            UpdateStatus(Effect.NONE);

        }
    }

    private void UpdateStatus(Effect effect)
    {
        switch (effect)
        {
            case Effect.FAST:
                gravityStatus.text = "NORMAL";
                speedStatus.text = "FAST";
                gravityStatus.color = Color.green;
                speedStatus.color = Color.blue;
                break;
            case Effect.SLOW:
                gravityStatus.text = "NORMAL";
                speedStatus.text = "SLOW";
                gravityStatus.color = Color.green;
                speedStatus.color = Color.red;
                break;
            case Effect.GRAVITY_HIGH:
                gravityStatus.text = "HIGH";
                speedStatus.text = "NORMAL";
                gravityStatus.color = Color.blue;
                speedStatus.color = Color.green;
                break;
            case Effect.GRAVITY_LOW:
                gravityStatus.text = "LOW";
                speedStatus.text = "NORMAL";
                gravityStatus.color = Color.red;
                speedStatus.color = Color.green;
                break;
            case Effect.NONE:
                gravityStatus.text = "NORMAL";
                speedStatus.text = "NORMAL";
                gravityStatus.color = Color.green;
                speedStatus.color = Color.green;
                break;
        }
    }
}
