using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmallHedge.SoundManager;

public class CountdownManager : MonoBehaviour
{
    public Image countdownImage;
    public Sprite[] countdownSprites;
    public TimerManager timerManager;
    public PlayerController playerController;

    private void Start()
    {
        if (timerManager != null)
        {
            timerManager.enabled = false;
        }
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        Time.timeScale = 0f;

        // 🔇 BGM 정지
        FindObjectOfType<BackgroundMusicPlayer>()?.PauseBGM();

        // 기존 코드 유지
        countdownImage.sprite = countdownSprites[0];
        countdownImage.gameObject.SetActive(true);
        SoundManager.PlaySound(SoundType.COUNT, null, 1f, 0);
        yield return WaitForRealSeconds(1f);

        for (int i = 1; i < countdownSprites.Length; i++)
        {
            countdownImage.sprite = countdownSprites[i];

            if (i == countdownSprites.Length - 1)
                SoundManager.PlaySound(SoundType.COUNT, null, 1f, 1);
            else
                SoundManager.PlaySound(SoundType.COUNT, null, 1f, 0);

            yield return WaitForRealSeconds(1f);
        }

        countdownImage.gameObject.SetActive(false);
        Time.timeScale = 1f;

        // 🔊 BGM 재개
        FindObjectOfType<BackgroundMusicPlayer>()?.ResumeBGM();

        if (timerManager != null) timerManager.enabled = true;
        if (playerController != null) playerController.enabled = true;
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
