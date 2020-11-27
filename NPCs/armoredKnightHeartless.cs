using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace KingdomTerrahearts.NPCs
{
    class armoredKnightHeartless:BasicEnemy
    {
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
        }

        public override void SpecialAction()
        {

        }

        public override void SpecialAttack()
        {

        }

        public override void FindFrame(int frameHeight)
        {
            curFrame = 0;
            npc.frame.Y = frameHeight * curFrame;
        }
    }
}
