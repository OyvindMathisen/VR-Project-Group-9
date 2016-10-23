using UnityEngine;
using System.Collections;

// GLOBAL VARIABLES CLASS
public class root : MonoBehaviour {
    public static bool isHolding = false;

    /// <summary>
    /// make the tiles stay at a certain height
    /// </summary>
    public static float BUILD_HEIGHT = 100.0f;
    /// <summary>
    /// value for smooth tile height management
    /// </summary>
    public static float BUILD_HEIGHT_LERP = 0.15f;
    /// <summary>
    /// important for choosing grid size (do not edit unless you edit the tile sizes)
    /// </summary>
    public static float SNAP_VALUE = 8.0f;
    /// <summary>
    /// 1 / SNAP_VALUE;
    /// </summary>
    public static float SNAP_INVERSE = 0.125f;
    /// <summary>
    /// the current object you're holding / dragging around
    /// (usually a building you haven't placed yet)
    /// </summary>
	public static Transform currentDrag;
    /// <summary>
	/// The distance between PreviewPlacement and the building you're trying to pick up.
	/// </summary>
	public static Vector3 distToPP;
}
