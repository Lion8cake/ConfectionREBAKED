using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.ModSupport.Thorium.Projectiles;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;

namespace TheConfectionRebirth.ModSupport.Thorium.Items.Weapons;

[ExtendsFromMod(TheConfectionRebirth.ThoriumModName)]
public sealed class NeapoliniteMegaphone : BardItem {
	public override bool IsLoadingEnabled(Mod mod) => TheConfectionRebirth.IsThoriumLoaded;

	public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;

	public override void SetStaticDefaults() {
		Empowerments.AddInfo<EmpowermentProlongation>(2);
	}

	public override void SetBardDefaults() {
		Item.width = 30;
		Item.height = 30;

		Item.useTime = 22;
		Item.useAnimation = 22;
		Item.holdStyle = ItemHoldStyleID.HoldHeavy;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.UseSound = SoundID.Item24;

		Item.damage = 47;
		Item.knockBack = 4f;
		InspirationCost = 2;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.shoot = ModContent.ProjectileType<NeapoliniteMegaphonePro1>();
		Item.shootSpeed = 12f;

		Item.value = Item.sellPrice(gold: 4, silver: 60);
		Item.rare = ItemRarityID.Pink;
	}

	public override Vector2? HoldoutOffset() => new Vector2(2f, 0f);

	public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		var vector = Vector2.Normalize(velocity) * 28f;
		if (CollisionHelper.CanHitLine(position, position + vector)) {
			position += vector;
		}

		const float magicConstant = 0.45f * 0.25f;
		float velocityLength = vector.Length();
		float velocityAngle = MathF.Atan2(velocity.X, velocity.Y);
		float circleDistance = Main.rand.NextFloat() * 0.2f + 0.95f;

		float circleAngle1 = velocityAngle + magicConstant;
		float circleAngle2 = velocityAngle;
		float circleAngle3 = velocityAngle - magicConstant;

		var (sinAngle1, cosAngle1) = MathF.SinCos(circleAngle1);
		var (sinAngle2, cosAngle2) = MathF.SinCos(circleAngle2);
		var (sinAngle3, cosAngle3) = MathF.SinCos(circleAngle3);

		velocityLength *= circleDistance;

		Projectile.NewProjectile(source, position.X, position.Y, velocityLength * sinAngle1, velocityLength * cosAngle1, ModContent.ProjectileType<NeapoliniteMegaphonePro3>(), damage, knockback, player.whoAmI, 0f, 0f);
		Projectile.NewProjectile(source, position.X, position.Y, velocityLength * sinAngle3, velocityLength * cosAngle3, ModContent.ProjectileType<NeapoliniteMegaphonePro1>(), damage, knockback, player.whoAmI, 0f, 0f);
		Projectile.NewProjectile(source, position.X, position.Y, velocityLength * sinAngle2, velocityLength * cosAngle2, ModContent.ProjectileType<NeapoliniteMegaphonePro2>(), damage, knockback, player.whoAmI, 0f, 0f);
		
		return false;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<NeapoliniteBar>(12)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
