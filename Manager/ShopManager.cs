using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance; // ✅ 싱글톤 인스턴스 추가

    public bool isDevelopmentMode = true; // 개발 모드 (true이면 실행 시 데이터 초기화)
    public List<ShopItem> shopItems = new List<ShopItem>(); // 상점 아이템 리스트

    [System.Serializable]
    public class ShopItem
    {
        public string itemName; // 아이템 이름
        public Button button; // 구매 버튼
        public TMP_Text buttonText; // 버튼에 표시될 가격 텍스트
        public int itemPrice; // 아이템 개별 가격

        // 아이템 구매 시 호출, PlayerPrefs에 저장하여 해금 상태 유지
        public void Purchase()
        {
            PlayerPrefs.SetInt("Unlocked_" + itemName, 1);
        }
    }

    void Awake()
    {
        // ✅ 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (isDevelopmentMode)
        {
            PlayerPrefs.DeleteAll(); // 개발 모드일 경우 모든 저장 데이터 초기화
        }

        LoadPurchasedItems(); // 구매 내역 불러오기
    }

    void LoadPurchasedItems()
    {
        foreach (ShopItem item in shopItems)
        {
            // 이미 구매한 아이템이면 버튼 숨김
            if (PlayerPrefs.GetInt("Unlocked_" + item.itemName, 0) == 1)
            {
                item.button.gameObject.SetActive(false);
                continue;
            }

            // 버튼에 가격 표시
            if (item.buttonText != null)
            {
                item.buttonText.text = item.itemPrice + "$";
            }

            // 버튼 클릭 시 구매 시도
            item.button.onClick.AddListener(() => TryPurchaseItem(item));
        }
    }

    void TryPurchaseItem(ShopItem item)
    {
        // 돈이 충분하면 아이템 구매 후 버튼 숨김
        if (IncomeManager.Instance != null && IncomeManager.Instance.SubtractMoney(item.itemPrice))
        {
            item.Purchase();
            item.button.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("돈이 부족합니다!");
        }
    }
}
