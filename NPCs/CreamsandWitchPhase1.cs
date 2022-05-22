using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;
using TheConfectionRebirth.Items;
using TheConfectionRebirth.Items.Armor;
using TheConfectionRebirth.Items.Placeable;

namespace TheConfectionRebirth.NPCs
{
    public class CreamsandWitchPhase1 : ModNPC
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Creamsand Witch");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 40;
            NPC.damage = 85;
            NPC.defense = 15;
            NPC.lifeMax = 1200;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 22;
            AIType = NPCID.FloatyGross;
            Banner = NPC.type;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<SandConfectionSurfaceBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

                new FlavorTextBestiaryInfoElement("A witch that uses her sugary powers do defeat anyone that get near her home land of the confection desert.")
            });
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;
        }

        /*public override void AI()
	{
		NPC.ai[0] += 1f;
		if (Main.rand.NextBool(1000) && NPC.CountNPCS(ModContent.NPCType<CrookedCookie>()) < 25)
		{
			NPC.ai[0] = 0f;
			int i = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CrookedCookie>(), 0, NPC.whoAmI);
			Main.npc[i].velocity.X = Main.rand.NextFloat(-0.4f, 0.4f);
			Main.npc[i].velocity.Y = Main.rand.NextFloat(-0.5f, -0.05f);
		}
		NPC.ai[0] += 2f;
		if (Main.rand.NextBool(1000) && NPC.CountNPCS(ModContent.NPCType<MintJr>()) < 25)
		{
			NPC.ai[0] = 0f;
			int i = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<MintJr>(), 0, NPC.whoAmI);
			Main.npc[i].velocity.X = Main.rand.NextFloat(-0.4f, 0.4f);
			Main.npc[i].velocity.Y = Main.rand.NextFloat(-0.5f, -0.05f);
		}
	}*/

        /*public override void HitEffect(int hitDirection, double damage) {
                if (NPC.life <= 0) {
                    Vector2 spawnAt = NPC.Center + new Vector2(0f, (float)NPC.height / 2f);
                    NPC.NewNPC((int)spawnAt.X, (int)spawnAt.Y, ModContent.NPCType<CreamsandWitchPhase2>());
                }
            }*/

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(5, ModContent.ItemType<CreamHat>(), ModContent.ItemType<CookieCorset>(), ModContent.ItemType<CakeDress>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Creamsand>(), 1, 30, 50));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PixieStick>(), 10));
            //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CreamySandwhich>(), 10));
        }
    }
}
