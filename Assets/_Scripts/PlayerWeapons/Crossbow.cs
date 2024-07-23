using UnityEngine;
public class Crossbow : MonoBehaviour
{
    [SerializeField] Transform _castPoint;
    [SerializeField] SelectEquipment _selectEquipment;

    ArrowData _normalArrow;

    private void Start()
    {
        _normalArrow = GameManager.Instance.arrows.Find(a => a.damageType == DamageType.Physical);
    }

    public void Cast()
    {
        if (_selectEquipment.selectedArrow != null)
        {
            Arrow arrow = ObjectPool.Instance.GetObject(_selectEquipment.selectedArrow.prefab.GetComponent<Arrow>());
            arrow.SetDamage(_normalArrow.damage, _selectEquipment.selectedArrow.damage, _normalArrow.damageType, _selectEquipment.selectedArrow.damageType);
            arrow.transform.position = _castPoint.position;
            arrow.transform.rotation = _castPoint.rotation;

            arrow.FlyInDirection(_castPoint.forward, _selectEquipment.selectedArrow.speed);
        }
    }
}
