using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IncomeManager : MonoBehaviour
{
    public static IncomeManager Instance { get; private set; }

    public TMP_Text dailyIncomeText;
    public TMP_Text publicityText;

    private float totalIncome = 0f;
    private float dailyIncome = 0f;
    private float publicity = 0f;

    [SerializeField] private Transform incomeTextTransform; // 돈 텍스트 UI
    [SerializeField] private Transform moneySpinIcon;       // 회전할 UI 아이콘
    [SerializeField] private float effectDuration = 0.3f;   // 연출 시간

    public TimerManager timer;
    public ResultPanelUI resultPanelUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        dailyIncomeText.text = "0";
        publicityText.text = "0";
        ModifyIncome(1000);
    }

    // ✅ 수익 반환
    public float GetMoney()
    {
        return dailyIncome;
    }

    // ✅ 수익 차감
    public bool SubtractMoney(float amount)
    {
        float modifiedAmount = Mathf.Ceil(amount);

        if (dailyIncome < modifiedAmount)
            return false;

        dailyIncome -= modifiedAmount;
        totalIncome -= modifiedAmount;

        UpdateUI();
        return true;
    }

    // ✅ 수익 설정
    public void SetMoney(float amount)
    {
        dailyIncome = amount;
        totalIncome = amount;
        UpdateUI();
    }

    // ✅ 수익 초기화
    public void ResetMoney()
    {
        dailyIncome = 0f;
        totalIncome = 0f;
        UpdateUI();
    }

    public float GetPublicity()
    {
        return publicity;
    }


    private void UpdateUI()
    {
        if (dailyIncomeText != null)
        {
            dailyIncomeText.text = $"{dailyIncome:N0}";
        }

        if (publicityText != null)
        {
            publicityText.text = $"{publicity:N0}";
        }
    }

    public float GetTotalIncome()
    {
        return totalIncome;
    }

    public void SetState()
    {
        timer.isTrouble = !timer.isTrouble;
    }

    public void SetRemove()
    {
        timer.AddTime(10f);
    }

    public void SetResult()
    {
        int incomeInt = Mathf.CeilToInt(dailyIncome);
        int publicityInt = Mathf.CeilToInt(publicity);
        resultPanelUI.ShowResultText(incomeInt, publicityInt);
    }

    // ✅ 주문 수익 계산 메서드
    public void CalculateOrderIncome(List<MenuData> deliveredOrder, List<CookLevel> burgerStates, List<MenuData> correctOrder)
    {
        float orderTotal = 0f;
        float publicityChange = 0f;
        bool hasRawBurger = false;
        bool isOrderCorrect = CompareOrders(deliveredOrder, correctOrder);

        foreach (var item in deliveredOrder)
        {
            orderTotal += item.price;
            publicityChange += item.publicity;
        }

        foreach (var state in burgerStates)
        {
            if (state == CookLevel.Raw)
            {
                hasRawBurger = true;
                break;
            }
        }

        if (hasRawBurger || !isOrderCorrect)
        {
            float penaltyIncome = Mathf.Ceil(orderTotal * 0.1f);
            float publicityPenalty = Mathf.Ceil(publicityChange * 0.5f);

            ModifyIncome(penaltyIncome);
            ModifyPublicity(publicityPenalty);

            timer.ReduceTime(15f);
            Debug.Log($"❌ 불만족: 수익 +{penaltyIncome}, 화제도 +{publicityPenalty}");
        }
        else
        {
            float income = Mathf.Ceil(orderTotal);
            float publicityBonus = Mathf.Ceil(publicityChange);

            ModifyIncome(income);
            ModifyPublicity(publicityBonus);

            int burgerCount = deliveredOrder.FindAll(item => item is BurgerData).Count;
            float ratio = 0.2f * burgerCount;
            timer.AddTime(ratio, true);

            Debug.Log($"✅ 만족: 수익 +{income}, 화제도 +{publicityBonus}, 버거 {burgerCount}개 → 시간 +{ratio * 100}%");
        }
    }

    // ✅ 주문 일치 여부 비교
    private bool CompareOrders(List<MenuData> order1, List<MenuData> order2)
    {
        if (order1.Count != order2.Count) return false;

        List<MenuData> sorted1 = new List<MenuData>(order1);
        List<MenuData> sorted2 = new List<MenuData>(order2);

        sorted1.Sort((a, b) => a.name.CompareTo(b.name));
        sorted2.Sort((a, b) => a.name.CompareTo(b.name));

        for (int i = 0; i < sorted1.Count; i++)
        {
            if (sorted1[i] != sorted2[i]) return false;
        }

        return true;
    }

    public bool ModifyIncome(float amount)
    {
        float modifiedAmount = Mathf.Ceil(amount);
        float newTotal = totalIncome + modifiedAmount;
        float newDaily = dailyIncome + modifiedAmount;

        if (newTotal < 0 || newDaily < 0)
        {
            return false;
        }

        totalIncome = newTotal;
        dailyIncome = newDaily;
        StartCoroutine(PlayIncomeEffect());

        UpdateUI();
        //Debug.Log($"수익 변경: {(modifiedAmount >= 0 ? "+" : "")}{modifiedAmount}");
        return true;
    }

    public bool ModifyPublicity(float amount)
    {
        float modifiedAmount = Mathf.Ceil(amount);
        float newPublicity = publicity + modifiedAmount;

        if (newPublicity < 0)
        {
            return false;
        }

        publicity = newPublicity;

        UpdateUI();
        //Debug.Log($"화제도 변경: {(modifiedAmount >= 0 ? "+" : "")}{modifiedAmount}");
        return true;
    }

    public void ResetDailyIncome()
    {
        dailyIncome = 0f;
        UpdateUI();
    }

    public void SetState(bool value)
    {
        timer.isTrouble = value;
    }

    private IEnumerator PlayIncomeEffect()
{
    float time = 0f;
    float halfTime = effectDuration / 2f;

    // 초기 상태 저장
    Vector3 originalScale = incomeTextTransform.localScale;
    Quaternion originalRotation = moneySpinIcon.rotation;

    // 커지기 + 회전
    while (time < halfTime)
    {
        time += Time.deltaTime;
        float t = time / halfTime;

        // 스케일: 1 → 1.2
        float scale = Mathf.Lerp(1f, 1.3f, t);
        incomeTextTransform.localScale = Vector3.one * scale;

        // 회전: 0 → 180도
        moneySpinIcon.localRotation = Quaternion.Euler(0f, Mathf.Lerp(0f, 180f, t), 0f);

        yield return null;
    }

    // 작아지기 + 회전 마무리
    time = 0f;
    while (time < halfTime)
    {
        time += Time.deltaTime;
        float t = time / halfTime;

        // 스케일: 1.2 → 1
        float scale = Mathf.Lerp(1.2f, 1f, t);
        incomeTextTransform.localScale = Vector3.one * scale;

        // 회전: 180 → 360도
        moneySpinIcon.localRotation = Quaternion.Euler(0f, Mathf.Lerp(180f, 360f, t), 0f);

        yield return null;
    }

    // 정확히 원래 상태로 정리
    incomeTextTransform.localScale = originalScale;
    moneySpinIcon.rotation = originalRotation;
}
}
