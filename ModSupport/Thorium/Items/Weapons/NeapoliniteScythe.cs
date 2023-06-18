using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Weapons
{
	[ExtendsFromMod("ThoriumMod")]

	public class NeapoliniteScythe : ThoriumMod.Items.HealerItems.ScytheItem
	{
		public Mod thorium;
		public override bool IsLoadingEnabled(Mod mod) {
			return ModLoader.TryGetMod("ThoriumMod", out thorium);
		}

		public override void SetStaticDefaults() {
			SetStaticDefaultsToScythe();
		}

		public override void SetDefaults() {
			SetDefaultsToScythe();
			Item.damage = 40;
			scytheSoulCharge = 2;
			Item.width = 58;
			Item.height = 56;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = 5;
			Item.shoot = ModContent.ProjectileType<Projectiles.NeapoliniteScythePro>();
		}

		public override void AddRecipes() {
			if (ModLoader.TryGetMod("ThoriumMod", out Mod mod)) {
				CreateRecipe()
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			}
		}
	}
}
