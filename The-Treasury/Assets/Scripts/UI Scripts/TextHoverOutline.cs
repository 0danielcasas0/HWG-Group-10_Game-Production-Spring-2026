using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TMPHoverOutline : MonoBehaviour, IPointerEnterHandler,	IPointerExitHandler
{
	private TextMeshProUGUI tmp;
	private Material mat;

	[SerializeField] private float hoverOutlineWidth = 0.075f;
	private float normalOutlineWidth = 0f;

	void Awake()
	{
		tmp = GetComponent<TextMeshProUGUI>();
		mat = tmp.fontMaterial;
		normalOutlineWidth = mat.GetFloat(ShaderUtilities.ID_OutlineWidth);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		mat.SetFloat(ShaderUtilities.ID_OutlineWidth, hoverOutlineWidth);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		mat.SetFloat(ShaderUtilities.ID_OutlineWidth, normalOutlineWidth);
	}
}
