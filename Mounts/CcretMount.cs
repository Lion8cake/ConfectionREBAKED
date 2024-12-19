using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ReLogic.Content;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Terraria.GameContent;

namespace TheConfectionRebirth.Mounts
{
	public class CcretMount : ModMount
	{
		public override void SetStaticDefaults() 
		{
			MountData.heightBoost = 25;
			MountData.flightTimeMax = 150;
			MountData.runSpeed = 2f;
			MountData.dashSpeed = 10f;
			MountData.acceleration = 0.09f;
			MountData.jumpHeight = 15;
			MountData.jumpSpeed = 5.5f; 
			MountData.fallDamage = 0f;
			MountData.fatigueMax = 0;

			MountData.buff = ModContent.BuffType<Buffs.CcretBuff>();
			MountData.spawnDust = ModContent.DustType<Dusts.CreamstoneDust>();

			MountData.totalFrames = 1;
			MountData.playerYOffsets = Enumerable.Repeat(12, MountData.totalFrames).ToArray();
			MountData.xOffset = 6;
			MountData.yOffset = 20;
			MountData.playerHeadOffset = 22;
			MountData.bodyFrame = 0;
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
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
				MountData.backTexture = Asset<Texture2D>.Empty;
			}
		}
	}
}