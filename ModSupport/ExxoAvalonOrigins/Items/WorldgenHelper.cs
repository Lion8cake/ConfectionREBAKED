/*using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.ModSupport.ExxoAvalonOrigins.Items;

class WorldgenHelper : ModItem
{
    public override bool IsLoadingEnabled(Mod mod)
    {
        return ModLoader.TryGetMod("AvalonTesting", out _);
    }

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("WorldGen Helper");
        Tooltip.SetDefault("Use this item to generate a pre-set structure at your location\nDO NOT USE IN NORMAL GAMEPLAY - IT WILL OVERWRITE BLOCKS");
    }

    public override void SetDefaults()
    {
        Item.rare = ItemRarityID.Purple;
        Item.width = 30;
        Item.maxStack = 1;
        Item.useAnimation = Item.useTime = 30;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = 0;
        Item.height = 30;
    }

    public override bool? UseItem(Player player)
    {
        World.ConfectedAltars.Generate();
        return true;
    }
}*/
