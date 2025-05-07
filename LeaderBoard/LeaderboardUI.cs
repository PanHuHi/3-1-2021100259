using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LeaderboardUI : MonoBehaviour
{
    public GameObject panel;

    [Header("수동으로 배치한 랭킹 오브젝트 5개")]
    public LeaderboardEntryUI[] rankEntries; // 👈 1~5위 오브젝트 등록

    public TMP_Text yourScoreText;
    public Sprite[] rankSprites; // 🥇 1~5위 스프라이트

     void Start()
    {
        Debug.Log($"📍 현재 스크립트가 붙어 있는 오브젝트 이름: {gameObject.name}", gameObject);
    }

    public void ShowLeaderboard()
    {
        panel.SetActive(true);

        var entries = LeaderboardManager.Instance.GetLeaderboard();

        for (int i = 0; i < rankEntries.Length; i++)
        {
            string name = i < entries.Count ? entries[i].playerName : "---";
            float score = i < entries.Count ? entries[i].score : 0;
            Sprite sprite = i < rankSprites.Length ? rankSprites[i] : null;

            rankEntries[i].SetEntry(name, score, sprite);
        }

        float myScore = PlayerPrefs.GetFloat("LastScore", 0);
        yourScoreText.text = $"Your Score: {myScore:N0}";
    }

    public void CloseLeaderboard()
    {
        panel.SetActive(false);
    }
}
