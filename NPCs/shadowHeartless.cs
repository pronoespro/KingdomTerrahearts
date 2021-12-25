using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace KingdomTerrahearts.NPCs
{
    public class shadowHeartless : BasicGroundEnemy
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadow heartless");
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("Heartless that emerge from various places" +
                "\nThey sneak up to their enemies and strike them with sharp claws" +
                "\nThey are extremely tenacious, often chasing their prey to the end")
            });
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 50;
            NPC.damage = 20;
            NPC.defense = 1;
            NPC.knockBackResist = 1;
            NPC.width = 38;
            NPC.height = 30;
            NPC.value = 100;
            NPC.npcSlots = 0.1f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;

            canTeleport = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return  SpawnCondition.OverworldDaySlime.Chance;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.writhingShard>(), 3, 1, 6));
        }

        public override void SpecialAction()
        {

        }

        public override void SpecialAttack()
        {

        }
    }
}
