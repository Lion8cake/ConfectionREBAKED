using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheConfectionRebirth.Walls;
using TheConfectionRebirth.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
 
namespace TheConfectionRebirth.Projectiles
{
	public class ConfectionGlobalProjectile : GlobalProjectile {
		/*public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) {
			return entity.type == ProjectileID.WorldGlobe;
		}

		public override void Kill(Projectile projectile, int timeLeft) {
			Player player = Main.LocalPlayer;
			if (Main.netMode != NetmodeID.MultiplayerClient && player.InModBiome<ConfectionBiome>()) {
				ConfectionWorldGeneration.confectionBackStyle = Main.rand.Next(4);
				NetMessage.SendData(MessageID.WorldData);
			}
		}*/
	}
}