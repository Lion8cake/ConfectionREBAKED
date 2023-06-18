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
			SacrificeTotal = 1;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.autoReuse = true;
			Item.knockBack = 6f;
			Item.width = 40;
			Item.height = 40;
			Item.damage = 69;
			Item.scale = 1f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ModContent.ProjectileType<Projectiles.TrueSucrosaSlash>();
			Item.shootsEveryUse = true;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.TrueSucrosaSlash2>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);
			Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Projectiles.TrueSucrosaSlash>(), damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);
			return false;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(3))
			{
				int num313 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<NeapoliniteCrumbs>());
				Main.dust[num313].noGravity = true;
				Main.dust[num313].fadeIn = 1.25f;
				Main.dust[num313].velocity *= 0.25f;
			}
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