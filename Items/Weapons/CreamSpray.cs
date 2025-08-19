using Newtonsoft.Json.Linq;
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
            Item.ResearchUnlockCount = 1;
			Item.staff[Type] = true;
		}

        public override void SetDefaults()
        {
            Item.damage = 44;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 16;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 26;
            Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
            Item.knockBack = 6f;
            Item.value = 400000;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item69;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<CookieBoulder>();
            Item.shootSpeed = 10f;
		}
    }
}