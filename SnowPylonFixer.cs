using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;

namespace TheConfectionRebirth
{
	public class SnowPylonFixer : GlobalPylon
	{
		public override bool? ValidTeleportCheck_PreBiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData) {
			if (pylonInfo.TypeOfPylon == TeleportPylonType.Snow) {
				if (ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount >= 120 && ModContent.GetInstance<ConfectionBiomeTileCount>().snowpylonConfectionCount >= SceneMetrics.SnowTileThreshold) {
					return true;
				}
				return null;
			}
			return base.ValidTeleportCheck_PreBiomeRequirements(pylonInfo, sceneData);
		}
	}
}
