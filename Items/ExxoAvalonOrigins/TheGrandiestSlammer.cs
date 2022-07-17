using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.ExxoAvalonOrigins;

class TheGrandiestSlammer : ModItem
{
    public Mod avalon;
    public override bool IsLoadingEnabled(Mod mod)
    {
        return ModLoader.TryGetMod("AvalonTesting", out avalon);
    }

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Grandiest Slammer");
        Tooltip.SetDefault("Strong enough to destroy Confected Altars");
        SacrificeTotal = 1;
    }

    public override void SetDefaults()
    {
        Item.damage = 30;
        Item.autoReuse = true;
        Item.hammer = 100;
        Item.useTurn = true;
        Item.scale = 1f;
        Item.rare = ItemRarityID.Lime;
        Item.width = 30;
        Item.useTime = 10;
        Item.knockBack = 12f;
        Item.DamageType = DamageClass.Melee;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.sellPrice(0, 2, 0, 0);
        Item.useAnimation = 17;
        Item.height = 30;
        Item.UseSound = SoundID.Item1;
    }
}
