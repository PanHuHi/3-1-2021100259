using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntryUI : MonoBehaviour
{
    public TMP_Text playerNameText;
    public TMP_Text scoreText;
    public Image rankImage;


    void Start()
    {
        Debug.Log($"📍 현재 스크립트가 붙어 있는 오브젝트 이름: {gameObject.name}", gameObject);
    }

    public void SetEntry(string name, float score, Sprite rankSprite)
    {
        playerNameText.text = name;
        scoreText.text = $"{score:N0}";
        if (rankImage != null && rankSprite != null)
            rankImage.sprite = rankSprite;
    }
}
