using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Mounts;

namespace TheConfectionRebirth.Items
{
    public class PixieStick : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pixie Stick");
            Tooltip.SetDefault("Summons a Pixie Stick to ride on");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = 250000;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item79;
            Item.noMelee = true;
            Item.mountType = ModContent.MountType<PixieStickMount>();
        }
    }
}