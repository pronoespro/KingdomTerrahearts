using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace KingdomTerrahearts.NPCs
{
    class armoredKnightHeartless:BasicGroundEnemy
    {
        int initAttackCooldownTime = 10;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Armored Knight heartless");
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
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath6;

            specialAttackCooldown = initAttackCooldownTime;
        }

        public override void SpecialAction()
        {
            specialActionCooldown = 0;
        }

        public override void SpecialAttack()
        {
            specialAttackTime++;
            npc.velocity.X *= 2;
            if (specialAttackTime > 5)
            {
                specialAttackTime = 0;
                specialAttackCooldown = initAttackCooldownTime;
            }
        }

    }
}
