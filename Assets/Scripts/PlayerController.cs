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

        CheckInteraction();

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
            anim.ResetTrigger("Moving");
        }

        // anim.SetFloat("DirX", currDirection.x);
        // anim.SetFloat("DirY", currDirection.y);

        if (currDirection.x < 0)
        {
            PlayerSR.flipX = false;
            anim.SetBool("FacingLeft", true);
        }
        else if (currDirection.x > 0)
        {
            PlayerSR.flipX = true;
            anim.SetBool("FacingLeft", false);
        }

        anim.SetFloat("Speed", movement.magnitude);
    }

    public void SetMovementMultiplier(float multiplier)
    {
        moveSpeed = baseMoveSpeed * multiplier;
    }

    public void SetPlayerPosition(Vector2 position)
    {
        PlayerRB.position = position;
        PlayerRB.linearVelocity = Vector2.zero; // Reset velocity to prevent sliding
    }
    #endregion

    #region Animation_functions
    public void Fish()
    {
        anim.SetTrigger("isFishing");
    }

    public void Release_Rod()
    {
        anim.SetTrigger("Release_Rod");
    }

    public void End_Fish()
    {
        anim.ResetTrigger("isFishing");
        anim.SetTrigger("End_Fish");
    }
    #endregion

    #region Interact_functions

    void CheckInteraction()
    {
        Vector2 colliderCenter = PlayerColl.bounds.center;
        Vector2 colliderSize = PlayerColl.bounds.size;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(
            colliderCenter + currDirection / 2f,
            new Vector2(colliderSize.x, colliderSize.y),
            0f,
            Vector2.zero,
            0f
        );

        float closestDistance = float.MaxValue;
        RaycastHit2D closestHit = new RaycastHit2D();

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Interactable"))
            {
                float distance = Vector2.Distance(colliderCenter + currDirection / 2, hit.point);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestHit = hit;
                }
            }
        }

        if (closestHit.collider != null)
        {
            if (interactablePointer == null)
            {
                Vector3 hitPosition = closestHit.collider.bounds.center;
                hitPosition.y += 1.5f;

                interactablePointer = Instantiate(pointerPrefab, hitPosition, Quaternion.identity);
            }
            else
            {
                Vector3 updatedPosition = closestHit.collider.bounds.center;
                updatedPosition.y += 1.5f;
                interactablePointer.transform.position = updatedPosition;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Starting object interaction");
                Destroy(interactablePointer);
                interactablePointer = null;
                closestHit.transform.GetComponent<InteractableManager>().Interact();
            }
        }
        else
        {
            if (interactablePointer != null)
            {
                Destroy(interactablePointer);
                interactablePointer = null;
            }
        }
    }
    


    #endregion
}
