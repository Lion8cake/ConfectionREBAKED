using Terraria;
using Terraria.GameContent.Creative;
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
            Item.CloneDefaults(ItemID.IlluminantHook);
            Item.shootSpeed = 18f;
            Item.shoot = Mod.Find<ModProjectile>("SweetHook").Type;
        }
    }
}
