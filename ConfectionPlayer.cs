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
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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

		public int neapoliniteArmorSetType;

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
			neapoliniteArmorSetType = 0;
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

		/*public override void OnHurt(Player.HurtInfo info) {
			//if (info.DamageSource.SourceItem.CountsAsClass(DamageClass.Melee)) {
				AddDamage(info.Damage);
			//}
			//Note due to HitInfo not having SourceProjectile melee projectiles may break
		}*/

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
	}

	public class NeapoliniteConeMailDrawing : PlayerDrawLayer {
		public override Position GetDefaultPosition() {
			return new AfterParent(PlayerDrawLayers.Torso);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {

			if (drawInfo.shadow != 0f || drawInfo.drawPlayer.dead) {
				return;
			}

			Player drawPlayer = drawInfo.drawPlayer;

			if (drawPlayer.GetModPlayer<ConfectionPlayer>().neapoliniteArmorSetType != 0) {
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/NeapoliniteConeMail_Body_" + drawPlayer.GetModPlayer<ConfectionPlayer>().neapoliniteArmorSetType);

				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
				Vector2 origin = drawInfo.bodyVect;
				Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
				Rectangle frame = new(0, 0, 40, 56);
				if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 7, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 8, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 9, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 14, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 15, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 16, 40, 56)) {
					frame = new(0, 2, 40, 56); //walking bop
				}
				if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 6, 40, 56)) {
					frame = new(40, 0, 40, 56); //jumping frame
				}
				if (!drawPlayer.Male) {
					frame = new(0, 112, 40, 56);
					if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 7, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 8, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 9, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 14, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 15, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 16, 40, 56)) {
						frame = new(0, 114, 40, 56); //walking bop
					}
					if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 6, 40, 56)) {
						frame = new(40, 112, 40, 56); //jumping frame
					}
				}
				float rotation = drawPlayer.bodyRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new(texture, position, frame, drawInfo.colorArmorBody, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cBody;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
	public class NeapoliniteGlenohumeralJointDrawing : PlayerDrawLayer { //Shoulder Drawing
		public override Position GetDefaultPosition() {
			return new AfterParent(PlayerDrawLayers.ArmOverItem);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {

			if (drawInfo.shadow != 0f || drawInfo.drawPlayer.dead) {
				return;
			}

			Player drawPlayer = drawInfo.drawPlayer;

			if (drawPlayer.GetModPlayer<ConfectionPlayer>().neapoliniteArmorSetType != 0) {
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/NeapoliniteConeMail_Body_" + drawPlayer.GetModPlayer<ConfectionPlayer>().neapoliniteArmorSetType);

				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
				Vector2 origin = drawInfo.bodyVect;
				Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
				Rectangle frame = new(0, 56, 40, 56);
				if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 7, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 8, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 9, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 14, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 15, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 16, 40, 56)) {
					frame = new(0, 58, 40, 56); //walking bop
				}
				if (!drawPlayer.Male) {
					frame = new(0, 168, 40, 56);
					if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 7, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 8, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 9, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 14, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 15, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 16, 40, 56)) {
						frame = new(0, 170, 40, 56); //walking bop
					}
				}
				float rotation = drawPlayer.bodyRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new(texture, position, frame, drawInfo.colorArmorBody, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cBody;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}

	public class NeapoliniteUpperLimbDrawing : PlayerDrawLayer { //Arm Drawing
		public override Position GetDefaultPosition() {
			return new AfterParent(PlayerDrawLayers.ArmOverItem);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo) {

			if (drawInfo.shadow != 0f || drawInfo.drawPlayer.dead) {
				return;
			}

			Player drawPlayer = drawInfo.drawPlayer;

			if (drawPlayer.GetModPlayer<ConfectionPlayer>().neapoliniteArmorSetType != 0) {
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("TheConfectionRebirth/Items/Armor/NeapoliniteConeMail_Body_" + drawPlayer.GetModPlayer<ConfectionPlayer>().neapoliniteArmorSetType);

				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
				Vector2 origin = drawInfo.bodyVect;
				Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
				Rectangle frame;
				if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 5, 40, 56)) {
					frame = new(80, 56, 40, 56); //Jumping
				}
				else if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 1, 40, 56)) {
					frame = new(120, 0, 40, 56); //Use1
				}
				else if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 2, 40, 56)) {
					frame = new(160, 0, 40, 56); //Use2
				}
				else if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 3, 40, 56)) {
					frame = new(200, 0, 40, 56); //Use3
				}
				else if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 4, 40, 56)) {
					frame = new(240, 0, 40, 56); //Use4
				}
				else {
					frame = new(0, 0, 0, 0); //None
				}
				float rotation = drawPlayer.bodyRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new(texture, position, frame, drawInfo.colorArmorBody, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.cBody;
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
}