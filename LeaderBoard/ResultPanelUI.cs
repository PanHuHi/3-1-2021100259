using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ResultPanelUI : MonoBehaviour
{
    [Header("텍스트 UI")]
    public TMP_Text moneyText;
    public TMP_Text publicityText;
    public TMP_Text totalText;

    [Header("내 랭킹 이미지")]
    public Image myRankImage;            // ✅ 이미지 컴포넌트
    public Sprite[] rankSprites;         // ✅ 1~5위 스프라이트 배열

    void Start()
    {
        Debug.Log($"📍 내 GameObject 이름: {gameObject.name}", gameObject);
    }

   public void ShowResultText(int money, int publicity)
    {
        int total = money + publicity;

        // 점수 출력
        moneyText.text = $"money: {money}$";
        publicityText.text = $"publicity: {publicity}";
        totalText.text = $"total: {total}";

        // 랭킹 계산
        var ranks = LeaderboardManager.Instance.GetLeaderboard();
        var tempList = new List<LeaderboardEntry>(ranks);

        // 내 점수 추가
        LeaderboardEntry me = new LeaderboardEntry { playerName = "You", score = total };
        tempList.Add(me);

        // 점수 기준으로 정렬
        tempList = tempList.OrderByDescending(e => e.score).ToList();

        int rawIndex = tempList.FindIndex(e => e.score == total && e.playerName == "You");
        int myRank = rawIndex + 1;

        if (myRank > LeaderboardManager.Instance.maxEntries)
        {
            myRank = 6;
        }

        // ✅ 결과창 열릴 때 무조건 이미지 활성화
       // myRankImage.gameObject.SetActive(true);

        if (myRank >= 1 && myRank <= 5)
        {
            int spriteIndex = Mathf.Clamp(myRank - 1, 0, rankSprites.Length - 1);
            myRankImage.sprite = rankSprites[spriteIndex];
        }
        else
        {
            // 6등은 이미지 비활성화
            myRankImage.gameObject.SetActive(false);
        }
    }

}
