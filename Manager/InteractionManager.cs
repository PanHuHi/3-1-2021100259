using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public float interactionRange = 1f; // 플레이어와 상호작용 거리
    private Camera playerCamera;
    private ItemPickup itemPickup; // 아이템을 들고 있는지 확인

    void Start()
    {
        playerCamera = Camera.main;
        itemPickup = FindObjectOfType<ItemPickup>(); // 아이템 픽업 시스템 가져오기
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //  좌클릭 감지
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            //  침대 클릭 감지
            if (hit.collider.CompareTag("Bed"))
            {
                BedInteraction bedInteraction = hit.collider.GetComponent<BedInteraction>();
                if (bedInteraction != null)
                {
                    bedInteraction.ShowWarningPanel(); //  침대 클릭 시 경고창 띄우기
                }
            }
            //  컴퓨터 클릭 감지
            else if (hit.collider.CompareTag("Computer"))
            {
                ToggleUI toggleUI = hit.collider.GetComponent<ToggleUI>();
                if (toggleUI != null)
                {
                    toggleUI.OpenUI(); //  컴퓨터 클릭 시 UI 열기
                }
            }
        }
    }
}
