using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
    public class NeapoliniteOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 680;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 975;
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(153, 96, 62), name);
            
            DustType = ModContent.DustType<NeapoliniteDust>();
            HitSound = SoundID.Tink;
            MineResist = 4f;
            MinPick = 180;
        }
    }
}