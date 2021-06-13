using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using KingdomTerrahearts.Extra;
using System;

namespace KingdomTerrahearts.CustomTownNPCAI
{
    public class PartyMemberAI
    {

        public static void GuidePartyMemberAI(NPC npc, Vector2 prevPos,int projType, ref int[] npcStats)
        {
            Vector2 desiredpos=Main.player[Main.myPlayer].Center;

            npc.ai[2]++;

            for (int i = 1; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && !Main.npc[i].friendly && MathHelp.Magnitude( GetDistanceToPlayer(Main.npc[i]))<400)
                {
                    npcStats[3] = (npcStats[3] == 0 ||
                        MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[i])) < MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[npcStats[3]])))
                        ? i : npcStats[3];
                }
            }

            if (npcStats[3] != 0 && Main.npc[npcStats[3]].active)
            {
                desiredpos = (Vector2.Distance(Main.npc[npcStats[3]].Center, Main.player[Main.myPlayer].Center) > 300) ? desiredpos : Main.npc[npcStats[3]].Center;

                if (npc.ai[2] > 25)
                {
                    Projectile.NewProjectile(npc.Center, MathHelp.Normalize(Main.npc[npcStats[3]].Center - npc.Center) * 25, projType, 25, 25, Owner: Main.myPlayer);
                    npcStats[4] = 5;
                    npc.ai[2] = 0;
                }
            }

            desiredpos = desiredpos - npc.Center;

            if (Math.Abs(desiredpos.X) > 75)
            {
                npc.velocity = new Vector2(MathHelp.Sign(desiredpos.X)*3, (prevPos == npc.Center) ? -10 : npc.velocity.Y);
            }
            else
            {
                npc.velocity.X -= (Math.Abs(npc.velocity.X)<1)?-npc.velocity.X: MathHelp.Sign(npc.velocity.X);
            }

        }

        public static void DryadPartyMemberAI(NPC npc, Vector2 prevPos, int projType, ref int[] npcStats)
        {
            Vector2 desiredpos = Main.player[Main.myPlayer].Center;

            npc.ai[2]++;

            for (int i = 1; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && !Main.npc[i].friendly && MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[i])) < 400)
                {
                    npcStats[3] = (npcStats[3] == 0 ||
                        MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[i])) < MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[npcStats[3]])))
                        ? i : npcStats[3];
                }
            }

            if (npcStats[3] != 0 && Main.npc[npcStats[3]].active)
            {
                if (npc.ai[2] > 25)
                {
                    Projectile.NewProjectile(npc.Center, MathHelp.Normalize(Main.npc[npcStats[3]].Center - npc.Center) * 15, projType, 45, 25, Owner: Main.myPlayer);
                    npcStats[4] = 5;
                    npc.ai[2] = 0;
                }
            }

            desiredpos = desiredpos - npc.Center;

            if (Math.Abs(desiredpos.X) > 75)
            {
                npc.velocity = new Vector2(MathHelp.Sign(desiredpos.X) * 3, (prevPos == npc.Center) ? -10 : npc.velocity.Y);
            }
            else
            {
                npc.velocity.X -= (Math.Abs(npc.velocity.X) < 1) ? -npc.velocity.X : MathHelp.Sign(npc.velocity.X);
            }

        }

        public static void NursePartyMemberAI(NPC npc, Vector2 prevPos, int projType, ref int[] npcStats)
        {
            Vector2 desiredpos = Main.player[Main.myPlayer].Center+new Vector2(Main.player[Main.myPlayer].direction*10,0);

            npc.ai[2]++;

            for (int i = 1; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].friendly && MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[i])) < 300 && Main.npc[i].life<Main.npc[i].lifeMax)
                {
                    npcStats[3] = (npcStats[3] == 0 ||
                        MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[i])) < MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[npcStats[3]])))
                        ? i : npcStats[3];
                }
            }

            if (npcStats[3] != 0 && Main.npc[npcStats[3]].active && Main.npc[npcStats[3]].life < Main.npc[npcStats[3]].lifeMax)
            {
                if (npc.ai[2] > 25)
                {
                    Projectile.NewProjectile(npc.Center, MathHelp.Normalize(Main.npc[npcStats[3]].Center - npc.Center) * 15, projType, 0, 0);

                    npcStats[4] = 5;
                    npc.ai[2] = 0;
                }
            }

            desiredpos = desiredpos - npc.Center;

            if (Math.Abs(desiredpos.X) > 75)
            {
                npc.velocity = new Vector2(MathHelp.Sign(desiredpos.X) * 3, (prevPos == npc.Center) ? -10 : npc.velocity.Y);
            }
            else
            {
                npc.velocity.X -= (Math.Abs(npc.velocity.X) < 1) ? -npc.velocity.X : MathHelp.Sign(npc.velocity.X);
            }

        }

        public static void HurtfullNursePartyMemberAI(NPC npc, Vector2 prevPos, int projType, ref int[] npcStats)
        {
            Vector2 desiredpos = Main.player[Main.myPlayer].Center + new Vector2(Main.player[Main.myPlayer].direction * 10, 0);

            npc.ai[2]++;

            for (int i = 1; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && !Main.npc[i].friendly && MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[i])) < 400)
                {
                    npcStats[3] = (npcStats[3] == 0 ||
                        MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[i])) < MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[npcStats[3]])))
                        ? i : npcStats[3];
                }
            }

            if (npcStats[3] != 0 && Main.npc[npcStats[3]].active)
            {
                if (npc.ai[2] > 25)
                {
                    Projectile.NewProjectile(npc.Center, MathHelp.Normalize(Main.npc[npcStats[3]].Center - npc.Center) * 15, projType, 45, 25, Owner: Main.myPlayer);
                    npcStats[4] = 5;
                    npc.ai[2] = 0;
                }
            }

            desiredpos = desiredpos - npc.Center;

            if (Math.Abs(desiredpos.X) > 75)
            {
                npc.velocity = new Vector2(MathHelp.Sign(desiredpos.X) * 3, (prevPos == npc.Center) ? -10 : npc.velocity.Y);
            }
            else
            {
                npc.velocity.X -= (Math.Abs(npc.velocity.X) < 1) ? -npc.velocity.X : MathHelp.Sign(npc.velocity.X);
            }

        }

        public static Vector2 GetDistanceToPlayer(NPC npc)
        {
            return npc.Center - Main.player[Main.myPlayer].Center;
        }

    }
}
