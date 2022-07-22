using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TheConfectionRebirth
{
    public class ConfectionWorld : ModSystem
    {
        internal static int[] ConfectionSurfaceBG = new int[4] { -1, -1, -1, -1};

        public override void SaveWorldData(TagCompound tag)
        {
            tag[nameof(ConfectionSurfaceBG) + "2"] = ConfectionSurfaceBG;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.ContainsKey(nameof(ConfectionSurfaceBG) + "2"))
                ConfectionSurfaceBG = tag.GetIntArray(nameof(ConfectionSurfaceBG) + "2");
        }

        public override void NetSend(BinaryWriter writer)
        {
writer.Write(false);
			writer.Write(ConfectionSurfaceBG[0]);
            writer.Write(ConfectionSurfaceBG[1]);
            writer.Write(ConfectionSurfaceBG[2]);
            writer.Write(ConfectionSurfaceBG[3]);
        }

        public override void NetReceive(BinaryReader reader)
        {
            ConfectionSurfaceBG = new int[4] { -1, -1, -1, -1 };
bool bl = reader.ReadBoolean();
if (!bl)
	return;
			ConfectionSurfaceBG[0] = reader.ReadInt32();
            ConfectionSurfaceBG[1] = reader.ReadInt32();
            ConfectionSurfaceBG[2] = reader.ReadInt32();
            ConfectionSurfaceBG[3] = reader.ReadInt32();
        }
    }
}