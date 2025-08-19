using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
    public class SweetHook : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
			Item.noUseGraphic = true;
			Item.damage = 0;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 15f;
			Item.width = 18;
			Item.height = 28;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.rare = ItemRarityID.LightPurple;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 6);
            Item.shoot = ModContent.ProjectileType<Projectiles.SweetHook>();
        }
    }
}
