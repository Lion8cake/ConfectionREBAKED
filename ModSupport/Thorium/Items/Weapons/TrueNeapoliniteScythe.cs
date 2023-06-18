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

	public class TrueNeapoliniteScythe : ThoriumMod.Items.HealerItems.ScytheItem
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
			Item.damage = 49;
			scytheSoulCharge = 2;
			Item.width = 58;
			Item.height = 56;
			Item.value = Item.sellPrice(0, 11, 0, 0);
			Item.rare = 8;
			Item.shoot = ModContent.ProjectileType<Projectiles.TrueNeapoliniteScythePro>();
		}

		public override void AddRecipes() {
			if (ModLoader.TryGetMod("ThoriumMod", out Mod mod)) {
				CreateRecipe()
				.AddIngredient(ModContent.ItemType<NeapoliniteScythe>())
				.AddIngredient(mod.Find<ModItem>("BrokenHeroFragment"), 2)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			}
		}
	}
}
