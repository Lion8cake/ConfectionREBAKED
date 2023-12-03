using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
    public class Trifle : ModItem
    {
		private int ShotAmount = 0;

		private int SecondSlotUsed = 55;

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

		public override void SetDefaults()
        {
            Item.damage = 23;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 40;
            Item.height = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item31;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.shootSpeed = 16f;
            Item.value = 300000;
            Item.useAmmo = AmmoID.Bullet;
            Item.useAnimation = 12;
            Item.useTime = 4;
            Item.reuseDelay = 14;
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			ShotAmount++;
			if (ShotAmount == 1) {
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback);
				player.ChooseAmmo(player.HeldItem).stack--;
			}
			else if (ShotAmount == 2) {
				int FailedSlots = 0;
				for (int StackSlot = 55; StackSlot < 58; StackSlot++) {
					if (player.inventory[StackSlot].ammo == AmmoID.Bullet) {
						player.inventory[StackSlot].stack--;
						Projectile.NewProjectile(source, position, velocity, player.inventory[StackSlot].shoot, damage, knockback);
						SecondSlotUsed = StackSlot;
						break;
					}
					else {
						FailedSlots++;
					}
				}
				if (FailedSlots == 3) {
					Projectile.NewProjectile(source, position, velocity, type, damage, knockback);
					player.ChooseAmmo(player.HeldItem).stack--;
				}
				
			}
			else if (ShotAmount == 3) {
				int FailedSlots2 = 0;
				for (int StackSlot2 = 56; StackSlot2 < 58; StackSlot2++) {
					if (player.inventory[StackSlot2].ammo == AmmoID.Bullet && StackSlot2 != SecondSlotUsed) {
						player.inventory[StackSlot2].stack--;
						Projectile.NewProjectile(source, position, velocity, player.inventory[StackSlot2].shoot, damage, knockback);
						break;
					}
					else {
						FailedSlots2++;
					}
				}
				if (FailedSlots2 == 2) {
					Projectile.NewProjectile(source, position, velocity, type, damage, knockback);
					player.ChooseAmmo(player.HeldItem).stack--;
				}
				ShotAmount = 0;
			}
			if (!player.HasAmmo(player.HeldItem)) {
				ShotAmount = 0;
			}
			return false;
		}

		public override bool CanConsumeAmmo(Item ammo, Player player) {
			return false;
		}

		public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ClockworkAssaultRifle)
                .AddIngredient(ItemID.IllegalGunParts)
                .AddIngredient<SoulofDelight>(15)
                .AddIngredient<Sprinkles>(20)
                .AddIngredient(ItemID.SoulofMight, 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
