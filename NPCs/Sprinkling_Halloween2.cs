using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;

namespace TheConfectionRebirth.NPCs
{
    public class Sprinkling_Halloween2 : Sprinkling
    {
		public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 10;
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, new NPCID.Sets.NPCBestiaryDrawModifiers
			{
				Hide = true
			});
		}

		public override void AI()
		{
			SprinklingAI_Variants(2);
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[Type] = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[ModContent.NPCType<Sprinkling>()];
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			return SprinklingDrawing(2, spriteBatch, drawColor, screenPos);
		}
	}
}