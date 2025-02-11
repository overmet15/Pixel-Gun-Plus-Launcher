using System;
using UnityEngine;

public sealed class ButtonHandler : MonoBehaviour
{
	public bool noSound;

	[NonSerialized]
	public bool isEnable = true;

	public event EventHandler Clicked;

	private void OnClick()
	{
		if (isEnable)
		{
			EventHandler clicked = this.Clicked;
			if (clicked != null)
			{
				clicked(this, EventArgs.Empty);
			}
		}
	}

	public void DoClick()
	{
		if (isEnable)
		{
			OnClick();
		}
	}
}
