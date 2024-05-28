using Newtonsoft.Json.Linq;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items
{
    public class SugarPowder : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.ViciousPowder;
			ItemID.Sets.ShimmerTransformToItem[ItemID.ViciousPowder] = ModContent.ItemType<SugarPowder>();
		}

        public override void SetDefaults()
        {
			Item.shoot = ModContent.ProjectileType<Projectiles.SugarPowder>();
            Item.useStyle = 1;
            Item.shootSpeed = 4f;
            Item.width = 16;
            Item.height = 24;
            Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
            Item.UseSound = SoundID.Item1;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.noMelee = true;
            Item.value = 75;
			
		}
	}
}
