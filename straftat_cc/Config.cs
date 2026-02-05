using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using STRAFTAT_CC;

public class Config
{
    public static Config Instance = new Config();


    private Vector2 _debugScrollPosition = Vector2.zero;
    private List<string> _debugLogs = new List<string>();
    private const int MAX_LOGS = 50;
    private bool _showDebugWindow = true;
    private bool _showCombat = true;
    private bool _showVisuals = true;
    private bool _showWeaponMods = true;
    private bool _showMisc = true;
    private bool _showDebug = true;

    // Animation variables
    private float _menuTransition = 0f;
    private string _previousMenu = "";
    private float _buttonHoverTime = 0f;
    private string _hoveredButton = "";
    private Dictionary<string, float> _buttonAnimations = new Dictionary<string, float>();
    private float _titleGlowIntensity = 0f;


    // Visuals
    public bool enableESP = true;
    public bool enable3DBoxESP = true;
    public bool enable2DBoxESP = false;
    public bool enableHealthBarESP = false;
    public bool healthBarHorizontal = true; // true = horizontal, false = vertical

    // Exploits
    public bool InfiniteAmmo = false;
    public bool RapidFire = false;
    public bool InstaKill = false;
    public bool NoSpread = false;
    public bool WeaponSpeed = false;

    public bool Aimbot = false;
    public bool AutoShoot = false;
    public bool FlyMode = false;
    public bool GodMode = false;
    public bool MagicBullet = false;
    public bool FreezeEnemy = false;

    
    public float speedValue = 6f;
    public float jumpSpeedValue = 8f;
    public float jumpHeightValue = 2f;
    public float crouchSpeedValue = 3f;
    public float gravityValue = 20f;

    
    public bool enableSpeedHack = false;
    public bool infiniteJumpEnabled = false;

    // testing menu
    public bool enableIsSlide = false;
    public bool enableIsGrounded = false;
    public bool enableIsSprinting = false;
    public bool enableIsScopeAiming = false;
    public bool enableIsLeaning = false;

    private GUIStyle _windowStyle;
    private GUIStyle _headerStyle;
    private GUIStyle _buttonStyle;
    private GUIStyle _activeButtonStyle;
    private GUIStyle _toggleStyle;
    private GUIStyle _logStyle;
    private GUIStyle _panelStyle;
    private GUIStyle _titleStyle;
    private GUIStyle _categoryButtonStyle;
    private GUIStyle _activeCategoryStyle;
    private GUIStyle _featureButtonStyle;
    private GUIStyle _featureButtonActiveStyle;
    private GUIStyle _sliderStyle;
    private GUIStyle _sliderThumbStyle;

    public KeyCode boundKey_aimbot = KeyCode.E;
    public bool waitingForKey = false;

    private Texture2D MakeTexture(int width, int height, Color color)
    {
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = color;

        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    private Texture2D MakeGradientTexture(int width, int height, Color colorStart, Color colorEnd, bool vertical = true)
    {
        Color[] pixels = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float t = vertical ? (float)y / height : (float)x / width;
                pixels[y * width + x] = Color.Lerp(colorStart, colorEnd, t);
            }
        }

        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    private void InitializeStyles()
    {
        if (_windowStyle == null)
        {
            // Modern dark theme colors - more vibrant
            Color primaryDark = new Color(0.12f, 0.12f, 0.15f, 1f);
            Color secondaryDark = new Color(0.18f, 0.18f, 0.22f, 1f);
            Color accentColor = new Color(0.6f, 0.2f, 1f, 1f); // Vibrant purple
            Color accentHover = new Color(0.7f, 0.3f, 1f, 1f);
            Color textColor = new Color(1f, 1f, 1f, 1f);
            Color mutedText = new Color(0.7f, 0.7f, 0.8f, 1f);

            // Main window style - completely custom
            _windowStyle = new GUIStyle(GUI.skin.box);
            _windowStyle.normal.background = MakeTexture(2, 2, new Color(0.12f, 0.12f, 0.15f, 1f));
            _windowStyle.normal.textColor = Color.clear; // Hide default title
            _windowStyle.fontSize = 1;
            _windowStyle.padding = new RectOffset(2, 2, 2, 2);
            _windowStyle.border = new RectOffset(1, 1, 1, 1);

            // Title style
            _titleStyle = new GUIStyle(GUI.skin.label);
            _titleStyle.normal.textColor = accentColor;
            _titleStyle.fontSize = 22;
            _titleStyle.fontStyle = FontStyle.Bold;
            _titleStyle.alignment = TextAnchor.MiddleCenter;
            _titleStyle.padding = new RectOffset(0, 0, 10, 10);

            // Category button style (left sidebar)
            _categoryButtonStyle = new GUIStyle(GUI.skin.button);
            _categoryButtonStyle.normal.background = MakeTexture(2, 2, new Color(0.18f, 0.18f, 0.22f, 1f));
            _categoryButtonStyle.hover.background = MakeTexture(2, 2, new Color(0.25f, 0.25f, 0.30f, 1f));
            _categoryButtonStyle.normal.textColor = mutedText;
            _categoryButtonStyle.hover.textColor = textColor;
            _categoryButtonStyle.fontSize = 13;
            _categoryButtonStyle.fontStyle = FontStyle.Bold;
            _categoryButtonStyle.alignment = TextAnchor.MiddleLeft;
            _categoryButtonStyle.padding = new RectOffset(15, 10, 10, 10);
            _categoryButtonStyle.margin = new RectOffset(3, 3, 2, 2);

            // Active category style
            _activeCategoryStyle = new GUIStyle(_categoryButtonStyle);
            _activeCategoryStyle.normal.background = MakeTexture(2, 2, new Color(0.6f, 0.2f, 1f, 1f));
            _activeCategoryStyle.hover.background = MakeTexture(2, 2, new Color(0.7f, 0.3f, 1f, 1f));
            _activeCategoryStyle.normal.textColor = Color.white;
            _activeCategoryStyle.hover.textColor = Color.white;
            _activeCategoryStyle.fontStyle = FontStyle.Bold;

            // Feature button style (middle panel)
            _featureButtonStyle = new GUIStyle(GUI.skin.button);
            _featureButtonStyle.normal.background = MakeTexture(2, 2, new Color(0.20f, 0.20f, 0.25f, 1f));
            _featureButtonStyle.hover.background = MakeTexture(2, 2, new Color(0.28f, 0.28f, 0.35f, 1f));
            _featureButtonStyle.normal.textColor = textColor;
            _featureButtonStyle.hover.textColor = Color.white;
            _featureButtonStyle.fontSize = 12;
            _featureButtonStyle.alignment = TextAnchor.MiddleLeft;
            _featureButtonStyle.padding = new RectOffset(15, 15, 9, 9);
            _featureButtonStyle.margin = new RectOffset(3, 3, 3, 3);

            // Active feature button
            _featureButtonActiveStyle = new GUIStyle(_featureButtonStyle);
            _featureButtonActiveStyle.normal.background = MakeTexture(2, 2, new Color(0.2f, 0.7f, 0.3f, 1f));
            _featureButtonActiveStyle.hover.background = MakeTexture(2, 2, new Color(0.25f, 0.8f, 0.35f, 1f));
            _featureButtonActiveStyle.normal.textColor = Color.white;
            _featureButtonActiveStyle.fontStyle = FontStyle.Bold;

            // Header style
            _headerStyle = new GUIStyle(GUI.skin.label);
            _headerStyle.normal.background = MakeTexture(2, 2, new Color(0.18f, 0.18f, 0.22f, 1f));
            _headerStyle.normal.textColor = accentColor;
            _headerStyle.fontSize = 14;
            _headerStyle.fontStyle = FontStyle.Bold;
            _headerStyle.alignment = TextAnchor.MiddleLeft;
            _headerStyle.padding = new RectOffset(12, 10, 10, 10);

            // Toggle style
            _toggleStyle = new GUIStyle(GUI.skin.toggle);
            _toggleStyle.normal.textColor = textColor;
            _toggleStyle.onNormal.textColor = accentColor;
            _toggleStyle.hover.textColor = Color.white;
            _toggleStyle.onHover.textColor = accentHover;
            _toggleStyle.fontSize = 12;
            _toggleStyle.padding = new RectOffset(20, 5, 5, 5);
            _toggleStyle.margin = new RectOffset(5, 5, 5, 5);

            // Log style
            _logStyle = new GUIStyle(GUI.skin.label);
            _logStyle.normal.textColor = new Color(0.9f, 0.9f, 0.95f, 1f);
            _logStyle.fontSize = 10;
            _logStyle.wordWrap = true;
            _logStyle.padding = new RectOffset(10, 10, 5, 5);
        }
    }

    private string currentMenu = "Aimbot";

    private bool DrawCategoryButton(string label, bool isActive)
    {
        GUIStyle style = isActive ? _activeCategoryStyle : _categoryButtonStyle;
        
        // Add animated glow effect
        if (!_buttonAnimations.ContainsKey(label))
            _buttonAnimations[label] = 0f;

        if (isActive)
            _buttonAnimations[label] = Mathf.Lerp(_buttonAnimations[label], 1f, Time.deltaTime * 5f);
        else
            _buttonAnimations[label] = Mathf.Lerp(_buttonAnimations[label], 0f, Time.deltaTime * 3f);

        return GUILayout.Button("▸ " + label, style, GUILayout.Height(35));
    }

    private bool DrawFeatureButton(string label, bool isEnabled)
    {
        GUIStyle style = isEnabled ? _featureButtonActiveStyle : _featureButtonStyle;
        string prefix = isEnabled ? "● " : "○ ";
        return GUILayout.Button(prefix + label, style, GUILayout.Height(32));
    }

    public void Draw()
    {
        InitializeStyles();

        // Animate title glow
        _titleGlowIntensity = Mathf.PingPong(Time.time * 0.5f, 1f);

        // Handle menu transitions
        if (currentMenu != _previousMenu)
        {
            _menuTransition = 0f;
            _previousMenu = currentMenu;
        }
        _menuTransition = Mathf.Lerp(_menuTransition, 1f, Time.deltaTime * 8f);

        int menuWidth = 180;
        int contentWidth = 440;
        int previewWidth = 380;

        // Custom title bar
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("STRAFTAT", _titleStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUI.color = new Color(0.6f, 0.2f, 1f, 0.5f);
        GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
        GUI.color = Color.white;

        GUILayout.Space(8);
        GUILayout.BeginHorizontal();

        // ===== LEFT SIDEBAR (Categories) =====
        GUILayout.BeginVertical(GUILayout.Width(menuWidth));
        
        GUILayout.Space(5);

        if (DrawCategoryButton("Ragebot", currentMenu == "Ragebot")) currentMenu = "Ragebot";
        if (DrawCategoryButton("Legitbot", currentMenu == "Legitbot")) currentMenu = "Legitbot";
        if (DrawCategoryButton("Visuals", currentMenu == "Visuals")) currentMenu = "Visuals";
        if (DrawCategoryButton("Exploits", currentMenu == "Exploits")) currentMenu = "Exploits";
        if (DrawCategoryButton("Misc", currentMenu == "Misc")) currentMenu = "Misc";
        if (DrawCategoryButton("AntiAim", currentMenu == "AntiAim")) currentMenu = "AntiAim";
        if (DrawCategoryButton("Debug", currentMenu == "Debug")) currentMenu = "Debug";
        
        GUILayout.FlexibleSpace();
        
        GUILayout.EndVertical();

        GUILayout.Space(5);

        // ===== MIDDLE PANEL (Content) =====
        GUILayout.BeginVertical(GUILayout.Width(contentWidth));
        
        GUILayout.Space(5);
        GUILayout.Label(currentMenu.ToUpper(), _headerStyle);
        GUILayout.Space(5);
        
        GUI.color = new Color(1f, 1f, 1f, _menuTransition);
        _debugScrollPosition = GUILayout.BeginScrollView(_debugScrollPosition, GUILayout.Height(420));
        GetMenuContent();
        GUILayout.EndScrollView();
        GUI.color = Color.white;
        
        GUILayout.EndVertical();

        GUILayout.Space(8);

        // ===== RIGHT PANEL (Preview/Debug) =====
        GUILayout.BeginVertical(GUILayout.Width(previewWidth));
        
        GUILayout.Space(5);
        if (currentMenu == "Debug")
        {
            GUILayout.Label("LOG MESSAGES", _headerStyle);
        }
        else if (currentMenu == "Visuals")
        {
            GUILayout.Label("ESP PREVIEW", _headerStyle);
        }
        GUILayout.Space(5);
        
        if (currentMenu == "Debug")
        {
            DrawDebugPanel();
        }
        else if (currentMenu == "Visuals")
        {
            DrawESPPreview();
        }
        
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
        GUILayout.Space(8);
    }

    void DrawDebugPanel()
    {
        Vector2 debugScroll = GUILayout.BeginScrollView(_debugScrollPosition, GUILayout.Height(350));
        GUI.backgroundColor = new Color(0.08f, 0.08f, 0.12f, 1f);
        GUILayout.BeginVertical(GUI.skin.box);
        
        foreach (string log in _debugLogs)
        {
            GUILayout.Label(log, _logStyle);
        }
        
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(8);
        if (DrawFeatureButton("Clear Logs", false))
        {
            _debugLogs.Clear();
        }
    }

    void DrawESPPreview()
    {
        // Render actual ESP from the game
        GUI.backgroundColor = new Color(0.08f, 0.08f, 0.12f, 1f);
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.Height(380));
        
        Rect previewRect = GUILayoutUtility.GetRect(360, 360);
        
        // Save current GUI state
        Matrix4x4 oldMatrix = GUI.matrix;
        
        // Render the actual ESP here
        if (enable3DBoxESP && Cheat.Instance != null && Cheat.Instance.Cache != null)
        {
            Camera camera = Cheat.Instance.Cache.MainCamera;
            if (camera != null)
            {
                // Draw actual ESP for each player
                foreach (var player in Cheat.Instance.Cache.Players)
                {
                    if (player.GameObject == null || player.Collider == null)
                        continue;
                        
                    Vector3 screenPos = camera.WorldToScreenPoint(player.GameObject.transform.position);
                    if (screenPos.z < 0)
                        continue;
                        
                    screenPos.y = Screen.height - screenPos.y;
                    
                    // Translate to preview area
                    float scale = 0.3f; // Scale down for preview
                    Vector2 previewPos = new Vector2(
                        previewRect.x + previewRect.width / 2 + (screenPos.x - Screen.width / 2) * scale,
                        previewRect.y + previewRect.height / 2 + (screenPos.y - Screen.height / 2) * scale
                    );
                    
                    if (previewPos.x < previewRect.x || previewPos.x > previewRect.xMax ||
                        previewPos.y < previewRect.y || previewPos.y > previewRect.yMax)
                        continue;
                    
                    // Draw box
                    GUI.color = Color.red;
                    GUI.Box(new Rect(previewPos.x - 20, previewPos.y - 30, 40, 60), "");
                    GUI.color = Color.white;
                    
                    // Draw health bar if enabled
                    if (enableHealthBarESP && player.PlayerHealth != null)
                    {
                        float healthPercent = Mathf.Clamp01(player.PlayerHealth.health / 100f);
                        Color barColor = Color.Lerp(Color.red, Color.green, healthPercent);
                        
                        GUI.color = new Color(0, 0, 0, 0.7f);
                        if (healthBarHorizontal)
                            GUI.Box(new Rect(previewPos.x - 20, previewPos.y - 35, 40, 8), "");
                        else
                            GUI.Box(new Rect(previewPos.x + 22, previewPos.y - 30, 8, 60), "");
                        
                        GUI.color = barColor;
                        if (healthBarHorizontal)
                            GUI.Box(new Rect(previewPos.x - 20, previewPos.y - 35, 40 * healthPercent, 8), "");
                        else
                            GUI.Box(new Rect(previewPos.x + 22, previewPos.y - 30 + 60 * (1 - healthPercent), 8, 60 * healthPercent), "");
                        
                        GUI.color = Color.white;
                    }
                }
            }
        }
        
        if (!enable3DBoxESP || Cheat.Instance == null || Cheat.Instance.Cache == null || Cheat.Instance.Cache.Players.Count == 0)
        {
            GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            GUIStyle centeredStyle = new GUIStyle(_logStyle);
            centeredStyle.alignment = TextAnchor.MiddleCenter;
            GUI.Label(previewRect, "Enable ESP and enter game\nto see live preview", centeredStyle);
            GUI.color = Color.white;
        }
        
        // Restore GUI state
        GUI.matrix = oldMatrix;
        
        GUILayout.EndVertical();
        GUI.backgroundColor = Color.white;
    }

    void GetMenuContent()
    {
        switch (currentMenu)
        {
            case "Ragebot":
                AimbotContents();
                break;
            case "Legitbot":
                LegitbotContents();
                break;
            case "Visuals":
                GetVisualsContent();
                break;
            case "Exploits":
                GetExploitsContent();
                break;
            case "Misc":
                GetMiscContent();
                break;
            case "Debug":
                GetDebugContent();
                break;
            default:
                GUILayout.Label("Select a category.", _logStyle);
                break;
            case "AntiAim":
                AntiAimMenu();
                break;
        }
    }

    void AimbotContents()
    {
        GUILayout.Space(10);
        
        if (DrawFeatureButton("Aimbot", Aimbot))
        {
            Aimbot = !Aimbot;
            AddDebugLog($"Aimbot: {(Aimbot ? "ENABLED" : "DISABLED")}");
        }

        if (DrawFeatureButton("Auto Shoot", AutoShoot))
        {
            AutoShoot = !AutoShoot;
            AddDebugLog($"Auto Shoot: {(AutoShoot ? "ENABLED" : "DISABLED")}");
        }
        
        GUILayout.Space(20);
        GUILayout.Label("Keybind Settings", _headerStyle);
        GUILayout.Space(10);
        
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label($"Bound Key: {boundKey_aimbot}", _logStyle);
        
        if (waitingForKey)
        {
            GUI.color = new Color(1f, 0.5f, 0.5f);
            GUILayout.Label("Press any key...", _logStyle);
            GUI.color = Color.white;
            
            Event e = Event.current;
            if (e.isKey)
            {
                boundKey_aimbot = e.keyCode;  
                waitingForKey = false; 
                AddDebugLog($"Set Key to: {boundKey_aimbot}");
            }
        }
        
        if (DrawFeatureButton("Change Keybind", waitingForKey))
        {
            waitingForKey = true;
            AddDebugLog("Waiting for key input...");
        }
        
        GUILayout.EndVertical();
    }

    void GetVisualsContent()
    {
        GUILayout.Space(10);
        
        if (DrawFeatureButton("Enable ESP", enableESP))
        {
            enableESP = !enableESP;
            AddDebugLog($"ESP: {(enableESP ? "ENABLED" : "DISABLED")}");
        }

        if (DrawFeatureButton("3D Box ESP", enable3DBoxESP))
        {
            enable3DBoxESP = !enable3DBoxESP;
            AddDebugLog($"3D Box ESP: {(enable3DBoxESP ? "ENABLED" : "DISABLED")}");
        }

        if (DrawFeatureButton("2D Box ESP", enable2DBoxESP))
        {
            enable2DBoxESP = !enable2DBoxESP;
            AddDebugLog($"2D Box ESP: {(enable2DBoxESP ? "ENABLED" : "DISABLED")}");
        }

        GUILayout.Space(15);
        GUILayout.Label("Health Bar Settings", _headerStyle);
        GUILayout.Space(10);

        if (DrawFeatureButton("Enable Health Bar ESP", enableHealthBarESP))
        {
            enableHealthBarESP = !enableHealthBarESP;
            AddDebugLog($"Health Bar ESP: {(enableHealthBarESP ? "ENABLED" : "DISABLED")}");
        }

        if (enableHealthBarESP)
        {
            GUILayout.Space(8);
            if (DrawFeatureButton("Layout: " + (healthBarHorizontal ? "HORIZONTAL" : "VERTICAL"), healthBarHorizontal))
            {
                healthBarHorizontal = !healthBarHorizontal;
                AddDebugLog($"Health Bar Layout: {(healthBarHorizontal ? "HORIZONTAL" : "VERTICAL")}");
            }
        }
    }

    void LegitbotContents()
    {
        GUILayout.Space(10);
        GUILayout.Label("Legitbot features coming soon...", _logStyle);
    }

    void GetExploitsContent()
    {
        GUILayout.Space(10);
        
        if (DrawFeatureButton("Rapid Fire", RapidFire))
        {
            RapidFire = !RapidFire;
            AddDebugLog($"Rapid Fire: {(RapidFire ? "ENABLED" : "DISABLED")}");
        }

        if (DrawFeatureButton("No Spread", NoSpread))
        { 
            NoSpread = !NoSpread;
            AddDebugLog($"No Spread: {(NoSpread ? "ENABLED" : "DISABLED")}");
        }

        if (DrawFeatureButton("Insta Kill", InstaKill))
        {
            InstaKill = !InstaKill;
            AddDebugLog($"Insta Kill: {(InstaKill ? "ENABLED" : "DISABLED")}");
        }

        if (DrawFeatureButton("Infinite Ammo", InfiniteAmmo))
        {
            InfiniteAmmo = !InfiniteAmmo;
            AddDebugLog($"Infinite Ammo: {(InfiniteAmmo ? "ENABLED" : "DISABLED")}");
        }

        if (DrawFeatureButton("Magic Bullet", MagicBullet))
        {
            MagicBullet = !MagicBullet;
            AddDebugLog($"Magic Bullet: {(MagicBullet ? "ENABLED" : "DISABLED")}");
        }

        if (DrawFeatureButton("Godmode [HOST ONLY]", GodMode))
        {
            GodMode = !GodMode;
            AddDebugLog($"Godmode: {(GodMode ? "ENABLED" : "DISABLED")}");
        }

        if (DrawFeatureButton("Freeze Enemy [HOST ONLY]", FreezeEnemy))
        {
            FreezeEnemy = !FreezeEnemy;
            AddDebugLog($"Freeze Enemy: {(FreezeEnemy ? "ENABLED" : "DISABLED")}");
        }
    }

    void GetMiscContent()
    {
        GUILayout.Space(10);
        
        if (DrawFeatureButton("Weapon Speed", WeaponSpeed))
        {
            WeaponSpeed = !WeaponSpeed;
            AddDebugLog($"Weapon Speed: {(WeaponSpeed ? "ENABLED" : "DISABLED")}");
        }

        GUILayout.Space(15);
        GUILayout.Label($"Speed Value: {speedValue:F1}", _logStyle);
        speedValue = GUILayout.HorizontalSlider(speedValue, 6f, 50f, GUILayout.Height(20));
        GUILayout.Space(10);
        
        if (DrawFeatureButton("Enable Speed Hack", enableSpeedHack))
        {
            enableSpeedHack = !enableSpeedHack;
            if (!enableSpeedHack)
            {
                speedValue = 6f;
                AddDebugLog($"Speed Hack DISABLED - Reset to default (6.0)");
            }
            else
            {
                AddDebugLog($"Speed Hack ENABLED - Value: {speedValue:F1}");
            }
        }

        GUILayout.Space(15);
        
        if (DrawFeatureButton("Fly Mode [Q/E: UP/DOWN]", FlyMode))
        {
            FlyMode = !FlyMode;
            AddDebugLog($"Fly Mode: {(FlyMode ? "ENABLED" : "DISABLED")}");
        }

        GUILayout.Space(15);
        
        if (DrawFeatureButton("Teleport to Enemy [Z]", false))
        {
            AddDebugLog("Teleport executed!");
        }
    }

    void GetDebugContent()
    {
        GUILayout.Space(10);
        
        Vector2 debugScroll = GUILayout.BeginScrollView(_debugScrollPosition, GUILayout.Height(350));
        GUI.backgroundColor = new Color(0.08f, 0.08f, 0.12f, 1f);
        GUILayout.BeginVertical(GUI.skin.box);
        
        foreach (string log in _debugLogs)
        {
            GUILayout.Label(log, _logStyle);
        }
        
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUI.backgroundColor = Color.white;
        
        GUILayout.Space(10);
        
        if (DrawFeatureButton("Clear All Logs", false))
        {
            _debugLogs.Clear();
            AddDebugLog("Logs cleared.");
        }
    }

    void AntiAimMenu()
    {
        GUILayout.Space(10);
        
        if (DrawFeatureButton("IsSlide", enableIsSlide))
        {
            enableIsSlide = !enableIsSlide;
            AddDebugLog($"IsSlide: {(enableIsSlide ? "ENABLED" : "DISABLED")}");
        }
        
        if (DrawFeatureButton("IsGrounded", enableIsGrounded))
        {
            enableIsGrounded = !enableIsGrounded;
            AddDebugLog($"IsGrounded: {(enableIsGrounded ? "ENABLED" : "DISABLED")}");
        }
        
        if (DrawFeatureButton("IsSprinting", enableIsSprinting))
        {
            enableIsSprinting = !enableIsSprinting;
            AddDebugLog($"IsSprinting: {(enableIsSprinting ? "ENABLED" : "DISABLED")}");
        }
        
        if (DrawFeatureButton("IsScopeAiming", enableIsScopeAiming))
        {
            enableIsScopeAiming = !enableIsScopeAiming;
            AddDebugLog($"IsScopeAiming: {(enableIsScopeAiming ? "ENABLED" : "DISABLED")}");
        }
        
        if (DrawFeatureButton("IsLeaning", enableIsLeaning))
        {
            enableIsLeaning = !enableIsLeaning;
            AddDebugLog($"IsLeaning: {(enableIsLeaning ? "ENABLED" : "DISABLED")}");
        }
    }
    public void AddDebugLog(string message)
    {
        _debugLogs.Insert(0, $"[{Time.time:F1}] {message}");
        if (_debugLogs.Count > MAX_LOGS)
            _debugLogs.RemoveAt(_debugLogs.Count - 1);
    }
}
