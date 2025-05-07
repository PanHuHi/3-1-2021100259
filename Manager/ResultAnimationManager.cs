using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResultAnimationManager : MonoBehaviour
{
    [Header("좌 → 우 이동")]
    public List<RectTransform> moveLeftToRight;

    [Header("우 → 좌 이동")]
    public List<RectTransform> moveRightToLeft;

    [Header("페이드 인")]
    public List<CanvasGroup> fadeInElements;

    [Header("애니메이션 사이 지연")]
    public float moveDelay = 0.5f; // 각 애니메이션 끝난 후 대기 시간

    [Header("결과창 판넬")]
    public GameObject resultPanel; // 🔥 결과창 판넬 추가
    

    public float moveDuration = 1.0f;
    public float fadeDuration = 1.0f;

    public void PlayResultSequence()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(true); // ✅ 판넬 활성화
        }
        else
        {
            Debug.LogWarning("⚠️ resultPanel is not assigned in ResultAnimationManager.");
        }

        StartCoroutine(PlaySequence());
    }

   private IEnumerator PlaySequence()
    {
        int maxCount = Mathf.Max(moveLeftToRight.Count, moveRightToLeft.Count);

        for (int i = 0; i < maxCount; i++)
        {
            List<IEnumerator> coroutines = new List<IEnumerator>();

            if (i < moveLeftToRight.Count)
                coroutines.Add(AnimateLeftToRight(moveLeftToRight[i], moveDuration));
            if (i < moveRightToLeft.Count)
                coroutines.Add(AnimateRightToLeft(moveRightToLeft[i], moveDuration));

            // 둘 다 동시에 실행
            List<Coroutine> running = new List<Coroutine>();
            foreach (var co in coroutines)
            {
                running.Add(StartCoroutine(co));
            }

            // 둘 다 끝날 때까지 대기
            foreach (var co in running)
            {
                yield return co;
            }

            // ✅ 끝난 후 지연
            yield return new WaitForSeconds(moveDelay);
        }

        // ✅ 이동 애니메이션 다 끝난 후 페이드 인
        foreach (var fade in fadeInElements)
        {
            yield return FadeIn(fade, fadeDuration);
            yield return new WaitForSeconds(moveDelay);
        }
    }


    private IEnumerator AnimateLeftToRight(RectTransform rect, float duration)
    {
        Vector2 endPos = rect.anchoredPosition;
        float canvasWidth = 1920f;  // 고정
        float margin = 200f;        // 여유 공간
        Vector2 startPos = endPos - new Vector2(canvasWidth + margin, 0);

        float elapsedTime = 0f;
        rect.anchoredPosition = startPos;
        rect.gameObject.SetActive(true);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsedTime / duration);
            yield return null;
        }

        rect.anchoredPosition = endPos;
    }

    private IEnumerator AnimateRightToLeft(RectTransform rect, float duration)
    {
        Vector2 endPos = rect.anchoredPosition;
        float canvasWidth = 1920f;
        float margin = 200f;
        Vector2 startPos = endPos + new Vector2(canvasWidth + margin, 0);

        float elapsedTime = 0f;
        rect.anchoredPosition = startPos;
        rect.gameObject.SetActive(true);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsedTime / duration);
            yield return null;
        }

        rect.anchoredPosition = endPos;
    }


    private IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(true);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
