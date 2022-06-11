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

            nurseHappiness.SetBiomeAffection<ConfectionBiomeSurface>(AffectionLevel.Like);
            wizardHappiness.SetBiomeAffection<ConfectionBiomeSurface>(AffectionLevel.Like);
            partygirlHappiness.SetBiomeAffection<ConfectionBiomeSurface>(AffectionLevel.Like);
            tavernkeepHappiness.SetBiomeAffection<ConfectionBiomeSurface>(AffectionLevel.Like);

            clothierHappiness.SetBiomeAffection<ConfectionBiomeSurface>(AffectionLevel.Dislike);
            witchdoctorHappiness.SetBiomeAffection<ConfectionBiomeSurface>(AffectionLevel.Dislike);
            taxcollectorHappiness.SetBiomeAffection<ConfectionBiomeSurface>(AffectionLevel.Dislike);
        }
    }
}
