using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class BobberBehavior : MonoBehaviour
{
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;
    public float biteDuration = 2f;

    [HideInInspector] public bool hasLanded = false;
    public bool isBiteActive = false;

    public Transform playerTransform;
    public PlayerController playerController;
    private LineRenderer lineRenderer;
    private Vector3 sourcePosition;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        hasLanded = false;
    }

    void Update()
    {

        if (lineRenderer != null && playerTransform != null)
        {
            sourcePosition = playerTransform.position + new Vector3(playerController.currDirection.x, 0f, 0f) * 0.9f; // Adjust bobber's line renderer to rod's position
            lineRenderer.SetPosition(0, transform.position + new Vector3(0f, 0.2f, 0f));       // Bobber end
            lineRenderer.SetPosition(1, sourcePosition + new Vector3(0f, 0.4f, 0f));    // Player end
        }
        if (isBiteActive && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Bite confirmed - entering fishing minigame");
            FishingController.isFishingGame = true;
            isBiteActive = false;
            StopAllCoroutines();
            StartFishingGame();
        }
    }

    public void ItsBobbinTime()
    {
        hasLanded = true;
        AudioManager.Instance.Play("Splish");
        StartCoroutine(BiteLoop());
    }

    IEnumerator BiteLoop()
    {
        while (!FishingController.isFishingGame)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            TriggerBite();

            yield return new WaitForSeconds(biteDuration);

            if (!FishingController.isFishingGame)
            {
                Debug.Log("Bite ignored - resetting loop");
                isBiteActive = false;
            }
        }
    }

    void TriggerBite()
    {
        isBiteActive = true;
        AudioManager.Instance.Play("FishBite");
        Debug.Log("Bite detected");
        // TODO: Trigger splash animation / sound / vibration
    }

    void StartFishingGame()
    {
        AudioManager.Instance.Stop("FishBite");
        AudioManager.Instance.PlayVaried("Confirm");
        // implement minigame logic here
        Debug.Log("Starting fishing minigame");
    }
}
