using UnityEngine;
using SmallHedge.SoundManager;

public class PlaySoundOnTrigger : MonoBehaviour
{
    public SoundType soundToPlay; // Inspector에서 COOKING 선택 가능
    public float volume = 1f;

    public void PlaySound()
    {
        SoundManager.PlaySound(soundToPlay, null, volume);
    }
}
