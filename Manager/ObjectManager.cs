using UnityEngine;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;
    public List<GameObject> fryMachinesPrefabs; // ✅ 감자튀김 기계 프리팹 리스트
    public List<Transform> spawnLocations; // ✅ 생성될 위치 리스트
    private List<GameObject> spawnedMachines = new List<GameObject>(); // ✅ 생성된 오브젝트 리스트

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ✅ 씬이 변경되어도 유지
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        LoadPurchasedMachines();
    }

    void LoadPurchasedMachines()
    {
        for (int i = 0; i < fryMachinesPrefabs.Count; i++)
        {
            string key = "Unlocked_" + fryMachinesPrefabs[i].name;
            if (PlayerPrefs.GetInt(key, 0) == 1) // ✅ 구매된 상태인지 확인
            {
                SpawnMachine(i);
            }
        }
    }

    void SpawnMachine(int index)
    {
        if (index >= fryMachinesPrefabs.Count || index >= spawnLocations.Count) return;

        GameObject newMachine = Instantiate(fryMachinesPrefabs[index], spawnLocations[index].position, Quaternion.identity);
        spawnedMachines.Add(newMachine);
    }
}
