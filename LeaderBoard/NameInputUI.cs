using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NameInputUI : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public GameObject inputPanel;

    void Start()
    {
        inputPanel.SetActive(false);

        if (IncomeManager.Instance == null)
            Debug.LogError("❌ IncomeManager가 씬에 없습니다!");
    }

    public void ShowInput()
    {
        inputPanel.SetActive(true);
        nameInputField.text = "";
        nameInputField.Select();
        nameInputField.ActivateInputField();
    }

    public void SubmitName()
    {
        string name = string.IsNullOrWhiteSpace(nameInputField.text) ? "???" : nameInputField.text;

        float money = IncomeManager.Instance?.GetTotalIncome() ?? 0;
        float publicity = IncomeManager.Instance?.GetPublicity() ?? 0;
        float total = money + publicity;

        PlayerPrefs.SetFloat("LastScore", total);
        PlayerPrefs.Save();

        LeaderboardManager.Instance?.AddEntry(name, total);

        Debug.Log($"✅ 이름: {name}, 수익: {money}, 화제도: {publicity}, 총점: {total}");
    }
}
