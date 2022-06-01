using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using TheConfectionRebirth.Biomes;

namespace TheConfectionRebirth.NPCs
{
    public class ConfectionNPCHappiness : GlobalNPC
    {
        public override void SetStaticDefaults()
        {
            var nurseHappiness = NPCHappiness.Get(NPCID.Nurse);
            var wizardHappiness = NPCHappiness.Get(NPCID.Wizard);
            var partygirlHappiness = NPCHappiness.Get(NPCID.PartyGirl);
            var tavernkeepHappiness = NPCHappiness.Get(550);

            var clothierHappiness = NPCHappiness.Get(NPCID.Clothier);
            var witchdoctorHappiness = NPCHappiness.Get(NPCID.WitchDoctor);
            var taxcollectorHappiness = NPCHappiness.Get(NPCID.TaxCollector);

            nurseHappiness.SetBiomeAffection<ConfectionBiomeBiome>(AffectionLevel.Like);
            wizardHappiness.SetBiomeAffection<ConfectionBiomeBiome>(AffectionLevel.Like);
            partygirlHappiness.SetBiomeAffection<ConfectionBiomeBiome>(AffectionLevel.Like);
            tavernkeepHappiness.SetBiomeAffection<ConfectionBiomeBiome>(AffectionLevel.Like);

            clothierHappiness.SetBiomeAffection<ConfectionBiomeBiome>(AffectionLevel.Dislike);
            witchdoctorHappiness.SetBiomeAffection<ConfectionBiomeBiome>(AffectionLevel.Dislike);
            taxcollectorHappiness.SetBiomeAffection<ConfectionBiomeBiome>(AffectionLevel.Dislike);
        }
    }
}
