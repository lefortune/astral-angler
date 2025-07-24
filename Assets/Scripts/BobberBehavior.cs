using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class BobberBehavior : MonoBehaviour
{
    public float minWaitTime = 2f;
    public float maxWaitTime = 5f;
    public float biteDuration = 2f;

    [HideInInspector] public bool hasLanded = false;
    private bool isBiteActive = false;


    void Start()
    {
        hasLanded = false;
    }

    void Update()
    {
        if (isBiteActive && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Bite confirmed - entering fishing minigame");
            FishingController.isFishingGame = true;
            isBiteActive = false;
            StopAllCoroutines(); // stop loop
            StartFishingGame();  // placeholder
        }
    }

    public void ItsBobbinTime()
    {
        hasLanded = true;
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
        Debug.Log("Bite detected");
        // TODO: Trigger splash animation / sound / vibration
    }

    void StartFishingGame()
    {
        // implement minigame logic here
        Debug.Log("Starting fishing minigame");
    }
}
