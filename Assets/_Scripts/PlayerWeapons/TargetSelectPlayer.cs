using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetSelectPlayer : MonoBehaviour
{
    [SerializeField] private float _visionAngle = 30f;
    [SerializeField] private float _visionRange = 10f;
    [SerializeField] private float _visionNearRange = 5f;
    [SerializeField] private float _visionHeightAbove = 5f; //How far above can they detect
    [SerializeField] private float _visionHeightBelow = 5f; //How far below can they detect

    private float _touchRange = 1f;
    private Transform _currentEnemy;
    private List<Enemy> _enemy = new List<Enemy>();
    
    public LayerMask vision_mask = ~(0);
    public VisionCone vision;

    public Transform CurrentEnemy { get => _currentEnemy; set => _currentEnemy = value; }

    private void Start()
    {
        vision.vision_angle = _visionAngle;
        vision.vision_range = _visionRange;
        vision.vision_near_range = _visionNearRange;
    }
    private void Update()
    {
        DetectVisionTargetOnly();
    }
    public void FindEnemy()
    {
        Collider[] _enemyDetect = Physics.OverlapSphere(transform.position, _visionRange);

        foreach (Collider enemy in _enemyDetect)
        {
            Debug.Log("Found collider: " + enemy.name);
            Enemy enemyComponent = enemy.GetComponentInParent<Enemy>();

            if (enemyComponent != null && !_enemy.Contains(enemyComponent))
            {

                _enemy.Add(enemyComponent);
            }
        }
    }
    public Vector3 GetEye()
    {
        return transform.position;
    }

    //In case using vision without enemy behavior, detect without changing state
    public void DetectVisionTargetOnly()
    {
        Debug.Log(_enemy.Count);
        Enemy closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (Enemy target in _enemy)
        {

            if (CanSeeVisionTarget(target))
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }
        }

        if (closestTarget != null)
        {
            _currentEnemy = closestTarget.transform;

        }
        else
        {
            _currentEnemy = null;
        }

    }

    //Can the enemy see a vision target?
    public bool CanSeeVisionTarget(Enemy target)
    {
        return target != null && CanSeeObject(target.gameObject, _visionRange, _visionAngle);
    }

    //Can the enemy see an object ?
    public bool CanSeeObject(GameObject obj, float see_range, float see_angle)
    {
        Vector3 forward = transform.forward;
        Vector3 dir = obj.transform.position - GetEye();
        Vector3 dir_touch = dir; //Preserve Y for touch
                                 //   dir.y = 0f; //Remove Y for cone vision range

        float vis_range = see_range;
        float vis_angle = see_angle;
        float losangle = Vector3.Angle(forward, dir);
        float losheight = obj.transform.position.y - GetEye().y;
        bool can_see_cone = losangle < vis_angle / 2f && dir.magnitude < vis_range && losheight < _visionHeightAbove && losheight > -_visionHeightBelow;
        bool can_see_touch = dir_touch.magnitude < _touchRange;
        // Debug.Log("Detected " + can_see_cone + " " + can_see_touch + " " + obj.activeSelf);
        if (obj.activeSelf && (can_see_cone || can_see_touch)) //In range and in angle
        {
            RaycastHit hit;
            bool raycast = Physics.Raycast(new Ray(GetEye(), dir.normalized), out hit, dir.magnitude, vision_mask.value);
            if (!raycast)
                return true; //No obstacles in the way (in case character not in layer)
            if (raycast && (hit.collider.gameObject == obj || hit.collider.transform.IsChildOf(obj.transform))) //See character
                return true; //The only obstacles is the character
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _visionRange);
    }
}
