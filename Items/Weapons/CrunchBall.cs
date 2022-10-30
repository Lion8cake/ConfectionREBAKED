using TheConfectionRebirth.Items;
using TheConfectionRebirth.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.Items.Weapons
{
	public class CrunchBall : ModItem
	{
		public override void SetStaticDefaults()
		{
			SacrificeTotal = 1;
		}
	
		public override void SetDefaults()
		{
			Item.damage = 38;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 4;
			Item.width = 28;
			Item.height = 30;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 2f;
			Item.value = 500000;
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item17;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<RockCandy>();
			Item.shootSpeed = 16f;
		}
	
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<Saccharite>(30)
				.AddIngredient<SoulofDelight>(15)
				.AddIngredient(ItemID.SpellTome)
				.AddTile(TileID.Bookcases)
				.Register();
		}
	}
}