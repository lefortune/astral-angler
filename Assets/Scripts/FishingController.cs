using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class FishingController : MonoBehaviour
{
    #region Casting Settings
    public float baseCastDistance = 4f;
    public float chargeSpeed = 1.5f;
    #endregion

    #region References
    public Tilemap fishingTilemap;
    public GameObject bobberPrefab;
    public Transform castOrigin; // on player 
    public Transform playerTransform;
    #endregion

    #region Player
    public PlayerController playerController;
    private float chargeMeter = 0f;
    public float slowMoveMultiplier = 0.25f;
    private bool isCharging = false;
    private bool isFishing = false;
    private bool isFishingGame = false;
    #endregion

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
            chargeMeter += Time.deltaTime * chargeSpeed;
            ShowChargeBar(chargeMeter);
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
            isFishing = false;
        }
    }
    void ShowChargeBar(float chargeValue)
    {
        // Optional: Update UI bar here
        Debug.Log($"Charge: {chargeValue:F2}. Facing: {playerController.currDirection}");
    }

    private IEnumerator CastBobber(float charge)
    {
        float distance = charge * baseCastDistance;

        Vector3 direction = playerController.currDirection; // adjust if using mouse aim or facing
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
        }
        else
        {
            Debug.Log("Invalid cast — no fishing tile");
            isFishing = false;
        }
        yield return null;
    }

    public void EndFishing() // Call this when fishing ends
    {
        isFishing = false;
        playerController.SetMovementMultiplier(1f); // reset speed
    }
}
