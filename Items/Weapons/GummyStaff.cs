using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Buffs;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
    public class GummyStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			ItemID.Sets.StaffMinionSlotsRequired[Item.type] = 1;
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.DamageType = DamageClass.Summon;
			Item.damage = 10;
			Item.knockBack = 6;
			Item.mana = 10;
			Item.useTime = 32;
			Item.rare = ItemRarityID.Pink;
			Item.useAnimation = 32;
			Item.buffType = ModContent.BuffType<ToothFairyBuff>();
			Item.shoot = ModContent.ProjectileType<ToothFairyCrystal>();
			Item.value = Item.buyPrice(0, 8, 0, 0);
			Item.UseSound = SoundID.Item44;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.altFunctionUse != 2)
			{
				player.AddBuff(Item.buffType, 2, true);
				position = Main.MouseWorld;

				player.SpawnMinionOnCursor(Item.GetSource_FromThis(), player.whoAmI, type, damage, knockback);
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return false;
		}
	}
}
