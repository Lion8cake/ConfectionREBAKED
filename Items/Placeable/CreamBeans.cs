using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Placeable
{
    public class CreamBeans : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.useStyle = 1;
            Item.useAnimation = 15;
            Item.value = Item.buyPrice(silver: 20);
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.createTile = Mod.Find<ModTile>("CreamGrass").Type;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            // WorldGen.destroyObject = false;
            TileID.Sets.BreakableWhenPlacing[0] = false;
            return false;
        }
    }
}
