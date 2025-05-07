using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class NameValidator : MonoBehaviour
{
    [Header("UI 참조")]
    public TMP_InputField nameInputField;
    public TMP_Text warningText;
    public Button confirmButton;
    public Image inputBackgroundImage;

    [Header("씬 로딩")]
    public SceneLoader sceneLoader; // ✅ 외부에서 연결
    public string targetSceneName = "Start"; // ✅ 전환할 씬 이름

    private readonly int minLength = 2;
    private readonly int maxLength = 6;
    private readonly string[] bannedWords = new string[]
    {
    "sex", "sexual", "sexy", "fuck", "pussy", "dick", "dildo", "cum", "tits",
    "boobs", "vagina", "penis", "blowjob", "anal", "ass", "orgy", "fetish",
    "rape", "자지", "보지", "섹스", "유두", "음란", "ㅈㅈ", "ㅈㄱ", "ㅈㅅ", "ㅂㅈ", "ㄷㄹ",
    "shit", "bitch", "bastard", "damn", "asshole", "crap", "cunt", "whore",
    "motherfucker", "retard", "fuckyou", "ㅅㅂ", "ㅄ", "병신", "시발", "씨발",
    "좆", "개새", "ㅗ", "tlqkf", "rktpdy", "존나", "놈", "ㄱㅅㄲ",
    "nigger", "nigga", "chink", "gook", "spic", "kike", "towelhead", "cracker",
    "김치녀", "짱깨", "쪽바리", "흑형", "백인놈", "monkey", "neekeri", "chingchong",
    "kill", "die", "hang", "terrorist", "bomb", "schoolshoot", "massacre",
    "죽어", "자살", "총", "칼", "폭파", "죽인다", "살해", "폭탄",
    "admin", "gm", "master", "mod", "system", "staff", "https", ".com", ".net",
    "discord", "kakaotalk", "카톡", "오픈채팅", "010", "주민번호", "id", "pw"
    };

     void Start()
    {
        Debug.Log($"📍 현재 스크립트가 붙어 있는 오브젝트 이름: {gameObject.name}", gameObject);
    }

    public void OnClickConfirm()
    {
        string input = nameInputField.text.Trim();

        if (!IsValidName(input))
        {
            warningText.text = "규칙에 맞지 않는 닉네임입니다";
            confirmButton.interactable = false;
            StartCoroutine(FlashInputField());
            return;
        }

        // 저장
        PlayerPrefs.SetString("PlayerName", input);
        PlayerPrefs.Save();

        // ✅ 씬 전환
        if (sceneLoader != null)
        {
            sceneLoader.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("❌ SceneLoader가 연결되지 않았습니다!");
        }
    }

    private bool IsValidName(string input)
    {
        if (input.Length < minLength || input.Length > maxLength) return false;
        if (!Regex.IsMatch(input, @"^[a-zA-Z0-9가-힣]+$")) return false;

        foreach (string banned in bannedWords)
        {
            if (input.ToLower().Contains(banned)) return false;
        }

        return true;
    }

    private IEnumerator FlashInputField()
    {
        Color originalColor = inputBackgroundImage.color;
        Color warningColor = new Color(1f, 0.5f, 0.5f); // 연한 빨강

        inputBackgroundImage.color = warningColor;

        yield return new WaitForSeconds(1f);

        inputBackgroundImage.color = originalColor;
        warningText.text = ""; // ✅ 텍스트 초기화
        confirmButton.interactable = true;
    }

}
