using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("게임 종료"); // 에디터에서 테스트할 때 로그 확인용
        Application.Quit();
    }
}
