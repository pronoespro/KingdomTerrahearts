using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria.Localization;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using KingdomTerrahearts.Extra;
using Terraria.Audio;

namespace KingdomTerrahearts.NPCs.TownNPCs
{

    public enum SoraHeldVanityItemType
    {
        none,
        phone,
        sword,
        keyblade,
        kairisCharm
    }

    public class SoraHeldVanityItem
    {
        public SoraHeldVanityItemType type;
        public Vector2 offset;
        public float rotation;
        public Vector2 scale;
    }

    [AutoloadHead]
    public class Sora_first : ModNPC
    {

        public override string Texture => "KingdomTerrahearts/NPCs/TownNPCs/sora_kh1";
        public override string HeadTexture => "KingdomTerrahearts/NPCs/TownNPCs/soraSymbol";

        EntitySource_Parent s;
        bool firstTalked;

        int viewRange = 1000;
        int aiWanderTypeAmmount = 10,aiAttackTypeAmmount=5;

        int npcTarget;
        int curAi;
        int frame;
        float aiProgress;
        int heldProjectile=-1;
        int heldProjType=-1;
        int heldProjDamage;
        SoraHeldVanityItem heldVanity;
        int rangedCloseupAttack;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Keyblade Wielder");
            Main.npcFrameCount[NPC.type] = 26;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 1500;
            NPC.defense = 25;
            NPC.knockBackResist = 1;
            NPC.width = 40;
            NPC.height = 56;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.townNPC = true;
            s = new EntitySource_Parent(NPC);
            NPC.friendly = true;
            heldVanity = new SoraHeldVanityItem();
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return NPC.downedBoss1;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return (spawnInfo.PlayerInTown && !NPC.AnyNPCs(NPC.type) && NPC.downedBoss1) ? 10 : 0;
        }

        public override void AI()
        {

            NPC.TargetClosest(false);

            if (NPC.homeless)
            {

                if (NPC.target >= 0 && Main.player[NPC.target].active)
                {
                    npcTarget = TargetClosestEnemy(false);

                    if (npcTarget >= 0 && Main.npc[npcTarget].active)
                    {
                        AttackEnemies();
                    }
                    else
                    {
                        WanderAround();
                    }

                    aiProgress++;
                }

            }
            else
            {


                if (NPC.target >= 0 && Main.player[NPC.target].active)
                {
                    CheckHeldProjectile();
                    npcTarget = TargetClosestEnemy(false);

                    if (npcTarget >= 0 && Main.npc[npcTarget].active)
                    {
                        AttackEnemies();
                    }
                    else
                    {
                        npcTarget = TargetClosestEnemy(true);

                        if (npcTarget >= 0 && Main.npc[npcTarget].active)
                        {
                            AttackEnemies();
                        }
                        else
                        {
                            if (Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) < NPC.width + NPC.height)
                            {
                                CloseToPlayer();
                            }
                            else
                            {
                                WanderAround();
                            }
                        }
                    }
                }
                else
                {
                    WanderAround();
                }
                aiProgress++;
            }
        }

        public void CloseToPlayer()
        {
            switch (curAi)
            {
                default:
                    NPC.rotation = 0;
                    NPC.velocity = Vector2.Zero;
                    frame = 0;
                    NPC.direction = MathF.Sign(NPC.Center.X - Main.player[NPC.target].Center.X);
                    heldVanity.type = SoraHeldVanityItemType.none;

                    if (aiProgress > 100)
                    {
                        aiProgress = 0;
                        curAi = Main.rand.Next(0, aiWanderTypeAmmount);
                    }
                    break;
                case 3:
                    NPC.rotation = 0;
                    npcTarget = GetClosestTownNPC();
                    if (npcTarget >= 0 && Vector2.Distance(Main.npc[npcTarget].Center, NPC.Center) < Vector2.Distance(Main.player[NPC.target].Center, NPC.Center))
                    {
                        NPC.direction = MathF.Sign(Main.npc[npcTarget].Center.X - NPC.Center.X);
                    }
                    else
                    {
                        NPC.direction = MathF.Sign(Main.player[NPC.target].Center.X - NPC.Center.X);
                        if (aiProgress == 50)
                        {
                            SoundEngine.PlaySound(SoundID.Camera, NPC.Center);
                        }
                    }

                    frame = 17;

                    heldVanity.type = SoraHeldVanityItemType.phone;
                    heldVanity.scale = new Vector2(1, 1);
                    heldVanity.offset = new Vector2(20, 0);

                    if (aiProgress > 100)
                    {
                        aiProgress = 0;
                        curAi = 0;
                    }

                    break;
            }
        }

        public void WanderAround()
        {
            heldProjectile = -1;
            heldProjType = -1;
            if (Main.dayTime)
            {
                NPC.stairFall = true;
                switch (curAi)
                {
                    default:
                        if (NPC.homeless)
                        {
                            aiProgress = 0;
                            curAi = Main.rand.Next(1, aiWanderTypeAmmount);
                            break;
                        }

                        NPC.rotation = 0;
                        if (NPC.direction == 0)
                        {
                            NPC.direction = 1;
                        }
                        heldVanity.type = SoraHeldVanityItemType.none;
                        if (Vector2.Distance(new Vector2(NPC.homeTileX, NPC.homeTileY).ToWorldCoordinates(), NPC.Center) > viewRange && aiProgress>10)
                        {
                            NPC.Teleport(new Vector2(NPC.homeTileX, NPC.homeTileY - 1).ToWorldCoordinates() - new Vector2(NPC.width, NPC.height), TeleportationStyleID.RodOfDiscord);
                            NPC.velocity = Vector2.Zero;
                        }
                        else
                        {
                            NPC.velocity = (Collision.SolidCollision(NPC.position, NPC.width, NPC.height+2, false)) ? Vector2.Zero:(NPC.velocity + new Vector2(0, 1));
                            frame = 0;
                        }

                        if (aiProgress > 25)
                        {
                            aiProgress = 0;
                            curAi = Main.rand.Next(1, aiWanderTypeAmmount);
                        }
                        break;
                    case 1:
                        NPC.rotation = 0;
                        if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height+2,true))
                        {
                            if (Math.Abs(NPC.direction)<1f){
                                NPC.direction = 1;
                            }
                            NPC.direction *= -1;
                            NPC.velocity = new Vector2(-3*NPC.direction, -5);
                            frame = 0;
                        }
                        else
                        {
                            if (NPC.velocity.Y <= 0)
                            {
                                frame =3;
                            }else{
                                frame =1;
                            }
                            NPC.velocity = NPC.velocity+ new Vector2(0, 0.1f);
                        }

                        if (aiProgress > 100)
                        {
                            aiProgress = 0;
                            curAi = 0;
                        }
                        break;
                    case 2:
                        if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height + 2, true))
                        {
                            NPC.rotation = 0;
                            NPC.velocity = new Vector2(0, -5);
                            frame = 0;

                            if (aiProgress > 10)
                            {
                                aiProgress = 0;
                                curAi =0;
                            }
                        }
                        else
                        {
                            if (NPC.velocity.Y <= 0)
                            {
                                frame = 3;
                            }
                            else
                            {
                                frame = 1;
                            }
                            NPC.velocity = NPC.velocity + new Vector2(0, 0.1f);
                            NPC.rotation += MathF.PI * 0.1f;
                        }
                        break;
                    case 3:
                        NPC.rotation = 0;

                        npcTarget = GetClosestTownNPC();
                        if (npcTarget >= 0 && Vector2.Distance(Main.npc[npcTarget].Center,NPC.Center)<Vector2.Distance(Main.player[NPC.target].Center,NPC.Center))
                        {
                            NPC.direction = MathF.Sign(Main.npc[npcTarget].Center.X - NPC.Center.X);
                        }
                        else
                        {
                            NPC.direction = MathF.Sign(Main.player[NPC.target].Center.X - NPC.Center.X);
                            if (aiProgress == 50)
                            {
                                SoundEngine.PlaySound(SoundID.Camera, NPC.Center);
                            }
                        }

                        frame = 17;

                        heldVanity.type = SoraHeldVanityItemType.phone;
                        heldVanity.scale = new Vector2(1, 1);
                        heldVanity.offset = new Vector2(20, 0);

                        if (aiProgress > 100)
                        {
                            aiProgress = 0;
                            curAi = 0;
                        }

                        break;
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        NPC.rotation = 0;
                        Vector2 pos = NPC.position + new Vector2((NPC.direction < 0 ? NPC.width+1 : -1), NPC.height)+ NPC.velocity;

                        bool changeDirection = true;

                        OpenAndCloseDoors();

                        for (int i = -3; i < 3; i++)
                        {
                            if (Collision.SolidCollision(pos+new Vector2(0,i).ToWorldCoordinates(),2,2, true) &&
                            !Collision.SolidCollision(pos + new Vector2(0, i-1).ToWorldCoordinates(), 2, 2, false) &&
                            !Collision.SolidCollision(pos + new Vector2(0, i-2).ToWorldCoordinates(), 2, 2, false) && 
                            !Collision.SolidCollision(pos + new Vector2(0, i-3).ToWorldCoordinates(), 2, 2,false))
                            {
                                changeDirection = false;
                                break;
                            }
                            if (Main.tile[pos.ToTileCoordinates().X, pos.ToTileCoordinates().Y].TileType == TileID.OpenDoor)
                            {
                                changeDirection = false;
                            }
                        }

                        if(Vector2.Distance(NPC.Center,new Vector2(NPC.homeTileX, NPC.homeTileY).ToWorldCoordinates()) > viewRange * 0.75f 
                            && MathF.Sign(NPC.Center.X-NPC.homeTileX)!=NPC.direction)
                        {
                            changeDirection = true;
                        }

                        if (changeDirection)
                        {
                            NPC.direction *= -1;
                        }

                        if (Collision.SolidCollision(NPC.position, NPC.width, NPC.height + 2, false))
                        {
                            NPC.frameCounter++;
                            NPC.Center += new Vector2(-NPC.direction, 0);

                            frame = (int)(2 + NPC.frameCounter / 3 % 13);

                        }
                        else{
                            NPC.velocity = new Vector2(NPC.velocity.X, NPC.velocity.Y + 0.1f);
                            if (NPC.velocity.Y <= 0)
                            {
                                frame = 3;
                            }
                            else
                            {
                                frame = 1;
                            }
                        }

                        if(Collision.SolidCollision(NPC.position+new Vector2(0,NPC.height/2f), NPC.width, NPC.height/2, false))
                        {
                            NPC.Center += new Vector2(0, -2);
                        }

                        if (aiProgress > 400)
                        {
                            aiProgress = 0;
                            curAi = 0;
                        }

                        break;
                }
            }
            else
            {
                if (aiProgress % 20 == 0 && Vector2.Distance(new Vector2(NPC.homeTileX, NPC.homeTileY).ToWorldCoordinates(), NPC.Center) > viewRange)
                {
                    NPC.Teleport(new Vector2(NPC.homeTileX, NPC.homeTileY-1).ToWorldCoordinates() - new Vector2(NPC.width, NPC.height) , TeleportationStyleID.RodOfDiscord);
                    NPC.velocity = Vector2.Zero;
                }
                frame = 0;
                HoldProjectile(-1);
                heldVanity.type = SoraHeldVanityItemType.none;
            }
        }

        public void OpenAndCloseDoors()
        {
            Point p = (NPC.Center + new Vector2((NPC.width / 2 + 1) * -NPC.direction, 0)).ToTileCoordinates();
            if (Main.tile[p.X, p.Y].TileType == TileID.ClosedDoor)
            {
                WorldGen.OpenDoor(p.X, p.Y, -NPC.direction);
            }

            p = (NPC.Center + new Vector2((NPC.width / 2 + 1) * NPC.direction, 0)).ToTileCoordinates();
            if (Main.tile[p.X, p.Y].TileType == TileID.OpenDoor)
            {
                WorldGen.CloseDoor(p.X, p.Y);
            }
        }

        public int GetClosestTownNPC()
        {
            int selected = -1;

            for(int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].townNPC && (selected<0 || Vector2.Distance(Main.npc[i].Center,NPC.Center)<Vector2.Distance(Main.npc[selected].Center,NPC.Center)) && Vector2.Distance(Main.npc[i].Center,NPC.Center)<viewRange && Collision.CanHitLine(NPC.Center,5,5,Main.npc[i].Center,Main.npc[i].width,Main.npc[i].height))
                {
                    selected = i;
                }
            }

            return selected;
        }

        public void AttackEnemies()
        {
            heldVanity.type = SoraHeldVanityItemType.none;
            if (aiProgress < 0)
            {
                return;
            }
            switch (curAi)
            {
                default:
                    curAi = Main.rand.Next(0, 4);
                    NPC.rotation = 0;
                    break;
                case 0://Physical attacks
                    if (aiProgress < 100)
                    {
                        //Flowmotion spin (bosses only)
                        if (Vector2.Distance(Main.npc[npcTarget].Center, NPC.Center) > viewRange / 2 || rangedCloseupAttack==0 && Main.npc[npcTarget].boss)
                        {
                            NPC.direction = MathF.Sign(NPC.Center.X - Main.npc[npcTarget].Center.X);
                            if (aiProgress == 1)
                            {
                                NPC.Center = Main.npc[npcTarget].Center + new Vector2(Main.npc[npcTarget].width / 2 + NPC.width / 2 + 2,0)*NPC.direction;
                            }
                            else if (aiProgress < 40)
                            {
                                NPC.Center += (Vector2.Distance(NPC.Center, Main.npc[npcTarget].Center) > 20) ? MathHelp.Normalize(Main.npc[npcTarget].Center - NPC.Center) * 10 : Vector2.Zero;
                                heldProjDamage = 20;
                                HoldProjectile(ModContent.ProjectileType<Projectiles.EnemyKingdomKey>(),rotation:NPC.rotation);
                                NPC.rotation += MathF.PI/4 * NPC.direction;
                                frame = 17+(int)Math.Clamp(aiProgress/20f*4f,0,4);
                            }else if (aiProgress == 50)
                            {
                                HoldProjectile(-1);
                                rangedCloseupAttack = -1;
                                NPC.rotation = 0;
                                curAi++;
                                aiProgress = -20;
                            }

                            rangedCloseupAttack = 0;


                        }
                        else if (Vector2.Distance(Main.npc[npcTarget].Center, NPC.Center) > NPC.width * 5 || rangedCloseupAttack==1)
                        {
                            //Slash closer

                            rangedCloseupAttack = 1;
                            NPC.direction = MathF.Sign(NPC.Center.X - Main.npc[npcTarget].Center.X);

                            if (aiProgress < 40)
                            {
                                NPC.Center += (Vector2.Distance(NPC.Center, Main.npc[npcTarget].Center) > 5) ? MathHelp.Normalize(Main.npc[npcTarget].Center - NPC.Center) * 20 : Vector2.Zero;
                                heldProjDamage = 20;
                                HoldProjectile(ModContent.ProjectileType<Projectiles.EnemyKingdomKey>());
                                NPC.rotation = 0;
                                frame = 17 + (int)Math.Clamp(aiProgress / 20f * 4f, 0, 4);
                            }
                            else if (aiProgress == 50)
                            {
                                HoldProjectile(-1);
                                rangedCloseupAttack = -1;
                                NPC.rotation = 0;
                                curAi++;
                                aiProgress = -25;
                            }
                        }
                        else
                        {
                            //Normal Combo
                            NPC.Center += (Vector2.Distance(NPC.Center, Main.npc[npcTarget].Center) > 5) ? MathHelp.Normalize(Main.npc[npcTarget].Center - NPC.Center) * 2 : Vector2.Zero;
                            NPC.direction = MathF.Sign(NPC.Center.X - Main.npc[npcTarget].Center.X);

                            if (aiProgress < 20)
                            {
                                heldProjDamage = 20;
                                HoldProjectile(ModContent.ProjectileType<Projectiles.EnemyKingdomKey>());
                                NPC.rotation = 0;
                                frame = 17 + (int)Math.Clamp(aiProgress / 20f * 4f, 0, 4);
                            }else if (aiProgress < 40)
                            {
                                heldProjDamage = 20;
                                HoldProjectile(ModContent.ProjectileType<Projectiles.EnemyKingdomKey>());
                                NPC.rotation = 0;
                                frame = 17 + (int)Math.Clamp((40-aiProgress) / 20f * 4f, 0, 4);
                            }
                            else if (aiProgress == 50)
                            {
                                HoldProjectile(-1);
                                rangedCloseupAttack = -1;
                                NPC.rotation = 0;
                                curAi++;
                                aiProgress = -5;
                            }
                        }
                    }
                    else
                    {
                        HoldProjectile(-1);
                        rangedCloseupAttack = -1;
                        NPC.rotation = 0;
                        curAi++;
                        aiProgress = -5;
                    }
                    break;
                case 1://Magic attacks
                    NPC.rotation = 0;
                    if (aiProgress < 100)
                    {
                        if (Vector2.Distance(Main.npc[npcTarget].Center, NPC.Center) > viewRange / 2 && Main.npc[npcTarget].boss)
                        {
                            // LIGHTNING (Boss only)

                            frame =17;
                            NPC.direction = MathF.Sign(NPC.Center.X - Main.npc[npcTarget].Center.X);

                            if (aiProgress == 50)
                            {
                                int proj = Projectile.NewProjectile(s, Main.npc[npcTarget].Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Magic.Lightning_Spell>(), 50, 5, Owner: NPC.target);
                                Main.projectile[proj].friendly = true;
                                Main.projectile[proj].hostile = false;
                            }
                            if (aiProgress < 30)
                            {
                                NPC.Center += (Vector2.Distance(NPC.Center, Main.npc[npcTarget].Center) > 50) ? MathHelp.Normalize(Main.npc[npcTarget].Center - NPC.Center) * 3 : Vector2.Zero;
                            }

                        }
                        else if (Vector2.Distance(Main.npc[npcTarget].Center, NPC.Center) > NPC.width * 5)
                        {
                            //Fire
                            NPC.direction = MathF.Sign(NPC.Center.X - Main.npc[npcTarget].Center.X);
                            frame = 19;

                            if (aiProgress == 75)
                            {
                                int proj = Projectile.NewProjectile(s, NPC.Center, MathHelp.Normalize(Main
                                    .npc[npcTarget].Center - NPC.Center) * 25, ProjectileID.Fireball, 50, 5, Owner: NPC.target);
                                Main.projectile[proj].friendly = true;
                                Main.projectile[proj].hostile = false;
                                Main.projectile[proj].timeLeft = 30;
                            }

                             if (aiProgress < 50)
                            {
                                NPC.Center +=  MathHelp.Normalize(Main.npc[npcTarget].Center - NPC.Center) * 3 ;
                            }

                        }
                        else
                        {
                            //Ice
                            NPC.direction = MathF.Sign(NPC.Center.X - Main.npc[npcTarget].Center.X);
                            frame = 17;

                            if (aiProgress == 60)
                            {
                                int proj;
                                Vector2 dir;
                                float tang;
                                for (int i = -3; i < 3; i++)
                                {
                                    dir = MathHelp.Normalize(Main.npc[npcTarget].Center - NPC.Center);
                                    tang = MathF.Atan2(dir.Y,dir.X)+MathF.PI*0.1f*i;
                                    dir = new Vector2(MathF.Cos(tang), MathF.Sin(tang));
                                    proj = Projectile.NewProjectile(s, NPC.Center, dir*15, ProjectileID.IceBolt, 50, 5, Owner: NPC.target);
                                    Main.projectile[proj].friendly = true;
                                    Main.projectile[proj].hostile = false;
                                    Main.projectile[proj].timeLeft = 30;
                                }
                            }


                            if (aiProgress < 40)
                            {
                                NPC.Center += (Vector2.Distance(NPC.Center, Main.npc[npcTarget].Center) > 50) ? MathHelp.Normalize(Main.npc[npcTarget].Center - NPC.Center) * 3 : Vector2.Zero;
                            }


                        }
                    }
                    else
                    {
                        curAi = (Main.npc[npcTarget].boss ? curAi + 1 : 0);
                        aiProgress = -20;
                    }
                    break;
                case 2://Special attack (Ragnarok, Srike Raid, Sonic Slash)

                    NPC.rotation = 0;
                    if (aiProgress < 500)
                    {
                        if (Vector2.Distance(Main.npc[npcTarget].Center, NPC.Center) > viewRange / 2 || rangedCloseupAttack==2)
                        {
                            rangedCloseupAttack = 2;
                            // STRIKE RAID
                            NPC.direction = MathF.Sign(NPC.Center.X - Main.npc[npcTarget].Center.X);

                            if (aiProgress %20==10)
                            {
                                int proj = Projectile.NewProjectile(s, NPC.Center, MathHelp.Normalize(Main
                                    .npc[npcTarget].Center - NPC.Center) * 15, ModContent.ProjectileType<Projectiles.EnemyKingdomKey>(), 50, 5, Owner: NPC.target);
                                Main.projectile[proj].friendly = true;
                                Main.projectile[proj].hostile = false;
                            }
                            else if (aiProgress < 15)
                            {
                                NPC.Center += (Vector2.Distance(NPC.Center, Main.npc[npcTarget].Center) > 50) ? MathHelp.Normalize(Main.npc[npcTarget].Center - NPC.Center) * 2 : Vector2.Zero;
                            }

                            if (aiProgress == 100)
                            {
                                HoldProjectile(-1);
                                rangedCloseupAttack = -1;
                                curAi = (Main.npc[npcTarget].boss ? curAi + 1 : 0);
                                aiProgress = -45;
                            }

                        }
                        else if (Vector2.Distance(Main.npc[npcTarget].Center, NPC.Center) > NPC.width * 5 || rangedCloseupAttack==1)
                        {
                            rangedCloseupAttack = 1;
                            //Sonic Slash
                            NPC.direction = MathF.Sign(NPC.Center.X - Main.npc[npcTarget].Center.X);
                            frame =19;
                            heldProjDamage = 20;
                            if (aiProgress%20 < 10)
                            {
                                NPC.direction = 1;
                                NPC.Center = Main.npc[npcTarget].Center+new Vector2((aiProgress%20-5)*50,0);
                                HoldProjectile(ModContent.ProjectileType<Projectiles.EnemyKingdomKey>(),30,10);
                            }
                            else
                            {
                                NPC.direction = -1;
                                NPC.Center = Main.npc[npcTarget].Center - new Vector2((aiProgress%20 - 15) * 50, 0);
                                HoldProjectile(ModContent.ProjectileType<Projectiles.EnemyKingdomKey>(),30,10,rotation:MathF.PI);
                            }
                            if (aiProgress == 100)
                            {
                                HoldProjectile(-1);
                                rangedCloseupAttack = -1;
                                curAi = (Main.npc[npcTarget].boss ? curAi + 1 : 0);
                                aiProgress = -50;
                            }
                        }
                        else
                        {
                            rangedCloseupAttack = 0;
                            //Ragnarok
                            NPC.direction = MathF.Sign(NPC.Center.X - Main.npc[npcTarget].Center.X);
                            HoldProjectile(ModContent.ProjectileType<Projectiles.EnemyKingdomKey>());

                            NPC.Center += (Vector2.Distance(NPC.Center, Main.npc[npcTarget].Center) > 50) ? MathHelp.Normalize(Main.npc[npcTarget].Center - NPC.Center) * 3 : Vector2.Zero;

                            if (aiProgress < 3)
                            {
                                HoldProjectile(-1);
                                rangedCloseupAttack = -1;
                                curAi = (Main.npc[npcTarget].boss ? curAi + 1 : 0);
                                aiProgress = -50;
                            }

                        }
                    }
                    else
                    {
                        HoldProjectile(-1);
                        rangedCloseupAttack = -1;
                        curAi = (Main.npc[npcTarget].boss ? curAi + 1 : 0);
                        aiProgress = 0;
                    }
                    break;
                case 3://Healing
                    NPC.rotation = 0;
                    if (aiProgress < 20)
                    {
                        frame = 17;
                        if (aiProgress == 40)
                        {
                            if (NPC.life < NPC.lifeMax / 2)
                            {
                                //Heals himself
                                NPC.life += 20;
                            }
                            else if (Main.player[NPC.target].statLife < Main.player[NPC.target].statLifeMax / 2)
                            {
                                //Heals Player
                                NPC.Center = Main.player[NPC.target].Center+new Vector2(NPC.direction*-10,0);
                                NPC.direction = MathF.Sign(NPC.Center.X - Main.player[NPC.target].Center.X);
                                Main.player[NPC.target].statLife += 20;
                            }
                            else
                            {
                                curAi=0;
                                aiProgress = 0;
                            }
                        }
                    }
                    else
                    {
                        curAi=0;
                        aiProgress = 0;
                    }
                    break;
                case 4://Unison attacks
                    frame = 0;
                    NPC.rotation = 0;
                    NPC.Center = Main.player[NPC.target].Center;
                    if (aiProgress > 20)
                    {
                        curAi=0;
                        aiProgress = 0;
                    }
                    break;
            }
        }

        public void HoldProjectile(int type,float offsetx=0,float offsetY=0,float rotation=0)
        {
            if (type < 0)
            {
                if(heldProjectile>=0 && heldProjType>=0 && Main.projectile[heldProjectile].active)
                {
                    Main.projectile[heldProjectile].timeLeft = 0;
                    heldProjectile = -1;
                    heldProjType = -1;
                }
            }
            else
            {
                if (heldProjType == type)
                {
                    CheckHeldProjectile();

                    Main.projectile[heldProjectile].Center=NPC.Center+new Vector2(offsetx,offsetY);
                    Main.projectile[heldProjectile].damage = heldProjDamage;
                    Main.projectile[heldProjectile].rotation = rotation;
                }
                else
                {
                    if (heldProjectile>=0 && Main.projectile[heldProjectile].active && Main.projectile[heldProjectile].type == heldProjType)
                    {
                        Main.projectile[heldProjectile].timeLeft = 0;
                    }
                    heldProjType = type;
                    heldProjectile = Projectile.NewProjectile(s, NPC.Center+new Vector2(offsetx,offsetY), Vector2.Zero, heldProjType, heldProjDamage, 2, NPC.target);
                    Main.projectile[heldProjectile].friendly = true;
                    Main.projectile[heldProjectile].hostile = false;
                    Main.projectile[heldProjectile].timeLeft = 3;
                    Main.projectile[heldProjectile].damage= heldProjDamage;
                    Main.projectile[heldProjectile].rotation = rotation;
                }
            }
        }

        public void CheckHeldProjectile()
        {
            if(heldProjType >= 0)
            {
                if (heldProjectile>=0 && Main.projectile[heldProjectile].active && Main.projectile[heldProjectile].type == heldProjType)
                {
                    Main.projectile[heldProjectile].timeLeft = 5;
                }else
                {
                    heldProjectile = Projectile.NewProjectile(s, NPC.Center, NPC.velocity, heldProjType,heldProjDamage,2, NPC.target);
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;
            NPC.frame.Y = frameHeight * frame;
        }

        public int TargetClosestEnemy(bool bossOnly = false)
        {
            int target = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (bossOnly)
                {
                    if (Main.npc[i].active && Main.player[NPC.target].GetModPlayer<SoraPlayer>().isBoss(i) && Vector2.Distance(Main.npc[i].Center, NPC.Center) < viewRange)
                    {
                        target = i;
                    }
                }
                else
                {
                    if (Main.npc[i].active)
                    {
                        if (target == -1 || !Main.npc[target].boss)
                        {
                            if ((!Main.npc[i].CountsAsACritter && !Main.npc[i].townNPC && !Main.npc[i].friendly) && Vector2.Distance(Main.npc[i].Center, NPC.Center) < viewRange && Main.npc[i].type != NPCID.TargetDummy)
                            {
                                target = i;
                            }
                        }
                    }
                }
            }
            return target;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D requested;
            Rectangle rect;
            Vector2 pivot;
            switch (heldVanity.type)
            {
                default:
                    break;
                case SoraHeldVanityItemType.phone:
                    requested = ModContent.Request<Texture2D>("KingdomTerrahearts/Items/gummiPhone").Value;
                    if (requested != null)
                    {
                        rect = new Rectangle(0, 0, requested.Width, requested.Height);
                        pivot = new Vector2(requested.Width / 2f, requested.Height / 2f);
                        spriteBatch.Draw(requested,NPC.Center + new Vector2(heldVanity.offset.X*-NPC.direction,heldVanity.offset.Y) - Main.screenPosition,rect,drawColor,heldVanity.rotation,pivot,heldVanity.scale,(NPC.direction<0?SpriteEffects.None:SpriteEffects.FlipHorizontally),0);
                    }
                    break;
            }
        }

        public override bool CanChat()
        {
            return !(npcTarget>=0 && Main.npc[npcTarget].active);
        }

        public override bool CheckDead()
        {
            return false;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void UpdateLifeRegen(ref int damage)
        {
            NPC.lifeRegen = 50;
        }

        public override string TownNPCName()
        {
            if (!NPC.downedBoss1)
            {
                return "Raxo Sack Of Rocks";
            }
            return "Sora";
        }

        public override string GetChat()
        {
            Random r = new Random();
            List<string> dialogOptions = new List<string>();

            if (!firstTalked)
            {
                dialogOptions.Add("HI! I'm " + NPC.GivenName+ "!");
                firstTalked = true;
            }
            else
            {
                if (NPC.AnyNPCs(ModContent.NPCType<goofy>()))
                {
                    dialogOptions.Add("I hope to be strong enough to protect "+Main.npc[NPC.FindFirstNPC(ModContent.NPCType<goofy>())].GivenName+" as much as he has protected me.");
                    if (NPC.AnyNPCs(ModContent.NPCType<donald>()))
                    {
                        dialogOptions.Add(Main.npc[NPC.FindFirstNPC(ModContent.NPCType<goofy>())].GivenName+" and "+ Main.npc[NPC.FindFirstNPC(ModContent.NPCType<donald>())].GivenName + " are always there for me, and I am for them.");
                    }
                }
                if (NPC.AnyNPCs(ModContent.NPCType<donald>()))
                {
                    dialogOptions.Add(Main.npc[NPC.FindFirstNPC(ModContent.NPCType<donald>())].GivenName + " might be a little loud and hot headed, but he is a great friend nevertheless!");
                }
                if(!NPC.AnyNPCs(ModContent.NPCType<goofy>()) && !NPC.AnyNPCs(ModContent.NPCType<donald>()))
                {
                    dialogOptions.Add("I hope Donald and Goofy are OK.");
                }
                dialogOptions.Add("My friends are my power!");
                dialogOptions.Add("Call me if you need help, ok?");
                dialogOptions.Add("Smiling faces only!");
            }

            return dialogOptions[r.Next(0, dialogOptions.Count)];
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Keyblade_Kingdom>()));
        }

    }
}
