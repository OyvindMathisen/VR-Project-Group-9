using UnityEngine;

// GLOBAL VARIABLES CLASS
public class GameSettings : MonoBehaviour {
    /// <summary>
    /// The set height buildings will remain at.
    /// </summary>
    public static float BUILD_HEIGHT = 100.0f;
    /// <summary>
    /// Value for smoothing out building movement towards BUILD_HEIGHT.
    /// </summary>
    public static float BUILD_HEIGHT_LERP = 0.15f;
    /// <summary>
    /// Size of grid for building placement.
    /// </summary>
    public static float SNAP_VALUE = 8.0f;
    /// <summary>
    /// 1 / SNAP_VALUE;
    /// </summary>
    public static float SNAP_INVERSE = 0.125f;
    /// <summary>
    /// The center of the newly landed and placed tile.
    /// </summary>
    public static Vector3 NewestLandedPosition;
    /// <summary>
    /// Height adjustment for the "previewplacement" and "relevanttile" squares.
    /// </summary>
    public static float PREVIEW_HEIGHT_ADJUST = -4.0f;
    /// <summary>
    /// The time the user gets to decide if the current combination alternative is the one the user wants.
    /// </summary>
    public static float COMBO_DECISION_TIME = 5.0f; //normally 2.5f - dev mode atm

}
