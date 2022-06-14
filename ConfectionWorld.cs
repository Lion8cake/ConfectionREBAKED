using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TheConfectionRebirth
{
    public class ConfectionWorld : ModSystem
    {
        internal static int ConfectionSurfaceBG = -1;
        internal static bool ConfectionSurfaceBGInit = false;

        public override void OnWorldLoad()
        {
            if (!ConfectionSurfaceBGInit)
            {
                ConfectionSurfaceBG = Main.rand.Next(Backgrounds.ConfectionSurfaceBackgroundStyle.backgroundVariatonsTotal);
                ConfectionSurfaceBGInit = true;
            }
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag[nameof(ConfectionSurfaceBG)] = ConfectionSurfaceBG;
            tag[nameof(ConfectionSurfaceBGInit)] = ConfectionSurfaceBGInit;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.ContainsKey(nameof(ConfectionSurfaceBG)))
                ConfectionSurfaceBG = tag.GetInt(nameof(ConfectionSurfaceBG));
            if (tag.ContainsKey(nameof(ConfectionSurfaceBGInit)))
                ConfectionSurfaceBGInit = tag.GetBool(nameof(ConfectionSurfaceBGInit));
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(ConfectionSurfaceBG);
            writer.Write(ConfectionSurfaceBGInit);
        }

        public override void NetReceive(BinaryReader reader)
        {
            ConfectionSurfaceBG = reader.ReadInt32();
            ConfectionSurfaceBGInit = reader.ReadBoolean();
        }
    }
}