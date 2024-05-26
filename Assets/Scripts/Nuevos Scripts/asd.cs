using UnityEngine;

public class asd : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public LayerMask floorLayer;
    public int playerNumber;

    private Rigidbody rb;
    private BoxCollider col;
    private bool isStatic = false;
    private bool canJump = false;
    private bool canMoveLeft = true;
    private bool canMoveRight = true;
    internal bool onFloor;

    internal RaycastHit leftHit, rightHit, downHit;
    public float distanceRay = 1.0f, downDistanceRay = 1.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    }

    public virtual void FixedUpdate()
    {
        if (GameManager.actualPlayer == playerNumber)
        {
            if (!isStatic)
            {
                Move();
            }
        }
    }

    private void Update()
    {
        if (GameManager.actualPlayer == playerNumber)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ToggleFreeze();
            }

            if (!isStatic)
            {
                Jump();
                canMoveLeft = !SomethingLeft();
                canMoveRight = !SomethingRight();
                canJump = IsOnSomething();
            }
        }
        else
        {
            FreezePlayer();
        }
    }

    void ToggleFreeze()
    {
        isStatic = !isStatic;

        if (isStatic)
        {
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }
    }

    void FreezePlayer()
    {
        isStatic = true;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, 0);
        rb.velocity = movement;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public virtual bool IsOnSomething()
    {
        return Physics.BoxCast(transform.position, new Vector3(transform.localScale.x * 0.9f, transform.localScale.y / 3, transform.localScale.z * 0.9f), Vector3.down, out downHit, Quaternion.identity, downDistanceRay);
    }

    public virtual bool SomethingRight()
    {
        Ray landingRay = new Ray(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2.2f), transform.position.z), Vector3.right);
        Debug.DrawRay(landingRay.origin, landingRay.direction, Color.green);
        return Physics.Raycast(landingRay, out rightHit, transform.localScale.x / 1.8f);
    }

    public virtual bool SomethingLeft()
    {
        Ray landingRay = new Ray(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2.2f), transform.position.z), Vector3.left);
        Debug.DrawRay(landingRay.origin, landingRay.direction, Color.green);
        return Physics.Raycast(landingRay, out leftHit, transform.localScale.x / 1.8f);
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            Destroy(this.gameObject);
            GameManager.gameOver = true;
        }
        if (collision.gameObject.CompareTag("Floor"))
        {
            onFloor = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onFloor = false;
        }
    }
}
