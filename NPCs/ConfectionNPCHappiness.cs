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

            nurseHappiness.SetBiomeAffection<ConfectionSurfaceBiome>(AffectionLevel.Like);
            wizardHappiness.SetBiomeAffection<ConfectionSurfaceBiome>(AffectionLevel.Like);
            partygirlHappiness.SetBiomeAffection<ConfectionSurfaceBiome>(AffectionLevel.Like);
            tavernkeepHappiness.SetBiomeAffection<ConfectionSurfaceBiome>(AffectionLevel.Like);

            clothierHappiness.SetBiomeAffection<ConfectionSurfaceBiome>(AffectionLevel.Dislike);
            witchdoctorHappiness.SetBiomeAffection<ConfectionSurfaceBiome>(AffectionLevel.Dislike);
            taxcollectorHappiness.SetBiomeAffection<ConfectionSurfaceBiome>(AffectionLevel.Dislike);
        }
    }
}
