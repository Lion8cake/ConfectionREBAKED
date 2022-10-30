using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
	public class TrueSucrosa : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("True Sucrosa");
			SacrificeTotal = 1;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<TrueSucrosaBolt>();
			Item.shootSpeed = 8f;
			Item.knockBack = 6f;
			Item.width = 40;
			Item.height = 40;
			Item.damage = 69;
			Item.scale = 1f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.DamageType = DamageClass.Melee;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Sucrosa>()
				.AddIngredient(ItemID.ChlorophyteBar, 24)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}