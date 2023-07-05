using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
	public class TrueSucrosa : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.autoReuse = true;
			Item.knockBack = 6f;
			Item.width = 40;
			Item.height = 40;
			Item.damage = 79;
			Item.scale = 1f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ModContent.ProjectileType<Projectiles.TrueSucrosaSlash>();
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.TrueSucrosaSlash>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.SucrosaSlash>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);
			return false;
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