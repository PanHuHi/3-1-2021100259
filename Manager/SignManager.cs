using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SignManager : MonoBehaviour
{
    public Image fadePanel; // 페이드 효과용 이미지
    public SceneLoader sceneLoader; // 씬 로더 참조
    public string nextSceneName; // 이동할 씬 이름

    public RectTransform signArea; // 서명할 수 있는 UI 영역
    public LineRenderer linePrefab; // 선을 그릴 LineRenderer 프리팹
    public PlayerController playerController; // 🎯 캐릭터 이동 (playerController 참조)

    private List<LineRenderer> lines = new List<LineRenderer>();
    private LineRenderer currentLine;
    private bool isDrawing = false;
    private int signCompleteCheck = 0;
    private bool isSigning = false; // 서명 중인지 체크

    void Start()
    {
        fadePanel.gameObject.SetActive(true);
        StartCoroutine(FadeInEffect());
    }

    void Update()
    {
        if (isSigning) return; // 서명 중이면 이동과 회전 방지

        if (Input.GetMouseButtonDown(0)) StartDrawing();
        else if (Input.GetMouseButton(0) && isDrawing) DrawLine();
        else if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
            signCompleteCheck++;

            if (signCompleteCheck > 10) StartCoroutine(StartSigningProcess());
        }
    }

    void StartDrawing()
    {
        isDrawing = true;
        GameObject newLine = Instantiate(linePrefab.gameObject, signArea);
        currentLine = newLine.GetComponent<LineRenderer>();
        currentLine.positionCount = 0;
        lines.Add(currentLine);
    }

    void DrawLine()
    {
        Vector2 mousePos = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(signArea, mousePos, null, out Vector2 localPoint);
        if (currentLine != null)
        {
            currentLine.positionCount++;
            currentLine.SetPosition(currentLine.positionCount - 1, new Vector3(localPoint.x, localPoint.y, 0));
        }
    }

    IEnumerator FadeInEffect()
    {
        float alpha = 1f;
        fadePanel.color = new Color(0, 0, 0, alpha);
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadePanel.gameObject.SetActive(false);
    }

    IEnumerator StartSigningProcess()
    {
        isSigning = true; // 이동 및 회전 비활성화
        if (playerController != null) playerController.SetMovementEnabled(false); // 🎯 이동 & 회전 멈춤

        // 🎯 마우스 포인터 활성화
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeOutAndChangeScene());
    }

    IEnumerator FadeOutAndChangeScene()
    {
        fadePanel.gameObject.SetActive(true);
        float alpha = 0f;
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        sceneLoader.LoadScene(nextSceneName);
    }
}