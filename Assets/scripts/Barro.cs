using UnityEngine;

public class Barro : MonoBehaviour
{
    [SerializeField] private float slowFactor = 0.4f;

    private PlayerMovement player;
    private float originalSpeed;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement pm = other.GetComponentInParent<PlayerMovement>();
        if (pm == null || pm == player) return;

        player = pm;
        originalSpeed = pm.Speed;
        pm.Speed = originalSpeed * slowFactor;
        
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovement pm = other.GetComponentInParent<PlayerMovement>();
        if (pm == null || pm != player) return;

        pm.Speed = originalSpeed;
        player = null;
    }
}
