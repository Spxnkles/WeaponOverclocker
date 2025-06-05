using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Unturned.Events;
using SDG.Unturned;
using Logger = Rocket.Core.Logging.Logger;
using Rocket.Unturned.Player;
using UnityEngine;
using Random = System.Random;

namespace WeaponOverclocker
{
    public class Main : RocketPlugin<Configuration>
    {
        private string Version = "1.0.5";

        public static Main Instance;
        public static Configuration Config;

        protected override void Load()
        {
            Instance = this;
            Config = Instance.Configuration.Instance;

            UnturnedEvents.OnPlayerDamaged += BulletHit;

            Logger.Log($"============================================================");
            Logger.Log($"Successfully loaded WeaponOverclocker version {Version}");
            Logger.Log($"This plugin is made by Spinkles");
            Logger.Log($"Discord Server: https://discord.gg/XCeG4MySZc");
            Logger.Log($"============================================================");
        }

        protected override void Unload()
        {
            UnturnedEvents.OnPlayerDamaged -= BulletHit;

            Logger.Log($"============================================================");
            Logger.Log($"Unloaded WeaponOverclocker version {Version}");
            Logger.Log($"This plugin is made by Spinkles");
            Logger.Log($"============================================================");
        }

        private void BulletHit(UnturnedPlayer player, ref EDeathCause cause, ref ELimb limb, ref UnturnedPlayer killer, ref Vector3 direction, ref float damage, ref float times, ref bool canDamage)
        {

            if (player == null || killer == null || killer.Player.equipment == null || killer == player)
            {
                if (Config.Debug) Logger.Log($"BulletHit null");
                return;
            }

            UnturnedPlayer plr = killer;

            List<ushort> attachmentList = new List<ushort>();
            List<DamageMultiplier> damageMultipliers = Config.DamageMultipliers;

            if (Config.Debug) Logger.Log($"Weapon: {plr.Player.equipment.itemID}");

            if (plr.Player.equipment.itemID == 0) return;

            Attachments.parseFromItemState(plr.Player.equipment.state, out ushort sight, out ushort tactical, out ushort grip, out ushort barrel, out ushort magazine);

            attachmentList.Add(sight);
            attachmentList.Add(tactical);
            attachmentList.Add(grip);
            attachmentList.Add(barrel);
            attachmentList.Add(magazine);

            if (Config.Debug)
            {
                Logger.Log($"Number of equipped attachments: {attachmentList.Count}");

                foreach (ushort attachment in attachmentList)
                {
                    Logger.Log($"{attachment}");
                }
            }


            foreach (DamageMultiplier damageMultiplierAttachment in damageMultipliers)
            {
                if (attachmentList.Contains(damageMultiplierAttachment.AttachmentID))
                {
                    if (Config.Debug) Logger.Log($"Attachment {damageMultiplierAttachment.AttachmentID} is on the gun.");
                    if (Config.Debug) Logger.Log($"Running percentage chance.");

                    Random random = new Random();

                    int rndm = random.Next(101);
                    if (rndm < damageMultiplierAttachment.ChancePercentage)
                    {
                        if (Config.Debug) Logger.Log($"{rndm} < {damageMultiplierAttachment.ChancePercentage} = true");

                        float damageAddition = (damage * damageMultiplierAttachment.Multiplier) - damage;

                        if (Config.Debug) Logger.Log($"Original damage: {damage}");
                        if (Config.Debug) Logger.Log($"Multiplier: {damageMultiplierAttachment.Multiplier}x");
                        if (Config.Debug) Logger.Log($"Additional damage: {damageAddition}");

                        DamageTool.damage(player.Player, cause, limb, killer.CSteamID, direction, damageAddition, 1, out _, true, true);

                        EffectManager.sendUIEffect(Config.SoundEffectID, 879, plr.Player.channel.owner.transportConnection, true);
                    }
                    else if (Config.Debug) Logger.Log($"{rndm} > {damageMultiplierAttachment.ChancePercentage} = false");
                }
                else
                {
                    if (Config.Debug) Logger.Log($"Attachment {damageMultiplierAttachment.AttachmentID} was not on the gun.");
                }
            }
        }
    }
}
