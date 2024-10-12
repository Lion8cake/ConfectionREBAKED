using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class LinedConfectionGemWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            WallID.Sets.Conversion.NewWall3[Type] = true;
            Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<SacchariteDust>();
            AddMapEntry(new Color(91, 109, 103));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.LinedConfectionGemWall>());
        }
    }
}