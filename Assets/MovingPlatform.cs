using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistanceX = 3f;
    public float moveDistanceY = 2f;
    public float moveSpeed = 2f;

    [Header("Начальное направление")]
    public bool startMovingRight = true;
    public bool startMovingUp = true;

    private Vector3 startPos;
    private float timeCounter;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        timeCounter += Time.deltaTime * moveSpeed;

        float offsetX = moveDistanceX != 0 ? Mathf.PingPong(timeCounter, moveDistanceX) : 0f;
        float offsetY = moveDistanceY != 0 ? Mathf.PingPong(timeCounter, moveDistanceY) : 0f;

        offsetX = startMovingRight ? offsetX : -offsetX;
        offsetY = startMovingUp ? offsetY : -offsetY;

        transform.position = startPos + new Vector3(offsetX, offsetY, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }
}