using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Serializing;
using FishNet;
using UnityEngine.SocialPlatforms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using static Mono.Security.X509.X520;
using DG.Tweening;
using STRAFTAT_CC;

namespace STRAFTAT_CC.Features
{
    public class Aimbot
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        public void FlyMode(FirstPersonController playerShootInstance, bool type)
        {
            try
            {
                var flyModeField = typeof(FirstPersonController).GetField("flymode", BindingFlags.NonPublic | BindingFlags.Instance);

                if (flyModeField != null)
                {
                    flyModeField.SetValue(playerShootInstance, type);

                    Config.Instance.AddDebugLog(type ? "Fly mode Enabled." : "Fly mode Disabled.");
                }
                else
                {
                    Config.Instance.AddDebugLog("Error: flymode field not found.");
                }
            }
            catch (Exception ex)
            {
                Config.Instance.AddDebugLog("Error setting flymode field via reflection: " + ex.Message);
            }
        }

        public void PlayHitMarker(Weapon _weapon)
        {
            try
            {
                _weapon.marker = UnityEngine.Object.Instantiate<GameObject>(_weapon.hitMarker, Crosshair.Instance.transform.position, Quaternion.identity, PauseManager.Instance.transform);
                _weapon.marker.transform.DOPunchScale(new Vector3(2.5f, 2.5f, 2.5f), 0.3f, 8, 2f);
                _weapon.marker.GetComponent<UnityEngine.UI.Image>().color = Color.red;
                UnityEngine.Object.Destroy(_weapon.marker, 0.3f);
                _weapon.audio.PlayOneShot(_weapon.headHitClip);
            }
            catch (Exception ex)
            {
                Config.Instance.AddDebugLog("Error hitmarker" + ex.Message);
            }
        }

        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;

        private readonly Cache _cache;
        private bool _isEnabled = false;
        private const float HEAD_OFFSET = 1.5f;
        private bool _isMouseHold = false;
        private bool _previousFlyMode = false;
        private bool _previousFreezeEnemy = false;


        public bool IsEnabled => _isEnabled;

        public Aimbot(Cache cache)
        {
            _cache = cache;
        }

        public void Update()
        {



            if (Config.Instance.MagicBullet && _cache.LocalWeaponRight != null && _cache.LocalWeaponRight.fire1.IsPressed())
            {
                var enemyHealthController = GetClosestTarget();
                if (enemyHealthController != null && enemyHealthController.PlayerHealth.health > 0)
                {
                    PlayHitMarker(_cache.LocalWeaponRight);
                    enemyHealthController.PlayerHealth.ChangeKilledState(true);
                    enemyHealthController.PlayerHealth.RemoveHealth(10f);
                    enemyHealthController.PlayerHealth.SetKiller(_cache.LocalWeaponLeft.transform);
                }
            }

            if (Config.Instance.FlyMode != _previousFlyMode)
            {
                var playerController = _cache?.LocalController;

                if (playerController != null)
                {


                    FlyMode(playerController, Config.Instance.FlyMode);
                }
                else
                {
                    Config.Instance.AddDebugLog("Error: Player controller not found.");

                }

                _previousFlyMode = Config.Instance.FlyMode;
            }



            if (Config.Instance.FreezeEnemy != _previousFreezeEnemy)
            {
                var enemy = GetClosestTarget();
                if (enemy != null && enemy.PlayerHealth.health > 0)
                    enemy.PlayerHealth.controller.sync___set_value_canMove(!enemy.PlayerHealth.controller.sync___get_value_canMove(), _cache.LocalPlayer.PlayerHealth.IsHost);
                _previousFreezeEnemy = Config.Instance.FreezeEnemy;
            }


            if (Input.GetKey(Config.Instance.boundKey_aimbot)) // Holding key
            {
                if (!Config.Instance.Aimbot)
                {
                    Config.Instance.Aimbot = true;
                    Config.Instance.AddDebugLog("Aimbot: Activated");
                }
                AimAtClosestPlayer(); // Call aimbot function while holding
            }
            else
            {
                if (Config.Instance.Aimbot)
                {
                    Config.Instance.Aimbot = false;
                    Config.Instance.AddDebugLog("Aimbot: Deactivated");
                    MouseRelease(); // Release mouse if it was held
                }
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                PlayerCache _closest = GetClosestTarget();
                _cache.LocalController.Teleport(_closest.HeadTransform.position, 0f, false, _closest.HeadTransform, 1, 1, false);
                _closest.PlayerHealth.sync___set_value_health(100f, true);

            }

            if (!Config.Instance.Aimbot || !_cache.LocalPlayer.IsValid || !_cache.MainCamera)
            {
                return;
            }
            AimAtClosestPlayer();
        }



        public void SimulateLeftMouseClick(GameObject targetObject)
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
                position = Input.mousePosition
            };

            ExecuteEvents.Execute(targetObject, pointer, ExecuteEvents.pointerClickHandler);

            Debug.Log("Simulated left mouse click on " + targetObject.name);
        }


        public static void MouseHold()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        }

        public static void MouseRelease()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }



        private void AimAtClosestPlayer()
        {
            PlayerCache closestPlayer = GetClosestTarget();
            if (closestPlayer == null || closestPlayer.PlayerHealth.health <= 0) return;

            Vector3 targetPosition;
            if (closestPlayer.HeadTransform != null)
            {
                targetPosition = closestPlayer.HeadTransform.position;
            }
            else
            {
                targetPosition = closestPlayer.GameObject.transform.position + Vector3.up * HEAD_OFFSET;
            }

            Vector3 direction = (targetPosition - _cache.MainCamera.transform.position).normalized;

            if (Config.Instance.AutoShoot)
            {
                RaycastHit hit;
                if (Physics.Raycast(_cache.MainCamera.transform.position, direction, out hit))
                {

                    Transform rootTransform = hit.transform;
                    while (rootTransform.parent != null)
                    {
                        rootTransform = rootTransform.parent;
                    }

                    PlayerHealth hitPlayerHealth = hit.transform.GetComponentInParent<PlayerHealth>();
                    if (hitPlayerHealth != null)
                    {
                        if (hitPlayerHealth.gameObject == closestPlayer.GameObject && closestPlayer.PlayerHealth.health > 0)
                        {
                            Config.Instance.AddDebugLog("Aimbot: Hit correct player");
                            MouseHold();
                            _isMouseHold = true;
                        }
                        else
                        {
                            MouseRelease();
                            _isMouseHold = false;
                        }
                    }
                }
            }
            else
            {
                MouseRelease();
                _isMouseHold = false;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _cache.LocalController.transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            float targetPitch = targetRotation.eulerAngles.x;
            if (targetPitch > 180) targetPitch -= 360;
            targetPitch = Mathf.Clamp(targetPitch, -89f, 89f);

            _cache.MainCamera.transform.rotation = Quaternion.Euler(
            targetPitch,
            targetRotation.eulerAngles.y,
            0
            );

        }

        public PlayerCache GetClosestTarget()
        {
            float closestDistance = float.MaxValue;
            PlayerCache closestPlayer = null;

            foreach (PlayerCache player in _cache.Players)
            {
                if (!player.IsValid)
                    continue;

                float distance = Vector3.Distance(_cache.LocalPlayer.GameObject.transform.position, player.GameObject.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = player;
                }
            }

            return closestPlayer;
        }
    }
}