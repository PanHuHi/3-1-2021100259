using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    [Header("오디오 믹서")]
    public AudioMixer audioMixer;

    public enum SoundType
    {
        AnimationSound,
        BackgroundMusic,
        SFX,
        Environment,
        Cooking,
        UI,
        // 필요한 사운드 그룹 enum으로 계속 추가
    }

    private Dictionary<SoundType, float> originalVolumes = new();
    private bool isPaused = false;

    void Start()
    {
        foreach (SoundType type in System.Enum.GetValues(typeof(SoundType)))
        {
            string exposedParam = type.ToString();
            if (audioMixer.GetFloat(exposedParam, out float volume))
            {
                originalVolumes[type] = volume;
                Debug.Log($"📦 저장됨 [{exposedParam}] = {volume} dB");
            }
            else
            {
                Debug.LogWarning($"❗ expose된 파라미터가 없음: '{exposedParam}'");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        foreach (var entry in originalVolumes)
        {
            string exposedParam = entry.Key.ToString();
            float targetVolume = isPaused ? -80f : entry.Value;

            bool success = audioMixer.SetFloat(exposedParam, targetVolume);
            Debug.Log($"{(isPaused ? "🔇 뮤트" : "🔊 복원")} '{exposedParam}' → {targetVolume} dB {(success ? "✅" : "❌")}");
        }

        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // ✅ 새로 추가: 버튼용 복원 메서드
    public void ResumeSound()
    {
        isPaused = false;
        Time.timeScale = 1f;

        foreach (var entry in originalVolumes)
        {
            string exposedParam = entry.Key.ToString();
            float targetVolume = entry.Value;

            bool success = audioMixer.SetFloat(exposedParam, targetVolume);
            Debug.Log($"🔊 복원 (Resume 버튼) '{exposedParam}' → {targetVolume} dB {(success ? "✅" : "❌")}");
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
