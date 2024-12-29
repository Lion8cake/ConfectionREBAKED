using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System;

namespace TheConfectionRebirth.Items
{
	public class MagicalKazoo : ModItem
	{
		public override void SetStaticDefaults() {
			Item.staff[Type] = true;
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.width = 20;
			Item.height = 24;
			Item.UseSound = null;
			Item.useAnimation = 12;
			Item.useTime = 12;
			Item.rare = ItemRarityID.Pink;
			Item.noMelee = true;
			Item.value = 250000;
			Item.buffType = ModContent.BuffType<Buffs.BirdnanaLightPetBuff>();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame) {
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(Item.buffType, 3600);
			}
		}

		public override void UseAnimation(Player player) {
			Vector2 vector2 = new Vector2(player.position.X + (float)player.width * 0.5f, player.position.Y + (float)player.height * 0.5f);
			float num5 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
			float num6 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
			float num7 = (float)Math.Sqrt(num5 * num5 + num6 * num6);
			float num8 = (float)Main.screenHeight / Main.GameViewMatrix.Zoom.Y;
			num7 /= num8 / 2f;
			if (num7 > 1f) {
				num7 = 1f;
			}
			num7 = num7 * 2f - 1f;
			if (num7 < -1f) {
				num7 = -1f;
			}
			if (num7 > 1f) {
				num7 = 1f;
			}
			num7 = (float)Math.Round(num7 * (float)Player.musicNotes);
			num7 = (Main.musicPitch = num7 / (float)Player.musicNotes);
			SoundEngine.PlaySound(new SoundStyle("TheConfectionRebirth/Sounds/Items/KazooSound") {
				Pitch = num7,
				Volume = 0.75f,
				MaxInstances = 0
			}, player.position);
			NetMessage.SendData(MessageID.InstrumentSound, -1, -1, null, player.whoAmI, num7);
		}
	}
}