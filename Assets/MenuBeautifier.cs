using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuBeautifier : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AutoSpawn()
    {
        if (SceneManager.GetActiveScene().name != "Menu") return;
        new GameObject("_MenuBeautifier").AddComponent<MenuBeautifier>();
    }

    Transform menuRoot;
    Transform lvlllPanel;
    Transform lvlGrid;

    static readonly Color[] LvlColors =
    {
        new Color(0.22f, 0.90f, 0.42f, 1f),   // 1 – hijau
        new Color(0.18f, 0.78f, 0.92f, 1f),   // 2 – cyan
        new Color(1f,    0.85f, 0.12f, 1f),   // 3 – gold
        new Color(1f,    0.55f, 0.08f, 1f),   // 4 – orange
        new Color(0.96f, 0.22f, 0.12f, 1f),   // 5 – merah
        new Color(0.85f, 0.12f, 0.98f, 1f),   // 6 – ungu
    };

    void Start() => StartCoroutine(Initialize());

    IEnumerator Initialize()
    {
        yield return null;
        FindPanels();
        if (lvlllPanel != null) { EnhanceLevelPanel(); AddPanelDecorations(); }
        if (lvlGrid != null) EnhanceLevelButtons();
    }

    // ─── Panels finder ──────────────────────────────────────────────────────

    void FindPanels()
    {
        foreach (var c in Resources.FindObjectsOfTypeAll<Canvas>())
        {
            if (!c.gameObject.scene.IsValid()) continue;
            if (c.gameObject.name == "menu") { menuRoot = c.transform; break; }
        }
        if (menuRoot == null) return;
        lvlllPanel = menuRoot.Find("lvlll");
        if (lvlllPanel != null) lvlGrid = lvlllPanel.Find("lvl");
    }

    // ─── Level panel header ──────────────────────────────────────────────────

    void EnhanceLevelPanel()
    {
        // Ubah background panel jadi navy blue
        var img = lvlllPanel.GetComponent<UnityEngine.UI.Image>();
        if (img != null) img.color = new Color(0.05f, 0.08f, 0.20f, 0.98f);

        // Header 88px di atas panel
        var hdr = new GameObject("_LvlHdr"); hdr.transform.SetParent(lvlllPanel, false);
        var hRT = hdr.AddComponent<RectTransform>();
        hRT.anchorMin = new Vector2(0, 1); hRT.anchorMax = new Vector2(1, 1);
        hRT.pivot = new Vector2(0.5f, 1); hRT.sizeDelta = new Vector2(0, 88);
        hRT.anchoredPosition = Vector2.zero;

        SetStretch(MakePanel(hdr.transform, new Color(0.06f, 0.1f, 0.28f, 1f)));

        // Gold top strip
        Pin(MakePanel(hdr.transform, new Color(1f, 0.78f, 0.12f, 1f)),
            0, 1, 1, 1, new Vector2(0, 5), Vector2.zero, new Vector2(0.5f, 1));
        // Thin separator bottom
        Pin(MakePanel(hdr.transform, new Color(1f, 0.78f, 0.12f, 0.4f)),
            0.04f, 0, 0.96f, 0, new Vector2(0, 1), Vector2.zero, new Vector2(0.5f, 0));

        // "PILIH LEVEL" – besar
        var title = MakeText(hdr.transform, "PILIH LEVEL", 46,
            new Color(1f, 0.88f, 0.15f, 1f), FontStyles.Bold);
        var tRT = title.GetComponent<RectTransform>();
        tRT.anchorMin = new Vector2(0, 0.40f); tRT.anchorMax = new Vector2(1, 1f);
        tRT.offsetMin = new Vector2(8, 0); tRT.offsetMax = new Vector2(-8, 0);

        // Subtitle
        var sub = MakeText(hdr.transform,
            "Kumpulkan semua koin untuk menyelesaikan level!",
            12, new Color(0.65f, 0.85f, 1f, 0.9f), FontStyles.Normal);
        var sRT = sub.GetComponent<RectTransform>();
        sRT.anchorMin = new Vector2(0, 0f); sRT.anchorMax = new Vector2(1, 0.43f);
        sRT.offsetMin = new Vector2(8, 4); sRT.offsetMax = new Vector2(-8, -4);
    }

    // ─── Outer panel decorations (border + corner) ───────────────────────────

    void AddPanelDecorations()
    {
        Color gold   = new Color(1f, 0.78f, 0.12f, 0.9f);
        Color corner = new Color(1f, 0.90f, 0.20f, 1f);
        float thick  = 3f;
        float csz    = 14f;   // corner square size

        // 4-side border frame
        Pin(MakePanel(lvlllPanel, gold), 0, 1, 1, 1, new Vector2(0, thick), Vector2.zero, new Vector2(0.5f, 1)); // top
        Pin(MakePanel(lvlllPanel, gold), 0, 0, 1, 0, new Vector2(0, thick), Vector2.zero, new Vector2(0.5f, 0)); // bot
        Pin(MakePanel(lvlllPanel, gold), 0, 0, 0, 1, new Vector2(thick, 0), Vector2.zero, new Vector2(0, 0.5f)); // left
        Pin(MakePanel(lvlllPanel, gold), 1, 0, 1, 1, new Vector2(thick, 0), Vector2.zero, new Vector2(1, 0.5f)); // right

        // Corner accent squares
        Corner(corner, csz, new Vector2(0, 1), new Vector2(0, 1));   // top-left
        Corner(corner, csz, new Vector2(1, 1), new Vector2(1, 1));   // top-right
        Corner(corner, csz, new Vector2(0, 0), new Vector2(0, 0));   // bot-left
        Corner(corner, csz, new Vector2(1, 0), new Vector2(1, 0));   // bot-right

        // Subtle inner glow strip just inside top border (below header)
        Pin(MakePanel(lvlllPanel, new Color(1f, 0.78f, 0.12f, 0.18f)),
            0.02f, 0.88f, 0.98f, 0.88f, new Vector2(0, 1), Vector2.zero, new Vector2(0.5f, 0.5f));
    }

    void Corner(Color col, float sz, Vector2 anchor, Vector2 pivot)
    {
        var c = MakePanel(lvlllPanel, col);
        var rt = c.GetComponent<RectTransform>();
        rt.anchorMin = anchor; rt.anchorMax = anchor;
        rt.pivot = pivot; rt.sizeDelta = new Vector2(sz, sz);
        rt.anchoredPosition = Vector2.zero;
    }

    // ─── Level buttons ───────────────────────────────────────────────────────

    void EnhanceLevelButtons()
    {
        string[] descs = { "Santai",    "Mudah",    "Normal",
                           "Tantangan", "Keras",    "B O S S" };

        // Buat rounded sprite sekali, pakai untuk semua button
        Sprite rounded = CreateRoundedSprite(64, 13);

        int i = 0;
        foreach (Transform child in lvlGrid)
        {
            if (i >= 6) break;
            Color c = LvlColors[i];

            // 1. Color tint rounded — pojok ikut button
            var tint = MakePanel(child, new Color(c.r, c.g, c.b, 0.20f));
            ApplyRounded(tint, rounded);
            SetStretch(tint);

            // 2. Label strip rounded di bawah saja (30%)
            var labelBg = MakePanel(child, new Color(0f, 0f, 0f, 0.80f));
            ApplyRounded(labelBg, rounded);
            var lRT = labelBg.GetComponent<RectTransform>();
            lRT.anchorMin = new Vector2(0, 0);
            lRT.anchorMax = new Vector2(1, 0.30f);
            lRT.offsetMin = lRT.offsetMax = Vector2.zero;

            // 3. Teks difficulty
            var descTxt = MakeText(labelBg.transform, descs[i], 17, Color.white, FontStyles.Bold);
            SetStretch(descTxt);

            i++;
        }
    }

    void ApplyRounded(GameObject go, Sprite rounded)
    {
        var img = go.GetComponent<UnityEngine.UI.Image>();
        img.sprite = rounded;
        img.type = UnityEngine.UI.Image.Type.Sliced;
    }

    // Buat texture putih rounded rect, jadikan Sprite 9-sliced
    Sprite CreateRoundedSprite(int size, int radius)
    {
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;
        var pixels = new Color[size * size];
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                pixels[y * size + x] = InsideRounded(x, y, size, size, radius)
                    ? Color.white : Color.clear;
        tex.SetPixels(pixels);
        tex.Apply();
        float b = radius;
        return Sprite.Create(tex, new Rect(0, 0, size, size),
            new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect,
            new Vector4(b, b, b, b));
    }

    bool InsideRounded(int x, int y, int w, int h, int r)
    {
        int cx = Mathf.Clamp(x, r, w - 1 - r);
        int cy = Mathf.Clamp(y, r, h - 1 - r);
        int dx = x - cx, dy = y - cy;
        return dx * dx + dy * dy <= r * r;
    }

    // ─── Helpers ─────────────────────────────────────────────────────────────

    // Shorthand untuk set anchor + pivot + size tanpa menulis RT berkali-kali
    void Pin(GameObject go, float axMin, float ayMin, float axMax, float ayMax,
             Vector2 sizeDelta, Vector2 anchoredPos, Vector2 pivot)
    {
        var rt = go.GetComponent<RectTransform>();
        if (rt == null) rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(axMin, ayMin);
        rt.anchorMax = new Vector2(axMax, ayMax);
        rt.pivot = pivot;
        rt.sizeDelta = sizeDelta;
        rt.anchoredPosition = anchoredPos;
    }

    GameObject MakePanel(Transform parent, Color color)
    {
        var go = new GameObject("_p"); go.transform.SetParent(parent, false);
        var img = go.AddComponent<UnityEngine.UI.Image>();
        img.color = color; img.raycastTarget = false;
        return go;
    }

    GameObject MakeText(Transform parent, string text, float size, Color color, FontStyles style)
    {
        var go = new GameObject("_t"); go.transform.SetParent(parent, false);
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text; tmp.fontSize = size; tmp.fontStyle = style;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = color; tmp.raycastTarget = false;
        return go;
    }

    void SetStretch(GameObject go)
    {
        var rt = go.GetComponent<RectTransform>();
        if (rt == null) rt = go.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;
    }
}
