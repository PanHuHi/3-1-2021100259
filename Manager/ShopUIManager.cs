using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopUIManager : MonoBehaviour
{
    public List<GameObject> shopPages; // 상점 페이지 리스트
    public List<Button> tabButtons; // 상단 탭 버튼 리스트

    void Start()
    {
        // 처음 실행 시 첫 번째 페이지만 활성화
        ShowShopPage(0);

        // 모든 버튼에 클릭 이벤트 추가
        for (int i = 0; i < tabButtons.Count; i++)
        {
            int index = i; // 람다 캡처 방지 (클로저 문제 해결)
            tabButtons[i].onClick.AddListener(() => ShowShopPage(index));
        }
    }

    void ShowShopPage(int pageIndex)
    {
        // 모든 페이지 숨기기
        foreach (GameObject page in shopPages)
        {
            page.SetActive(false);
        }

        // 선택한 페이지만 활성화
        if (pageIndex >= 0 && pageIndex < shopPages.Count)
        {
            shopPages[pageIndex].SetActive(true);
        }
    }
}
