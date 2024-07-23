using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Interactions;

#region PLAYER CONTROLLER GUI
//using UnityEditor;
//[CustomEditor(typeof(PlayerController))]
//public class PlayerControllerEditor : Editor
//{
//    private PlayerController playerController;

//    private void OnEnable()
//    {
//        playerController = (PlayerController)target;
//    }

//    public override void OnInspectorGUI()
//    {
//        if (playerController == null) return;

//        serializedObject.Update();
//        EditorGUILayout.BeginVertical();


//        EditorGUILayout.Space();
//        EditorGUILayout.LabelField("Player in space", EditorStyles.boldLabel);
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("_playerSpeed"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("_jumpHeight"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("_gravity"));

//        EditorGUILayout.Space();
//        EditorGUILayout.LabelField("Aattacks", EditorStyles.boldLabel);
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxCombo"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("_comboTimeout"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("_comboLock"));

//        //EditorGUILayout.Space();
//        //if (GUILayout.Button("Respawn player"))
//        //    playerController.RespawnPlayer();

//        EditorGUILayout.EndVertical();

//        serializedObject.ApplyModifiedProperties();

//        if (GUI.changed)
//            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
//    }
//}
#endregion

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    CharacterController _controller;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] Animator btnUI;
    public PlayerHealth _playerHealth;

    //Player in space
    [SerializeField, Range(1, 10)] private float _playerSpeed = 5.0f;
    [SerializeField, Range(1, 5)] private float _jumpHeight = 2f;
    [SerializeField, Range(-5, -15)] private float _gravity = -9.81f;
    private Vector2 _playerVelocity;
    private Vector2 _inputDirection;
    private bool _isJumping;
    private bool _isDoubleJumping;
    private int _jumpCount = 0;
    private bool _isIdle;

    //For cheack player on the ground
    [SerializeField] private float _radiusSphere;
    [SerializeField] private Transform _spherePosition;
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;
    
    //Attacks
    private bool _isAttack;
    //Combo
    [SerializeField] private float _timeForNextCombo;
    [SerializeField] private float _comboLock;
    private int _currentCombo;
    private bool _canAttack = true;
    private float _comboCurrentTimer;
    //Shooting
    private bool _isShooting;
    public UnityAction PlayerShoot;

    //Get damage
    private bool _isGetingHit;
    private bool _isDead;


    public Transform PlayerPosition { get => transform; }
    public bool IsAttack { get => _isAttack; set => _isAttack = value; }
    public float PlayerSpeed { get => _playerSpeed; set => _playerSpeed = value; }
    public Vector2 PlayerVelocity { get => _playerVelocity; set => _playerVelocity = value; }
    public Vector2 InputDirection { get => _inputDirection; set => _inputDirection = value; }
    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }
    public bool IsJumping { get => _isJumping; set => _isJumping = value; }
    public bool IsDoubleJumping { get => _isDoubleJumping; set => _isDoubleJumping = value; }
    public int CurrentCombo { get => _currentCombo; set => _currentCombo = value; }
    public bool IsShooting { get => _isShooting; set => _isShooting = value; }
    public bool IsGetingHit { get => _isGetingHit; set => _isGetingHit = value; }
    public bool IsDead { get => _isDead; set => _isDead = value; }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _comboCurrentTimer += Time.deltaTime;

        if (_comboCurrentTimer > _timeForNextCombo)
        {
            _currentCombo = 0;
            _isAttack = false;
        }

        InputUser();
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(_spherePosition.position, _radiusSphere, _groundLayer);

        if (_isGrounded && _playerVelocity.y < 0)
        {
            _jumpCount = 0;
            _playerVelocity.y = -2f;
            
        }

        Land();
    }

    private void InputUser()
    {
        _inputDirection = _gameInput.GetMovementVector();

        if (_inputDirection != Vector2.zero && IsGrounded)
        {
            _isIdle = true;
        }

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

        if (_gameInput.playerInput.Player.Attack1.triggered && _isGrounded)
        {
            _isAttack = true;

            if (_canAttack)
            {
                if (_currentCombo < 4)
                {
                    _canAttack = false;
                    _currentCombo++;
                    _comboCurrentTimer = 0;

                    StartCoroutine(LockComboAttack(_comboLock));
                }
                else
                {
                    _canAttack = false;
                    _currentCombo = 0;

                    StartCoroutine(LockComboAttack(1.5f));
                }
            }
        }

        _gameInput.playerInput.Player.Attack2.performed += context =>
        {
            if (context.interaction is PressInteraction && _isGrounded)
            {
                _isShooting = true;
                btnUI.SetTrigger("Pressed");

                PlayerShoot?.Invoke();
                
                StartCoroutine(LockShooting( 0.5f));
            }
        };

        if (_gameInput.playerInput.Player.Respawn.triggered && IsDead)
        {
            RespawnPlayer();
        }
    }

    private IEnumerator LockComboAttack(float comboLock)
    {
        yield return new WaitForSeconds(comboLock);
        _canAttack = true;
    }
    private IEnumerator LockShooting(float lockShooting)
    {
        yield return new WaitForSeconds(lockShooting);
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
        _isAttack = false;
    }

    public void RespawnPlayer()
    {
        _isDead = false;
        _playerHealth.health = 100;
        Debug.Log("SAYNTRES");
    }
}
