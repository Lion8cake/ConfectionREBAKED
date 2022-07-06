using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Buffs.NeapoliniteBuffs;

namespace TheConfectionRebirth.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class NeapoliniteHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("8% Increased Summoner Damage"
                    + "\nIncreased Max number of minions by 3");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 10000;
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<NeapoliniteConeMail>() && legs.type == ModContent.ItemType<NeapoliniteGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Every 8 seconds since you were last hit your whip speed is increased by 10% for the first 3 times before giving 10% critical stike chance on the 4th time and 10% damage on the 5th.";
            player.GetModPlayer<ConfectionPlayer>().NeapoliniteSummonerSet = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<Items.Placeable.NeapoliniteBar>(), 12).AddTile(TileID.MythrilAnvil).Register();
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.08f;
            player.maxMinions++;
            player.maxMinions++;
            player.maxMinions++;
        }
    }
}