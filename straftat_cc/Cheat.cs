
using STRAFTAT_CC.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace STRAFTAT_CC
{
    public class Cheat : MonoBehaviour
    {
        private Cache _cache = new Cache(1);
        private PlayerMods _playermods = new PlayerMods();
        private Vector2 _watermarkPos = new Vector2(10, 10);

        private bool _menuOpen = true;
        private Rect _windowRect = new Rect(50, 50, 1100, 520);
        private GUIStyle _customWindowStyle;
        
        public Cache Cache { get => _cache; }
        public PlayerMods PlayerMods { get => _playermods; }
        public static Cheat Instance { get; private set; }

        private void Awake()
        {


            if (Instance != null)
                Destroy(this);
            else
                Instance = this;

            Instance = this;
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Insert))
                _menuOpen = !_menuOpen;

            Cache.Update();

            WeaponMods.Update();
            PlayerMods.Update();

        }

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

        private Texture2D MakeRoundedTexture(int width, int height, Color color, int cornerRadius = 10)
        {
            Color[] pixels = new Color[width * height];
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    
                    // Check if in corner region
                    bool inCorner = false;
                    float distToCorner = 0f;
                    
                    // Top-left corner
                    if (x < cornerRadius && y < cornerRadius)
                    {
                        distToCorner = Vector2.Distance(new Vector2(x, y), new Vector2(cornerRadius, cornerRadius));
                        inCorner = distToCorner > cornerRadius;
                    }
                    // Top-right corner
                    else if (x > width - cornerRadius && y < cornerRadius)
                    {
                        distToCorner = Vector2.Distance(new Vector2(x, y), new Vector2(width - cornerRadius, cornerRadius));
                        inCorner = distToCorner > cornerRadius;
                    }
                    // Bottom-left corner
                    else if (x < cornerRadius && y > height - cornerRadius)
                    {
                        distToCorner = Vector2.Distance(new Vector2(x, y), new Vector2(cornerRadius, height - cornerRadius));
                        inCorner = distToCorner > cornerRadius;
                    }
                    // Bottom-right corner
                    else if (x > width - cornerRadius && y > height - cornerRadius)
                    {
                        distToCorner = Vector2.Distance(new Vector2(x, y), new Vector2(width - cornerRadius, height - cornerRadius));
                        inCorner = distToCorner > cornerRadius;
                    }
                    
                    pixels[index] = inCorner ? Color.clear : color;
                }
            }

            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        private void InitWindowStyle()
        {
            if (_customWindowStyle == null)
            {
                _customWindowStyle = new GUIStyle(GUI.skin.box);
                _customWindowStyle.normal.background = MakeRoundedTexture(64, 64, new Color(0.12f, 0.12f, 0.15f, 1f), 8);
                _customWindowStyle.border = new RectOffset(8, 8, 8, 8);
                _customWindowStyle.padding = new RectOffset(6, 6, 6, 6);
                _customWindowStyle.normal.textColor = Color.clear;
            }
        }

        private void Menu(int id)
        {
            Config.Instance.Draw();
            GUI.DragWindow();
        }

        private void OnGUI()
        {
            if (_menuOpen)
            {
                InitWindowStyle();
                _windowRect = GUI.Window(0, _windowRect, Menu, "", _customWindowStyle);
            }

            ESP.OnGUI();
        }
    }
}
