using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;

    [SerializeField] private float speed;

    private void Awake()
    {
        //Get reference for rigidbidy from object
        body = GetComponent<Rigidbody2D>();

        //Get reference for animator from object
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, speed);
        }

        //Set animator parameters
        anim.SetBool("Run", horizontalInput != 0);

    }
}
