using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Raycast Shoot Settings")]
    [SerializeField] private Transform gun;
    [SerializeField] private float rayDistance = 20f;
    [SerializeField] private Vector3 offsetGun;

    [Header("Melee Settings")]
    [SerializeField] private Transform sword;
    [SerializeField] private float radius = 1.5f;
    [SerializeField] private Vector3 offsetSword;

    [Header("Attack Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float cooldownAttack = 0.5f;
    private bool canAttack = true;

    [Header("Layers")]
    [SerializeField] private LayerMask targetLayers;

    [SerializeField] private bool isPlayer = true;
    [SerializeField] private bool isMelee = true;

    private Vector3 origin;
    private Vector3 dir;
    private bool isCharacter;
    private RaycastHit hit;

    private PlayerMovement pM;

    private void Awake()
    {
        pM = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (!isPlayer) return;

        if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
        {
            if (isMelee) Melee();
            else Shoot();
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 gizmoOrigin = sword != null
            ? sword.TransformPoint(isMelee ? offsetSword : offsetGun)
            : transform.position;

        Gizmos.color = isCharacter ? Color.green : Color.red;

        if (!isMelee)
        {
            if (hit.collider == null) return;
            Gizmos.DrawLine(gizmoOrigin, hit.point);
        }
        else
        {
            Gizmos.DrawWireSphere(gizmoOrigin, radius);
        }
    }

    public void Shoot()
    {
        if (gun == null)
        {
            return;
        }

        RaycastInfo(offsetGun);

        if (Physics.Raycast(origin, dir, out hit, rayDistance, targetLayers))
        {
            AplyDamage(hit.collider.gameObject);
        }
        else
        {
            isCharacter = false;
        }

        StartCoroutine(AttackCooldown());
    }

    public void Melee()
    {
        if (sword == null)
        {
            return;
        }

        RaycastInfo(offsetSword);

        Collider[] colliders = Physics.OverlapSphere(origin, radius, targetLayers);

        if (colliders.Length == 0)
            colliders = Physics.OverlapSphere(origin, radius);

        bool hitSomething = false;

        foreach (Collider col in colliders)
        {
            if (col.gameObject == gameObject) continue;
            if (isPlayer && col.CompareTag("Player")) continue;
            if (!isPlayer && col.CompareTag("Character")) continue;

            if (AplyDamage(col.gameObject)) hitSomething = true;
        }

        isCharacter = hitSomething;

        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        if (!isPlayer) yield break;

        canAttack = false;

        float currentSpeed = 0f;
        if (pM != null)
        {
            currentSpeed = pM.Speed;
            pM.Speed = 0f;
        }

        yield return new WaitForSeconds(cooldownAttack);

        if (pM != null) pM.Speed = currentSpeed;
        canAttack = true;
    }

    private bool AplyDamage(GameObject hitObj)
    {
        if (hitObj == null) return false;
        if (hitObj == gameObject) return false;

        HealthSystem hs = hitObj.GetComponent<HealthSystem>();

        if (hs == null) hs = hitObj.GetComponentInParent<HealthSystem>();

        if (hs == null)
        {
            return false;
        }

        hs.TakeDamage(damage);
        Debug.Log($"<color=red>[Attack] {damage} de daño a {hs.gameObject.name}</color>");
        return true;
    }

    private void RaycastInfo(Vector3 offset)
    {
        Transform reference = isMelee ? sword : gun;
        origin = reference != null ? reference.TransformPoint(offset) : transform.position;
        dir = transform.forward;
    }
}
