using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace TheConfectionRebirth.Projectiles
{
	public class ToothFairyCrystal : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 6;
			ProjectileID.Sets.MinionShot[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.netImportant = true;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.timeLeft = 60;
			Projectile.hide = true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (player.ownedProjectileCounts[Type] > 1 && Projectile.localAI[0] == 0f)
			{
				Projectile.localAI[0] = 1f;
				SoundEngine.PlaySound(in SoundID.AbigailUpgrade, Projectile.Center);
			}
			if (player.dead)
			{
				player.GetModPlayer<ConfectionPlayer>().toothfairyMinion = false;
			}
			if (player.GetModPlayer<ConfectionPlayer>().toothfairyMinion)
			{
				Projectile.timeLeft = 2;
			}
			if (++Projectile.frameCounter >= 4)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 6)
				{
					Projectile.frame = 0;
				}
			}
			List<int> ai164_blacklistedTargets = new List<int>();
			ai164_blacklistedTargets.Clear();
			AI_GetMyGroupIndexAndFillBlackList(ai164_blacklistedTargets, out var index, out var totalIndexesInGroup);
			Projectile.Center = Projectile.AI_164_GetHomeLocation(player, index, totalIndexesInGroup);
		}

		private void AI_GetMyGroupIndexAndFillBlackList(List<int> blackListedTargets, out int index, out int totalIndexesInGroup)
		{
			index = 0;
			totalIndexesInGroup = 0;
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active && projectile.owner == Projectile.owner && projectile.type == Type)
				{
					if (Projectile.whoAmI > i)
					{
						index++;
					}
					totalIndexesInGroup++;
				}
			}
		}

		public override bool? CanDamage()
		{
			return false;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}
	}
}
