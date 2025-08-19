using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
    public class SacchariteDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {

            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 26;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.height = 30;

            Item.shoot = ModContent.ProjectileType<Projectiles.SacchariteDart>();
            Item.shootSpeed = 22f;
            Item.knockBack = 4;
            Item.value = 10;
            Item.rare = ItemRarityID.Orange;
            Item.ammo = 283;
        }
    }
}