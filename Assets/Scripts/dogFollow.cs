using UnityEngine;
using UnityEngine.Tilemaps; 

public class dogFollow : MonoBehaviour
{
    public Transform playerTransform; 
    public Tilemap path; 
    public float followDistance = 0.5f; 
    public float moveSpeed = 2f; 

    // Update is called once per frame
    void Update()
    {
        followPlayer();
        
    }

    void followPlayer()
    {
        if (playerTransform == null || path == null)
        {
        Debug.LogWarning("Missing references! Make sure playerTransform and path are assigned.");
        return;
        }

        Vector3 direction = playerTransform.position - transform.position; 

        if (direction.magnitude > followDistance)
        {
            Vector3 nextPos = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime); 
            Vector3Int tilePos = path.WorldToCell(nextPos);

            Debug.Log($"Dog trying to move to: {nextPos}, tile: {tilePos}, has tile: {path.HasTile(tilePos)}");
 

            if (path.HasTile(tilePos))
            {
                transform.position = nextPos; 
            } else {
                Debug.Log("Next tile is not walkable, dog not moving.");

            }
        }
    }
}
