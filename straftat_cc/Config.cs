using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

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


    // Visuals
    public bool enableESP = true;
    public bool enable3DBoxESP = true;
    public bool enable2DBoxESP = false;

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
    private GUIStyle _toggleStyle;
    private GUIStyle _logStyle;
    private GUIStyle _panelStyle;

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

    private void InitializeStyles()
    {
        if (_windowStyle == null)
        {
            _windowStyle = new GUIStyle(GUI.skin.window);
            _windowStyle.normal.background = MakeTexture(1, 1, new Color(0.1f, 0.1f, 0.1f, 0.95f));
            _windowStyle.normal.textColor = Color.white;
            _windowStyle.fontSize = 14;
            _windowStyle.fontStyle = FontStyle.Bold;
            _headerStyle = new GUIStyle(GUI.skin.button);
            _headerStyle.normal.background = MakeTexture(2, 2, new Color(0.2f, 0.2f, 0.2f, 1));
            _headerStyle.hover.background = MakeTexture(2, 2, new Color(0.25f, 0.25f, 0.25f, 1));
            _headerStyle.normal.textColor = Color.magenta;
            _headerStyle.fontSize = 12;
            _headerStyle.fontStyle = FontStyle.Bold;
            _headerStyle.alignment = TextAnchor.MiddleLeft;
            _headerStyle.padding = new RectOffset(10, 10, 5, 5);
            _toggleStyle = new GUIStyle(GUI.skin.toggle);
            _toggleStyle.normal.textColor = Color.white;
            _toggleStyle.onNormal.textColor = Color.white;
            _toggleStyle.hover.textColor = Color.white;
            _toggleStyle.onHover.textColor = Color.white;
            _toggleStyle.fontSize = 12;
            _toggleStyle.padding = new RectOffset(20, 5, 5, 5);
            _toggleStyle.margin = new RectOffset(5, 5, 5, 5);
            _buttonStyle = new GUIStyle(GUI.skin.button);
            _buttonStyle.normal.background = MakeTexture(2, 2, new Color(0.2f, 0.2f, 0.2f, 1));
            _buttonStyle.hover.background = MakeTexture(2, 2, new Color(0.3f, 0.3f, 0.3f, 1));
            _buttonStyle.normal.textColor = Color.white;
            _buttonStyle.fontSize = 12;
            _logStyle = new GUIStyle(GUI.skin.label);
            _logStyle.normal.textColor = Color.white;
            _logStyle.fontSize = 11;
            _logStyle.wordWrap = true;
        }
    }

    private string currentMenu = "Aimbot";

    public void Draw()
    {
        InitializeStyles();

        int menuWidth = 200;

        GUILayout.BeginHorizontal(GUI.skin.box);

        // left
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(menuWidth));
        if (GUILayout.Button("Aimbot", _headerStyle)) currentMenu = "Aimbot";
        if (GUILayout.Button("Visuals", _headerStyle)) currentMenu = "Visuals";
        if (GUILayout.Button("Exploits", _headerStyle)) currentMenu = "Exploits";
        if (GUILayout.Button("Misc", _headerStyle)) currentMenu = "Misc";
        if (GUILayout.Button("AntiAim", _headerStyle)) currentMenu = "AntiAim";
        GUILayout.EndVertical();

        // middle
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(400));
        GUILayout.Label("TAB: " + currentMenu, _headerStyle);
        GUILayout.Label(GetMenuContent(), _logStyle);
        GUILayout.EndVertical();

        // right (debug box)
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(400));
        _debugScrollPosition = GUILayout.BeginScrollView(_debugScrollPosition, GUILayout.Height(400));
        foreach (string log in _debugLogs)
        {
            GUILayout.Label(log, _logStyle);
        }
        GUILayout.EndScrollView();
        if (GUILayout.Button("Clear Logs", _buttonStyle))
        {
            _debugLogs.Clear();
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }

    string GetMenuContent()
    {
        switch (currentMenu)
        {
            case "Aimbot":
                return AimbotContents();
            case "Visuals":
                return GetVisualsContent();
            case "Exploits":
                return GetExploitsContent();
            case "Misc":
                return GetMiscContent();
            default:
                return "Select a category.";
            case "AntiAim":
                return AntiAimMenu();
        }
    }

    string AimbotContents()
    {
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(400));
        if (GUILayout.Button("Aimbot", _buttonStyle))
            Aimbot = !Aimbot;

        if (GUILayout.Button("Enable Auto Shoot", _buttonStyle))
        {
            AutoShoot = !AutoShoot;
            AddDebugLog($"Auto Shoot: {AutoShoot}");
        }
        
        GUILayout.Label("Bound Key: " + boundKey_aimbot);
        GUILayout.Label("Waiting for Key: " + waitingForKey);
        
        if (GUILayout.Button("Press Key after Button", _buttonStyle))
        {
            waitingForKey = true;  
        }
        if (waitingForKey)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                boundKey_aimbot = e.keyCode;  
                waitingForKey = false; 
                AddDebugLog($"Set Key to: {boundKey_aimbot}");
            }
        }
        GUILayout.EndVertical();
        return "";
    }

    string GetVisualsContent()
    {
        // Visual Toggles : borken
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(400));
        if (GUILayout.Button("Enable ESP", _buttonStyle))
            enableESP = !enableESP;

        if (GUILayout.Button("Enable 3D Box ESP", _buttonStyle))
            enable3DBoxESP = !enable3DBoxESP;
        GUILayout.EndVertical();
        return "";  
    }

    string GetExploitsContent()
    {
        // Exploits Toggles
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(400));
        if (GUILayout.Button("Rapid Fire", _buttonStyle))
        {
            RapidFire = !RapidFire;
            AddDebugLog($"Rapid Fire: {RapidFire}");
        }

        if (GUILayout.Button("No Spread", _buttonStyle))
        { 
            NoSpread = !NoSpread;
            AddDebugLog($"No Spread: {NoSpread}");
        }

        if (GUILayout.Button("Insta Kill", _buttonStyle))
        {
            InstaKill = !InstaKill;
            AddDebugLog($"Insta Kill: {InstaKill}");
        }

        if (GUILayout.Button("Infinite Ammo", _buttonStyle))
        {
            InfiniteAmmo = !InfiniteAmmo;
            AddDebugLog($"Infinite Ammo: {InfiniteAmmo}");
        }

        if (GUILayout.Button("Enable Magic Bullet", _buttonStyle))
        {
            MagicBullet = !MagicBullet;
            AddDebugLog($"Magic Bullet: {MagicBullet}");
        }

        if (GUILayout.Button("Enable Godmode [HOST]", _buttonStyle))
        {
            GodMode = !GodMode;
            AddDebugLog($"Godmode: {GodMode}");
        }

        if (GUILayout.Button("Freeze Enemy [HOST]", _buttonStyle))
        {
            FreezeEnemy = !FreezeEnemy;
            AddDebugLog($"Freeze Enemy: {FreezeEnemy}");
        }

        GUILayout.EndVertical();
        return "";  
    }

    string GetMiscContent()
    {
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(400));
        GUILayout.Label("Weapon Hold Speed", _logStyle);
        if (GUILayout.Button("WeaponSpeed", _buttonStyle))
        {
            WeaponSpeed = !WeaponSpeed;
            AddDebugLog($"Weapon Speed: {WeaponSpeed}");
        }

        GUILayout.Label("Speed", _logStyle);
        speedValue = GUILayout.HorizontalSlider(speedValue, 6f, 50f);
        if (GUILayout.Button("Enable Speed Hack", _buttonStyle))
        {
            enableSpeedHack = !enableSpeedHack;
            AddDebugLog($"Speed Hack: {enableSpeedHack}");
        }

        GUILayout.Label("Fly [UP / DOWN : Q / E]", _logStyle);
        if (GUILayout.Button("Enable Fly Hack", _buttonStyle))
        {
            FlyMode = !FlyMode;
            AddDebugLog($"Fly Hack: {FlyMode}");
        }

        GUILayout.Label("Teleport to Enemy:", _logStyle);
        if (GUILayout.Button("Teleport [Z]", _buttonStyle))
        {
            AddDebugLog("Teleported");
        }
        GUILayout.EndVertical();
        return "";  
    }

    string AntiAimMenu()
    {
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(400));
        if (GUILayout.Button("IsSlide", _buttonStyle))
        {
            enableIsSlide = !enableIsSlide;
            AddDebugLog($"IsSlide: {enableIsSlide}");
        }
        /*
        if (GUI.Button(new Rect(250, 100, 250, 20), $"IsGrounded: {enableIsGrounded}"))
            enableIsGrounded = !enableIsGrounded;
        if (GUI.Button(new Rect(250, 150, 250, 20), $"IsSprinting: {enableIsSprinting}"))
            enableIsSprinting = !enableIsSprinting;
        if (GUI.Button(new Rect(250, 200, 250, 20), $"IsScopeAiming: {enableIsScopeAiming}"))
            enableIsScopeAiming = !enableIsScopeAiming;
        if (GUI.Button(new Rect(250, 250, 250, 20), $"IsLeaning: {enableIsLeaning}"))
            enableIsLeaning = !enableIsLeaning;
        */
        GUILayout.EndVertical();
        return "";
    }
    public void AddDebugLog(string message)
    {
        _debugLogs.Insert(0, $"[{Time.time:F1}] {message}");
        if (_debugLogs.Count > MAX_LOGS)
            _debugLogs.RemoveAt(_debugLogs.Count - 1);
    }
}
