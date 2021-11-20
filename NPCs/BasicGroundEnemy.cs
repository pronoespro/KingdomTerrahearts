using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.NPCs
{
    public abstract class BasicGroundEnemy:ModNPC
    {

        public float desiredVel = 0;
        public float maxVel = 2;
        public float jumpForce = 8;
        public bool followPlayers = true;
        public bool canTeleport;
        public int curFrame = 0;
        public int walkcycle = 30;

        public Vector2 lastPos = new Vector2();
        public float notMovedTime = 0;
        public float specialActionTime,specialAttackTime;
        public float specialActionCooldown,specialAttackCooldown;

        public override void AI()
        {

            NPC.TargetClosest(false);
            Player p = Main.player[NPC.target];

            NPC.immortal = false;

            Vector2 moveTo = p.Center - NPC.Center;

            if (NPC.wet)
            {
                NPC.velocity.Y = -jumpForce;
            }

            if (followPlayers)
            {

                desiredVel += Math.Sign(moveTo.X) * 0.5f;
                desiredVel = (Math.Abs(desiredVel) > maxVel / 5) ? Math.Sign(desiredVel) * maxVel / 5 : desiredVel;

                NPC.velocity.X += desiredVel;
                if (Math.Abs(NPC.velocity.X) > maxVel) NPC.velocity.X = Math.Sign(NPC.velocity.X) * maxVel;

                if (lastPos == NPC.Center)
                {
                    NPC.velocity.Y = -jumpForce;
                }
                if (lastPos.X == NPC.Center.X)
                {
                    notMovedTime++;
                    if (notMovedTime > 120)
                    {
                        followPlayers = false;
                        notMovedTime = 0;
                    }
                }


            }
            else
            {

                NPC.velocity.X += -moveTo.X / Math.Abs(moveTo.X) * .25f;
                NPC.velocity.X = (Math.Abs(NPC.velocity.X) > 2) ? NPC.velocity.X / Math.Abs(NPC.velocity.X) * 2 : NPC.velocity.X;

                if (lastPos == NPC.Center)
                {
                    NPC.velocity.Y = -jumpForce;
                }

                notMovedTime++;
                if (notMovedTime > 120)
                {
                    followPlayers = true;
                    notMovedTime = 0;
                }

            }

            if (Math.Abs(NPC.velocity.X) > 0)
            {
                NPC.direction = (int)(Math.Abs(NPC.velocity.X)/NPC.velocity.X);
            }
            if (NPC.velocity.Y > 0)
            {
                curFrame = 4;
            }
            else if (NPC.velocity.Y < 0)
            {
                curFrame = 3;
            }
            else
            {
                curFrame = (curFrame < 3) ? curFrame : 0;
            }

            lastPos = NPC.Center;

            specialActionCooldown--;
            if (specialActionCooldown < 0) SpecialAction();
            specialAttackCooldown--;
            if (specialAttackCooldown < 0) SpecialAttack();

        }

        public abstract void SpecialAction();

        public abstract void SpecialAttack();

        public override void FindFrame(int frameHeight)
        {
            if (curFrame < 3)
            {
                walkcycle--;
                walkcycle = (walkcycle < -30) ? 30 : walkcycle;
                int walk = 3;
                if (walkcycle > -15) walk = 2;
                if (walkcycle > 0) walk = 1;
                if (walkcycle > 15) walk = 0;

                switch (walk)
                {
                    case 0:
                    case 2:
                        curFrame = 0;
                        break;
                    case 1:
                        curFrame = 1;
                        break;
                    case 3:
                    default:
                        curFrame = 2;
                        break;
                }
            }

            NPC.frame.Y = frameHeight * curFrame;
            NPC.spriteDirection = NPC.direction;
        }
    }
}
