#region 'Using' inofrmation
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    float horizontalMove = 0f;
    public float runSpeed = 14f;

    bool jump = false;
    bool fall = false;
    bool punch = false;

    public Animator animator; // Lets me mess with animations through code.

    // Update is called once per frame
    void Update()
    {
        Input.GetAxisRaw("Horizontal");
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove)); // Makes sure the run animation plays whichever way you go (left or right).

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("Jumping", true);
        }

        if (Input.GetButtonDown("Punch"))
        { 
            punch = true;
            StartCoroutine(PunchingAnimation());
        }
        
    }

    IEnumerator PunchingAnimation()
    {
        animator.SetBool("Punching", true);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("Punching", false);
        punch = false;
    }

    public void OnLanding()
    {
        animator.SetBool("Jumping", false);
        animator.SetBool("Falling", false);
    }

    public void OnFalling()
    {
        animator.SetBool("Falling", true);
        fall = true;
    }

    private void FixedUpdate()
    {
        // Move character.
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump); // When the player moves horizontally, they simply move in that direction without jumping.
        jump = false; // Stops the player from jumping forever and ever and ever and ever.
    }
}