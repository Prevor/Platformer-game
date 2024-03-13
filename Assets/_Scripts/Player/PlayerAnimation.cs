using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    private Animator animator;

    private const string IS_RUNING = "IsRunning";
    private const string IS_JUMPING = "IsJumping";
    private const string IS_IDLE = "IsIdle";
    private const string IS_LAND = "IsLand";
    private const string IS_DOUBLE_JUMPING = "IsDoubleJumping";
    private const string IS_ATTACK = "IsAttack";


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
       
    }
    private void OnDisable()
    {
        
        
    }
    private void Update()
    {
      /* player.IsAttack = IsAttack;
       animator.SetBool(IS_RUNING, player.IsRunning());
       animator.SetBool(IS_JUMPING, player.IsJumping());
       animator.SetBool(IS_IDLE, player.IsIdle());
       animator.SetBool(IS_LAND, player.IsLand());
       animator.SetBool(IS_DOUBLE_JUMPING, player.IsDoubleJumping());*/
    }

    private void AttackAnimation(int comboHit)
    {
      //  animator.SetTrigger(IS_ATTACK + comboHit);
    }
}
