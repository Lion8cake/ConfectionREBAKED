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
		}

		public override void SetDefaults() 
		{
			Item.damage = 64;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 25;
			Item.useAnimation = 15;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(silver: 1000);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<TrueSucrosaBolt>();
			Item.shootSpeed = 8f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int numberProjectiles = 2 + Main.rand.Next(2);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(25));

				int spirit = Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}
			return false; 
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Weapons.Sucrosa>()).AddIngredient(ItemID.ChlorophyteBar, 24).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}