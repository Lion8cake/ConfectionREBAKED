using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Items;

namespace TheConfectionRebirth.Projectiles
{
	public class NeapoliniteCookies : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}

		public override void SetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 9600;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (!player.active || player.dead || player.GetModPlayer<ConfectionPlayer>().neapolinitePowerLevel <= Projectile.ai[0])
			{
				Projectile.Kill();
				return;
			}
			Projectile.timeLeft = 2;
			if (Projectile.frameCounter == 0)
			{
				Projectile.frameCounter = 1;
				Projectile.frame = (int)Projectile.ai[0];
			}
			AI_GetMyGroupIndexAndFillBlackList(null, out var index, out var totalIndexesInGroup);
			float f = ((float)index / (float)totalIndexesInGroup + player.miscCounterNormalized * 6f) * ((float)Math.PI * 2f);
			float num = 24f + (float)totalIndexesInGroup * 6f;
			Vector2 vector = player.position - player.oldPosition;
			Projectile.Center += vector;
			Vector2 vector2 = f.ToRotationVector2();
			Projectile.localAI[0] = vector2.Y;
			Vector2 value = player.Center + vector2 * new Vector2(1f, 0.05f) * num;
			Projectile.Center = Vector2.Lerp(Projectile.Center, value, 0.1f);
		}

		private void AI_GetMyGroupIndexAndFillBlackList(List<int> blackListedTargets, out int index, out int totalIndexesInGroup)
		{
			index = 0;
			totalIndexesInGroup = 0;
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active && projectile.owner == Projectile.owner && projectile.type == Projectile.type && (projectile.type != 759 || projectile.frame == Main.projFrames[projectile.type] - 1))
				{
					if (Projectile.whoAmI > i)
					{
						index++;
					}
					totalIndexesInGroup++;
				}
			}
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			if (Projectile.localAI[0] <= 0f)
			{
				behindProjectiles.Add(index);
			}
			else
			{
				overPlayers.Add(index);
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.PrepareDrawnEntityDrawing(Projectile, Main.player[Projectile.owner].cBody, Projectile.isAPreviewDummy ? Main.UIScaleMatrix : Main.Transform);
			return true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dustID;
				if (Projectile.ai[0] < 2)
					dustID = ModContent.DustType<NeapoliniteVanillaDust>();
				else if (Projectile.ai[0] < 4 && Projectile.ai[0] > 1)
					dustID = ModContent.DustType<NeapoliniteChocolateDust>();
				else
					dustID = ModContent.DustType<NeapoliniteStrawberryDust>();

				Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, dustID, Main.rand.NextFloat(-0.5f, 0.5f), 0f);
			}
		}
	}
}
