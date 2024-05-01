using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Mounts
{
	public class CcretMount : ModMount
	{
		public override void SetStaticDefaults() {
			MountData.jumpHeight = 50;
			MountData.acceleration = 0.09f;
			MountData.jumpSpeed = 9f; 
			MountData.blockExtraJumps = false; 
			MountData.constantJump = false;
			MountData.heightBoost = 0;
			MountData.fallDamage = 0f;
			MountData.runSpeed = 0.25f;
			MountData.dashSpeed = 4f;
			MountData.flightTimeMax = 50;

			MountData.fatigueMax = 0;
			MountData.buff = ModContent.BuffType<Buffs.CcretBuff>();

			MountData.spawnDust = ModContent.DustType<Dusts.CreamDust>();

			MountData.totalFrames = 1;
			MountData.playerYOffsets = Enumerable.Repeat(20, MountData.totalFrames).ToArray();
			MountData.xOffset = 13;
			MountData.yOffset = -24;
			MountData.playerHeadOffset = 22;
			MountData.bodyFrame = 3;
			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 12;
			MountData.standingFrameStart = 0;
			MountData.runningFrameCount = 1;
			MountData.runningFrameDelay = 12;
			MountData.runningFrameStart = 0;
			MountData.flyingFrameCount = 0;
			MountData.flyingFrameDelay = 0;
			MountData.flyingFrameStart = 0;
			MountData.inAirFrameCount = 1;
			MountData.inAirFrameDelay = 12;
			MountData.inAirFrameStart = 0;
			MountData.idleFrameCount = 1;
			MountData.idleFrameDelay = 12;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = true;
			MountData.swimFrameCount = MountData.inAirFrameCount;
			MountData.swimFrameDelay = MountData.inAirFrameDelay;
			MountData.swimFrameStart = MountData.inAirFrameStart;

			if (!Main.dedServ) {
				MountData.textureWidth = MountData.backTexture.Width() + 20;
				MountData.textureHeight = MountData.backTexture.Height();
			}
		}
	}
}