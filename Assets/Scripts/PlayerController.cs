using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Movement_variables
    public float baseMoveSpeed = 0.08f;
    public float moveSpeed;
    float x_input;
    float y_input;
    [HideInInspector] public bool canMove = true;
    #endregion

    #region Physics_components
    Rigidbody2D PlayerRB;
    SpriteRenderer PlayerSR;
    BoxCollider2D PlayerColl;
    public Vector2 currDirection;
    #endregion

    #region Animation_components
    Animator anim;
    #endregion

    #region Other_variables

    public GameObject pointerPrefab;
    GameObject interactablePointer;
    #endregion

    #region Unity_functions
    private void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        PlayerSR = GetComponent<SpriteRenderer>();
        PlayerColl = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        moveSpeed = baseMoveSpeed;

    }

    private void Update()
    {
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");

        Move();

    }
    #endregion

    #region Movement_functions
    private void Move()
    {
        if (!canMove) return;

        Vector2 movement = new Vector2(x_input, y_input);
        if (movement.sqrMagnitude > 1)
        {
            movement = movement.normalized;
        }
        movement *= moveSpeed;
        PlayerRB.MovePosition(PlayerRB.position + movement);
        // anim.speed = 1;

        if (movement != Vector2.zero)
        {
            anim.SetTrigger("Moving");
            currDirection = movement.normalized;
        }
        else
        {
            Debug.Log("NotMoving");
            anim.ResetTrigger("Moving");
        }

        // anim.SetFloat("DirX", currDirection.x);
        // anim.SetFloat("DirY", currDirection.y);

        if (currDirection.x < 0)
        {
            PlayerSR.flipX = true;
            anim.SetBool("FacingLeft", true); 
        }
        else if (currDirection.x > 0)
        {
            PlayerSR.flipX = false;
            anim.SetBool("FacingLeft", false); 
        }

        anim.SetFloat("Speed", movement.magnitude);
    }

    public void Fish()
    {
        anim.SetTrigger("Fishing");
    }

    public void Release_Rod()
    {
        anim.SetTrigger("Release_Rod");
    }

    public void End_Fish()
    {
        anim.ResetTrigger("Fishing");
        anim.SetTrigger("End_Fish");
    }
    public void SetMovementMultiplier(float multiplier)
    {
        moveSpeed = baseMoveSpeed * multiplier;
    }
    #endregion

    #region Interact_functions

    #endregion
}
