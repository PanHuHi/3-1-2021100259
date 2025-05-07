using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCInteraction : MonoBehaviour
{
    public GameObject interactionUI; // "Press F to use" UI
    public GameObject pcUI;          // 아이템 구매용 UI
    public Camera mainCamera;
    public Transform pcCameraPosition; // 확대 위치
    public float cameraMoveSpeed = 3f;

    private bool isPlayerNear = false;
    private bool isUsingPC = false;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;


    void Start()
    {
        interactionUI.SetActive(false);
    }


    void Update()
    {
        if (isPlayerNear && !isUsingPC && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(ZoomToPC());
        }

        if (isUsingPC && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPC();
        }
    }

    private IEnumerator ZoomToPC()
    {
        isUsingPC = true;
        interactionUI.SetActive(true);

        // 저장
        originalCameraPosition = mainCamera.transform.position;
        originalCameraRotation = mainCamera.transform.rotation;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * cameraMoveSpeed;
            mainCamera.transform.position = Vector3.Lerp(originalCameraPosition, pcCameraPosition.position, t);
            mainCamera.transform.rotation = Quaternion.Lerp(originalCameraRotation, pcCameraPosition.rotation, t);
            yield return null;
        }

        pcUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ExitPC()
    {
        isUsingPC = false;
        pcUI.SetActive(false);
        StartCoroutine(ZoomOutFromPC());
    }

    private IEnumerator ZoomOutFromPC()
    {
        float t = 0;
        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        while (t < 1)
        {
            t += Time.deltaTime * cameraMoveSpeed;
            mainCamera.transform.position = Vector3.Lerp(startPos, originalCameraPosition, t);
            mainCamera.transform.rotation = Quaternion.Lerp(startRot, originalCameraRotation, t);
            yield return null;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            interactionUI.SetActive(false);
        }
    }
}

