using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Archived
{
    public class SacchariteWorkBench : ModItem, IArchived
    {

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 22;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.consumable = true;
            Item.value = 0;
            Item.createTile = Mod.Find<ModTile>("SacchariteWorkbench").Type;
        }

        public int ArchivatesTo() => Mod.Find<ModItem>("SacchariteWorkbench").Type;
    }
}
