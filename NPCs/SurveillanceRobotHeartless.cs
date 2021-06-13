using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.NPCs
{
    class SurveillanceRobotHeartless:ModNPC
    {

        float maxDistance=250;
        float speed = 5;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Watcher Heartless");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 30;
            npc.damage = 0;
            npc.defense = 0;
            npc.knockBackResist = 1;
            npc.width = 31;
            npc.height = 42;
            npc.scale = 1.25f;
            npc.value = 10;
            npc.npcSlots = 0.1f;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
        }
        public override bool PreAI()
        {
            return true;
        }
        public override void AI()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.TargetClosest(true);

            Player target = Main.player[npc.target];

            Vector2 dir = target.position - npc.position;
            if (Math.Abs(dir.X)>maxDistance|| Math.Abs(dir.Y) > maxDistance)
            {
                npc.velocity = dir / magnitude(dir)*speed;
                npc.ai[0] =(npc.ai[0]<120)? 120:npc.ai[0];
            }
            else
            {
                if(npc.direction!= (dir / magnitude(dir)).X)
                    npc.velocity = dir / magnitude(dir) * speed;
                float yvel = target.position.Y - npc.position.Y;
                if (Math.Abs(yvel) > speed/2)
                {
                    npc.velocity.Y = yvel / Math.Abs(yvel) * speed/2;
                }
                else
                {
                    npc.velocity = new Vector2();
                }
                npc.ai[0]++;
                if (npc.ai[0] > 180)
                {
                    Shoot();
                }
            }
            npc.direction = (npc.velocity.X==0)?npc.direction: (npc.velocity.X > 0) ? 1 : -1;

        }

        public void Shoot()
        {

            Vector2 projVel =new Vector2(npc.direction,0);
            Projectile newProj=Projectile.NewProjectileDirect(npc.Center, projVel*2, ProjectileID.DeathLaser, 3, 0.5f);
            newProj.friendly = false;
            newProj.timeLeft = 175;
            newProj.scale = 0.85f;

            npc.ai[0]=0;
        }

        public float magnitude(Vector2 vector)
        {
            return Math.Abs(vector.X) + Math.Abs(vector.Y);
        }

        public override void FindFrame(int frameHeight)
        {
            npc.spriteDirection = npc.direction;
            npc.frame.Y = frameHeight * 0;
        }

    }
}
