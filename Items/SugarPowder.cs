using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
    public class SugarPowder : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Cleanses the evil");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }

        public override void SetDefaults()
        {
            Item.shoot = Mod.Find<ModProjectile>("SugarPowder").Type;
            Item.useStyle = 1;
            Item.shootSpeed = 4f;
            Item.width = 16;
            Item.height = 24;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.noMelee = true;
            Item.value = 75;
        }
    }
}