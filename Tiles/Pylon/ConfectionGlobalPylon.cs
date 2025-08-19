using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.Map;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;

namespace TheConfectionRebirth.Tiles.Pylon
{
	public class ConfecionGlobalPylon : GlobalPylon
	{
		public override bool? ValidTeleportCheck_PreBiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData) {
			if (pylonInfo.TypeOfPylon == TeleportPylonType.SurfacePurity) {
				return ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount <= 119;
			}

			return base.ValidTeleportCheck_PreBiomeRequirements(pylonInfo, sceneData);
		}
	}
}
