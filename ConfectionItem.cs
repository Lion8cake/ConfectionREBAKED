using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth
{
    public class ConfectionItem : GlobalItem
    {
        public override void OnConsumeMana(Item item, Player player, int manaConsumed)
        {
            Vector2 velocity = Main.MouseWorld - player.Center;
            velocity.Normalize();
            velocity *= 4;
            const float rotPerIter = MathF.PI / 6;
            StackableBuffData.StrawberryStrike.FindBuff(player, out byte rank);
            float initialRot = (rank - 1) * -rotPerIter / 2;
            while (rank > 0)
            {
                Vector2 vel = velocity.RotatedBy(initialRot);
                Projectile.NewProjectile(item.GetSource_FromThis(), player.Center, vel, ModContent.ProjectileType<StrawberryStrike>(), item.damage / 2, 8f, player.whoAmI);
                rank--;
                initialRot += rotPerIter;
            }
        }
    }
}
