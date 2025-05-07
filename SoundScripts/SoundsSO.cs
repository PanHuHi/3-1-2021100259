//Author: Small Hedge Games
//Updated: 13/06/2024
using UnityEngine.Audio; // ✅ AudioMixer 관련 타입 포함

using UnityEngine;

namespace SmallHedge.SoundManager
{
    [CreateAssetMenu(menuName = "Small Hedge/Sounds SO", fileName = "Sounds SO")]
    public class SoundsSO : ScriptableObject
    {
        public SoundList[] sounds;
        public AudioMixer mixer; // 각 SoundType에 대해 AudioMixer 그룹을 설정
        
    }
}