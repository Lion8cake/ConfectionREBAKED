using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Items.Weapons;
using TheConfectionRebirth.Tiles;

namespace TheConfectionRebirth.Items.Placeable
{
    public class HeavensForge : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 50;
            Item.height = 26;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 150;
            Item.createTile = ModContent.TileType<Tiles.HeavensForgeTile>();
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrystalShard, 10)
                .AddIngredient(ItemID.PearlstoneBlock, 30)
                .AddIngredient(ItemID.SoulofLight, 8)
                .AddTile(TileID.DemonAltar)
                .Register();

			AddAndReplace<KeyofDelight>(ItemID.LightKey);
            AddAndReplace<ConfectionBiomeKey>(ItemID.HallowedKey);
            AddAndReplace<GrandSlammer>(ItemID.Pwnhammer);
			AddAndReplace<NPCs.Critters.RoyalCherryBugItem>(ItemID.EmpressButterfly);
        }

        private static void AddAndReplace<TConf>(int hall) where TConf : ModItem
        {
            Recipe recipe = Recipe.Create(hall);
            recipe.AddIngredient(ContentInstance<TConf>.Instance.Type);
            recipe.AddTile(ModContent.TileType<HeavensForgeTile>());
			recipe.DisableDecraft();
			recipe.Register();
            recipe = Recipe.Create(ContentInstance<TConf>.Instance.Type);
            recipe.AddIngredient(hall);
            recipe.AddTile(ModContent.TileType<HeavensForgeTile>());
			recipe.DisableDecraft();
			recipe.Register();
        }

        private static void AddAndReplace<THall, TConf>() where TConf : ModItem where THall : ModItem
        {
            int ht = ContentInstance<THall>.Instance.Type;
            int ct = ContentInstance<TConf>.Instance.Type;

            Recipe recipe = Recipe.Create(ht);
            recipe.AddIngredient(ct);
            recipe.AddTile(ModContent.TileType<HeavensForgeTile>());
			recipe.DisableDecraft();
			recipe.Register();
            recipe = Recipe.Create(ct);
            recipe.AddIngredient(ht);
            recipe.AddTile(ModContent.TileType<HeavensForgeTile>());
			recipe.DisableDecraft();
			recipe.Register();
        }
    }
}