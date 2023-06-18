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
        private int uses;

        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.damage = 32;
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
            Item.rare = 5;
            Item.shoot = Mod.Find<ModProjectile>("BakersDozen").Type;
            Item.shootSpeed = 16f;
            Item.maxStack = 13;
        }

        public override bool CanUseItem(Player player)
        {
            int stack = Item.stack;
            bool canuse = true;
            for (int m = 0; m < 1000; m++)
            {
                if (Main.projectile[m].active && Main.projectile[m].owner == Main.myPlayer && Main.projectile[m].type == Item.shoot)
                    stack -= 1;
            }
            if (stack <= 0) canuse = false;
            return canuse;
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            int index = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[index].frame = uses++ % 4;
			return false;
		}

		public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Placeable.NeapoliniteBar>(), 15)
                .AddIngredient(ModContent.ItemType<Items.SoulofDelight>(), 20)
                .AddIngredient(ItemID.SoulofMight, 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

		public override void SaveData(TagCompound tag) => tag[nameof(uses)] = uses;

		public override void LoadData(TagCompound tag) => uses = tag.GetInt(nameof(uses));

		public override ModItem Clone(Item newEntity)
		{
            var bakersDozen = (BakersDozen)base.Clone(newEntity);
            bakersDozen.uses = uses;
			return bakersDozen;
		}

		protected override bool CloneNewInstances => true;
	}
}
