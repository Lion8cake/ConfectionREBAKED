using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ID;
using Terraria.Audio;

namespace TheConfectionRebirth.Mounts {
	public class Rollercycle : ModMount
	{
		public float oldDir = 0f;

		public override void SetStaticDefaults() {
			MountData.jumpHeight = 20;
			MountData.acceleration = 0.06f;
			MountData.jumpSpeed = 4f;
			MountData.blockExtraJumps = true;
			MountData.constantJump = true;
			MountData.heightBoost = 20;
			MountData.fallDamage = 0f;
			MountData.runSpeed = 15f;
			MountData.dashSpeed = 10f;
			MountData.flightTimeMax = 0; 

			MountData.fatigueMax = 0;
			MountData.buff = ModContent.BuffType<Buffs.RollercycleBuff>();

			MountData.spawnDust = ModContent.DustType<Dusts.NeapoliniteDust>();

			MountData.totalFrames = 8;
			MountData.playerYOffsets = new int[8] { 30, 30, 30, 30, 18, 18, 18, 18 };
			MountData.xOffset = 13;
			MountData.yOffset = 0;
			MountData.playerHeadOffset = 22;
			MountData.bodyFrame = 3;
			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 1;
			MountData.standingFrameStart = 4;
			MountData.runningFrameCount = 4;
			MountData.runningFrameDelay = 12;
			MountData.runningFrameStart = 4;
			MountData.flyingFrameCount = 0;
			MountData.flyingFrameDelay = 0;
			MountData.flyingFrameStart = 0;
			MountData.inAirFrameCount = 0; 
			MountData.inAirFrameDelay = 0; 
			MountData.inAirFrameStart = 0; 
			MountData.idleFrameCount = 1;
			MountData.idleFrameDelay = 1;
			MountData.idleFrameStart = 4;
			MountData.idleFrameLoop = true;
			MountData.swimFrameCount = MountData.inAirFrameCount;
			MountData.swimFrameDelay = MountData.inAirFrameDelay;
			MountData.swimFrameStart = MountData.inAirFrameStart;

			if (!Main.dedServ) {
				MountData.textureWidth = MountData.backTexture.Width() + 20;
				MountData.textureHeight = MountData.backTexture.Height();
			}
		}

		public override void UpdateEffects(Player player)
		{
			Mount mount = player.mount;
            if (player.wet)
            {
				mount.Dismount(player);
            }
			float speed = Math.Abs(player.velocity.X);

			if (oldDir == 0f)
			{
				oldDir = player.direction;
			}

			if (player.direction != oldDir && Math.Abs(player.velocity.Y) == 0)
				player.velocity = -player.velocity;

			mount._frameExtraCounter++;
			bool isJumpFrame = mount._frame >= 0 && mount._frame <= 3;
			Vector2 position = (isJumpFrame ? new Vector2(-12 * player.direction + 6, 26) : new Vector2(-16 * player.direction + 6, 16)) + player.position;
			Vector2 position2 = (isJumpFrame ? new Vector2(-2 * player.direction + 6, 32) : new Vector2(-12 * player.direction + 6, 32)) + player.position;
			Vector2 dustVel = new Vector2(-1 * player.direction, -0.5f);
			float secondaryDustScale = 0.88f;
			if (mount._frameExtraCounter > 8 / ((speed / 10) + 1))
			{
				mount._frameExtraCounter = 0;
				Dust dust = Dust.NewDustDirect(position, 1, 1, DustID.Smoke);
				dust.velocity = dustVel;

				Dust dust2 = Dust.NewDustDirect(position2, 1, 1, DustID.Smoke);
				dust2.velocity = dustVel * secondaryDustScale;
				dust2.scale = secondaryDustScale;
			}
			if (mount._frameExtraCounter > 2 && mount._frameExtraCounter < 4)
			{
				Dust dust = Dust.NewDustDirect(position, 8, 8, DustID.Smoke);
				dust.velocity = dustVel;

				Dust dust2 = Dust.NewDustDirect(position2, 8, 8, DustID.Smoke);
				dust2.velocity = dustVel * secondaryDustScale;
				dust2.scale = secondaryDustScale;
			}

			if (speed > 10)
			{
				Rectangle hitbox = player.getRect();
				if (player.direction == 1)
				{
					hitbox.Offset(player.width - 1, 0);
				}
				hitbox.Width = 2;
				hitbox.Inflate(6, 12);
				float damage = player.GetTotalDamage(DamageClass.Summon).ApplyTo(100f);
				float knockback = 10f;
				int NPCImmuneTime = 30;
				int playerImmuneTime = 12;
				player.CollideWithNPCs(hitbox, damage, knockback, NPCImmuneTime, playerImmuneTime, DamageClass.Summon);

				Vector2 cycleBottom = player.position + new Vector2(player.width + 20, player.height - 20);
				if (Collision.WetCollision(cycleBottom, 50, 20))
				{
					player.velocity.Y = -4f;

					for (int i = 0; i < 50; i++) //just straight up taken from the player code of splashing, was expecting to move the position of the splash but ig not
					{
						int dustID = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(player.height / 2) - 8f), player.width + 12, 24, Dust.dustWater());
						Dust dust = Main.dust[dustID];
						dust.velocity.Y -= 3f;
						dust.velocity.X *= 2.5f;
						dust.scale = 1.8f; //changed from 0.8 to 1.8
						dust.alpha = 100;
						dust.noGravity = true;
					}
					SoundEngine.PlaySound(SoundID.SplashWeak, position);
				}
			}
			oldDir = player.direction;
		}

		public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
		{
			Mount mount = mountedPlayer.mount;
			float speed = Math.Abs(velocity.X);
			bool isFalling = Math.Abs(velocity.Y) > 0;
			if (mountedPlayer.controlUp && speed > 10 && !isFalling) //speed 10 is 50 mph
			{
				mount._frameCounter++;
				if (mount._frameCounter >= 12 / speed)
				{
					mount._frameCounter = 0;
					mount._frame++;
				}
				if (mount._frame > 3)
					mount._frame = 0;
				return false;
			}
			return true;
		}
	}
}