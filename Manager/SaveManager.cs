using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static void SaveGame()
    {
        // ✅ 현재 씬 저장
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("SavedScene", currentScene);

        // ✅ 플레이어 위치 저장
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 pos = player.transform.position;
            PlayerPrefs.SetFloat("PlayerPosX", pos.x);
            PlayerPrefs.SetFloat("PlayerPosY", pos.y);
            PlayerPrefs.SetFloat("PlayerPosZ", pos.z);
        }

       // ✅ 플레이어 돈 저장
        if (IncomeManager.Instance != null)
        {
            PlayerPrefs.SetInt("PlayerMoney", Mathf.CeilToInt(IncomeManager.Instance.GetTotalIncome()));
        }


        // ✅ 구매한 재료 저장
        foreach (var item in ShopManager.Instance.shopItems)
        {
            PlayerPrefs.SetInt("Unlocked_" + item.itemName, PlayerPrefs.GetInt("Unlocked_" + item.itemName, 0));
        }

        PlayerPrefs.Save(); // ✅ 데이터 저장
        Debug.Log("✅ 게임 저장 완료!");
    }

    public static void LoadGame()
    {
        if (!PlayerPrefs.HasKey("SavedScene"))
        {
            Debug.LogError("❌ 저장된 데이터가 없습니다!");
            return;
        }

        // ✅ 저장된 씬 불러오기
        string savedScene = PlayerPrefs.GetString("SavedScene");

        // ✅ 씬 로드
        SceneManager.LoadScene(savedScene);

        // ✅ 씬이 변경된 후 플레이어 위치 복원
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        float posX = PlayerPrefs.GetFloat("PlayerPosX", 0);
        float posY = PlayerPrefs.GetFloat("PlayerPosY", 0);
        float posZ = PlayerPrefs.GetFloat("PlayerPosZ", 0);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(posX, posY, posZ);
            Debug.Log($"🔄 플레이어 위치 복원 완료: ({posX}, {posY}, {posZ})");
        }

        // ✅ 플레이어 돈 불러오기
        if (IncomeManager.Instance != null)
        {
            int savedMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
            IncomeManager.Instance.SetMoney(savedMoney);
        }

        // ✅ 구매한 재료 불러오기
        foreach (var item in ShopManager.Instance.shopItems)
        {
            if (PlayerPrefs.GetInt("Unlocked_" + item.itemName, 0) == 1)
            {
                item.Purchase(); // ✅ 구매 상태 반영
            }
        }

        SceneManager.sceneLoaded -= OnSceneLoaded; // ✅ 이벤트 해제 (메모리 최적화)
    }
}
