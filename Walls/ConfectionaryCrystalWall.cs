using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Walls
{
    public class ConfectionaryCrystalWall : ModWall
    {
        public override void SetStaticDefaults()
        {
			WallID.Sets.Conversion.NewWall2[Type] = true;
			Main.wallHouse[Type] = true;
            DustType = ModContent.DustType<SacchariteDust>();
            AddMapEntry(new Color(45, 104, 117));
			RegisterItemDrop(ModContent.ItemType<Items.Placeable.ConfectionaryCrystalsWall>());
        }
    }
}