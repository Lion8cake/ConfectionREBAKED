using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheConfectionRebirth.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class NeapoliniteHeadgear : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("6% Increased Magic Damage"
                    + "\nIncreased Max manna by 80");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 10000;
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 4;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<NeapoliniteConeMail>() && legs.type == ModContent.ItemType<NeapoliniteGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "For every 50 mana consumed in the past 3 seconds," 
                + "\nsummon an additional strawberry per attack. Caps at 5 strawberries.";
            ConfectionPlayer playerFuncs = player.GetModPlayer<ConfectionPlayer>();
            playerFuncs.NeapoliniteMagicSet = true;
            int rank = playerFuncs.ManaConsumed / 50;
            if (rank > 5)
                rank = 5;
            StackableBuffData.StrawberryStrike.AscendBuff(player, rank - 1, 300);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Placeable.NeapoliniteBar>(), 12).AddTile(TileID.MythrilAnvil).Register();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.06f;
            player.statManaMax2 += 80;
        }
    }
}
