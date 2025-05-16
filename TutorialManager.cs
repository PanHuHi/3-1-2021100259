using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class TutorialSlide
{
    public List<string> descriptions;                    // 설명 텍스트들
    public List<TextMeshProUGUI> descriptionTexts;       // 설명 텍스트 UI 오브젝트들
    public List<GameObject> imageObjects;                // ✅ 이미지/움짤 오브젝트 통합 리스트
}

public class TutorialManager : MonoBehaviour
{
    public List<TutorialSlide> slides;                   // 전체 슬라이드 리스트
    public Button prevButton;
    public Button nextButton;

    private int currentIndex = 0;

    public GameObject tutorialPanel;                     // 전체 튜토리얼 UI 패널
    public Button inGameButton;                          // 마지막 슬라이드에서 등장할 InGame 버튼

    void Start()
    {
        // 첫 슬라이드 제외하고 모든 이미지, 텍스트 비활성화
        for (int i = 0; i < slides.Count; i++)
        {
            if (i == 0) continue;

            var slide = slides[i];

            if (slide.imageObjects != null)
            {
                foreach (var image in slide.imageObjects)
                {
                    if (image != null)
                        image.SetActive(false);
                }
            }

            if (slide.descriptionTexts != null)
            {
                foreach (var text in slide.descriptionTexts)
                {
                    if (text != null)
                        text.gameObject.SetActive(false);
                }
            }
        }

        // InGame 버튼 비활성화
        if (inGameButton != null)
            inGameButton.gameObject.SetActive(false);

        UpdateSlide();
    }

    public void NextSlide()
    {
        currentIndex = Mathf.Min(currentIndex + 1, slides.Count - 1);
        UpdateSlide();
    }

    public void PrevSlide()
    {
        currentIndex = Mathf.Max(currentIndex - 1, 0);
        UpdateSlide();
    }

    private void UpdateSlide()
    {
        // 모든 텍스트, 이미지 숨김
        foreach (var slide in slides)
        {
            if (slide.descriptionTexts != null)
            {
                foreach (var text in slide.descriptionTexts)
                {
                    if (text != null)
                        text.gameObject.SetActive(false);
                }
            }

            if (slide.imageObjects != null)
            {
                foreach (var image in slide.imageObjects)
                {
                    if (image != null)
                        image.SetActive(false);
                }
            }
        }

        // 현재 슬라이드 활성화
        var currentSlide = slides[currentIndex];

        if (currentSlide.descriptionTexts != null && currentSlide.descriptions != null)
        {
            for (int i = 0; i < currentSlide.descriptionTexts.Count; i++)
            {
                if (currentSlide.descriptionTexts[i] != null)
                {
                    currentSlide.descriptionTexts[i].gameObject.SetActive(true);
                    if (i < currentSlide.descriptions.Count)
                        currentSlide.descriptionTexts[i].text = currentSlide.descriptions[i];
                    else
                        currentSlide.descriptionTexts[i].text = "";
                }
            }
        }

        if (currentSlide.imageObjects != null)
        {
            foreach (var image in currentSlide.imageObjects)
            {
                if (image != null)
                    image.SetActive(true);
            }
        }

        // 첫/마지막 슬라이드 여부 확인
        bool isFirstSlide = currentIndex == 0;
        bool isLastSlide = currentIndex == slides.Count - 1;

        // InGame 버튼은 마지막 슬라이드에서만
        if (inGameButton != null)
            inGameButton.gameObject.SetActive(isLastSlide);

        // Prev/Next 버튼 상태 제어
        if (prevButton != null)
            prevButton.gameObject.SetActive(!isFirstSlide);
        if (nextButton != null)
            nextButton.gameObject.SetActive(!isLastSlide);
    }

    public void CloseTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
            Debug.Log("✅ 튜토리얼 닫기 완료!");
        }
        else
        {
            Debug.LogWarning("⚠️ tutorialPanel이 연결 안 돼 있습니다!");
        }
    }

    public void OpenTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
            Debug.Log("✅ 튜토리얼 열기 완료!");
        }
        else
        {
            Debug.LogWarning("⚠️ tutorialPanel이 연결 안 돼 있습니다!");
        }
    }
}
