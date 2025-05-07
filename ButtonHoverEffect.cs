using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite normalSprite; // 기본 이미지
    public Sprite hoverSprite;  // 마우스 올라갈 때 이미지

    private Image image;
    private Vector3 originalScale;

    private void Start()
    {
        image = GetComponent<Image>();
        originalScale = transform.localScale;

        if (image != null && normalSprite != null)
            image.sprite = normalSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image != null && hoverSprite != null)
            image.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (image != null && normalSprite != null)
            image.sprite = normalSprite;

        transform.localScale = originalScale;
    }
}
