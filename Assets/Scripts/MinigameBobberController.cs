using UnityEngine;

public class MinigameBobberController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Transform bobberLocation;
    private Transform borderBounds;

    // private RectTransform catchBounds;
    private float minX;
    private float minY;
    private float maxX;
    private float maxY;


    void Start()
    {
        borderBounds = GetComponentInParent<Transform>();
        bobberLocation = GetComponent<Transform>();
        minX = -35f;
        minY = -35f;
        maxX = 35f;
        maxY = 35f;
    }

    // Update is called once per frame

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 pos = new Vector3(Mathf.Clamp(mousePosition.x, minX, maxX), Mathf.Clamp(mousePosition.y, minY, maxY), 0);

        bobberLocation.position = pos;

    }   
}
