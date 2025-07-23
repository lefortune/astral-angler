using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/FishingTile")]
public class FishingTile : Tile
{
    // Used to mark fishing zones on the map.
    // Identifies the type of fishing zone, like "coral_reef" etc.
    // Can be used to determine the fish avialble in this zone
    public string fishingZoneID;
}
