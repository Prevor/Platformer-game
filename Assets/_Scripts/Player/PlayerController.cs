using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private float _playerSpeed = 3.0f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private int _maxCombo = 4;
    [SerializeField] private float _comboTimeout;
    [SerializeField] private float _comboLock;
    [SerializeField] private float _radiusSphere;
    [SerializeField] private Transform _spherePosition;

    CharacterController _controller;

    [SerializeField] Animator btnUI;

    public UnityAction PlayerShoot;

    private Vector2 _playerVelocity;
    private Vector2 _inputDirection;
    private bool _isGrounded;
    private bool _isJumping;
    private bool _isDoubleJumping;
    private bool _isAttack;


    private int _jumpCount = 0;

    private int _currentCombo = 0;
    private float _comboCurrentTimer;
    private bool _comboEnd;
    private bool _attack;
    private bool _firstAttack;
    private bool _isShooting;
    private object _shootCount;

    public bool IsAttack { get => _isAttack; set => _isAttack = value; }
    public float PlayerSpeed { get => _playerSpeed; set => _playerSpeed = value; }
    public Vector2 PlayerVelocity { get => _playerVelocity; set => _playerVelocity = value; }
    public Vector2 InputDirection { get => _inputDirection; set => _inputDirection = value; }
    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }
    public bool IsJumping { get => _isJumping; set => _isJumping = value; }
    public bool IsDoubleJumping { get => _isDoubleJumping; set => _isDoubleJumping = value; }
    public int CurrentCombo { get => _currentCombo; set => _currentCombo = value; }
    public bool ComboEnd { get => _comboEnd; set => _comboEnd = value; }
    public bool IsShooting { get => _isShooting; set => _isShooting = value; }


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _comboCurrentTimer += Time.deltaTime;

        if (_comboCurrentTimer > _comboTimeout)
        {
            _currentCombo = 0;
            _isAttack = false;
        }

        InputUser();
    }


    private void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(_spherePosition.position, _radiusSphere);

        if (_isGrounded && _playerVelocity.y < 0)
        {
            _jumpCount = 0;
            _playerVelocity.y = -2f;

        }
    }


    private void InputUser()
    {
        _inputDirection = _gameInput.GetMovementVector();

        if (_gameInput.playerInput.Player.Jump.triggered)
        {
            if (_isGrounded)
            {
                _isJumping = true;
                _jumpCount++;

            }
            else if (_jumpCount == 1)
            {
                _isDoubleJumping = true;
                _jumpCount++;

            }
            else
            {
                _isJumping = false;
                _isDoubleJumping = false;
            }
        }


        if (_gameInput.playerInput.Player.Attack1.triggered && _isGrounded && !_comboEnd)
        {
            if (_currentCombo < _maxCombo)
            {
                _isAttack = true;
                if (!_attack)
                {
                    _currentCombo++;


                    if (_firstAttack)
                    {
                        _firstAttack = false;
                    }
                    else
                    {
                        StartCoroutine(LockCombo(_comboLock));
                    }
                }
                _comboCurrentTimer = 0;
            }
            if (_currentCombo >= _maxCombo)
            {
                _isAttack = false;
                StartCoroutine(LockCombo(2.5f));


            }

        }

        _gameInput.playerInput.Player.Attack2.performed += context =>
        {
            if (context.interaction is PressInteraction && _isGrounded)
            {
                _isShooting = true;
                if (!_attack)
                {
                    btnUI.SetTrigger("Pressed");

                    PlayerShoot?.Invoke();
                    StartCoroutine(LockCombo(0.5f));
                }
            }
        };

    }

    private IEnumerator LockCombo(float comboLock)
    {
        _attack = true;
        if (comboLock >= 1f)
        {
            _comboEnd = true;
        }
        yield return new WaitForSeconds(comboLock);
        _attack = false;
        _comboEnd = false;
    }

    public void Land()
    {
        _playerVelocity.y += _gravity * Time.fixedDeltaTime;
        _controller.Move(_playerVelocity * Time.fixedDeltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_spherePosition.position, _radiusSphere);
    }

    public void AttackFinish()
    {
        _isAttack = false;
    }

    public void Jump()
    {
        _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
    }

    public void ResetInput()
    {
        _isShooting = false;
        _isJumping = false;
        _isDoubleJumping = false;
        _jumpCount = 0;
        _shootCount = 0;
        IsAttack = false;
    }
}
