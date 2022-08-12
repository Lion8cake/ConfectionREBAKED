using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TheConfectionRebirth
{
    public class ConfectionWorld : ModSystem
    {
        internal static int[] ConfectionSurfaceBG = new int[TheConfectionRebirth.bgVarAmount] { -1, -1, -1, -1};
        internal static bool Secret = false;

        public override void SaveWorldData(TagCompound tag)
        {
            tag[nameof(ConfectionSurfaceBG) + "2"] = ConfectionSurfaceBG;
            tag[nameof(Secret)] = Secret;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.ContainsKey(nameof(ConfectionSurfaceBG) + "2"))
                ConfectionSurfaceBG = tag.GetIntArray(nameof(ConfectionSurfaceBG) + "2");
            Secret = tag.ContainsKey(nameof(Secret)) && tag.GetBool(nameof(Secret));
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(TheConfectionRebirth.bgVarAmount);
            for (int i = 0; i < TheConfectionRebirth.bgVarAmount; i++)
                writer.Write(ConfectionSurfaceBG[i]);
            writer.Write(Secret);
        }

        public override void NetReceive(BinaryReader reader)
        {
            int bgVar = reader.ReadInt32();
            ConfectionSurfaceBG = new int[TheConfectionRebirth.bgVarAmount] { -1, -1, -1, -1 };
            for (int i = 0; i < bgVar; i++)
                ConfectionSurfaceBG[i] = reader.ReadInt32();
            Secret = reader.ReadBoolean();
        }
    }
}