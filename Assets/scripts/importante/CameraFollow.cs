using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;

    private float distance;
    private float yaw;
    private float pitch;
    private float minPitch;
    private float maxPitch;
    private float rotateSpeed;
    private float lookAhead;
    private float smoothSpeed;

    private float collisionPadding;
    private float minDistance;
    private int obstacleMask;

    private void Start()
    {
        distance = 8f;
        yaw = 0f;
        pitch = 20f;
        minPitch = 5f;
        maxPitch = 60f;
        rotateSpeed = 90f;
        lookAhead = 4f;
        smoothSpeed = 8f;

        collisionPadding = 0.4f;
        minDistance = 1f;
        obstacleMask = ~0;

        GameObject player = GameObject.Find("Jugador");
        if (player != null) target = player.transform;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        float dt = Time.unscaledDeltaTime;
        if (Time.timeScale != 0f)
        {
            // WASD: A/D giran horizontal, W/S suben y bajan
            if (Input.GetKey(KeyCode.D)) yaw += rotateSpeed * dt;
            if (Input.GetKey(KeyCode.A)) yaw -= rotateSpeed * dt;
            if (Input.GetKey(KeyCode.W)) pitch += rotateSpeed * dt;
            if (Input.GetKey(KeyCode.S)) pitch -= rotateSpeed * dt;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        Vector3 pivot = target.position;
        Vector3 offset = Quaternion.Euler(pitch, yaw, 0f) * (Vector3.back * distance);
        Vector3 desired = pivot + offset;

        Vector3 direction = desired - pivot;
        float dist = direction.magnitude;
        direction /= dist;

        float closest = dist;
        RaycastHit[] hits = Physics.RaycastAll(pivot, direction, dist, obstacleMask, QueryTriggerInteraction.Ignore);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.IsChildOf(target)) continue;
            if (hit.distance < closest) closest = hit.distance;
        }

        if (closest < dist)
            desired = pivot + direction * Mathf.Max(closest - collisionPadding, minDistance);

        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);

        Vector3 lookDir = Quaternion.Euler(0f, yaw, 0f) * Vector3.forward;
        transform.LookAt(pivot + Vector3.up * 1.5f + lookDir * lookAhead);
    }
}
