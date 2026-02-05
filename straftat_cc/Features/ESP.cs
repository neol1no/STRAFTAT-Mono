using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace STRAFTAT_CC.Features
{
    public class ESP
    {
        public static void OnGUI()
        {
            if (Config.Instance.enable3DBoxESP)
            {
                foreach (var player in Cheat.Instance.Cache.Players)
                    player.Draw(Cheat.Instance.Cache.MainCamera);
            }

            if (Config.Instance.enableHealthBarESP)
            {
                DrawHealthBars();
            }
        }

        private static void DrawHealthBars()
        {
            Camera camera = Cheat.Instance.Cache.MainCamera;
            if (camera == null)
                return;

            foreach (var player in Cheat.Instance.Cache.Players)
            {
                if (player.GameObject == null || player.PlayerHealth == null)
                    continue;

                Vector3 screenPos = camera.WorldToScreenPoint(player.GameObject.transform.position);
                if (screenPos.z < 0)
                    continue;

                screenPos.y = Screen.height - screenPos.y;

                // Calculate health percentage
                float healthPercent = Mathf.Clamp01(player.PlayerHealth.health / 100f);
                
                // Get color based on health
                Color barColor = GetHealthColor(healthPercent);

                // Bar dimensions
                float barWidth = Config.Instance.healthBarHorizontal ? 50f : 12f;
                float barHeight = Config.Instance.healthBarHorizontal ? 12f : 50f;

                // Draw background (dark)
                GUI.color = new Color(0, 0, 0, 0.7f);
                GUI.Box(new Rect(screenPos.x - barWidth / 2, screenPos.y - 35, barWidth, barHeight), "");

                // Draw health bar
                GUI.color = barColor;
                if (Config.Instance.healthBarHorizontal)
                {
                    GUI.Box(new Rect(screenPos.x - barWidth / 2, screenPos.y - 35, barWidth * healthPercent, barHeight), "");
                }
                else
                {
                    GUI.Box(new Rect(screenPos.x - barWidth / 2, screenPos.y - 35 + barHeight * (1 - healthPercent), barWidth, barHeight * healthPercent), "");
                }

                // Draw border
                GUI.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                GUI.Box(new Rect(screenPos.x - barWidth / 2 - 1, screenPos.y - 36, barWidth + 2, barHeight + 2), "");

                GUI.color = Color.white;
            }
        }

        private static Color GetHealthColor(float healthPercent)
        {
            // Green when healthy, yellow when medium, red when low
            if (healthPercent > 0.5f)
            {
                // Green to Yellow (100% to 50%)
                return Color.Lerp(Color.yellow, Color.green, (healthPercent - 0.5f) * 2f);
            }
            else
            {
                // Yellow to Red (50% to 0%)
                return Color.Lerp(Color.red, Color.yellow, healthPercent * 2f);
            }
        }
    }
}

