using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Weapons
{
    public class AncientCosmicCookieCannon : CosmicCookieCannon
    {
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.Placeable.NeapoliniteBar>(), 12)
                .AddIngredient(ItemID.StarCannon)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}