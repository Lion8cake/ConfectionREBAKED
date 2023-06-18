using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Util;
using TheConfectionRebirth.Items.Weapons;
using System.Reflection.Metadata;
using TheConfectionRebirth.Projectiles;
using Terraria.Localization;
using Terraria.Audio;

namespace TheConfectionRebirth {
	public class ConfectionPlayer : ModPlayer
    {
        public bool RollerCookiePet;
        public bool CreamsandWitchPet;
        public bool minitureCookie;
        public bool littleMeawzer;
        public bool miniGastropod;
        public bool flyingGummyFish;
        public bool birdnanaLightPet;
        public bool MeawzerPet;
        public bool DudlingPet;
        public bool FoxPet;
        public bool NeapoliniteMagicSet;
        public bool NeapoliniteSummonerSet;
        public bool cookiePet;

        public Projectile DimensionalWarp;
        public Projectile BananawarpPeelWarp;

        public float neapoliniteSummonTimer;

        public BinaryHeap<TimerData> Timer;
        public int VanillaValorDamageDealt;
        public int ManaConsumed;
        public bool StrawberryStrikeOnCooldown;

        public override void OnEnterWorld()
        {
            Timer = new(TimerData.Comparer);
            VanillaValorDamageDealt = 0;
            ManaConsumed = 0;
            StrawberryStrikeOnCooldown = false;
        }
        public override void ResetEffects()
        {
            RollerCookiePet = false;
            CreamsandWitchPet = false;
            minitureCookie = false;
            littleMeawzer = false;
            miniGastropod = false;
            flyingGummyFish = false;
            birdnanaLightPet = false;
            MeawzerPet = false;
            DudlingPet = false;
            FoxPet = false;
            NeapoliniteMagicSet = false;
            NeapoliniteSummonerSet = false;
            cookiePet = false;
        }

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (damageSource.SourceCustomReason == "DimensionSplit")
            {
                WeightedRandom<string> deathmessage = new();
                deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.DimensionSplit.0", Player.name));
                deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.DimensionSplit.1", Player.name));
                deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.DimensionSplit.2", Player.name));
                deathmessage.Add(Language.GetTextValue("Mods.TheConfectionRebirth.PlayerDeathReason.DimensionSplit.3", Player.name));
                damageSource = PlayerDeathReason.ByCustomReason(deathmessage);
                return true;
            }
			return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
		}

		public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            bool inWater = !attempt.inLava && !attempt.inHoney;
            bool inConfectionSurfaceBiome = Player.InModBiome(ModContent.GetInstance<ConfectionBiome>());

            if (!inWater || !inConfectionSurfaceBiome) {
                return;
            }

            if (Main.hardMode && Main.rand.NextBool(50)) {
				itemDrop = ModContent.ItemType<Items.Weapons.Minions.DuchessPrincess.GummyStaff>();
			}
            else if (attempt.rare && !attempt.veryrare && !attempt.legendary) {
				itemDrop = !Main.hardMode
                    ? ModContent.ItemType<Items.Placeable.BananaSplitCrate>()
                    : ModContent.ItemType<Items.Placeable.ConfectionCrate>();
			}
        }
        const int oneStageNeapolioniteSummoner = 8 * 60;
        public override void PostUpdate()
        {
            if (NeapoliniteSummonerSet)
            {
                neapoliniteSummonTimer++;
                float progress = neapoliniteSummonTimer / oneStageNeapolioniteSummoner;
                int rank = (int)progress;
                int timer = (int)(oneStageNeapolioniteSummoner - neapoliniteSummonTimer % oneStageNeapolioniteSummoner);
                StackableBuffData.SwirlySwarm.AscendBuff(Player, rank - 1, timer, rank == 5);
                if (neapoliniteSummonTimer >= 2400)
                {
                    neapoliniteSummonTimer = 2400;
                }
                if (NeapoliniteSummonerSet == false)
                {
                    neapoliniteSummonTimer = 0;
                }
            }

            Timer ??= new(TimerData.Comparer);
            while (Timer.items.Count > 0 && Timer.items[0].endTime == Main.GameUpdateCount)
            {
                TimerData top = Timer.Pop();
                switch (top.type)
                {
                    case TimerDataType.MeleeDamage:
                        VanillaValorDamageDealt -= top.value;
                        break;
                    case TimerDataType.MagicManaRegeneration:
                        ManaConsumed -= top.value;
                        break;
                    case TimerDataType.StrawberryStrikeDelay:
                        StrawberryStrikeOnCooldown = false;
                        break;
                }
            }
            //Main.NewText(VanillaValorDamageDealt);
        }

        public override void OnConsumeMana(Item item, int manaConsumed)
        {
            if (item.CountsAsClass(DamageClass.Magic))
            {
                ManaConsumed += manaConsumed;
                Timer.Add(new(manaConsumed, 180, TimerDataType.MagicManaRegeneration));
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (item.CountsAsClass(DamageClass.Melee))
                AddDamage(hit.Damage);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.CountsAsClass(DamageClass.Melee))
                AddDamage(hit.Damage);
        }

		public override void OnHurt(Player.HurtInfo info) {
			if (info.DamageSource.SourceItem.CountsAsClass(DamageClass.Melee)) {
				AddDamage(info.Damage);
			}
			//Note due to HitInfo not having SourceProjectile melee projectiles may break
		}

		void AddDamage(int damage)
        {
            this.VanillaValorDamageDealt += damage;
            Timer.Add(new(damage, 300, TimerDataType.MeleeDamage));
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            StackableBuffData.SwirlySwarm.DeleteBuff(Player);
            neapoliniteSummonTimer = Math.Max(neapoliniteSummonTimer - (neapoliniteSummonTimer % oneStageNeapolioniteSummoner) - oneStageNeapolioniteSummoner * 2, 0);
        }

        /*public override void PostItemCheck()
        {
            Player player = Main.LocalPlayer;

            Vector2 vector = new Vector2(position.X + (float)width * 0.5f, position.Y + (float)height * 0.5f);
            float num = (float)Main.mouseX + Main.screenPosition.X - vector.X;
            float num2 = (float)Main.mouseY + Main.screenPosition.Y - vector.Y;
            float num3 = (float)Math.Sqrt(num * num + num2 * num2);
            float num4 = (float)Main.screenHeight / Main.GameViewMatrix.Zoom.Y;
            num3 /= num4 / 2f;
            if (num3 > 1f)
            {
                num3 = 1f;
            }
            musicDist = num3;
            if (player.HeldItem.type == ModContent.ItemType<Items.Kazoo>())
            {
                Vector2 vector2 = new Vector2(position.X + (float)width * 0.5f, position.Y + (float)height * 0.5f);
                float num5 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                float num6 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                float num7 = (float)Math.Sqrt(num5 * num5 + num6 * num6);
                float num8 = (float)Main.screenHeight / Main.GameViewMatrix.Zoom.Y;
                num7 /= num8 / 2f;
                if (num7 > 1f)
                {
                    num7 = 1f;
                }
                num7 = num7 * 2f - 1f;
                if (num7 < -1f)
                {
                    num7 = -1f;
                }
                if (num7 > 1f)
                {
                    num7 = 1f;
                }
                num7 = (float)Math.Round(num7 * (float)musicNotes);
                num7 = (Main.musicPitch = num7 / (float)musicNotes);
                SoundEngine.PlaySound(type, position);
                NetMessage.SendData(58, -1, -1, null, whoAmI, num7);
            }
        }*/
    }
}