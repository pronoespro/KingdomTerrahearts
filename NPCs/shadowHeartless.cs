using Microsoft.Xna.Framework;
using System;
using Terraria;
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
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lucidShard>(), 10, 1, 6));
        }

        public override void SpecialAction()
        {

        }

        public override void SpecialAttack()
        {

        }
    }
}
