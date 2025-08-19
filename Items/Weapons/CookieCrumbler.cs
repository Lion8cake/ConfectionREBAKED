using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;
using Terraria;

namespace TheConfectionRebirth.Items.Weapons
{
    public class CookieCrumbler : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 18;
            Item.useTime = 24;
            Item.shootSpeed = 3.7f;
            Item.knockBack = 6.5f;
            Item.width = 32;
            Item.height = 32;
            Item.scale = 1f;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(silver: 460);
            Item.crit = 23;

            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<CookiePikelet>();
            Item.shootSpeed = 10f;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
    }
}