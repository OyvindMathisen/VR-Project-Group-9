﻿using UnityEngine;

public class MoveObject : MonoBehaviour
{
	public Transform ParentOnRelease;

	protected Wand Holder;

	public virtual bool DropMe(Wand controller) // Checks if one of the controllers has released the object.
	{
		if (controller != Holder) return false; // If its not the controller dropping it, return false.
		transform.parent = ParentOnRelease; // Set the objects parent back to what it should be.
		Holder = null;
		return true; // Confirm the object has now been dropped.
	}

	public virtual void GrabMe(Wand controller)
	{
		if (Holder)
		{
			Holder.AreaCheck.DeletePreviews();
			Holder.Drop();
		}
		Holder = controller;
		transform.parent = controller.transform;
	}
}
