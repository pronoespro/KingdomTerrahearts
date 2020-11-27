using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.NPCs
{
    class shadowHeartless : BasicEnemy
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadow heartless");
            Main.npcFrameCount[npc.type] = 5;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 50;
            npc.damage = 20;
            npc.defense = 1;
            npc.knockBackResist = 1;
            npc.width = 38;
            npc.height = 30;
            npc.value = 100;
            npc.npcSlots = 0.1f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath6;

            canTeleport = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return  SpawnCondition.OverworldDaySlime.Chance;
        }

        public override void NPCLoot()
        {
            Random r = new Random();
            if (r.Next(10) <= 1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("lucidShard"), Main.rand.Next(5)+1);
            }
        }

        public override void SpecialAction()
        {

        }

        public override void SpecialAttack()
        {

        }
    }
}
