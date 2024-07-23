using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class SelectEquipment : MonoBehaviour
{
    [SerializeField] private GameInput _gameInput;
    [HideInInspector] public  ArrowData selectedArrow;
    
    // UI
    [SerializeField] Animator btnUI;
    [SerializeField] Image _selectedArrowSprite;

    private void Start()
    {
        selectedArrow = GameManager.Instance.arrows.Find(a => a.damageType == DamageType.Physical);
        SetupInputs();
    }

    private void SetupInputs()
    {
        _gameInput.playerInput.Player.Attack2.performed += HoldInteraction;
        _gameInput.playerInput.Player.Item1.performed += ctx => ArrowSelection(DamageType.Physical, ctx);
        _gameInput.playerInput.Player.Item2.performed += ctx => ArrowSelection(DamageType.Ice, ctx);
        _gameInput.playerInput.Player.Item3.performed += ctx => ArrowSelection(DamageType.Fire, ctx);
    }

    private void HoldInteraction(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            btnUI.SetTrigger("Selected");
        }
    }

    private void ArrowSelection(DamageType damageType, InputAction.CallbackContext context)
    {
        if (context.interaction is PressInteraction)
        {
            selectedArrow = GameManager.Instance.arrows.Find(a => a.damageType == damageType);
            _selectedArrowSprite.sprite = selectedArrow.image;
            btnUI.SetTrigger("Normal");
        }
    }
}
