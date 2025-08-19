using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ReLogic.Content;

namespace TheConfectionRebirth.Mounts
{
	public class PixieStickMount : ModMount
	{
		public override void SetStaticDefaults() {
			MountData.heightBoost = 0;
			MountData.flightTimeMax = 480;
			MountData.fatigueMax = 20;
			MountData.fallDamage = 0f;
			MountData.usesHover = true;
			MountData.runSpeed = 9f;
			MountData.dashSpeed = 8f;
			MountData.acceleration = 0.13f;
			MountData.jumpHeight = 8;
			MountData.jumpSpeed = 3f; 
			MountData.blockExtraJumps = true; 
			MountData.constantJump = false;
			
			MountData.buff = ModContent.BuffType<Buffs.PixieStickBuff>();

			MountData.spawnDust = ModContent.DustType<Dusts.FairyFlossDust>();

			MountData.totalFrames = 1;
			MountData.playerYOffsets = Enumerable.Repeat(6, MountData.totalFrames).ToArray();
			MountData.xOffset = 0;
			MountData.bodyFrame = 0;
			MountData.yOffset = 8;
			MountData.playerHeadOffset = 0;
			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 0;
			MountData.standingFrameStart = 0;
			MountData.runningFrameCount = 1;
			MountData.runningFrameDelay = 0;
			MountData.runningFrameStart = 0;
			MountData.flyingFrameCount = 1;
			MountData.flyingFrameDelay = 0;
			MountData.flyingFrameStart = 0;
			MountData.inAirFrameCount = 1;
			MountData.inAirFrameDelay = 0;
			MountData.inAirFrameStart = 0;
			MountData.idleFrameCount = 0;
			MountData.idleFrameDelay = 0;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = true;
			MountData.swimFrameCount = 0;
			MountData.swimFrameDelay = 0;
			MountData.swimFrameStart = 0;

			if (!Main.dedServ) {
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
				MountData.frontTexture = Asset<Texture2D>.Empty;
			}
		}

		public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
		{
			mountedPlayer.legFrame.Y = 0;
			mountedPlayer.sitting.isSitting = true;
			return true;
		}
	}
}