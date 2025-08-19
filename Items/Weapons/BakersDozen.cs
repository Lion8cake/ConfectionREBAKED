using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TheConfectionRebirth.Items.Weapons
{
    public class BakersDozen : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.damage = 38;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 14;
            Item.useStyle = 1;
            Item.useTime = 14;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
            Item.height = 38;
            Item.value = 600000;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<Projectiles.BakersDozen>();
            Item.shootSpeed = 16f;
        }

        public override bool CanUseItem(Player player)
        {
            int stack = 13;
            bool canuse = true;
            for (int m = 0; m < 1000; m++)
            {
                if (Main.projectile[m].active && Main.projectile[m].owner == Main.myPlayer && Main.projectile[m].type == Item.shoot)
                    stack -= 1;
            }
            if (stack <= 0) canuse = false;
            return canuse;
        }
	}
}
