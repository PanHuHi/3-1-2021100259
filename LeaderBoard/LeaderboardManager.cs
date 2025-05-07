using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    public List<LeaderboardEntry> topScores = new(); // 랭킹 리스트
    public int maxEntries = 5;                       // 상위 5명까지 저장

    public bool isDevelopmentMode = true; // ✅ 개발 모드 활성화 시 시작과 함께 초기화

    private const string SaveKey = "LeaderboardData"; // PlayerPrefs 키

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // ✅ 개발 모드일 경우 자동 초기화
            if (isDevelopmentMode)
            {
                Debug.Log("🧹 개발 모드 활성화: 랭킹 초기화 실행됨");
                ResetLeaderboard();
            }

            LoadLeaderboard();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddEntry(string playerName, float score)
    {
        topScores.Add(new LeaderboardEntry { playerName = playerName, score = score });

        topScores = topScores
            .OrderByDescending(entry => entry.score)
            .Take(maxEntries)
            .ToList();

        SaveLeaderboard();

        Debug.Log($"[LeaderboardManager] 랭킹 등록됨: {playerName} - {score}");

        for (int i = 0; i < topScores.Count; i++)
        {
            Debug.Log($"[LeaderboardManager] {i + 1}위 - {topScores[i].playerName} : {topScores[i].score}");
        }
    }

    public void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(new LeaderboardListWrapper { entries = topScores });
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public void LoadLeaderboard()
    {
        string json = PlayerPrefs.GetString(SaveKey, "");
        if (!string.IsNullOrEmpty(json))
        {
            topScores = JsonUtility.FromJson<LeaderboardListWrapper>(json).entries;
            Debug.Log("[LeaderboardManager] 저장된 랭킹 불러옴");
        }
        else
        {
            topScores = new List<LeaderboardEntry>();
            Debug.Log("[LeaderboardManager] 저장된 랭킹 없음. 새로 시작");
        }
    }

    public void ResetLeaderboard()
    {
        topScores.Clear();
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();
        Debug.Log("🧹 랭킹 데이터 초기화 완료");
    }

    public List<LeaderboardEntry> GetLeaderboard()
    {
        return topScores;
    }

    [System.Serializable]
    private class LeaderboardListWrapper
    {
        public List<LeaderboardEntry> entries = new();
    }
}
