using UnityEngine;
using System.Collections;

// GLOBAL VARIABLES CLASS
public class root : MonoBehaviour {

	public static Vector3 _screenPoint;
    public static Vector3 _offset;

    public static KeyCode nextPreviewKey = KeyCode.X;
    public static KeyCode prevPreviewKey = KeyCode.Z;

    // temporary mouse control notes:
    // - right click while dragging to rotate 90 degrees
    // - left click a placed tile to delete it
    // - X and Z scrolls through tiles, while 1 and 2 are tile category/type
    // - hold tab for centered mouse
    
    // WASD or arrow keys to rotate and move forward (can also use Q and E to rotate fast)
}
