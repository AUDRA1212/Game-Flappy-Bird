using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bird : MonoBehaviour
{
    public float jumpForce = 5f;

    private Rigidbody2D rb;

    public static bool gameOver = false;
    public static bool gameStarted = false;

    public GameObject ulangiPanel;
    public GameObject gameOverPanel;

    private bool waitingForInput = false;
    private bool isFullGameOver = false;

    private RectTransform homeInGameOver;
    private RectTransform homeInUlangi;
    private RectTransform retryInUlangi;
    private GameOverManager gameOverManager;

    private GameObject startPromptCanvas;
    private bool promptShown = false;

    void Awake()
    {
        LevelDisplay.isShowing = true;
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        gameOver = false;
        gameStarted = false;
        waitingForInput = false;
        isFullGameOver = false;
        promptShown = false;

        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
        }

        gameOverManager = FindObjectOfType<GameOverManager>(true);
        StartCoroutine(BannerTimeout());
    }

    IEnumerator BannerTimeout()
    {
        yield return new WaitForSeconds(6f);
        if (!gameStarted)
            LevelDisplay.isShowing = false;
    }

    void Update()
    {
        if (!gameStarted && !gameOver)
        {
            if (!LevelDisplay.isShowing)
            {
                if (!promptShown)
                {
                    promptShown = true;
                    ShowStartPrompt();
                }
                if (Input.GetKeyDown(KeyCode.Space))
                    BeginGame();
            }
            return;
        }

        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                rb.linearVelocity = Vector2.up * jumpForce;
            return;
        }

        if (waitingForInput && Input.GetMouseButtonDown(0))
        {
            Vector2 mp = Input.mousePosition;

            if (isFullGameOver)
            {
                if (homeInGameOver != null &&
                    RectTransformUtility.RectangleContainsScreenPoint(homeInGameOver, mp))
                {
                    GoHome();
                    return;
                }
            }
            else
            {
                if (retryInUlangi != null &&
                    RectTransformUtility.RectangleContainsScreenPoint(retryInUlangi, mp))
                {
                    Retry();
                    return;
                }
                if (homeInUlangi != null &&
                    RectTransformUtility.RectangleContainsScreenPoint(homeInUlangi, mp))
                {
                    GoHome();
                    return;
                }
            }
        }
    }

    void BeginGame()
    {
        gameStarted = true;
        if (rb != null)
        {
            rb.gravityScale = 1f;
            rb.linearVelocity = Vector2.up * jumpForce; // jump on the same press
        }
        if (startPromptCanvas != null)
            Destroy(startPromptCanvas);
    }

    void ShowStartPrompt()
    {
        startPromptCanvas = new GameObject("_StartPrompt");
        Canvas cv = startPromptCanvas.AddComponent<Canvas>();
        cv.renderMode = RenderMode.ScreenSpaceOverlay;
        cv.sortingOrder = 80;
        startPromptCanvas.AddComponent<UnityEngine.UI.CanvasScaler>();
        CanvasGroup cg = startPromptCanvas.AddComponent<CanvasGroup>();
        cg.interactable = false;
        cg.blocksRaycasts = false;

        // Background pill
        GameObject pill = new GameObject("pill");
        pill.transform.SetParent(startPromptCanvas.transform, false);
        var pillImg = pill.AddComponent<UnityEngine.UI.Image>();
        pillImg.color = new Color(0f, 0f, 0f, 0.55f);
        pillImg.raycastTarget = false;
        var pillRT = pill.GetComponent<RectTransform>();
        pillRT.anchorMin = new Vector2(0.15f, 0.22f);
        pillRT.anchorMax = new Vector2(0.85f, 0.36f);
        pillRT.offsetMin = Vector2.zero;
        pillRT.offsetMax = Vector2.zero;

        // Main prompt text
        GameObject txtObj = new GameObject("txt");
        txtObj.transform.SetParent(pill.transform, false);
        var tmp = txtObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "TEKAN  SPACE  UNTUK  MULAI";
        tmp.fontSize = 22;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = new Color(1f, 0.9f, 0.2f, 1f);
        tmp.raycastTarget = false;
        var txtRT = txtObj.GetComponent<RectTransform>();
        txtRT.anchorMin = new Vector2(0, 0.45f);
        txtRT.anchorMax = new Vector2(1, 1f);
        txtRT.offsetMin = Vector2.zero;
        txtRT.offsetMax = Vector2.zero;

        // Subtitle
        GameObject subObj = new GameObject("sub");
        subObj.transform.SetParent(pill.transform, false);
        var subTmp = subObj.AddComponent<TextMeshProUGUI>();
        subTmp.text = "Bird siap terbang!";
        subTmp.fontSize = 13;
        subTmp.fontStyle = FontStyles.Normal;
        subTmp.alignment = TextAlignmentOptions.Center;
        subTmp.color = new Color(0.8f, 0.9f, 1f, 0.85f);
        subTmp.raycastTarget = false;
        var subRT = subObj.GetComponent<RectTransform>();
        subRT.anchorMin = new Vector2(0, 0f);
        subRT.anchorMax = new Vector2(1, 0.48f);
        subRT.offsetMin = Vector2.zero;
        subRT.offsetMax = Vector2.zero;

        StartCoroutine(PulsePrompt(cg));
    }

    IEnumerator PulsePrompt(CanvasGroup cg)
    {
        while (cg != null)
        {
            float t = 0f;
            while (t < 0.65f && cg != null)
            {
                cg.alpha = Mathf.Lerp(0.25f, 1f, t / 0.65f);
                t += Time.deltaTime;
                yield return null;
            }
            t = 0f;
            while (t < 0.65f && cg != null)
            {
                cg.alpha = Mathf.Lerp(1f, 0.25f, t / 0.65f);
                t += Time.deltaTime;
                yield return null;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameStarted) return;

        if (collision.gameObject.CompareTag("Pipe") ||
            collision.gameObject.CompareTag("Ground"))
        {
            gameOver = true;

            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;

            if (ScreenEffects.instance != null)
            {
                ScreenEffects.instance.ShakeCamera(0.35f, 0.18f);
                ScreenEffects.instance.FlashScreen(Color.red, 0.25f);
            }

            PlayerHealth ph = FindObjectOfType<PlayerHealth>();
            StartCoroutine(ShowPanelDelayed(ph));
        }
    }

    System.Collections.IEnumerator ShowPanelDelayed(PlayerHealth ph)
    {
        yield return new WaitForSeconds(0.4f);

        if (PlayerHealth.health - 1 > 0)
        {
            if (ulangiPanel != null)
            {
                ulangiPanel.SetActive(true);
                CacheUlangiButtons();
            }
            isFullGameOver = false;
            waitingForInput = true;
        }
        else
        {
            PlayerHealth.health = 0;

            if (ph != null)
                ph.UpdateHearts();

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
                CacheGameOverButtons();
            }
            isFullGameOver = true;
            waitingForInput = true;
        }
    }

    void CacheGameOverButtons()
    {
        if (gameOverPanel == null) return;
        foreach (var btn in gameOverPanel.GetComponentsInChildren<UnityEngine.UI.Button>(true))
        {
            homeInGameOver = btn.GetComponent<RectTransform>();
            break;
        }
    }

    void CacheUlangiButtons()
    {
        if (ulangiPanel == null) return;
        UnityEngine.UI.Button[] btns =
            ulangiPanel.GetComponentsInChildren<UnityEngine.UI.Button>(true);
        foreach (var btn in btns)
        {
            string nm = btn.gameObject.name.ToLower();
            if (nm.Contains("ulang") || nm.Contains("retry"))
                retryInUlangi = btn.GetComponent<RectTransform>();
            else if (nm.Contains("kembali") || nm.Contains("home"))
                homeInUlangi = btn.GetComponent<RectTransform>();
        }
    }

    void GoHome()
    {
        waitingForInput = false;
        PlayerHealth.health = 3;
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    void Retry()
    {
        waitingForInput = false;
        if (gameOverManager != null)
            gameOverManager.LoseAndRetry();
        else
        {
            PlayerHealth.health = Mathf.Max(0, PlayerHealth.health - 1);
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
