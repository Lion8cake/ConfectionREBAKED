using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Tiles
{
    public class SherbetBricks : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileLighted[Type] = true;
            HitSound = SoundID.Dig;
            AnimationFrameHeight = 90;
            Main.tileBlockLight[Type] = true;
            ItemDrop = ModContent.ItemType<Items.Placeable.SherbetBricks>();
            AddMapEntry(new Color(213, 105, 89));
            DustType = ModContent.DustType<SherbetDust>();
        }

        /*public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter > 5)
            {
                frameCounter = 0;
                frame++;
                if (frame > 12)
                {
                    frame = 0;
                }
            }
        }*/

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 2f;
            g = 1f;
            b = 1f;
        }
    }
}
