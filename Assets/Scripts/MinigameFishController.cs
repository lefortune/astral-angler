using UnityEngine;
using System.Collections;
using System;

public class MinigameFishController : MonoBehaviour
{

    private Transform fishLocation;
    private Transform borderBounds;

    [SerializeField]
    private Transform bobberLocation;

    [SerializeField]
    private Transform goalLocation;


    private float minX;
    private float minY;
    private float maxX;
    private float maxY;

    static float t = 0f;
    static float reelMultiplier = 1f;


    private float fishX;
    private float fishY;

    [SerializeField]
    private float maxMultiplier = 0.04f;
    private float minMultiplier = 0.005f;
    [SerializeField]
    private float maxStrain = 300f;
    private float strain = 0f;

    private bool fishRunning;
    private float timer;
    [SerializeField]
    private float averageRunTime;
    [SerializeField]
    private float averagePullTime;
    private bool win;

    private bool fastRun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fishRunning = false;
        fastRun = false;

        timer = UnityEngine.Random.Range(-2f, 2f) + averageRunTime;
        
        fishLocation = GetComponent<Transform>();

        borderBounds = GetComponentInParent<Transform>();
        // Debug.Log(borderBounds.position);
        // bobberLocation = GetComponent<Transform>();
        minX = -35f;
        minY = -35f;
        maxX = 35f;
        maxY = 35f;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (!fishRunning)
        {
            FishCome();
        }
        else
        {
            FishRun();
        }
        if (t > timer && !fishRunning)
        {
            averageRunTime -= UnityEngine.Random.Range(0.01f, 0.1f);
            float randomNum = UnityEngine.Random.Range(-0.5f, 0.5f);
            if (randomNum < 0) {
                timer = randomNum + averageRunTime;
                timer = timer / 1.5f;
                fastRun = true;
            }
            else {
                timer = randomNum + averageRunTime;
                fastRun = false;
            }
            t = 0;
            fishRunning = !fishRunning;
        }
        else if (t > timer && fishRunning)
        {
            averagePullTime += UnityEngine.Random.Range(0.01f, 0.1f);
            timer = UnityEngine.Random.Range(-.5f, .5f) + averagePullTime;
            if (timer < 0)
            {
                timer = 0;
            }
            t = 0;
            fishRunning = !fishRunning;
        }
    }

    private void FishCome()
    {
        FishMove(CalculateDirection(bobberLocation, fishLocation));
    }
    private void FishRun()
    {
        if (fastRun)
        {
            FishMove(-2f * CalculateDirection(goalLocation, fishLocation));
        }
        else
        {
            FishMove(-.4f * CalculateDirection(goalLocation, fishLocation));
        }
    }
    private void FishMove(Vector3 direction)
    {
        UpdateStrainAndMultiplier();
        reelMultiplier = maxMultiplier;

        fishX = fishLocation.position.x;
        fishY = fishLocation.position.y;

        Vector3 pos = new Vector3(fishX, fishY, 0) + direction * reelMultiplier;
        fishLocation.position = new Vector3(Mathf.Clamp(pos.x, minX + 5f, maxX - 5f), Mathf.Clamp(pos.y, minY + 5f, maxY - 5f), 0);
    }

    private Vector3 CalculateDirection(Transform obj1, Transform obj2)
    {
        Vector3 direction = obj1.position - obj2.position;
        return direction.normalized;
    }
    
    private void UpdateStrainAndMultiplier()
    {
        Vector3 difference = bobberLocation.position - fishLocation.position;
        float magnitude = difference.magnitude;
        float widthRatio = magnitude / (maxX - minX);
        strain += 50f * (float)Time.deltaTime * widthRatio;
        reelMultiplier = maxMultiplier * widthRatio + minMultiplier;
    }

}
