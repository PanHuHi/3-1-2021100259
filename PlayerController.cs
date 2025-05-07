using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("UI 속성")]
    public GameObject aimUI;
    public GameObject interUI;
    public GameObject discardUI;
    public GameObject pcUIPanel;
    public GameObject menuPanel;

    [Header("상호작용 설정")]
    public Transform handPosition;
    public LayerMask pickupLayer;
    public LayerMask ignoreLayer;
    public LayerMask interactiveLayer;

    [Header("기타 UI")]
    public GameObject mp3Panel;

    [HideInInspector] public GameObject selectedPrefab;
    [HideInInspector] public IngredientData ingredientData;

    private Vector3 moveDirection;
    private CharacterController controller;
    private float gravity = 100f;
    private float moveSpeed = 5f;
    private float mouseSensitivity = 2f;
    private float verticalRotation = 0f;

    private bool aimActive = true;
    private bool isDiscard = false;
    private bool interActive = false;
    private bool isMovementEnabled = true;

    private Thrower thrower;
    private ItemPickup itemPickup;
    private Interactive interactive;
    private UIRaycast uiRaycast;
    private Animator animator;

    void Start()
    {
        thrower = GetComponent<Thrower>();
        controller = GetComponent<CharacterController>();
        itemPickup = GetComponent<ItemPickup>();
        interactive = GetComponent<Interactive>();
        uiRaycast = GetComponent<UIRaycast>();
        animator = GetComponent<Animator>();

        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }

        Canvas[] canvases = FindObjectsOfType<Canvas>();

        LockCursor(true);
        InitializeUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIController.isAnyUIOpen && !menuPanel.activeSelf) return;

            ToggleMenu();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (mp3Panel != null)
            {
                bool willOpen = !mp3Panel.activeSelf;
                mp3Panel.SetActive(willOpen);
                UIController.isAnyUIOpen = willOpen;

                LockCursor(!willOpen);
                isMovementEnabled = !willOpen;
            }
        }

        if (!isMovementEnabled) return;


        MovePlayer();
        RotateCamera();
        HandleThrowing();
        UpdateRay();
    }

    private void InitializeUI()
    {
        aimUI.SetActive(true);
        interUI.SetActive(false);
        discardUI.SetActive(false);
    }

    private void ToggleMenu()
    {
        bool willOpen = !menuPanel.activeSelf;
        menuPanel.SetActive(willOpen);
        UIController.isAnyUIOpen = willOpen;
        isMovementEnabled = !willOpen;
        LockCursor(!willOpen);
        Time.timeScale = willOpen ? 0f : 1f;
    }

    public void ResumeGame()
    {
        isMovementEnabled = true;
        menuPanel.SetActive(false);
        UIController.isAnyUIOpen = false;
        LockCursor(true);
        Time.timeScale = 1f;
    }

    private void LockCursor(bool shouldLock)
    {
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldLock;
    }

    private void UpdateRay()
    {
        RaycastHit hit;
        ToggleUI(true, false);
        int layerMask = ~ignoreLayer;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 3f, pickupLayer))
        {
            ToggleUI(false, true);
            itemPickup.TryPickup(hit);
        }

        if (Physics.Raycast(ray, out hit, 3f, interactiveLayer))
        {
            ToggleUI(false, true);
            interactive.TryInteractive(hit);

            if (hit.transform.GetComponentInParent<StackerBurger>() != null)
            {
                SetDiscardUI(true);
            }
        }
        else
        {
            SetDiscardUI(false);
        }
    }

    private void ToggleUI(bool isAimActive, bool isInterActive)
    {
        if (aimActive != isAimActive || interActive != isInterActive)
        {
            aimActive = isAimActive;
            interActive = isInterActive;
            aimUI.SetActive(isAimActive);
            interUI.SetActive(isInterActive);
        }
    }

    private void SetDiscardUI(bool state)
    {
        if (isDiscard != state)
        {
            isDiscard = state;
            discardUI.SetActive(state);
        }
    }

    private void HandleThrowing()
    {
        if (selectedPrefab == null || ingredientData == null) return;

        if (Input.GetMouseButtonDown(1))
        {
            thrower.StartThrowing();
        }

        if (Input.GetMouseButtonUp(1))
        {
            thrower.ReleaseThrow();
            selectedPrefab = null;
        }
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        moveDirection = move * moveSpeed;

        float speed = new Vector3(moveX, 0f, moveZ).magnitude;
        animator.SetFloat("Speed", speed);

        if (!controller.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        controller.Move(moveDirection * Time.deltaTime);
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        verticalRotation = Mathf.Clamp(verticalRotation - mouseY, -90f, 90f);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        isMovementEnabled = isEnabled;
        LockCursor(isEnabled);
    }
}
