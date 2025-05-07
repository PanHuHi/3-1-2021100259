using UnityEngine;

public class HomeSceneAutoSaver : MonoBehaviour
{
    void Start()
    {
        SaveManager.SaveGame();
        Debug.Log(" 홈 씬 진입 시 자동 저장 완료");
    }
}
