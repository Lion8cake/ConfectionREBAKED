using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Placeable;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Items.Weapons
{
    public class Sucrosa : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 76;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(silver: 460);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.SucrosaSlash>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax);
            return false;
        }

		public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo) {
			Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
			ParticleSystem.AddParticle(new NeapoliniteSlash(), positionInWorld, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), 1));
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
			ParticleSystem.AddParticle(new NeapoliniteSlash(), positionInWorld, new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), 1));
		}
    }
}