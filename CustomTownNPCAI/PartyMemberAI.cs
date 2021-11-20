using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using KingdomTerrahearts.Extra;
using Terraria.DataStructures;
using System;
using KingdomTerrahearts.Logic;

namespace KingdomTerrahearts.CustomTownNPCAI
{
    public class PartyMemberAI
    {

        public static void GuidePartyMemberAI(NPC npc, Vector2 prevPos,int projType, ref int[] npcStats)
        {
            int npcOwner = PartyMemberLogic.IsPartyMember(npcStats[6]);
            if (npcOwner >= 0)
            {
                Vector2 desiredpos = Main.player[npcOwner].Center;

                npc.ai[2]++;

                for (int i = 1; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].friendly && MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[i], npcOwner)) < 400)
                    {
                        npcStats[3] = (npcStats[3] == 0 ||
                            MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[i], npcOwner)) < MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[npcStats[3]], npcOwner)))
                            ? i : npcStats[3];
                    }
                }

                if (npcStats[3] != 0 && Main.npc[npcStats[3]].active)
                {
                    desiredpos = (Vector2.Distance(Main.npc[npcStats[3]].Center, Main.player[Main.myPlayer].Center) > 300) ? desiredpos : Main.npc[npcStats[3]].Center;

                    if (npc.ai[2] > 25)
                    {
                        ProjectileSource_NPC s = new ProjectileSource_NPC(npc);
                        Attack(ref npcStats[5], projType, MathHelp.Normalize(Main.npc[npcStats[3]].Center - npc.Center) * 15, npc.Center, s);
                        npcStats[4] = 5;
                        npc.ai[2] = 0;
                    }
                }

                desiredpos = desiredpos - npc.Center;

                if (Math.Abs(desiredpos.X) > 125 + PartyMemberLogic.GetPartySlotOcupied(npc.type)*30)
                {
                    npc.velocity = new Vector2(MathHelp.Sign(desiredpos.X) * 3, (prevPos == npc.Center) ? -10 : npc.velocity.Y);
                }
                else
                {
                    npc.velocity.X -= (Math.Abs(npc.velocity.X) < 1) ? -npc.velocity.X : MathHelp.Sign(npc.velocity.X);
                }
            }

        }

        public static void DryadPartyMemberAI(NPC npc, Vector2 prevPos, int projType, ref int[] npcStats)
        {
            int npcOwner = PartyMemberLogic.IsPartyMember(npcStats[6]);
            if (npcOwner >= 0)
            {
                Vector2 desiredpos = Main.player[npcOwner].Center;

                npc.ai[2]++;

                for (int i = 1; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].friendly && MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[i], npcOwner)) < 400)
                    {
                        npcStats[3] = (npcStats[3] == 0 ||
                            MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[i], npcOwner)) < MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[npcStats[3]], npcOwner)))
                            ? i : npcStats[3];
                    }
                }

                if (npcStats[3] != 0 && Main.npc[npcStats[3]].active)
                {
                    if (npc.ai[2] > 25)
                    {
                        ProjectileSource_NPC s = new ProjectileSource_NPC(npc);
                        Attack(ref npcStats[5], projType, MathHelp.Normalize(Main.npc[npcStats[3]].Center - npc.Center) * 15, npc.Center, s);
                        npcStats[4] = 5;
                        npc.ai[2] = 0;
                    }
                }

                desiredpos = desiredpos - npc.Center;

                if (Math.Abs(desiredpos.X) > 25 + PartyMemberLogic.GetPartySlotOcupied(npc.type) * 30)
                {
                    npc.velocity = new Vector2(MathHelp.Sign(desiredpos.X) * 3, (prevPos == npc.Center) ? -10 : npc.velocity.Y);
                }
                else
                {
                    npc.velocity.X -= (Math.Abs(npc.velocity.X) < 1) ? -npc.velocity.X : MathHelp.Sign(npc.velocity.X);
                }

            }
        }

        public static void NursePartyMemberAI(NPC npc, Vector2 prevPos, int projType, ref int[] npcStats)
        {
            int npcOwner = PartyMemberLogic.IsPartyMember(npcStats[6]);
            if (npcOwner >= 0)
            {
                Vector2 desiredpos = Main.player[npcOwner].Center;

                npc.ai[2]++;

                for (int i = 1; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].friendly && MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[i], npcOwner)) < 300 && Main.npc[i].life < Main.npc[i].lifeMax)
                    {
                        npcStats[3] = (npcStats[3] == 0 ||
                            MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[i], npcOwner)) < MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[npcStats[3]], npcOwner)))
                            ? i : npcStats[3];
                    }
                }

                if (npcStats[3] != 0 && Main.npc[npcStats[3]].active && Main.npc[npcStats[3]].life < Main.npc[npcStats[3]].lifeMax)
                {
                    if (npc.ai[2] > 25)
                    {
                        ProjectileSource_NPC s = new ProjectileSource_NPC(npc);
                        Attack(ref npcStats[5], projType, MathHelp.Normalize(Main.npc[npcStats[3]].Center - npc.Center) * 15, npc.Center, s);

                        npcStats[4] = 5;
                        npc.ai[2] = 0;
                    }
                }

                desiredpos = desiredpos - npc.Center;

                if (Math.Abs(desiredpos.X) > 150 + PartyMemberLogic.GetPartySlotOcupied(npc.type) * 30)
                {
                    npc.velocity = new Vector2(MathHelp.Sign(desiredpos.X) * 3, (prevPos == npc.Center) ? -10 : npc.velocity.Y);
                }
                else
                {
                    npc.velocity.X -= (Math.Abs(npc.velocity.X) < 1) ? -npc.velocity.X : MathHelp.Sign(npc.velocity.X);
                }

            }
        }

        public static void HurtfullNursePartyMemberAI(NPC npc, Vector2 prevPos, int projType, ref int[] npcStats)
        {
            int npcOwner = PartyMemberLogic.IsPartyMember(npcStats[6]);
            if (npcOwner >= 0)
            {
                Vector2 desiredpos = Main.player[npcOwner].Center;

                npc.ai[2]++;

                for (int i = 1; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].active && !Main.npc[i].friendly && MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[i], npcOwner)) < 400)
                    {
                        npcStats[3] = (npcStats[3] == 0 ||
                            MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[i],npcOwner)) < MathHelp.Magnitude(GetDistanceToPlayer(Main.npc[npcStats[3]], npcOwner)))
                            ? i : npcStats[3];
                    }
                }

                if (npcStats[3] != 0 && Main.npc[npcStats[3]].active)
                {
                    if (npc.ai[2] > 25)
                    {
                        ProjectileSource_NPC s = new ProjectileSource_NPC(npc);
                        Attack(ref npcStats[5], projType, MathHelp.Normalize(Main.npc[npcStats[3]].Center - npc.Center) * 15, npc.Center, s);
                        npcStats[4] = 5;
                        npc.ai[2] = 0;
                    }
                }

                desiredpos = desiredpos - npc.Center;

                if (Math.Abs(desiredpos.X) > 150 + PartyMemberLogic.GetPartySlotOcupied(npc.type)*30)
                {
                    npc.velocity = new Vector2(MathHelp.Sign(desiredpos.X) * 3, (prevPos == npc.Center) ? -10 : npc.velocity.Y);
                }
                else
                {
                    npc.velocity.X -= (Math.Abs(npc.velocity.X) < 1) ? -npc.velocity.X : MathHelp.Sign(npc.velocity.X);
                }

            }
        }

        public static Vector2 GetDistanceToPlayer(NPC npc,int npcType)
        {
            int npcOwner = PartyMemberLogic.IsPartyMember(npcType);
            if (npcOwner >= 0)
            {
                return npc.Center - Main.player[npcOwner].Center;
            }
            return npc.Center - Main.player[Main.myPlayer].Center;
        }
        
        public static void Attack(ref int curCombo,int projType,Vector2 direction,Vector2 originalpos,IProjectileSource s)
        {
            curCombo++;

            if (curCombo >= 0)
            {
                int createdProj=Projectile.NewProjectile(s, originalpos,direction, projType, 20, 1, Owner: Main.myPlayer);
                Main.projectile[createdProj].hostile = false;
                Main.projectile[createdProj].friendly = true;
                if (curCombo > 10)
                {
                    curCombo = -15;
                    Vector2 randPos;
                    for(int i = 0; i < 5; i++)
                    {
                        randPos = new Vector2(Main.rand.Next(-15, 15), Main.rand.Next(-15, 15));
                        createdProj= Projectile.NewProjectile(s, originalpos+ randPos, direction, projType, 20, 1, Owner: Main.myPlayer);
                        Main.projectile[createdProj].hostile = false;
                        Main.projectile[createdProj].friendly = true;
                    }
                }
            }
        }

    }
}
