using UnityEngine;

public class PushButton : MonoBehaviour
{
	public GameObject ParentBoard;
	public ButtonType ButtonDirection;
	private DisplaySwitcher _displayBoard;
	private Animator _animationHandler;

    // Use this for initialization
    void Start()
    {
	    _animationHandler = GetComponent<Animator>();
	    _displayBoard = ParentBoard.GetComponent<DisplaySwitcher>();
    }

    public void ButtonPush()
    {
	    _animationHandler.SetBool("Pushed", true); // Sets animation state, and lets it handle the animation
		_displayBoard.ChangeCategory(ButtonDirection); // Tells the board to change category
    }

	public void ButtonUnpush()
	{
		_animationHandler.SetBool("Pushed", false); // Resets animation state once more.
	}
}
