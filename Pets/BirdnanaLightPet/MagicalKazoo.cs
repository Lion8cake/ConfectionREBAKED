using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System;

namespace TheConfectionRebirth.Pets.BirdnanaLightPet
{
	public class MagicalKazoo : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.damage = 0;
			Item.useStyle = 2;
			Item.shoot = ModContent.ProjectileType<BirdnanaLightPetProjectile>();
			Item.width = 16;
			Item.height = 30;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.rare = ItemRarityID.Yellow;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 5, 50);
			Item.buffType = ModContent.BuffType<BirdnanaLightPetBuff>();
			Item.UseSound = new SoundStyle($"{nameof(TheConfectionRebirth)}/Sounds/Items/KazooSound")
			{
				Volume = 1f,
				PitchVariance = 0f,
				MaxInstances = 0,
			};
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
	}
}