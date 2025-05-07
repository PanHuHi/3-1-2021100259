using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PattySliderManager : MonoBehaviour
{
    public GameObject sliderUI;   // ✅ 전체 UI (필 이미지 포함)
    public Image fillImage;       // ✅ 필 이미지 (슬라이더 역할)
    public Image backgroundImage; // ✅ 새로 추가: 슬라이더 배경 이미지
    public GameObject followCanvas;
    public RectTransform minuteHand;

    public Color rawColor = new Color(1f, 0.5f, 0.5f);      // 생 패티 (빨강)
    public Color mediumColor = new Color(1f, 0.75f, 0.5f);  // 중간 익힘 (주황)
    public Color burntColor = new Color(0.4f, 0.2f, 0.2f);  // 탄 패티 (검정)

    private bool backgroundChanged = false; // ✅ 배경 색 변경 여부
    private void Start()
    {
        sliderUI.SetActive(false);
        if (backgroundImage != null)
            backgroundImage.color = Color.white; // 초기값
    }

    private void Update()
    {
        if (followCanvas != null)
        {
            float cameraYRotation = Camera.main.transform.eulerAngles.y;
            followCanvas.transform.eulerAngles = new Vector3(
                followCanvas.transform.eulerAngles.x, cameraYRotation, followCanvas.transform.eulerAngles.z);
        }
    }

    public void UpdateSlider(float cookProgress, float totalCookTime, float burnTime)
    {
    if (fillImage == null) return;

    if (!sliderUI.activeSelf)
        sliderUI.SetActive(true);

    float fill;

    if (cookProgress <= totalCookTime)
    {
        // ✅ 첫 바퀴 (익는 중)
        fill = cookProgress / totalCookTime;
        fillImage.fillAmount = fill;

        float t = fill;
        fillImage.color = Color.Lerp(rawColor, mediumColor, t);

        backgroundChanged = false;
    }
    else if (cookProgress <= burnTime)
    {
        // ✅ 두 번째 바퀴 (타는 중)
        float burnProgress = cookProgress - totalCookTime;
        float burnDuration = burnTime - totalCookTime;
        fill = burnProgress / burnDuration;
        fillImage.fillAmount = fill;

        float t = fill;
        fillImage.color = Color.Lerp(mediumColor, burntColor, t);

        if (!backgroundChanged && backgroundImage != null)
        {
            backgroundImage.color = mediumColor;
            backgroundChanged = true;
        }
    }
    else
    {
        sliderUI.SetActive(false);
        backgroundChanged = false;
        return;
    }

    // ✅ 분침 회전 로직 추가
    if (minuteHand != null)
{
    float angle = 0f;

    if (cookProgress <= totalCookTime)
    {
        float progress = cookProgress / totalCookTime;
        angle = -progress * 360f;
    }
    else if (cookProgress <= burnTime)
    {
        float burnProgress = cookProgress - totalCookTime;
        float burnDuration = burnTime - totalCookTime;
        float progress = burnProgress / burnDuration;
        angle = -progress * 360f;
    }

    minuteHand.localRotation = Quaternion.Euler(0, 0, angle);
}
}

    public void StopCooking()
    {
        sliderUI.SetActive(false);
        backgroundChanged = false;

        if (backgroundImage != null)
            backgroundImage.color = Color.white;
    }
}