using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

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
        }

        public override void SetDefaults()
        {
            npc.lifeMax = 50;
            npc.aiStyle = -1;
            npc.scale = 0.65f;
            npc.damage = 0;
            npc.defense = 2;
            npc.knockBackResist = 20f;
            npc.width = 34;
            npc.height = 70;
            npc.value = 100;
            npc.npcSlots = 0.1f;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.noGravity = true;
            npc.noTileCollide = true;
            attackTimer = Main.rand.Next(15, 45);
        }

        public override void AI()
        {
            npc.TargetClosest(true);
            Player p = Main.player[npc.target];

            float distToPlayer= Vector2.Distance(npc.Center, p.Center);
            Vector2 pRelDist = p.Center - npc.Center;

            if (hitRotation > 0)
            {
                npc.rotation = hitRotation / rotationSpeed;
                hitRotation--;
                npc.velocity *= speedReduction;
            }
            else
            {
                if (p == null || distToPlayer<targetRange)
                {
                    attackTimer--;
                    if (attackTimer > 0)
                    {
                        npc.rotation = (float)(Math.Sin(Main.time / 10) / 6);
                        npc.velocity *= speedReduction;
                    }
                    else if (attackTimer > -100/((Main.expertMode)?4:1))
                    {
                        npc.rotation = (float)(Math.Sin(Main.time / 2) / 6);
                        npc.velocity *= speedReduction;
                    }
                    else
                    {
                        pRelDist = MathHelp.Normalize(pRelDist) * speed/4;
                        int proj=Projectile.NewProjectile(npc.Center, pRelDist, ProjectileID.Fireball, 10,1) ;
                        Main.projectile[proj].scale = 0.25f;
                        Main.projectile[proj].tileCollide =!Main.expertMode;
                        Main.projectile[proj].timeLeft =100*((Main.expertMode)?2:1);
                        attackTimer = 25*((Main.expertMode)?2:1);
                    }
                }
                else
                {
                    npc.velocity = MathHelp.Normalize(p.Center - npc.Center) * speed;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Underworld.Chance+SpawnCondition.SolarTower.Chance;
        }

        public override void NPCLoot()
        {
            Random r = new Random();
            if (r.Next(5) <= 3)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("blazingShard"), Main.rand.Next(5) + 1);
            }
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Hellstone, Main.rand.Next(3)+1);
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
