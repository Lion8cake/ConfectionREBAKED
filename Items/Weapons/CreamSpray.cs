using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Weapons
{
    public class CreamSpray : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 21;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 6;
            Item.useAnimation = 26;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = 400000;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item17;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<CreamySpray>();
            Item.shootSpeed = 16f;
        }
    }
}