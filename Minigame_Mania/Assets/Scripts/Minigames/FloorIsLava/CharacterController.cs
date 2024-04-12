using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

	public Animator animator;

    private Rigidbody2D rb;
    private Vector3 originalScale;

	private Collider2D groundCheckCollider;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
		groundCheckCollider = GetComponent<Collider2D>();
		
		// animator = getComponent<Animator>();
    }

    void Update()
    {
		
        // Horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
		float verticalSpeed = rb.velocity.y;

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Flip character sprite if moving left or right
        if (moveInput < 0)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

        }
        else if (moveInput > 0)
        {
            transform.localScale = originalScale;
        }

		// if(verticalSpeed == 0){
		// 	animator.SetBool("IsIdle", true);
		// }else{
		// 	animator.SetBool("IsIdle", false);
		// }

		bool isGrounded = IsGrounded();

		// bool isJumpingUp = verticalSpeed > 0;
   		// bool isJumpingDown = verticalSpeed < 0;

		animator.SetFloat("Speed", Mathf.Abs(moveInput));
  		// animator.SetBool("IsJumpingUp", isJumpingUp);
   		// animator.SetBool("IsJumpingDown", isJumpingDown);
		animator.SetFloat("Jump", verticalSpeed);
		animator.SetFloat("JumpDown", verticalSpeed);
		animator.SetBool("IsGrounded", isGrounded);

		// if (moveInput == 0 && verticalSpeed == 0){
		// 	animator.SetBool("IsIdle", true);
		// }

		// if (verticalSpeed > 0){
		// 	animator.SetBool("IsJumpingUp", true);
		// }
		// else{
		// 	animator.SetBool("IsJumpingUp", false);
		// }

        // Jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
		transform.rotation = Quaternion.Euler(Vector3.zero);
    }

	bool IsGrounded()
    {
    //     // Perform a physics overlap check to see if the ground check collider is overlapping with any colliders
    //     Vector2 bottomCenter = groundCheckCollider.bounds.center;
    //     Vector2 size = new Vector2(groundCheckCollider.bounds.size.x, 0.1f); // Adjust the size as needed
    //     // Collider2D[] colliders = Physics2D.OverlapBoxAll(bottomCenter, size, 0f, groundLayer);
	// 	Collider2D[] colliders = Physics2D.OverlapBoxAll(bottomCenter, size, 0f, LayerMask.GetMask("Everything"));


    //     // Check if any colliders were found (excluding the character's own collider)
    //     foreach (Collider2D collider in colliders)
    //     {
    //         if (collider != groundCheckCollider)
    //         {
	// 			Debug.Log("Character is grounded");
    //             return true; // Character is considered grounded
    //         }
    //     }
	// 	Debug.Log("Character is NOT grounded");
    //     return false; // Character is not grounded
    // }

	    // Check if the character's collider is touching any other collider.
    if (groundCheckCollider.IsTouchingLayers())
    {
        Debug.Log("Character is touching another collider.");
        return true; // The collider is touching another collider
    }
    
    Debug.Log("Character is not touching any colliders.");
    return false; // The collider is not touching any other colliders
	}


private void OnCollisionEnter2D(Collision2D collision)
{
    // Set the character's parent to the collided object universally.
    this.transform.SetParent(collision.transform, true);
}

private void OnCollisionExit2D(Collision2D collision)
{
    // Unset the parent when the character is no longer in contact with the collider.
    if (this.transform.parent == collision.transform)
    {
        this.transform.SetParent(null);
    }
}




}
