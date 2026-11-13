using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isStop = false;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public bool IsStop
    {
        get { return isStop; }
        set { isStop = value; }
    }

    private float moveHorizontal;
    private float moveVertical;
    private Rigidbody rb;
    private Animator anim;
    private Transform cameraTransform;

    [Header("Raycast Settings")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask groundLayer;

    [Header("Respawn Settings")]
    [SerializeField] private float fallLimit = -10f;   
    private Vector3 respawnPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        if (Camera.main != null)
            cameraTransform = Camera.main.transform;

        respawnPosition = transform.position;
    }

    void Update()
    {
        if (isStop) return;

        moveHorizontal = 0f;
        moveVertical = 0f;
        if (Input.GetKey(KeyCode.UpArrow)) moveVertical = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) moveVertical = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) moveHorizontal = 1f;
        if (Input.GetKey(KeyCode.LeftArrow)) moveHorizontal = -1f;

        Vector2 rel = CameraRelative(new Vector2(moveHorizontal, moveVertical));
        moveHorizontal = rel.x;
        moveVertical = rel.y;

        anim.SetBool("isWalking", moveHorizontal != 0 || moveVertical != 0);

        CheckGround();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0));
        }
    }

    private void FixedUpdate()
    {
        if (isStop)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.z * speed);

        if (transform.position.y < fallLimit)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = respawnPosition;
    }

    private Vector2 CameraRelative(Vector2 input)
    {
        if (cameraTransform == null) return input;

        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = cameraTransform.right;
        right.y = 0f;
        right.Normalize();

        Vector3 direction = right * input.x + forward * input.y;
        return new Vector2(direction.x, direction.z);
    }

    private void CheckGround()
    {
        Collider col = GetComponent<Collider>();
        Vector3 origin = new Vector3(transform.position.x, col.bounds.min.y + 0.1f, transform.position.z);

        isGrounded = Physics.Raycast(origin, Vector3.down, rayDistance, groundLayer);

        Color ColorRay = isGrounded ? Color.green : Color.red;
        Debug.DrawRay(origin, Vector3.down * rayDistance, ColorRay);
    }

    public void RotateCharacter(float rotateY)
    {
        transform.rotation = Quaternion.Euler(0f, rotateY, 0f);
    }

    public void ActivarTurbo()
    {
        StartCoroutine(TurboTemporal());
    }

    private IEnumerator TurboTemporal()
    {
        float velocidadOriginal = speed;
        speed = velocidadOriginal * 2f;
        yield return new WaitForSeconds(5f);
        speed = velocidadOriginal;
    }
}
