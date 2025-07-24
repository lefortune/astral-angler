using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System;

public class FishingController : MonoBehaviour
{
    #region Casting Settings
    public float maxCharge = 10.4f;    // Max charge for casting the fishing line. Split in half for increasing and decreasing
    public float baseCastDistance = 1f; // Base distance for casting the fishing line when charge=1
    public float chargeSpeed = 2f;    // How fast the charge meter fills
    #endregion

    #region References
    public Tilemap fishingTilemap;
    public ChargeBarUI chargeBarUI; 
    public GameObject bobberPrefab;
    private GameObject currentBobber; // Only supports one bobber at a time as of now.
    private BobberBehavior bobberBehavior ; // active bobber's script reference
    public Transform castOrigin; // on player 
    public Transform playerTransform;
    #endregion

    #region Player
    public PlayerController playerController;
    private float chargeMeter = 0f;
    public float slowMoveMultiplier = 0.3f;
    [HideInInspector] public static bool isCharging = false;
    [HideInInspector] public static bool isFishing = false;
    [HideInInspector] public static bool isFishingGame = false;
    #endregion

    void Start()
    {
        playerController = playerTransform.GetComponent<PlayerController>();
        castOrigin = playerTransform;
        isCharging = false;
        isFishing = false;
        isFishingGame = false;
    }

    void Update()
    {
        HandleInput();

        if (isCharging)
            chargeBarUI.Show();
        else
            chargeBarUI.Hide();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && !isFishing && !isFishingGame)
        {
            isCharging = true;
            playerController.SetMovementMultiplier(slowMoveMultiplier); // slow down when charging or fihsing
            AudioManager.Instance.Play("Creak");
        }

        if (Input.GetMouseButton(0) && isCharging)
        {
            // Charge goes from 0 to 10.4 (default maxCharge)
            // 0-5.2 increasing, 5.2-10.4 decreasing
            // 5 is max charge, and 0.4 buffer grace period

            chargeMeter += Time.deltaTime * chargeSpeed;

            if (chargeMeter > maxCharge)
            {
                isCharging = false;
                chargeMeter = 0f;
                EndFishing();
            }

            chargeBarUI.UpdatePointer(chargeMeter / maxCharge);
            Debug.Log($"Charge: {chargeMeter:F2}. Facing: {playerController.currDirection}");
        }

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            isCharging = false;

            float halfCharge = maxCharge / 2f;  // for convenience 
            float finalCharge = chargeMeter > halfCharge ? maxCharge - chargeMeter : chargeMeter; // if chargeMeter is over half, use the decreasing charge
            finalCharge = Math.Min(finalCharge, 5f);    // Manually capped at 5, change if needed

            if (chargeMeter >= halfCharge - 0.2f && chargeMeter <= halfCharge + 0.2f)   // Manually 0.4 grace period, change if needed
            {
                Debug.Log($"MAX CAST!)");
            }

            StartCoroutine(CastBobber(finalCharge));
            chargeMeter = 0f;
        }

        if (Input.GetMouseButtonDown(0) && isFishing && !bobberBehavior.isBiteActive)   // retrieve an uneventful bobber
        {
            StartCoroutine(RecallBobber(false));
        }
    }

    private IEnumerator CastBobber(float charge)
    {
        float distance = charge * baseCastDistance + 0.1f;

        Vector3 direction = playerController.currDirection; // adjust if using mouse aim or facing. currently uses facing
        Vector3 worldTarget = castOrigin.position + direction * distance;
        worldTarget += new Vector3(0f, -0.4f, 0f);  // Adjust cast origin to be at the player's feet

        // Convert to tile position
        Vector3Int tilePos = fishingTilemap.WorldToCell(worldTarget);
        TileBase tile = fishingTilemap.GetTile(tilePos);
        FishingTile fishingTile = tile as FishingTile;

        AudioManager.Instance.Stop("Creak");
        AudioManager.Instance.Play("CastLine");

        isFishing = true;   // isFishing is true from bobber cast, not just when it lands

        currentBobber = Instantiate(bobberPrefab, castOrigin.position, Quaternion.identity);    // Instantiate bobber at cast origin
        bobberBehavior = currentBobber.GetComponent<BobberBehavior>();
        yield return StartCoroutine(AnimateBobberThrow(currentBobber.transform, worldTarget, 0.5f));    // 0.5f is duration

        if (fishingTile != null)
        {
            bobberBehavior.hasLanded = true;
            bobberBehavior.ItsBobbinTime(); // Start bobber's logic
        }
        else
        {
            Debug.Log("Invalid cast - no fishing tile");
            EndFishing();
        }
        yield return null;
    }

    private IEnumerator RecallBobber(bool caught)
    {
        if (caught)
        {
            Debug.Log("Bobber caught a fish!");
            AudioManager.Instance.Play("CatchFish");
        }
        else
        {
            Debug.Log("Bobber recalled without catch");
            AudioManager.Instance.Play("Line");
        }
        yield return StartCoroutine(AnimateBobberThrow(currentBobber.transform, castOrigin.position, 0.5f));

        EndFishing();
    }

    private IEnumerator AnimateBobberThrow(Transform bobber, Vector3 target, float duration)
    {
        Vector3 start = bobber.position;
        float elapsed = 0f;
        float arcHeight = 1.5f; // control arc height of throw

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // Parabolic arc
            Vector3 flat = Vector3.Lerp(start, target, t);
            float height = Mathf.Sin(Mathf.PI * t) * arcHeight;
            bobber.position = new Vector3(flat.x, flat.y + height, flat.z);

            elapsed += Time.deltaTime;
            yield return null;
        }
        // Ensure final position
        bobber.position = target;
    }

    public void EndFishing()    // should be called whenever fishing ends
    {
        Destroy(currentBobber);
        AudioManager.Instance.Stop("Creak");
        isFishing = false;
        chargeMeter = 0f;
        chargeBarUI.UpdatePointer(0f);
        playerController.SetMovementMultiplier(1f); // reset speed
    }
}
