using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class TextGroup : MonoBehaviour
{
	[SerializeField]
	private List<UILabel> _labels = new List<UILabel>();

	[SerializeField]
	private string _text;

	[SerializeField]
	private string _localizationKey;

	public string text
	{
		get
		{
			return _text;
		}
		set
		{
			_text = value;
			if (_labels == null || !_labels.Any())
			{
				SetLabels();
			}
			_labels.ForEach(delegate(UILabel l)
			{
				l.text = _text;
			});
		}
	}

	private void OnEnable()
	{
		SetLabels();
		text = _text;
	}

	private void SetLabels()
	{
		if (_labels == null)
		{
			_labels = new List<UILabel>();
		}
		else
		{
			_labels.Clear();
		}
		_labels.AddRange(GetComponentsInChildren<UILabel>(true));
	}

	private void HandleLocalizationChanged()
	{
		text = _text;
	}
}
