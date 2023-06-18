using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheConfectionRebirth.Tiles
{
    class GummyWormCage : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSmallCage);
            TileObjectData.addTile(Type);

            AnimationFrameHeight = 36;

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(122, 217, 232), name);
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            Tile tile = Main.tile[i, j];
            Main.critterCage = true;
            int left = i - tile.TileFrameX / 18;
            int top = j - tile.TileFrameY / 18;
            int offset = left / 3 * (top / 3);
            offset %= Main.cageFrames;
            frameYOffset = Main.wormCageFrame[offset] * AnimationFrameHeight;
        }
    }

    internal class GummyWormCageItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WormCage);
            Item.createTile = ModContent.TileType<GummyWormCage>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(null, "GummyWormItem", 1).AddIngredient(ItemID.Terrarium, 1).Register();
        }
    }
}
