using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;


namespace TheConfectionRebirth.ModSupport.ExxoAvalonOrigins.Items;

class ConfectedAltar : ModItem
{
    public Mod avalon;
    public override bool IsLoadingEnabled(Mod mod)
    {
        return ModLoader.TryGetMod("Avalon", out avalon);
    }

    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Confected Altar");
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.consumable = true;
        Item.createTile = ModContent.TileType<ModSupport.ExxoAvalonOrigins.Tiles.ConfectedAltar>();
        Item.rare = ItemRarityID.Pink;
        Item.width = 30;
        Item.useTime = 20;
        Item.maxStack = 99;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 20;
        Item.height = 30;
    }
}
