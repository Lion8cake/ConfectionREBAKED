using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System;

namespace TheConfectionRebirth.Items
{
	public class MagicalKazoo : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.damage = 0;
			Item.useStyle = 1;
			Item.shoot = ModContent.ProjectileType<Projectiles.BirdnanaLightPetProjectile>();
			Item.width = 16;
			Item.height = 30;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.rare = ItemRarityID.Yellow;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 5, 50);
			Item.buffType = ModContent.BuffType<Buffs.BirdnanaLightPetBuff>();
			Item.UseSound = null;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Items.Sprinkles>(), 80);
			recipe.AddIngredient(ModContent.ItemType<Items.SoulofDelight>(), 12);
			recipe.AddIngredient(ItemID.SoulofSight, 20);
			recipe.AddIngredient(ModContent.ItemType<Items.Kazoo>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
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
			}, player.position);
			NetMessage.SendData(58, -1, -1, null, player.whoAmI, num7);
		}
	}
}