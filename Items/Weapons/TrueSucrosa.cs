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
			Item.useAnimation = 18;
			Item.useTime = 18;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<TrueSucrosaSwing>();
			Item.shootSpeed = 11f;
			Item.knockBack = 4.5f;
			Item.width = 30;
			Item.height = 30;
			Item.damage = 85;
			Item.scale = 1f;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			//Item.shootsEveryUse = true;
		}

		/*public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int numberProjectiles = 2 + Main.rand.Next(2);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(25));

				int spirit = Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}
			return false; 
		}*/

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