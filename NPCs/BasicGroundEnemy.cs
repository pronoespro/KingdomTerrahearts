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

            npc.TargetClosest(false);
            Player p = Main.player[npc.target];

            npc.immortal = false;

            Vector2 moveTo = p.Center - npc.Center;

            if (npc.wet)
            {
                npc.velocity.Y = -jumpForce;
            }

            if (followPlayers)
            {

                desiredVel += Math.Sign(moveTo.X) * 0.5f;
                desiredVel = (Math.Abs(desiredVel) > maxVel / 5) ? Math.Sign(desiredVel) * maxVel / 5 : desiredVel;

                npc.velocity.X += desiredVel;
                if (Math.Abs(npc.velocity.X) > maxVel) npc.velocity.X = Math.Sign(npc.velocity.X) * maxVel;

                if (lastPos == npc.Center)
                {
                    npc.velocity.Y = -jumpForce;
                }
                if (lastPos.X == npc.Center.X)
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

                npc.velocity.X += -moveTo.X / Math.Abs(moveTo.X) * .25f;
                npc.velocity.X = (Math.Abs(npc.velocity.X) > 2) ? npc.velocity.X / Math.Abs(npc.velocity.X) * 2 : npc.velocity.X;

                if (lastPos == npc.Center)
                {
                    npc.velocity.Y = jumpForce;
                }

                notMovedTime++;
                if (notMovedTime > 120)
                {
                    followPlayers = true;
                    notMovedTime = 0;
                }

            }

            if (Math.Abs(npc.velocity.X) > 0)
            {
                //npc.direction = (int)(Math.Abs(npc.velocity.X)/npc.velocity.X);
            }
            if (npc.velocity.Y > 0)
            {
                curFrame = 4;
            }
            else if (npc.velocity.Y < 0)
            {
                curFrame = 3;
            }
            else
            {
                curFrame = (curFrame < 3) ? curFrame : 0;
            }

            lastPos = npc.Center;

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

            npc.frame.Y = frameHeight * curFrame;
            //npc.spriteDirection = -npc.direction;
        }
    }
}
