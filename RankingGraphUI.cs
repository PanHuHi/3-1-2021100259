using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RankingGraphUI : MonoBehaviour
{
    public SalesManager salesManager;         // 매출 계산 스크립트
    public GameObject barPrefab;              // Bar 프리팹
    public Transform barContainer;            // 그래프를 담을 부모 오브젝트
    public int maxSales = 2000;               // 최대 매출 기준
    public float animationDuration = 3f;      // 애니메이션 지속 시간

    public Button showGraphButton;
    public GameObject rankingImage;
    public GameObject MainImg;
    public GameObject Panel;

    // ✅ 파란선 위치 기준으로 막대 최대 너비 설정 (직접 지정)
    public float maxBarWidth = 600f;

    void Start()
    {
        if (rankingImage != null) rankingImage.SetActive(false);
        if (Panel != null) Panel.SetActive(false);
        if (MainImg != null) MainImg.SetActive(false);
        if (barContainer != null) barContainer.gameObject.SetActive(false);

        showGraphButton.onClick.AddListener(OnShowGraphClicked);
    }

    void OnShowGraphClicked()
    {
        if (rankingImage != null) rankingImage.SetActive(true);
        if (Panel != null) Panel.SetActive(true);
        if (MainImg != null) MainImg.SetActive(true);
        if (barContainer != null) barContainer.gameObject.SetActive(true);

        // ✅ float → int로 변환 (명시적 캐스팅)
        int playerSales = Mathf.FloorToInt(IncomeManager.Instance.GetTotalIncome());
        salesManager.GenerateSales(playerSales);
        ShowGraph();
    }

    void ShowGraph()
    {
        foreach (Transform child in barContainer)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }

        for (int i = 0; i < salesManager.stores.Count; i++)
        {
            var store = salesManager.stores[i];
            GameObject bar = Instantiate(barPrefab, barContainer);

            // ✅ BarFill 찾기
            RectTransform rt = bar.transform.Find("BarFill").GetComponent<RectTransform>();

            // ✅ 오른쪽 기준으로 설정
            rt.anchorMin = new Vector2(1f, 0f);
            rt.anchorMax = new Vector2(1f, 1f);
            rt.pivot = new Vector2(1f, 0.5f);

            // ✅ 초기 크기 설정
            rt.sizeDelta = new Vector2(0f, 30f);

            // 텍스트 설정
            TMP_Text topText = bar.transform.Find("Money").GetComponent<TMP_Text>();
            TMP_Text bottomText = bar.transform.Find("Name").GetComponent<TMP_Text>();
            TMP_Text rankText = bar.transform.Find("RankText").GetComponent<TMP_Text>();

            topText.text = store.sales.ToString("N0") + "₩";
            bottomText.text = store.storeName;
            rankText.text = $"{i + 1}등";

            // ✅ 파란선까지의 최대 길이 기준으로 제한
            float targetWidth = Mathf.Clamp01((float)store.sales / maxSales) * maxBarWidth;

            StartCoroutine(AnimateBar(rt, targetWidth));
        }
    }

    IEnumerator AnimateBar(RectTransform bar, float targetWidth)
    {
        float w = 0;
        while (w < targetWidth)
        {
            w += Time.deltaTime * 100f; // 애니메이션 속도
            bar.sizeDelta = new Vector2(Mathf.Min(w, targetWidth), bar.sizeDelta.y);
            yield return null;
        }
    }
}
