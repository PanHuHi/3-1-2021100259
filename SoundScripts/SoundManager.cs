// Author: Small Hedge Games (수정 포함)
// Updated: 2025-04-21

using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

namespace SmallHedge.SoundManager
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundsSO SO;
        private static SoundManager instance = null;
        private AudioSource audioSource;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
                audioSource = GetComponent<AudioSource>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // 🔊 일반 단발 사운드 (지정된 클립 or 랜덤)
        public static void PlaySound(SoundType sound, AudioSource source = null, float volume = 1f, int clipIndex = -1)
        {
            SoundList soundList = instance.SO.sounds[(int)sound];
            AudioClip[] clips = soundList.sounds;
            if (clips.Length == 0) return;

            AudioClip selectedClip = (clipIndex >= 0 && clipIndex < clips.Length)
                ? clips[clipIndex]
                : clips[UnityEngine.Random.Range(0, clips.Length)];

            if (source)
            {
                source.outputAudioMixerGroup = soundList.mixer;
                source.clip = selectedClip;
                source.volume = volume * soundList.volume;
                source.Play();
            }
            else
            {
                instance.audioSource.outputAudioMixerGroup = soundList.mixer;
                instance.audioSource.PlayOneShot(selectedClip, volume * soundList.volume);
            }
        }

        // 🔁 사운드 루프 관리
        private static Dictionary<SoundType, AudioSource> activeLoops = new();

        public static void PlayLoop(SoundType sound, GameObject owner, int clipIndex = 0, float volume = 1f, float fadeIn = 0.5f)
        {
            SoundList soundList = instance.SO.sounds[(int)sound];
            if (soundList.sounds.Length <= clipIndex) return;

            AudioSource source = owner.AddComponent<AudioSource>();
            source.clip = soundList.sounds[clipIndex];
            source.outputAudioMixerGroup = soundList.mixer;
            source.volume = 0f; // 시작은 0 → 점점 증가
            source.loop = true;
            source.spatialBlend = 1f;
            source.Play();

            activeLoops[sound] = source;
            instance.StartCoroutine(FadeInCoroutine(source, volume * soundList.volume, fadeIn));
        }

       public static void StopLoop(SoundType sound, float fadeOut = 0.5f)
    {
        if (activeLoops.TryGetValue(sound, out AudioSource source) && source != null)
        {
            Debug.Log($"[SoundManager] ⏹ StopLoop: '{sound}' 사운드 정지 요청");
            instance.StartCoroutine(FadeOutCoroutine(sound, source, fadeOut));
        }
        else
        {
            Debug.Log($"[SoundManager] ⚠️ StopLoop: '{sound}' 에 대한 사운드가 없음");
        }
    }

        //자연스럽게 사운드 켜젓다 꺼짐(페이드 기능)
        private static IEnumerator FadeInCoroutine(AudioSource source, float targetVolume, float duration)
        {
            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                source.volume = Mathf.Lerp(0f, targetVolume, time / duration);
                yield return null;
            }
            source.volume = targetVolume;
                Debug.Log($"[SoundManager] 📈 페이드 인 완료 → 최종 볼륨: {targetVolume}");
        }

        private static IEnumerator FadeOutCoroutine(SoundType sound, AudioSource source, float duration)
        {
            float startVolume = source.volume;
            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                source.volume = Mathf.Lerp(startVolume, 0f, time / duration);
                yield return null;
            }

            source.Stop();
            UnityEngine.Object.Destroy(source);
            activeLoops.Remove(sound);
               Debug.Log($"[SoundManager] 📉 페이드 아웃 + AudioSource 제거 완료 → '{sound}'");
        }

        public static void SetMixerVolume(SoundType soundType, float volumeDb)
        {
            string exposedParam = soundType.ToString();
            bool success = instance.SO.mixer.SetFloat(exposedParam, volumeDb);
            Debug.Log($"🔊 믹서 볼륨 설정: [{exposedParam}] → {volumeDb} dB {(success ? "✅" : "❌")}");
        }

        public static SoundList GetSoundList(SoundType sound)
        {
            return instance.SO.sounds[(int)sound];
        }

        // 📍 위치 기반 3D 사운드
        public static void PlaySound(SoundType sound, Vector3 worldPosition, float volume = 1f)
        {
            SoundList soundList = instance.SO.sounds[(int)sound];
            AudioClip[] clips = soundList.sounds;
            if (clips.Length == 0) return;

            AudioClip clip = clips[UnityEngine.Random.Range(0, clips.Length)];

            GameObject tempGO = new GameObject("TempAudio_" + sound.ToString());
            tempGO.transform.position = worldPosition;

            AudioSource tempSource = tempGO.AddComponent<AudioSource>();
            tempSource.clip = clip;
            tempSource.outputAudioMixerGroup = soundList.mixer;
            tempSource.volume = volume * soundList.volume;
            tempSource.spatialBlend = 1f;
            tempSource.minDistance = 1f;
            tempSource.maxDistance = 20f;
            tempSource.rolloffMode = AudioRolloffMode.Logarithmic;

            tempSource.Play();
            GameObject.Destroy(tempGO, clip.length + 0.5f);
        }
    }

    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string name;
        [Range(0f, 1f)] public float volume;
        public AudioMixerGroup mixer;
        public AudioClip[] sounds;
    }
}
