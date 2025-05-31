using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        private bool isMove;
        private float moveInput;
        private float ButtonMoveInput;

        private bool facingRight = false;
        [HideInInspector]
        public bool deathState = false;

        private bool isGrounded;
        public Transform groundCheck;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;
        private bool CancheckGr = true;

        [SerializeField] private PortalLogic _portalLogic;
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject[] _enemys;

        [SerializeField] private UnityEvent _eventJump;
        [SerializeField] private UnityEvent _eventCollectCrystall;
        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        private void OnEnable()
        {
            foreach (GameObject enemy in _enemys)
            {
                enemy.SetActive(true);
            }
            if (_pauseMenu != null)
                _pauseMenu.SetActive(false);
        }
        public void Pause()
        {
            gameObject.SetActive(false);
            _pauseMenu.SetActive(true);
            foreach (GameObject enemy in _enemys)
            {
                enemy.SetActive(false);
            }
        }
        private void FixedUpdate()
        {
            CheckGround();
        }
        public void Moved(int i)
        {
            if(i != 0)
            {
                isMove = true;
                ButtonMoveInput = i;
            }
            else if(i == 0)
            {
                isMove = false;
                ButtonMoveInput = 0;
            }

        }
        public void Jump()
        {
            if (isGrounded && CancheckGr)
            {
                CancheckGr = false;
                _eventJump?.Invoke();
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                Invoke(nameof(CanCheckGround), 0.4f);
            }
        }
        private void CanCheckGround()
        {
            CancheckGr = true;
        }
        void Update()
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                isMove = true;
            }
            else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                isMove= false;
            }
            if (isMove) 
            {
                moveInput = Input.GetAxis("Horizontal");
                if (moveInput == 0)
                    moveInput = ButtonMoveInput;
                Vector3 direction = transform.right * moveInput;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
                animator.SetInteger("playerState", 1); // Turn on run animation
            }
            else
            {
                if (isGrounded) animator.SetInteger("playerState", 0); // Turn on idle animation
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded) Jump();
            if (!isGrounded)animator.SetInteger("playerState", 2); // Turn on jump animation
            
            if(facingRight == false && moveInput > 0)
            {
                Flip();
            }
            else if(facingRight == true && moveInput < 0)
            {
                Flip();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                deathState = true; // Say to GameManager that player is dead
                _portalLogic._wall.GetComponent<MovedWallLogic>().enabled = false;
            }
            else
            {
                deathState = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Coin")
            {
                _eventCollectCrystall?.Invoke();
                gameManager.coinsCounter += 1;
                _portalLogic.CoinChecker();
                Destroy(other.gameObject);
            }
        }
    }
}
