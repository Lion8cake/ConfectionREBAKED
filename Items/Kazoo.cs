using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheConfectionRebirth.Items
{
    public class Kazoo : ModItem
    {
		public float pitch;

		public override void SetStaticDefaults()
        {
			Item.staff[Type] = true;
			Item.ResearchUnlockCount = 1;
		}
        public override void SetDefaults()
        {
			Item.rare = ItemRarityID.Orange;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 12;
			Item.useTime = 12;
			Item.width = 20;
			Item.height = 24;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.value = 10000;
			Item.UseSound = null;
		}

		public override void UseAnimation(Player player) 
		{
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
			SoundEngine.PlaySound(new SoundStyle("TheConfectionRebirth/Sounds/Items/KazooSound")
			{
				Pitch = num7,
				Volume = 0.75f,
				MaxInstances = 0
			}, player.position);
			NetMessage.SendData(MessageID.InstrumentSound, -1, -1, null, player.whoAmI, num7);
		}
	}
}