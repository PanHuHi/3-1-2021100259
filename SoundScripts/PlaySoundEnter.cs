using UnityEngine;
using UnityEngine.Audio;

namespace SmallHedge.SoundManager
{
    public class PlaySoundEnter : StateMachineBehaviour
    {
        [SerializeField] private SoundType sound;
        [SerializeField, Range(0f, 1f)] private float volume = 1f;
        [SerializeField] private int clipIndex = 0; // ✅ 추가: 재생할 인덱스 지정

        private AudioSource loopSource;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            SoundList soundList = SoundManager.GetSoundList(sound);
            if (soundList.sounds.Length == 0) return;
            if (clipIndex >= soundList.sounds.Length) clipIndex = 0; // ✅ 예외 처리

            loopSource = animator.gameObject.AddComponent<AudioSource>();
            loopSource.clip = soundList.sounds[clipIndex]; // ✅ 랜덤 X, 지정한 인덱스로
            loopSource.volume = volume * soundList.volume;
            loopSource.outputAudioMixerGroup = soundList.mixer;
            loopSource.loop = true;
            loopSource.spatialBlend = 1f;
            loopSource.Play();
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (loopSource != null)
            {
                loopSource.Stop();
                Object.Destroy(loopSource);
            }
        }
    }
}
