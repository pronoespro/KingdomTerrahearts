using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.DataStructures;

namespace KingdomTerrahearts.Logic
{
    public class CutsceneLogic:ModSystem
    {

        public static CutsceneLogic instance;

        public override void Load()
        {
            instance = this;
            CutsceneReset();
        }

        public override void Unload()
        {
            doneCutscenes = new bool[0];
            EndCutscene();
            instance = null;
        }

        //General cutscene stuff
        public int currentCutscene = -1;
        public bool[] doneCutscenes = new bool[50];
        public int[] cutscenesToSave = {0 };
        public int[] noCheckIfDoneCutscenes = new int[] {4,5 };

        public int cutsceneProgress=0;

        //Keyblade Finishers stuff
        public List<NPC> enemiesTargetedOn=new List<NPC>();
        public int curEnemyTargeted;

        public int weaponDamage;

        public override void SaveWorldData(TagCompound tag)
        {
            for(int i = 0; i < cutscenesToSave.Length; i++)
            {
                if (doneCutscenes[cutscenesToSave[i]])
                {
                    tag.Add("doneCutscene"+i, true);
                }
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            for(int i = 0; i < cutscenesToSave.Length; i++)
            {
                if (tag.ContainsKey("doneCutscene" + i))
                {
                    doneCutscenes[i] = true;
                }
            }
        }

        public override void PostUpdateNPCs()
        {
            if (KingdomTerrahearts.canDoCutscenes)
            {
                if (NPC.AnyNPCs(NPCID.Guide))
                {
                    //ChangeCutscene(0);
                }
                if (NPC.AnyNPCs(NPCID.EyeofCthulhu))
                {
                    ChangeCutscene(1);
                }
                if (NPC.AnyNPCs(NPCID.MoonLordCore) && NPC.AnyNPCs(NPCID.MoonLordHead))
                {
                    ChangeCutscene(2);
                }
                if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Org13.xion_finalPhase>()))
                {
                    ChangeCutscene(3);
                }

                DoCutscene();
            }
        }

        public void ChangeCutscene(int cutsceneNum,int damage=0)
        {
            if (!IsNoCheck(cutsceneNum))
            {
                ResizeDoneCutscenes(cutsceneNum);

                if (doneCutscenes[cutsceneNum] || currentCutscene >= 0)
                {
                    return;
                }
                doneCutscenes[cutsceneNum] = true;
            }

            currentCutscene = cutsceneNum;
            cutsceneProgress = 0;
            enemiesTargetedOn.Clear();
            curEnemyTargeted = -1;
            weaponDamage = damage;

        }

        public bool IsNoCheck(int cutsceneNum)
        {
            for(int i = 0; i < noCheckIfDoneCutscenes.Length; i++)
            {
                if (noCheckIfDoneCutscenes[i] == cutsceneNum)
                {
                    return true;
                }
            }
            return false;
        }

        public void ResizeDoneCutscenes(int maxNum)
        {
            if (maxNum >= doneCutscenes.Length)
            {
                bool[] newDoneCutscenes = new bool[maxNum + 1];
                for(int i = 0; i < doneCutscenes.Length; i++)
                {
                    newDoneCutscenes[i] = doneCutscenes[i];
                }
                for(int i = doneCutscenes.Length; i < newDoneCutscenes.Length; i++)
                {
                    newDoneCutscenes[i] = false;
                }
                doneCutscenes = newDoneCutscenes;
            }

        }

        public void DoCutscene()
        {
            cutsceneProgress++;
            NPC[] actors=new NPC[0];
            Player p = Main.CurrentPlayer;
            SoraPlayer sora = p.GetModPlayer<SoraPlayer>();
            NPCOverride globNPCBehav;

            switch (currentCutscene)
            {
                default:
                    EndCutscene();
                    break;
                case 0:
                    actors = new NPC[] { GetNPCFromType(NPCID.Guide) };
                    sora.ControlDuringCutscene();
                    globNPCBehav = actors[0].GetGlobalNPC<NPCOverride>();

                    if (actors[0] == null){
                        EndCutscene();
                        break;
                    }else if (cutsceneProgress > 500) {
                        actors[0].Teleport(new Vector2(actors[0].homeTileX, actors[0].homeTileY)*16);
                        actors[0].immortal = false;
                        globNPCBehav.SetCutsceneActor(false);
                        EndCutscene();
                        break;
                    }

                    actors[0].immortal = true;
                    actors[0].aiAction = 0;
                    actors[0].ai[0]=actors[0].ai[1]=0;
                    actors[0].direction = (int)MathHelp.Sign(Main.CurrentPlayer.Center.X- actors[0].Center.X);

                    globNPCBehav.SetCutsceneActor(true);

                    if (cutsceneProgress == 1)
                    {
                        actors[0].Center = Main.CurrentPlayer.Center + new Vector2(550, 0);
                    }
                    else if (cutsceneProgress < 300)
                    {
                        if (Vector2.Distance(actors[0].Center, Main.CurrentPlayer.Center) > Main.CurrentPlayer.width * 2)
                        {
                            actors[0].Center = new Vector2(actors[0].Center.X-4, p.Center.Y);
                            actors[0].velocity = new Vector2(0.1f * MathHelp.Sign((actors[0].Center.X - Main.CurrentPlayer.Center.X)), 0);
                        }
                        else
                        {
                            actors[0].Center = new Vector2(Main.CurrentPlayer.Center.X + Main.CurrentPlayer.width * 1.9f * MathHelp.Sign((actors[0].Center.X - Main.CurrentPlayer.Center.X)), p.Center.Y);
                            actors[0].velocity = Vector2.Zero;
                        }
                    }
                    else
                    {
                        actors[0].Center = new Vector2(Main.CurrentPlayer.Center.X+Main.CurrentPlayer.width*1.9f*MathHelp.Sign((actors[0].Center.X-Main.CurrentPlayer.Center.X)), p.Center.Y);
                        actors[0].velocity = Vector2.Zero;
                    }
                    
                    break;
                case 1:
                    actors = new NPC[] { GetNPCFromType(NPCID.EyeofCthulhu) };
                    sora.ControlDuringCutscene();
                    globNPCBehav = actors[0].GetGlobalNPC<NPCOverride>();

                    if (actors[0] == null)
                    {
                        sora.ModifyCutsceneCamera(Vector2.Zero,camPercentChange:100);
                        EndCutscene();
                        break;
                    }
                    else if (cutsceneProgress > 300)
                    {
                        actors[0].immortal = false;
                        globNPCBehav.SetCutsceneActor(false);
                        sora.ModifyCutsceneCamera(Vector2.Zero,camPercentChange:100);
                        EndCutscene();
                        break;
                    }

                    actors[0].immortal = true;
                    actors[0].aiAction = 0;
                    actors[0].ai[0] = actors[0].ai[1] = 0;
                    actors[0].direction = (int)MathHelp.Sign(Main.CurrentPlayer.Center.X - actors[0].Center.X);

                    sora.ModifyCutsceneCamera(actors[0].Center - p.Center,2-(cutsceneProgress-300f)/300f, Math.Clamp((cutsceneProgress - 300f) / 300*2-1f,0,30),shakeSpeed:1.2f,camPercentChange:25);

                    globNPCBehav.SetCutsceneActor(true);

                    if (cutsceneProgress == 1)
                    {
                        actors[0].Center = p.Center + new Vector2(550f * Math.Sign(actors[0].Center.X - p.Center.X), -450f);
                        p.direction = Math.Sign(actors[0].Center.X - p.Center.X);
                    }
                    else if (cutsceneProgress > 150)
                    {
                        Lighting.AddLight(actors[0].Center, 1f, 1f, 1f);
                        actors[0].velocity = Vector2.Zero;
                        actors[0].rotation =MathHelp.Lerp(actors[0].rotation,(float)Math.Atan2((p.Center.Y-actors[0].Center.Y),-(p.Center.X-actors[0].Center.X)),0.35f) * Math.Sign(actors[0].Center.X - p.Center.X);
                    }
                    else
                    {
                        actors[0].rotation += (float)Math.Clamp(Math.PI/2*(1f/4f-cutsceneProgress/900f/3f*6f),0,(float)Math.PI)*Math.Sign(actors[0].Center.X-p.Center.X);
                        Lighting.AddLight(actors[0].Center, 1f, 1f, 1f);
                        actors[0].velocity = Vector2.Zero;
                    }

                    break;
                case 2:
                    actors = new NPC[] { GetNPCFromType(NPCID.MoonLordCore),GetNPCFromType(NPCID.MoonLordHead) };
                    sora.ControlDuringCutscene();
                    globNPCBehav = actors[0].GetGlobalNPC<NPCOverride>();

                    if (actors[0] == null)
                    {
                        sora.ModifyCutsceneCamera(Vector2.Zero);
                        EndCutscene();
                        break;
                    }
                    else if (cutsceneProgress > 100)
                    {
                        actors[0].immortal = false;
                        globNPCBehav.SetCutsceneActor(false);
                        sora.ModifyCutsceneCamera(Vector2.Zero);
                        EndCutscene();
                        break;
                    }

                    actors[0].immortal = true;
                    actors[0].aiAction = 0;
                    actors[0].ai[0] = actors[0].ai[1] = 0;
                    actors[0].direction = (int)MathHelp.Sign(Main.CurrentPlayer.Center.X - actors[0].Center.X);

                    sora.ModifyCutsceneCamera(actors[1].Center - p.Center,zoom:0.75f,camPercentChange:25);

                    globNPCBehav.SetCutsceneActor(true);

                    if (cutsceneProgress == 1)
                    {
                        p.direction = Math.Sign(actors[0].Center.X - p.Center.X);
                    }

                    break;
                case 3:
                    actors = new NPC[] { GetNPCFromType(ModContent.NPCType<NPCs.Bosses.Org13.xion_finalPhase>()) };
                    sora.ControlDuringCutscene();
                    globNPCBehav = actors[0].GetGlobalNPC<NPCOverride>();

                    if (actors[0] == null)
                    {
                        sora.ModifyCutsceneCamera(Vector2.Zero, zoom: -1,camPercentChange:100);
                        EndCutscene();
                        break;
                    }
                    else if (cutsceneProgress > 300)
                    {
                        actors[0].immortal = false;
                        globNPCBehav.SetCutsceneActor(false);
                        sora.ModifyCutsceneCamera(Vector2.Zero, zoom: -1,camPercentChange:100);
                        EndCutscene();
                        break;
                    }

                    actors[0].immortal = true;
                    actors[0].aiAction = 0;
                    actors[0].ai[0] = actors[0].ai[1] = 0;
                    actors[0].direction = (int)MathHelp.Sign(Main.CurrentPlayer.Center.X - actors[0].Center.X);


                    globNPCBehav.SetCutsceneActor(true);

                    if (cutsceneProgress == 1)
                    {
                        actors[0].Center = p.Center + new Vector2(0, -250f);
                        p.direction = Math.Sign(actors[0].Center.X - p.Center.X);
                    }
                    else if (cutsceneProgress > 150)
                    {
                        Lighting.AddLight(actors[0].Center, 1f, 1f, 1f);
                        actors[0].velocity = Vector2.Zero;
                        actors[0].rotation = MathHelp.Lerp(actors[0].rotation, (float)Math.Atan2((p.Center.Y - actors[0].Center.Y), -(p.Center.X - actors[0].Center.X)), 0.35f) * Math.Sign(actors[0].Center.X - p.Center.X);
                        sora.ModifyCutsceneCamera(actors[0].Center - p.Center, 2 - (cutsceneProgress - 300f) / 300f,0.000001f);
                    }
                    else
                    {
                        actors[0].rotation += (float)Math.Clamp(Math.PI / 2 * (1f / 3f - cutsceneProgress / 900f / 3f * 6f), 0, (float)Math.PI) * Math.Sign(actors[0].Center.X - p.Center.X);
                        Lighting.AddLight(actors[0].Center, 1f, 1f, 1f);
                        actors[0].velocity = Vector2.Zero;
                        sora.ModifyCutsceneCamera(actors[0].Center - p.Center, 2 - (cutsceneProgress - 300f) / 300f, Math.Clamp((cutsceneProgress - 300f) / 300 * 2 - 1f, 0, 30));
                    }

                    break;
                case 4:

                    actors = new NPC[0];
                    sora.ControlDuringCutscene();

                    if (cutsceneProgress > 20)
                    {
                        sora.ModifyCutsceneCamera(Vector2.Zero, zoom: -1, camPercentChange: 100);
                        EndCutscene();
                        break;
                    }
                    else if (cutsceneProgress == 1)
                    {
                        EntitySource_ItemUse s = new EntitySource_ItemUse(p,p.HeldItem);
                        Projectile.NewProjectile(s, sora.Player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.guardExpandProjectile>(), weaponDamage * 5, 1);
                    }
                    else
                    {
                        sora.ModifyCutsceneCamera(Vector2.Zero, zoom: 3, camPercentChange: 100);
                    }

                    break;
                case 5:

                    actors = new NPC[] { };
                    sora.ControlDuringCutscene();

                    if (cutsceneProgress > 7)
                    {
                        sora.ModifyCutsceneCamera(Vector2.Zero, zoom: -1, camPercentChange: 100);
                        EndCutscene();
                        break;
                    }

                    if (enemiesTargetedOn.Count == 0)
                    {
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            if (Main.npc[i].active && !Main.npc[i].townNPC && !Main.npc[i].CountsAsACritter)
                            {
                                enemiesTargetedOn.Add(Main.npc[i]);
                            }
                        }
                    }
                    if (cutsceneProgress > 5)
                    {
                        EntitySource_ItemUse s = new EntitySource_ItemUse(p, p.HeldItem);
                        for (int i = enemiesTargetedOn.Count;i>=0 ; i--)
                        {
                            Projectile.NewProjectile(s, sora.Player.Center, MathHelp.Normalize(enemiesTargetedOn[i].Center - sora.Player.Center) * 15, ProjectileID.JestersArrow, weaponDamage * 5, 0.5f, sora.Player.whoAmI);
                            p.velocity = Vector2.Zero;
                            enemiesTargetedOn.RemoveAt(i);
                        }
                    }


                    break;
            }
        }

        public void CutsceneReset()
        {
            doneCutscenes = new bool[doneCutscenes.Length];
            for(int i = 0; i < doneCutscenes.Length; i++)
            {
                doneCutscenes[i] = false;
            }
        }

        public void EndCutscene()
        {
            currentCutscene = -1;
            cutsceneProgress = 0;
        }

        public NPC GetNPCFromType(int type)
        {
            for(int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].type == type)
                {
                    return Main.npc[i];
                }
            }
            return null;
        }

    }
}
