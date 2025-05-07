using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public string firstSceneName = "GameScene"; // 새 게임 씬 이름
    public Button startButton; // 새로 시작하기 버튼
    public Button loadButton; // 이어 하기 버튼
    public GameObject warningPopup; // 경고창 UI
    public Button yesButton; // 경고창의 YES 버튼
    public Button noButton; // 경고창의 NO 버튼

    private bool hasSavedData = false;

    void Start()
    {
        hasSavedData = PlayerPrefs.HasKey("SavedScene");

        // 이어하기 버튼은 저장된 데이터가 없으면 비활성화
        if (!hasSavedData)
        {
            loadButton.interactable = false;
        }

        // 버튼 이벤트 추가
        startButton.onClick.AddListener(OnStartButtonPressed);
        loadButton.onClick.AddListener(ContinueGame);
        yesButton.onClick.AddListener(ConfirmStartNewGame);
        noButton.onClick.AddListener(CancelStartNewGame);

        warningPopup.SetActive(false); // 시작 시 경고창 비활성화
    }

    // 새로 시작하기 버튼 눌렀을 때
    public void OnStartButtonPressed()
    {
        if (hasSavedData)
        {
            // 경고창 띄우기
            warningPopup.SetActive(true);
        }
        else
        {
            // 저장된 데이터 없으면 바로 새 게임 시작
            ConfirmStartNewGame();
        }
    }

    // YES 버튼 → 데이터 삭제하고 새 게임 시작
    public void ConfirmStartNewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // ✅ 돈 초기화
        if (IncomeManager.Instance != null)
        {
            IncomeManager.Instance.ResetMoney();
        }

        SceneManager.LoadScene(firstSceneName);
    }


    // NO 버튼 → 경고창 닫기
    public void CancelStartNewGame()
    {
        warningPopup.SetActive(false);
    }

    // 이어 하기
    public void ContinueGame()
    {
        if (!hasSavedData)
        {
            Debug.LogError("❌ 저장된 데이터가 없습니다!");
            return;
        }

        SaveManager.LoadGame();
    }
}
