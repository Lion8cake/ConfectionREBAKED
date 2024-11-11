using Newtonsoft.Json.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
    public class Pix : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 25;
			Item.useTime = 7;
			Item.knockBack = 4.75f;
			Item.width = 20;
			Item.height = 12;
			Item.damage = 38;
			Item.pick = 200;
			Item.axe = 22;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 220000;
			Item.DamageType = DamageClass.Melee;
			Item.scale = 1.1f;
		}
    }
}