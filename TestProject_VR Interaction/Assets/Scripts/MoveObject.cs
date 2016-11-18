using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour
{
	public Transform ParentOnRelease;

	protected Wand Holder;

	public virtual bool DropMe(Wand controller) // Checks if one of the controllers has released the object.
	{		
		if (controller != Holder)
			return false;
		transform.parent = ParentOnRelease;
		Holder = null;
		return true;
	}

	// The wand script on the controllers tells the object which one is holding it.
	public virtual void GrabMe(Wand controller)
	{
		if(Holder){
			Holder.AreaCheck.DeletePreviews ();
			Holder.Drop ();
		}
		Holder = controller;
		transform.parent = controller.transform;
	}
}
