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

    public void Cast(Transform enemyPosition)
    {
        if (_selectEquipment.selectedArrow != null)
        {
            Arrow arrow = ObjectPool.Instance.GetObject(_selectEquipment.selectedArrow.prefab.GetComponent<Arrow>());
            arrow.SetDamage(_normalArrow.damage, _selectEquipment.selectedArrow.damage, _normalArrow.damageType, _selectEquipment.selectedArrow.damageType);
            arrow.transform.position = _castPoint.position;
            if (enemyPosition != null)
            {
                Vector3 arrowDirection = (new Vector3(enemyPosition.position.x, enemyPosition.position.y + 0.4f, enemyPosition.position.z) - _castPoint.position).normalized;
                
                arrow.transform.rotation = Quaternion.LookRotation(arrowDirection);
                arrow.FlyInDirection(arrowDirection, _selectEquipment.selectedArrow.speed, _selectEquipment.selectedArrow.forceKnockBack);
            }
            else
            {
                arrow.transform.rotation = _castPoint.rotation;
                arrow.FlyInDirection(_castPoint.forward, _selectEquipment.selectedArrow.speed, _selectEquipment.selectedArrow.forceKnockBack);
            }

        }
    }
}
