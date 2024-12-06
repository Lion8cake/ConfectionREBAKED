using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;

namespace TheConfectionRebirth.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class SacchariteWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(130, 6.75f);
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.value = 400000;
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

		public override bool WingUpdate(Player player, bool inUse)
		{
			int dustType = ModContent.DustType<NeapoliniteCrumbs>();
			bool noLightEmittence = player.wingsLogic != player.wings;
			if (!inUse)
			{
				if (player.wingsLogic > 0 && player.controlJump && player.velocity.Y > 0f && !player.mount.CanHover() && !(player.mount.CanFly() && player.controlJump && player.jump == 0) && !(player.slowFall && !player.TryingToHoverDown) && !(player.rocketDelay > 0))
				{
					if (player.velocity.Y > 0f)
					{
						if (Main.rand.NextBool(10))
						{
							int addedPos = 4;
							if (player.direction == 1)
							{
								addedPos = -40;
							}
							int dustID = Dust.NewDust(new Vector2(player.position.X + (float)(player.width / 2) + (float)addedPos, player.position.Y + (float)(player.height / 2) - 12f), 30, 20, dustType, 0f, 0f);
							Dust dust = Main.dust[dustID];
							dust.noLightEmittence = noLightEmittence;
							dust.velocity *= 0.3f;
							dust.shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
						}
					}
				}
			}
			else
			{
				if (Main.rand.NextBool(4))
				{
					int addedPos = 4;
					if (player.direction == 1)
					{
						addedPos = -40;
					}
					int dustID = Dust.NewDust(new Vector2(player.position.X + (float)(player.width / 2) + (float)addedPos, player.position.Y + (float)(player.height / 2) - 15f), 30, 30, dustType, 0f, 0f);
					Dust dust = Main.dust[dustID];
					dust.velocity *= 0.3f;
					dust.noLightEmittence = noLightEmittence;
					dust.shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
				}
			}
			return false;
		}
	}
}