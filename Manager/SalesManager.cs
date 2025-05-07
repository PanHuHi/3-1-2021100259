using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class StoreData
{
    public string storeName;
    public int sales;
}

public class SalesManager : MonoBehaviour
{
    public List<StoreData> stores = new List<StoreData>();

    public void GenerateSales(int playerSales)
    {
        stores.Clear();

        // 플레이어 데이터
        stores.Add(new StoreData { storeName = "내 가게", sales = playerSales });

        // 나머지 랜덤 매출 가게
        stores.Add(new StoreData { storeName = "버거킹", sales = Random.Range(800, 2000) });
        stores.Add(new StoreData { storeName = "맥도리아", sales = Random.Range(800, 2000) });
        stores.Add(new StoreData { storeName = "엄마의손길", sales = Random.Range(800, 2000) });
        stores.Add(new StoreData { storeName = "비카마이드", sales = Random.Range(800, 2000) });

        // 내림차순 정렬
        stores = stores.OrderByDescending(s => s.sales).ToList();
    }
}