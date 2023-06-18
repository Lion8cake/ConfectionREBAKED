using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.ModSupport.Thorium.Items
{
    public class ConfectedCharm : ModItem
    {
		public Mod thorium;
		public override bool IsLoadingEnabled(Mod mod) {
			return ModLoader.TryGetMod("ThoriumMod", out thorium);
		}

		public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
			Item.value = 2500;
			Item.rare = 4;
        }

        public override void AddRecipes()
        {
            CreateRecipe(3)
                .AddIngredient<Sprinkles>(3)
				.AddIngredient<SoulofDelight>(1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}