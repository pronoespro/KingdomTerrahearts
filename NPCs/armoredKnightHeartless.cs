using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;

namespace KingdomTerrahearts.NPCs
{
    class armoredKnightHeartless:BasicGroundEnemy
    {
        int initAttackCooldownTime = 10;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Armored Knight heartless");
            Main.npcFrameCount[NPC.type] = 5;

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
                              // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
                              // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("Armored to the bone, this heartless carries a sword and is weak to a sun rising.\nWill only appear in really large groups alongsode Surveilance Robots.")
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
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath6;

            specialAttackCooldown = initAttackCooldownTime;
        }

        public override void SpecialAction()
        {
            specialActionCooldown = 0;
        }

        public override void SpecialAttack()
        {
            specialAttackTime++;
            NPC.velocity.X *= 2;
            if (specialAttackTime > 5)
            {
                specialAttackTime = 0;
                specialAttackCooldown = initAttackCooldownTime;
            }
        }

    }
}
