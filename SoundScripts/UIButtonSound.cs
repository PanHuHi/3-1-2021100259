using UnityEngine;
using SmallHedge.SoundManager;

public class UIButtonSound : MonoBehaviour
{
    public enum ButtonSoundContext { Default, Error, Confirm }

    public ButtonSoundContext context = ButtonSoundContext.Default;

    // ✅ 버튼 클릭 이벤트로 연결 가능한 함수
    public void OnButtonClick()
    {
        switch (context)
        {
            case ButtonSoundContext.Default:
                SoundManager.PlaySound(SoundType.UI_Click);
                break;

            case ButtonSoundContext.Error:
                SoundManager.PlaySound(SoundType.UI_Error);
                break;

            case ButtonSoundContext.Confirm:
                SoundManager.PlaySound(SoundType.UI_Confirm);
                break;
        }
    }
}
