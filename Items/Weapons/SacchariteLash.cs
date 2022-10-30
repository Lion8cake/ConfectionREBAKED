using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
	public class SacchariteLash : ModItem
	{
		public override void SetStaticDefaults() {
			SacrificeTotal = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.SacchariteLash>(), 68, 2, 4); 

			Item.shootSpeed = 4;
			Item.rare = ItemRarityID.Pink;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<NeapoliniteBar>(12)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

		public override bool MeleePrefix() {
			return true;
		}
	}
}
