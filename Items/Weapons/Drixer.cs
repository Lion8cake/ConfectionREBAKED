using Newtonsoft.Json.Linq;
using System.Threading.Channels;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
    public class Drixer : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 25;
			Item.useTime = 7;
			Item.shootSpeed = 36f;
			Item.knockBack = 4.75f;
			Item.width = 20;
			Item.height = 12;
			Item.damage = 38;
			Item.pick = 200;
			Item.axe = 22;
			Item.UseSound = SoundID.Item23;
			Item.shoot = ModContent.ProjectileType<Projectiles.Drixer>();
			Item.rare = ItemRarityID.LightRed;
			Item.value = 220000;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.channel = true;
		}
    }
}