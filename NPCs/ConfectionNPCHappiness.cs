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

            nurseHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Like);
            wizardHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Like);
            partygirlHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Like);
            tavernkeepHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Like);

            clothierHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Dislike);
            witchdoctorHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Dislike);
            taxcollectorHappiness.SetBiomeAffection<ConfectionBiome>(AffectionLevel.Dislike);
        }
    }
}
