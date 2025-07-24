using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class FishingController : MonoBehaviour
{
    #region Casting Settings
    public float baseCastDistance = 1f; // Base distance for casting the fishing line when charge=1
    public float chargeSpeed = 1.5f;    // How fast the charge meter fills
    #endregion

    #region References
    public Tilemap fishingTilemap;
    public GameObject bobberPrefab;
    public Transform castOrigin; // on player 
    public Transform playerTransform;
    #endregion

    #region Player
    public PlayerController playerController;
    public BobberBehavior bobberBehavior;
    private float chargeMeter = 0f;
    public float slowMoveMultiplier = 0.25f;
    [HideInInspector] public static bool isCharging = false;
    [HideInInspector] public static bool isFishing = false;
    [HideInInspector] public static bool isFishingGame = false;
    #endregion

    void Start()
    {
        playerController = playerTransform.GetComponent<PlayerController>();
        bobberBehavior = bobberPrefab.GetComponent<BobberBehavior>();
        isCharging = false;
        isFishing = false;
        isFishingGame = false;        
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && !isFishing && !isFishingGame)
        {
            isCharging = true;
            playerController.SetMovementMultiplier(slowMoveMultiplier); // slow down when charging or fihsing
        }

        if (Input.GetMouseButton(0) && isCharging)
        {
            // Charge goes from 0 to 5
            chargeMeter += Time.deltaTime * chargeSpeed;
            ShowChargeBar(chargeMeter);

            if (chargeMeter < 5f && chargeMeter > 4.8f)
            {
                Debug.Log($"MAX CAST! (Charge: {chargeMeter:F2})");
            }
            if (chargeMeter > 5f)
            {
                isCharging = false;
                chargeMeter = 0f;
                EndFishing();
            }
        }

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            isCharging = false;
            float finalCharge = chargeMeter;
            StartCoroutine(CastBobber(finalCharge));
            chargeMeter = 0f;
        }

        if (Input.GetMouseButtonDown(0) && isFishing)
        {
            EndFishing();
        }
    }
    void ShowChargeBar(float chargeValue)
    {
        // insert UI bar here
        Debug.Log($"Charge: {chargeValue:F2}. Facing: {playerController.currDirection}");
    }

    private IEnumerator CastBobber(float charge)
    {
        float distance = charge * baseCastDistance + 0.1f;

        Vector3 direction = playerController.currDirection; // adjust if using mouse aim or facing. currently uses facing
        Vector3 worldTarget = castOrigin.position + direction * distance;

        // Convert to tile position
        Vector3Int tilePos = fishingTilemap.WorldToCell(worldTarget);
        TileBase tile = fishingTilemap.GetTile(tilePos);
        FishingTile fishingTile = tile as FishingTile;

        isFishing = true;
        if (fishingTile != null)
        {
            Debug.Log($"Valid cast into fishing tile: {fishingTile.fishingZoneID}");
            Instantiate(bobberPrefab, worldTarget, Quaternion.identity);
            bobberBehavior.ItsBobbinTime();
        }
        else
        {
            Debug.Log("Invalid cast - no fishing tile");
            EndFishing();
        }
        yield return null;
    }

    public void EndFishing()    // should be called whenever fishing ends
    {
        isFishing = false;
        playerController.SetMovementMultiplier(1f); // reset speed
    }
}
