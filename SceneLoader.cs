using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;

        // ✅ UI 상태 초기화
        UIController.isAnyUIOpen = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료"); // 에디터에서 테스트할 때 로그 확인용
        Application.Quit();
    }
}
