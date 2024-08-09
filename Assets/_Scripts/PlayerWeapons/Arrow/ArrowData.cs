using UnityEngine;

[CreateAssetMenu(fileName = "ArrowData", menuName = "Arrows/New ArrowType", order = 1)]
public class ArrowData : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Sprite _arrowImage;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _forceKnockBack;

    public GameObject prefab => _prefab;
    public Sprite image => _arrowImage;
    public DamageType damageType => _damageType;
    public int damage => _damage;
    public float speed => _speed;
    public float forceKnockBack => _forceKnockBack;
}
