using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Buffs.NeapoliniteBuffs
{
    public class StrawberryStrikeV : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<StrawberryStrike>()] < 5)
            {
                Projectile.NewProjectile(new EntitySource_Misc("Strawberry Strike V 1"), player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<StrawberryStrike>(), 50, 8f, player.whoAmI);
                Projectile.NewProjectile(new EntitySource_Misc("Strawberry Strike V 2"), player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<StrawberryStrike>(), 50, 8f, player.whoAmI);
                Projectile.NewProjectile(new EntitySource_Misc("Strawberry Strike V 3"), player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<StrawberryStrike>(), 50, 8f, player.whoAmI);
                Projectile.NewProjectile(new EntitySource_Misc("Strawberry Strike V 4"), player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<StrawberryStrike>(), 50, 8f, player.whoAmI);
                Projectile.NewProjectile(new EntitySource_Misc("Strawberry Strike V 5"), player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<StrawberryStrike>(), 50, 8f, player.whoAmI);
            }
        }
    }
}
