using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BedInteraction : MonoBehaviour
{
    public GameObject warningPanel;
    public Button yesButton;
    public Button noButton;
    private PlayerController playerController;

    private void Start()
    {
        if (warningPanel != null)
        {
            warningPanel.SetActive(false);
        }

        playerController = FindObjectOfType<PlayerController>();

        if (yesButton != null)
            yesButton.onClick.AddListener(ConfirmSaveAndChangeScene);

        if (noButton != null)
            noButton.onClick.AddListener(CloseWarningPanel);
    }

    public void ShowWarningPanel()
    {
        if (UIController.isAnyUIOpen) return;

        if (warningPanel != null)
        {
            warningPanel.SetActive(true);
        }

        UIController.isAnyUIOpen = true;

        if (playerController != null)
        {
            playerController.SetMovementEnabled(false);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ConfirmSaveAndChangeScene()
    {
        SaveManager.SaveGame();
        Debug.Log("게임 데이터 저장 완료");

        if (playerController != null)
        {
            playerController.SetMovementEnabled(true);
        }

        UIController.isAnyUIOpen = false;

        // ✅ 페이드 기능 제거 후, 바로 씬 전환
        SceneManager.LoadScene("Test");
    }

    public void CloseWarningPanel()
    {
        if (warningPanel != null)
        {
            warningPanel.SetActive(false);
        }

        UIController.isAnyUIOpen = false;

        if (playerController != null)
        {
            playerController.SetMovementEnabled(true);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
