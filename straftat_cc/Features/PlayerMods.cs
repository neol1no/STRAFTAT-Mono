using UnityEngine;
using System.Reflection;
using HeathenEngineering.SteamworksIntegration.API;

namespace STRAFTAT_CC.Features
{
    public class PlayerMods
    {
        // Reflection fields
        private static FieldInfo walkSpeedField = typeof(FirstPersonController).GetField("walkSpeed", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo sprintSpeedField = typeof(FirstPersonController).GetField("sprintSpeed", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo jumpForceField = typeof(FirstPersonController).GetField("jumpForce", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo crouchSpeedField = typeof(FirstPersonController).GetField("crouchSpeed", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo gravityField = typeof(FirstPersonController).GetField("gravity", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo canMoveField = typeof(FirstPersonController).GetField("canMove", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo canJumpField = typeof(FirstPersonController).GetField("canJump", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo canCrouchField = typeof(FirstPersonController).GetField("canCrouch", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo jumpFactorField = typeof(FirstPersonController).GetField("jumpFactor", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo moveDirectionField = typeof(FirstPersonController).GetField("moveDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        private static FieldInfo coyoteTimerField = typeof(FirstPersonController).GetField("coyoteTimer", BindingFlags.NonPublic | BindingFlags.Instance);

        // Speed Hack
        private static void SpeedHack()
        {
            var controller = Cheat.Instance.Cache.LocalController;
            if (controller != null && walkSpeedField != null)
            {
                float speedValue = Config.Instance.speedValue;
                walkSpeedField.SetValue(controller, speedValue);
                sprintSpeedField?.SetValue(controller, speedValue * 1.5f);
                Debug.Log($"[SpeedHack] WalkSpeed set to {speedValue}");
            }
        }

        private void GodMode(PlayerHealth _localPlayer)
        {

            _localPlayer.sync___set_value_health(100f, _localPlayer.IsHost);
        }

        private void GodModeEnemy(PlayerHealth _enemyPlayer)
        {

            _enemyPlayer.sync___set_value_health(100f, true);
        }

        private readonly Cache _cache;


        private static void AntiAimMenu()
        {
            var controller = Cheat.Instance.Cache.LocalController;

            if (Config.Instance.enableIsSlide) 
            {
                controller.isSliding = true;
            }
        }

        // Update Method
        public void Update()
        {
            PlayerCache _player = Cheat.Instance.Cache.LocalPlayer;

            if (Config.Instance.GodMode)
            {
                GodMode(_player.PlayerHealth);
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.F5))
            {
                _cache.Settings.rocketJumpsHatAch.Unlock();
                _cache.Settings.windowsBrokenHatAch.Unlock();
                _cache.Settings.headshotHatAch.Unlock();
                _cache.Settings.ragdollsThrownAwayHatAch.Unlock();
                _cache.Settings.taserShotsHatAch.Unlock();
                _cache.Settings.noscopeHatAch.Unlock();
                _cache.Settings.potsBrokenHatAch.Unlock();
                _cache.Settings.fiveGamesHatAch.Unlock();
                _cache.Settings.killsHatAch.Unlock();
                _cache.Settings.propKillsHatAch.Unlock();
                StatsAndAchievements.Client.StoreStats();
                _cache.Settings.SteamAchievementsCheck();
            }
            AntiAimMenu();
            SpeedHack();
           
        }
    }
}
