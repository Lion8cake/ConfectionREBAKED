using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Dusts;
using TheConfectionRebirth.Projectiles;

namespace TheConfectionRebirth.Items.Archived
{
	public class TrueDeathsRaze : ModItem, IArchived
	{
		public override void SetDefaults()
		{
			Item.damage = 85;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 15;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 500000;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item1;
			//Item.shoot = Mod.Find<ModProjectile>("TrueIchorBolt").Type;
			Item.shootSpeed = 10f;
		}

		public int ArchivatesTo() => ItemID.TrueNightsEdge;
	}
}