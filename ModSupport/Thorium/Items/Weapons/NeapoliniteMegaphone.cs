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

	public class NeapoliniteMegaphone : ThoriumMod.Items.BardItem
	{
		public override ThoriumMod.BardInstrumentType InstrumentType => ThoriumMod.BardInstrumentType.Electronic;

		public Mod thorium;
		public override bool IsLoadingEnabled(Mod mod) {
			return ModLoader.TryGetMod("ThoriumMod", out thorium);
		}

		public override void SetStaticDefaults()
		{
			Empowerments.AddInfo<ThoriumMod.Empowerments.EmpowermentProlongation>(2);
		}

		public override void SetBardDefaults() {
			Item.damage = 47;
			InspirationCost = 2;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = 5;
			Item.holdStyle = 3;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.knockBack = 4f;
			Item.value = Item.sellPrice(0, 4, 60, 0);
			Item.rare = 5;
			Item.UseSound = SoundID.Item24;
			Item.shoot = ModContent.ProjectileType<Projectiles.NeapoliniteMegaphonePro>();
			Item.shootSpeed = 12f;
		}

		public override Vector2? HoldoutOffset() {
			return new Vector2(2f, 0f);
		}

		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 vector = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 28f;
			if (ConfectionMiscHelper.CanHitLine(position, position + vector))
			{
				position += vector;
			}
			float num = 0.25f;
			float num2 = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
			double num3 = Math.Atan2(velocity.X, velocity.Y);
			double num4 = num3 + (double)(0.35f * num);
			double num5 = num3;
			double num6 = num3 - (double)(0.35f * num);
			float num7 = Utils.NextFloat(Main.rand) * 0.2f + 0.95f;
			velocity.X = num2 * num7 * (float)Math.Sin(num4);
			velocity.Y = num2 * num7 * (float)Math.Cos(num4);
			Projectile.NewProjectile(source, position.X, position.Y, num2 * num7 * (float)Math.Sin(num4), num2 * num7 * (float)Math.Cos(num4), ModContent.ProjectileType<Projectiles.NeapoliniteMegaphonePro2>(), damage, knockback, player.whoAmI, 0f, 0f);
			Projectile.NewProjectile(source, position.X, position.Y, num2 * num7 * (float)Math.Sin(num6), num2 * num7 * (float)Math.Cos(num6), ModContent.ProjectileType<Projectiles.NeapoliniteMegaphonePro>(), damage, knockback, player.whoAmI, 0f, 0f);
			Projectile.NewProjectile(source, position.X, position.Y, num2 * num7 * (float)Math.Sin(num5), num2 * num7 * (float)Math.Cos(num5), ModContent.ProjectileType<Projectiles.NeapoliniteMegaphonePro1>(), damage, knockback, player.whoAmI, 0f, 0f);
			return false;
		}

		public override void AddRecipes() {
			if (ModLoader.TryGetMod("ThoriumMod", out Mod mod)) {
				CreateRecipe()
				.AddIngredient(ModContent.ItemType<NeapoliniteBar>(), 12)
				.AddTile(TileID.MythrilAnvil)
				.Register();
			}
		}
	}
}
