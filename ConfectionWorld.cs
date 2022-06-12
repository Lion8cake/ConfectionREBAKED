using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TheConfectionRebirth
{
    public class ConfectionWorld : ModSystem
    {
        internal static int ConfectionSurfaceBG = -1;

        public override void Load()
        {
            ConfectionSurfaceBG = 0;
        }

        public override void Unload()
        {
            ConfectionSurfaceBG = 0;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag[nameof(ConfectionSurfaceBG)] = ConfectionSurfaceBG;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.ContainsKey(nameof(ConfectionSurfaceBG)))
                ConfectionSurfaceBG = tag.GetInt(nameof(ConfectionSurfaceBG));
        }

        public override void OnWorldLoad()
        {
            if (ConfectionSurfaceBG == -1)
                ConfectionSurfaceBG = Main.rand.Next(Backgrounds.ConfectionSurfaceBackgroundStyle.backgroundVariatonsTotal);
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(ConfectionSurfaceBG);
        }

        public override void NetReceive(BinaryReader reader)
        {
            ConfectionSurfaceBG = reader.ReadInt32();
        }
    }
}