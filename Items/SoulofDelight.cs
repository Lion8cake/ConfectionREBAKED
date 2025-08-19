using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework;

namespace TheConfectionRebirth.Items
{
    public class SoulofDelight : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));

            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;

            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item refItem = new();
            refItem.SetDefaults(ItemID.SoulofLight);
            Item.width = refItem.width;
            Item.height = refItem.height;
            Item.value = Item.sellPrice(silver: 2);
            Item.rare = ItemRarityID.Orange;
            Item.maxStack = 9999;
        }

		public override void PostUpdate() {
			Lighting.AddLight(Item.Center, new Vector3(2.39f, 1.64f, 0.05f) * 0.22f * Main.essScale);
		}

		public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}