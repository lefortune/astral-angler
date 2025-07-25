using UnityEngine;
using UnityEngine.Tilemaps; 

public class dogFollow : MonoBehaviour
{
    #region variables
    public Transform playerTransform; 
    public Tilemap path; 
    public float followDistance = 0.5f; 
    public float moveSpeed = 2f; 
    #endregion

    #region animation
    // private Animator anim;
    private SpriteRenderer sr;
    #endregion

    // damn i guess i do need a start fml
    void Start()
    {
        // anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
        followPlayer();
    }

    void followPlayer()
    {
        Vector3 direction = playerTransform.position - transform.position; 

        if (direction.magnitude > followDistance)
        {
            Vector3 nextPos = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime); 
            Vector3Int tilePos = path.WorldToCell(nextPos); 

            if (path.HasTile(tilePos))
            {
                transform.position = nextPos; 

                if (direction.x < -0.05f)
                {
                    sr.flipX = true; // Facing left
                    // anim.SetBool("Walking", true);
                } 
                else if (direction.x > 0.05f)
                {
                    sr.flipX = false; // Facing right
                    // anim.SetBool("Walking", true);
                }
            } else {

            }
        }
    }
}
