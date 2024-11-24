using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
    public class CreamwoodHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
			Item.autoReuse = true;
			Item.useStyle = 1;
			Item.useTurn = true;
			Item.useAnimation = 29;
			Item.useTime = 19;
			Item.hammer = 55;
			Item.width = 24;
			Item.height = 28;
			Item.damage = 10;
			Item.knockBack = 5.5f;
			Item.scale = 1f;
			Item.UseSound = SoundID.Item1;
			Item.value = 50;
			Item.DamageType = DamageClass.Melee;
		}
    }
}
