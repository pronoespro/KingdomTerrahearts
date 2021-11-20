using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.UI;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;

namespace KingdomTerrahearts.NPCs
{
    public class RedNocturne:ModNPC
    {

        float speed = 5;
        float speedReduction = 0.75f;

        float hitRotation=0;
        float rotationSpeed = 10;

        float attackTimer = 25;
        float targetRange = 250;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Nocturne");

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

        public override void SetDefaults()
        {
            NPC.lifeMax = 50;
            NPC.aiStyle = -1;
            NPC.scale = 0.65f;
            NPC.damage = 0;
            NPC.defense = 2;
            NPC.knockBackResist = 20f;
            NPC.width = 34;
            NPC.height = 70;
            NPC.value = 100;
            NPC.npcSlots = 0.1f;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            attackTimer = Main.rand.Next(15, 45);
        }

        public override void AI()
        {
            NPC.TargetClosest(true);
            Player p = Main.player[NPC.target];

            float distToPlayer= Vector2.Distance(NPC.Center, p.Center);
            Vector2 pRelDist = p.Center - NPC.Center;

            if (hitRotation > 0)
            {
                NPC.rotation = hitRotation / rotationSpeed;
                hitRotation--;
                NPC.velocity *= speedReduction;
            }
            else
            {
                if (p == null || distToPlayer<targetRange)
                {
                    attackTimer--;
                    if (attackTimer > 0)
                    {
                        NPC.rotation = (float)(Math.Sin(Main.time / 10) / 6);
                        NPC.velocity *= speedReduction;
                    }
                    else if (attackTimer > -100/((Main.expertMode)?4:1))
                    {
                        NPC.rotation = (float)(Math.Sin(Main.time / 2) / 6);
                        NPC.velocity *= speedReduction;
                    }
                    else
                    {

                        ProjectileSource_NPC s = new ProjectileSource_NPC(NPC);

                        pRelDist = MathHelp.Normalize(pRelDist) * speed/4;
                        int proj=Projectile.NewProjectile(s,NPC.Center, pRelDist, ProjectileID.Fireball, 10,1) ;
                        Main.projectile[proj].scale = 0.25f;
                        Main.projectile[proj].tileCollide =!Main.expertMode;
                        Main.projectile[proj].timeLeft =100*((Main.expertMode)?2:1);
                        attackTimer = 25*((Main.expertMode)?2:1);
                    }
                }
                else
                {
                    NPC.velocity = MathHelp.Normalize(p.Center - NPC.Center) * speed;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Underworld.Chance/2+SpawnCondition.SolarTower.Chance;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.blazingShard>(),5,1,6));

            if (Main.player[NPC.target].Center.Y > Main.UnderworldLayer)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Hellstone,1,1,5));
            }


        }

        public override void HitEffect(int hitDirection, double damage)
        {
            hitRotation = rotationSpeed*1.75f;
        }

        public override void FindFrame(int frameHeight)
        {

        }

    }
}
