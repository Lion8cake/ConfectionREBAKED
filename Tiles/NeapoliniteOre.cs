using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
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
            TheConfectionRebirth.tileMerge[Type, (TileID.Stone)] = true;
            TheConfectionRebirth.tileMerge[Type, ModContent.TileType<Creamstone>()] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            DustType = ModContent.DustType<NeapoliniteDust>();

            ModTranslation name = CreateMapEntryName();
            AddMapEntry(new Color(153, 96, 62), name);

            ItemDrop = ModContent.ItemType<Items.Placeable.NeapoliniteOre>();
            HitSound = SoundID.Tink;
            MineResist = 4f;
            MinPick = 180;
        }
    }
}