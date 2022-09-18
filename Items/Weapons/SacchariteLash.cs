using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
	public class SacchariteLash : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Tooltip.SetDefault("Your summons will focus stuck enemies"
				+ "\nShatters into pieces");
		}

		public override void SetDefaults() {
			Item.DefaultToWhip(ModContent.ProjectileType<Projectiles.SacchariteLash>(), 68, 2, 4); 

			Item.shootSpeed = 4;
			Item.rare = ItemRarityID.Pink;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Placeable.NeapoliniteBar>(), 12).AddTile(TileID.MythrilAnvil).Register();
		}

		public override bool MeleePrefix() {
			return true;
		}
	}
}
