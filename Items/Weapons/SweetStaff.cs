using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using TheConfectionRebirth.Projectiles;
using TheConfectionRebirth.Buffs;
using Terraria.DataStructures;

namespace TheConfectionRebirth.Items.Weapons
{
    public class SweetStaff : ModItem
    {
		public sealed override void SetStaticDefaults()
		{
			ItemID.Sets.StaffMinionSlotsRequired[Type] = 1;
			ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Type] = true;
		}

		public override void SetDefaults()
		{
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = DamageClass.Summon;
			Item.damage = 27;
			Item.knockBack = 3;
			Item.crit = 0;
			Item.mana = 10;
			Item.useTime = 32;
			Item.rare = ItemRarityID.Pink;
			Item.useAnimation = 32;
			Item.buffType = ModContent.BuffType<RollerCookieBuff>();
			Item.shoot = ModContent.ProjectileType<RollerCookieSummon>();
			Item.value = Item.buyPrice(0, 4, 0, 0);
			Item.UseSound = SoundID.Item44;
		}

		public sealed override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.altFunctionUse != 2)
			{
				player.AddBuff(Item.buffType, 2, true);
				position = Main.MouseWorld;

				player.SpawnMinionOnCursor(Item.GetSource_FromThis(), player.whoAmI, type, Item.damage, knockback);
			}
		}

		public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return false;
		}
	}
}
