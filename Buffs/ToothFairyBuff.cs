using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Buffs
{
	public class ToothFairyBuff : ModBuff
	{
		public sealed override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public sealed override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<ToothFairyCrystal>()] > 0)
			{
				player.GetModPlayer<ConfectionPlayer>().toothfairyMinion = true;
			}
			if (!player.GetModPlayer<ConfectionPlayer>().toothfairyMinion)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else
			{
				player.buffTime[buffIndex] = 18000;
			}
			if (player.whoAmI == Main.myPlayer)
			{
				UpdateToothFairyStatus(player);
			}
		}

		private void UpdateToothFairyStatus(Player player)
		{
			int fairy = ModContent.ProjectileType<ToothFairy>();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<ToothFairyCrystal>()] < 1)
			{
				for (int i = 0; i < 1000; i++)
				{
					Projectile projectile = Main.projectile[i];
					if (projectile.active && projectile.owner == player.whoAmI && projectile.type == fairy)
					{
						projectile.Kill();
					}
				}
			}
			else if (player.ownedProjectileCounts[fairy] < 1)
			{
				int damage = 15;
				for (int i = 0; i < 1000; i++)
				{
					Projectile projectile = Main.projectile[i];
					if (projectile.active && projectile.owner == player.whoAmI && projectile.type == ModContent.ProjectileType<ToothFairyCrystal>())
					{
						damage = projectile.damage;
						break;
					}
				}
				Projectile.NewProjectile(player.GetSource_Misc("AbigailTierSwap"), player.Center, Vector2.Zero, fairy, damage, 0f, player.whoAmI);
			}
		}
	}
}
