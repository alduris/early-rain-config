using System.Security.Permissions;
using BepInEx;
using BepInEx.Logging;
using Menu;
using UnityEngine;

// Allows access to private members
#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace EarlyRainConfig
{
    [BepInPlugin("alduris.earlyrainconfig", "Early Rain Config", "1.0")]
    internal sealed class Plugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger;

        public void OnEnable()
        {
            Logger = base.Logger;
            On.Menu.ArenaSettingsInterface.ctor += ArenaSettingsInterface_ctor;
            On.ArenaSetup.GameTypeSetup.InitAsGameType += GameTypeSetup_InitAsGameType;
        }

        private static void GameTypeSetup_InitAsGameType(On.ArenaSetup.GameTypeSetup.orig_InitAsGameType orig, ArenaSetup.GameTypeSetup self, ArenaSetup.GameTypeID gameType)
        {
            bool rainWhenOnePlayerLeft = self.rainWhenOnePlayerLeft;
            orig(self, gameType);
            if (gameType == ArenaSetup.GameTypeID.Competitive)
            {
                self.rainWhenOnePlayerLeft = rainWhenOnePlayerLeft;
            }
        }

        private static void ArenaSettingsInterface_ctor(On.Menu.ArenaSettingsInterface.orig_ctor orig, Menu.ArenaSettingsInterface self, Menu.Menu menu, MenuObject owner)
        {
            orig(self, menu, owner);
            if (self.GetGameTypeSetup.gameType == ArenaSetup.GameTypeID.Competitive)
            {
                self.earlyRainCheckbox = new CheckBox(menu, self, self, self.evilAICheckBox.pos + new Vector2(0f, 30f), ArenaSettingsInterface.GetEarlyRainCheckboxWidth(menu.CurrLang), menu.Translate("Early Rain:"), "EARLYRAIN", false);
                self.subObjects.Add(self.earlyRainCheckbox);
            }
        }
    }
}
