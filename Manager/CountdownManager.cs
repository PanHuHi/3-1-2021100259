using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownManager : MonoBehaviour
{
    public Image countdownImage;          
    public Sprite[] countdownSprites;    
    public TimerManager timerManager;    
    public PlayerController playerController;  // ✅ PlayerController 추가 연결

    private void Start()
    {
        if (timerManager != null)
        {
            timerManager.enabled = false;
        }
        if (playerController != null)
        {
            playerController.enabled = false; // ✅ 플레이어 회전 멈춤
        }
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        Time.timeScale = 0f;

        for (int i = 0; i < countdownSprites.Length; i++)
        {
            countdownImage.sprite = countdownSprites[i];
            countdownImage.gameObject.SetActive(true);
            yield return WaitForRealSeconds(1f);
        }

        countdownImage.gameObject.SetActive(false);

        Time.timeScale = 1f;

        if (timerManager != null)
        {
            timerManager.enabled = true;
        }
        if (playerController != null)
        {
            playerController.enabled = true; // ✅ 플레이어 회전 재개
        }
    }

    private IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }
}
