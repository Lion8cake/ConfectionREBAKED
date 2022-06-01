using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Tiles
{
    class PipCage : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
            TileObjectData.addTile(Type);

            AnimationFrameHeight = 36;

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Pip Cage");
            AddMapEntry(new Color(122, 217, 232), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, ModContent.ItemType<PipCageItem>());
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            Tile tile = Main.tile[i, j];
            Main.critterCage = true;
            int left = i - tile.TileFrameX / 18;
            int top = j - tile.TileFrameY / 18;
            int offset = left / 3 * (top / 3);
            offset %= Main.cageFrames;
            frameYOffset = Main.grasshopperCageFrame[offset] * AnimationFrameHeight;
        }
    }

    internal class PipCageItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pip Cage");
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GrasshopperCage);
            Item.createTile = ModContent.TileType<PipCage>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(null, "PipItem", 1).AddIngredient(ItemID.Terrarium, 1).Register();
        }
    }
}
