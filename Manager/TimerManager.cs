using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class TimerManager : MonoBehaviour
{
    public Slider circularSlider; 
    public Image fadePanel; 
    public float startTime = 60f; 
    private float currentTime;
    private float initialStartTime; 
    private bool isTimerRunning = false;
    private float timerSpeedMultiplier = 1f; 
    public PlayerController playerController; 

    public RectTransform minuteHand; 

    public bool isTrouble = false;
    private Coroutine troubleCoroutine = null; 

    // ✅ 추가: 결과 애니메이션 매니저 연결
    public ResultAnimationManager resultAnimationManager;

    void Start()
    {
        currentTime = startTime;
        initialStartTime = startTime;
        isTimerRunning = true;
        fadePanel.gameObject.SetActive(true);
        StartCoroutine(FadeInEffect());
        circularSlider.value = 0f;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime * timerSpeedMultiplier;
                UpdateTimerUI();
            }
            else
            {
                isTimerRunning = false;
                currentTime = 0;
                UpdateTimerUI();
                EndTimer();
            }
        }

        if (isTrouble && troubleCoroutine == null)
        {
            troubleCoroutine = StartCoroutine(TroubleTimerRoutine());
        }
        else if (!isTrouble && troubleCoroutine != null)
        {
            StopCoroutine(troubleCoroutine);
            troubleCoroutine = null;
        }
    }

    void UpdateTimerUI()
    {
        if (circularSlider != null)
        {
            float progress = 1f - (currentTime / initialStartTime);
            circularSlider.value = progress;

            if (minuteHand != null)
            {
                float angle = -progress * 360f;
                minuteHand.localRotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    void EndTimer()
    {
        DisablePlayerController();

        if (IncomeManager.Instance != null)
        {
            IncomeManager.Instance.SetResult();
        }

        if (resultAnimationManager != null)
        {
            resultAnimationManager.PlayResultSequence();
        }
        else
        {
            Debug.LogWarning("⚠️ ResultAnimationManager is not assigned in TimerManager.");
        }
    }

    void DisablePlayerController()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator FadeInEffect()
    {
        float alpha = 1f;
        fadePanel.color = new Color(0, 0, 0, alpha);

        while (alpha > 0)
        {
            alpha -= Time.deltaTime / 1.0f;
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadePanel.gameObject.SetActive(false);
    }

    IEnumerator TroubleTimerRoutine()
    {
        while (isTrouble && isTimerRunning)
        {
            yield return new WaitForSeconds(1f);
            ReduceTime(2f);
        }
    }

    public void SetTimerSpeed(float multiplier)
    {
        timerSpeedMultiplier = multiplier;
    }

    public void AddTime(float amount, bool isRatio = false)
    {
        float addedTime = isRatio ? currentTime * amount : amount;
        currentTime += addedTime;

        if (currentTime > initialStartTime)
        {
            currentTime = initialStartTime;
        }

        UpdateTimerUI();
    }

    public void ReduceTime(float penaltyTime)
    {
        currentTime -= penaltyTime;
        if (currentTime < 0)
        {
            currentTime = 0;
        }
        UpdateTimerUI();
    }
}
