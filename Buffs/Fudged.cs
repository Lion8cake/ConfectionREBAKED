using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace TheConfectionRebirth.Buffs
{
	public class Fudged : ModBuff
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Fudged");
			Description.SetDefault("Super Sticky");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			player.pickSpeed -= 1.2f;
            player.tileSpeed -= 1.2f;
            player.wallSpeed -= 1.2f;
			player.moveSpeed -= 1.5f;
			player.jumpSpeedBoost -= 5f;
			player.runAcceleration -= 0.8f;
		}
	}
}
