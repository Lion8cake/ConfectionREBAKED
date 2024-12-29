using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TheConfectionRebirth.Buffs;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
	public class GastropodStaff : ModItem
	{
		public override void SetStaticDefaults() 
		{
			ItemID.Sets.StaffMinionSlotsRequired[Type] = 1;
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
			ConfectionIDs.Sets.RecipeBlacklist.SoulofLightOnlyItem[Type] = true;
			ConfectionIDs.Sets.RecipeBlacklist.CrystalShardOnlyItem[Type] = true;
		}

		public override void SetDefaults() 
		{
			Item.damage = 28;
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
			Item.shoot = ModContent.ProjectileType<GastropodSummon>();
			Item.buffType = ModContent.BuffType<GastropodSummonBuff>();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.altFunctionUse != 2)
			{
				player.AddBuff(Item.buffType, 2, true);
				position = Main.MouseWorld;

				player.SpawnMinionOnCursor(Item.GetSource_FromThis(), player.whoAmI, type, Item.damage, knockback);
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return false;
		}
	}
}
