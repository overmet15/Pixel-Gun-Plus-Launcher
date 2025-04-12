public class MyCenterScrollonClick : UIDragScrollView
{
	private MyCenterOnChild center;

	public CycleImages cycleImages;

	private void Awake()
	{
		if (center == null)
		{
			center = NGUITools.FindInParents<MyCenterOnChild>(base.gameObject);
		}
	}

	private void OnClick()
	{
		center.CenterOn(base.transform);
		cycleImages.ChangeIndex(GetComponent<PreviewImage>().currentIndex);
	}
}
