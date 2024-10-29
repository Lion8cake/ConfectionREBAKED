using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;

namespace TheConfectionRebirth.Tiles.Pylons
{
    public class ConfectionGlobalPylon : GlobalPylon
    {
        public override bool? ValidTeleportCheck_PreBiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData)
        {
            if (pylonInfo.TypeOfPylon == TeleportPylonType.Snow)
            {
                if (ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount >= 125 && ModContent.GetInstance<ConfectionBiomeTileCount>().snowpylonConfectionCount >= SceneMetrics.SnowTileThreshold)
                {
                    return true;
                }
                return null;
            }
            else if (pylonInfo.TypeOfPylon == TeleportPylonType.Desert)
            {
                if (ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount >= 125 && ModContent.GetInstance<ConfectionBiomeTileCount>().desertpylonConfectionCount >= SceneMetrics.DesertTileThreshold)
                {
                    return true;
                }
                return null;
            }
			else if (pylonInfo.TypeOfPylon == TeleportPylonType.SurfacePurity)
			{
				return ModContent.GetInstance<ConfectionBiomeTileCount>().confectionBlockCount <= 124;
			}
			return base.ValidTeleportCheck_PreBiomeRequirements(pylonInfo, sceneData);
        }
    }
}
