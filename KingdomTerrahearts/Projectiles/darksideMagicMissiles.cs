using KingdomTerrahearts.NPCs.Bosses;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Projectiles
{
    class darksideMagicMissiles:ModNPC
    {

        Player player;
        NPC target;
        float speed=5f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Missile");
            Main.npcFrameCount[npc.type] = 3;
        }

        public override void SetDefaults()
        {
            npc.width = 15;
            npc.height = 25;
            npc.aiStyle = -1;
            npc.lifeMax = 10;
            npc.damage = 25;
            npc.defense = 0;
            npc.npcSlots = 0.1f;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.velocity = new Vector2(0, -5);
        }

        public override void AI()
        {
            npc.immortal = false;
            npc.damage = 25;
            Target();
            Player p = Main.player[npc.target];

            Move(new Vector2(0, 0));
            DespawnHandler();
            npc.rotation = (float)Math.Atan2((double)npc.velocity.Y, (double)npc.velocity.X) + 1.57f;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            npc.frameCounter %= 10;
            int frame = (int)(npc.frameCounter / 2);
            if (frame >= Main.npcFrameCount[npc.type]) frame = 0;
            npc.frame.Y = frame * frameHeight;
        }

        void Target()
        {
            if (!npc.friendly)
            {
                player = Main.player[npc.target];
            }
            else
            {
                int boss = NPC.FindFirstNPC(mod.NPCType("Darkside"));
                target = Main.npc[boss];
            }
        }

        void Move(Vector2 offset)
        {
            float turnResistance = 50;
            Vector2 moveTo;
            if(npc.friendly)
                moveTo = (target.Center + offset) - npc.Center;
            else
                moveTo = (player.Center+offset)-npc.Center;
            float magnitude = Magnitude(moveTo);
            if (magnitude > speed)
            {
                moveTo *= speed / magnitude;
            }
            moveTo = (npc.velocity * turnResistance + moveTo) / (turnResistance + 1f);
            magnitude = Magnitude(moveTo);
            if (magnitude > speed)
            {
                moveTo *= speed / magnitude;
            }
            npc.velocity = moveTo;
        }

        float Magnitude(Vector2 vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        void DespawnHandler()
        {
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.timeLeft = 0;
                }
            }
        }

        public override void DrawEffects(ref Color drawColor)
        {
            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 200, -npc.velocity.X/2, -npc.velocity.Y/2);
            Main.dust[dust].color = Color.Black;
            Main.dust[dust].noGravity = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            HitPlayerInAnyWay();
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            HitPlayerInAnyWay();
            base.HitEffect(hitDirection, damage);
        }

        void HitPlayerInAnyWay()
        {
            if (npc.timeLeft > 10)
                npc.timeLeft = 10;
            for (int i = 0; i < 15; i++)
            {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, 200, Main.rand.Next(-2, 2), Main.rand.Next(-2, 2));
                Main.dust[dust].color = Color.Black;
                Main.dust[dust].noGravity = true;
            }
            if (npc.friendly)
                npc.damage = 0;
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            npc.friendly = true;
            npc.velocity = new Vector2(0, -5);
            damage = 0;
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            npc.friendly = true;
            npc.velocity = new Vector2(0, -5);
            npc.immortal = true;
        }

    }
}
