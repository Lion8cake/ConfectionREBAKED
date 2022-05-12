using System;
using System.Collections.Generic;
using TheConfectionRebirth;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.NPCs;
using TheConfectionRebirth.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Projectiles
{
	public class ConfectionGlobalProjectile : GlobalProjectile
	{
		public override void PostAI(Projectile projectile)
		{
			Main.player[projectile.owner].Confection();
			if ((projectile.type != 10 && projectile.type != 145 && projectile.type != 147 && projectile.type != 149 && projectile.type != 146) || projectile.owner != Main.myPlayer)
			{
				return;
			}
			int x = (int)(projectile.Center.X / 16f);
			int y = (int)(projectile.Center.Y / 16f);
			bool isPowder = projectile.type == 10;
			for (int i = x - 1; i <= x + 1; i++)
			{
				for (int j = y - 1; j <= y + 1; j++)
				{
					if (projectile.type == 145 || projectile.type == 10)
					{
						ConvertConfection.ConvertFromConfection(i, j, ConvertType.Pure, !isPowder);
					}
					if (projectile.type == 147)
					{
						ConvertConfection.ConvertFromConfection(i, j, ConvertType.Corrupt, !isPowder);
					}
					if (projectile.type == 149)
					{
						ConvertConfection.ConvertFromConfection(i, j, ConvertType.Crimson, !isPowder);
					}
					if (projectile.type == 146)
					{
						ConvertConfection.ConvertFromConfection(i, j, ConvertType.Hallow);
					}
				}
			}
		}
	}
}
