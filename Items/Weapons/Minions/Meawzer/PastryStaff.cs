using Terraria.DataStructures;
using TheConfectionRebirth.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons.Minions.Meawzer
{
	public class PastryStaff : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults() {
			Item.damage = 24;
			Item.DamageType = DamageClass.Summon;
			Item.mana = 10;
			Item.width = 26;
			Item.height = 28;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.knockBack = 3;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item44;
			Item.shoot = ModContent.ProjectileType<MeawzerSummonProj>();
			Item.buffType = ModContent.BuffType<MeawzerSummonBuff>();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position = Main.MouseWorld;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.AddBuff(Item.buffType, 2);

			var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
			projectile.originalDamage = Item.damage;

			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Saccharite>(), 50);
			recipe.AddIngredient(ModContent.ItemType<SoulofDelight>(), 10);
			recipe.AddIngredient(ModContent.ItemType<PastryBlock>(), 80);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
