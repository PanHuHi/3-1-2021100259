using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopUIManager : MonoBehaviour
{
    public List<GameObject> shopPages; // ���� ������ ����Ʈ
    public List<Button> tabButtons; // ��� �� ��ư ����Ʈ

    void Start()
    {
        // ó�� ���� �� ù ��° �������� Ȱ��ȭ
        ShowShopPage(0);

        // ��� ��ư�� Ŭ�� �̺�Ʈ �߰�
        for (int i = 0; i < tabButtons.Count; i++)
        {
            int index = i; // ���� ĸó ���� (Ŭ���� ���� �ذ�)
            tabButtons[i].onClick.AddListener(() => ShowShopPage(index));
        }
    }

    void ShowShopPage(int pageIndex)
    {
        // ��� ������ �����
        foreach (GameObject page in shopPages)
        {
            page.SetActive(false);
        }

        // ������ �������� Ȱ��ȭ
        if (pageIndex >= 0 && pageIndex < shopPages.Count)
        {
            shopPages[pageIndex].SetActive(true);
        }
    }
}
