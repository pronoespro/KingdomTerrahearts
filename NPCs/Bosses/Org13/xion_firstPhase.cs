using KingdomTerrahearts.Extra;
using KingdomTerrahearts.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using System.Collections.Generic;

namespace KingdomTerrahearts.NPCs.Bosses.Org13
{
    [AutoloadBossHead]
    public class xion_firstPhase : ModNPC
    {

        Player player;

        int curAttack = 0;
        int attackSpeed = 1;
        float attackSpeedMult = 1;
        int[] attacksDamage = new int[]{0,50,20,20,20};
        
        int attackCooldown = 15;
        int nextAttack = 1;

        int keyProj=-1;

        bool defeated;
        int defeatTime = 75;
        ProjectileSource_NPC s;

        int hitRecoil;
        int hitCombo;

        void Target()
        {
            player = Main.player[NPC.target];
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Xion");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 1500;
            NPC.damage = 0;
            NPC.defense = 15;
            NPC.knockBackResist = 1f;
            NPC.width = 40;
            NPC.height = 60;
            NPC.scale = 1;
            NPC.value = 500;
            NPC.npcSlots = 4;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = false;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Music = MusicLoader.GetMusicSlot("KingdomTerrahearts/Sounds/Music/Vector to the Heaven");

            s = new ProjectileSource_NPC(NPC);
            NPC.ai[0] = 50;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
            for(int i = 0; i < attacksDamage.Length; i++)
            {
                attacksDamage[i]=(int)(attacksDamage[i]* 1.25f * numPlayers);
            }
            NPC.defense = (int)(NPC.defense + numPlayers);
            attackSpeedMult += numPlayers*0.5f;
        }

        public override void AI()
        {

            NPC.TargetClosest();
            Target();
            DespawnHandler();

            if (NPC.timeLeft <= 10)
            {
                return;
            }
            if (hitRecoil > 0)
            {
                hitRecoil--;
                return;
            }
            NPC.velocity = Vector2.Zero;

            if (defeated)
            {
                defeatTime--;
                if (defeatTime == 5)
                {
                    Projectile.NewProjectile(s,NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.darkPortal>(), 0, 0);
                    NPC.NPCLoot();
                }else if (defeatTime <= 0)
                {
                    NPC.life = -1;
                    NPC.timeLeft = 1;
                    defeatTime = 1000;
                }

                return;
            }


            if (keyProj == -1)
            {
                NPC.Center= player.Center + new Vector2(0, -300);
                Conversation[] conv =new Conversation[] { new Conversation("NO HOLDING BACK!",Color.Yellow,DialogSystem.BOSS_DIALOGTIME,"Xion") };

                if (KingdomWorld.downedXionPhases[0])
                    conv[0].dialog = "Let's go again!";
                else if (KingdomWorld.downedXionPhases[1])
                    conv[0].dialog = "Come on!";

                DialogSystem.AddConversation(conv);
                keyProj = Projectile.NewProjectile(s,NPC.Center, NPC.velocity, ModContent.ProjectileType<Projectiles.EnemyKingdomKey>(), attacksDamage[curAttack], 1);
                Projectile.NewProjectile(s,NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.darkPortal>(), 0, 0);
            }
            else
            {
                if(Main.projectile[keyProj].type!= ModContent.ProjectileType<Projectiles.EnemyKingdomKey>())
                {
                    keyProj = Projectile.NewProjectile(s, NPC.Center, NPC.velocity, ModContent.ProjectileType<Projectiles.EnemyKingdomKey>(), attacksDamage[curAttack], 1);
                }
                Main.projectile[keyProj].timeLeft = 5;
            }

            if (player!=null && player.active)
            {
                if (attackCooldown > 0)
                {
                    attackCooldown--;
                    NPC.velocity = Vector2.Zero;
                    NPC.ai[0] =0;
                    NPC.ai[1] = 0;
                    Main.projectile[keyProj].Center = new Vector2((NPC.spriteDirection - 1) * 17.5f + NPC.Center.X, NPC.Center.Y + 5);
                    Main.projectile[keyProj].rotation = (NPC.spriteDirection > 0) ? 0.25f : 4.5f;
                }
                else
                {
                    switch (curAttack)
                    {
                        case 0:
                            NPC.velocity = Vector2.Zero;
                            NPC.ai[0] += attackSpeed* attackSpeedMult;
                            if (NPC.ai[0] > 50)
                            {
                                curAttack = nextAttack;
                                nextAttack = Main.rand.Next(1, 5);
                                CheckCurAttack();
                                hitCombo = 0;
                            }
                            else
                            {
                                for (int i = 0; i < Main.rand.Next(0, 2); i++)
                                {
                                    Dust.NewDust(AttackPos(nextAttack), NPC.width, NPC.height, DustID.GoldCoin);
                                }
                            }
                            break;
                        case 1:
                            NPC.velocity = Vector2.Zero;
                            Attack(100);
                            break;
                        case 2:
                            NPC.velocity = new Vector2(30, 0);
                            Attack(100);
                            break;
                        case 3:
                            NPC.velocity = new Vector2(-30, 0);
                            Attack(75);
                            break;
                        case 4:
                            NPC.velocity = new Vector2(0, 30);
                            Attack(100);
                            break;
                    }
                }

            }
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            if (projectile.type == ModContent.ProjectileType<Projectiles.Weapons.KeybladeHoldDisplay>())
            {
                NPCHit();
            }
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            NPCHit();
        }

        public void NPCHit()
        {
            if (hitCombo < 7)
            {
                if (curAttack != 0)
                {
                    curAttack = 0;
                    CheckCurAttack();
                    attackCooldown = 15;

                    NPC.ai[0] = 0;
                }
                hitRecoil = 15;
                hitCombo++;
            }
            else
            {
                curAttack = 0;
                CheckCurAttack();
                attackCooldown = 0;

                NPC.ai[0] = 55;
            }
        }

        void Attack(int maxAI)
        {
            NPC.ai[1] += attackSpeed * attackSpeedMult;
            if (NPC.ai[1] >= maxAI)
            {
                curAttack = 0;
                CheckCurAttack();
                attackCooldown = 15;
            }
        }

        public Vector2 AttackPos(int attack)
        {
            switch (attack)
            {
                default:
                case 0:
                    return player.Center;
                case 1:
                    return player.Center + new Vector2(0, -300);
                case 2:
                    return player.Center + new Vector2(-500, 0);
                case 3:
                    return player.Center + new Vector2(500, 0);
                case 4:
                    return player.Center + new Vector2(0, -450);
            }
        }

        void CheckCurAttack()
        {
            attackSpeed = 1;
            Main.projectile[keyProj].damage = attacksDamage[curAttack];
            switch (curAttack)
            {
                case 0:
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    NPC.stairFall = false;
                    return;
                case 1:
                    attackSpeed = 2;
                    NPC.Center =AttackPos(curAttack);
                    NPC.stairFall  = NPC.noGravity = true;
                    Main.projectile[keyProj].Center = NPC.Center;
                    NPC.ai[1] = 0;
                    return;
                case 2:
                    NPC.Center = AttackPos(curAttack) ;
                    NPC.stairFall = NPC.noGravity = true;
                    NPC.direction = -1;
                    NPC.ai[1] = 76;
                    return;
                case 3:
                    NPC.Center = AttackPos(curAttack);
                    NPC.stairFall  = NPC.noGravity = true;
                    NPC.ai[1] = 51;
                    NPC.direction = 1;
                    return;
                case 4:
                    NPC.Center = AttackPos(curAttack);
                    NPC.stairFall =NPC.noGravity = true;
                    NPC.ai[1] = 76;
                    return;
            }
        }

        void DespawnHandler()
        {
            if (!player.active || player.dead || player.statLife==0)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead || player.statLife == 0)
                {
                    NPC.velocity = new Vector2(0, 100000);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 1;
                        Conversation[] conv = new Conversation[] { new Conversation("Try again", Color.Yellow, DialogSystem.BOSS_DIALOGTIME, "Xion") };
                        DialogSystem.AddConversation(conv);
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            int frame;
            if (curAttack != 0)
            {
                if (NPC.ai[1] > 75)
                    frame = 4;
                else if (NPC.ai[1] > 50)
                    frame = 3;
                else if (NPC.ai[1] > 25)
                    frame = 2;
                else
                    frame = 1;
            }
            else
            {
                if (NPC.velocity.Y != 0)
                    frame = 5;
                else
                    frame = 0;
            }


            if (keyProj != -1 && Main.projectile[keyProj].active)
            {
                Main.projectile[keyProj].spriteDirection = NPC.spriteDirection;
                GrabKey(frame);
            }

            NPC.frame.Y = frame * frameHeight;
        }

        void GrabKey(int frame)
        {
            if (curAttack == 1 && attackCooldown<=0)
            {
                Main.projectile[keyProj].velocity = new Vector2(0, 10);
                return;
            }
            switch (frame)
            {
                default:
                    Main.projectile[keyProj].Center = new Vector2((NPC.spriteDirection - 1) * 17.5f + NPC.Center.X, NPC.Center.Y + 5);
                    Main.projectile[keyProj].rotation = (NPC.spriteDirection > 0) ? 0.25f : 4.5f;
                    break;
                case 0:
                    Main.projectile[keyProj].Center = new Vector2((NPC.spriteDirection-1)*17.5f+ NPC.Center.X, NPC.Center.Y+5);
                    Main.projectile[keyProj].rotation = (NPC.spriteDirection>0)?0.25f:4.5f;
                    break;
                case 1:
                    Main.projectile[keyProj].Center = NPC.Center;
                    Main.projectile[keyProj].rotation = MathHelp.DegreeToQuat(45);
                    break;
                case 2:
                    Main.projectile[keyProj].Center = NPC.Center;
                    Main.projectile[keyProj].rotation = 0;
                    break;
                case 3:
                    Main.projectile[keyProj].Center = NPC.Center;
                    Main.projectile[keyProj].rotation = 0;
                    break;
                case 4:
                    Main.projectile[keyProj].Center = NPC.Center;
                    Main.projectile[keyProj].rotation = 0;
                    break;
                case 5:
                    Main.projectile[keyProj].Center = new Vector2((NPC.spriteDirection - 1) * 25 + NPC.Center.X+7, NPC.Center.Y-30);
                    Main.projectile[keyProj].rotation = (NPC.spriteDirection > 0) ? -MathHelp.DegreeToQuat(90) : MathHelp.DegreeToQuat(180)-0.75f;
                    break;
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

            if (!Main.hardMode)
            {
                int[] dropOptions = new int[] { ModContent.ItemType<Items.Weapons.Keyblade_Kingdom>(), ModContent.ItemType<Items.Armor.orgCoat>(), ModContent.ItemType<Items.seasaltIcecream>() };
                npcLoot.Add(ItemDropRule.OneFromOptions(3, dropOptions));


                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.twilightStone>(), 1, 5, 15));
            }
        }

        public override bool CheckDead()
        {
            if (!defeated)
            {
                if (!Main.hardMode)
                {
                    if (NPC.timeLeft > 20)
                    {
                        Conversation[] conv = new Conversation[] { new Conversation("Maybe I'll fight you for real next time.", Color.Yellow, DialogSystem.BOSS_DIALOGTIME, "Xion") };
                        DialogSystem.AddConversation(conv);
                    }
                }
                else
                {
                    NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<xion_secondPhase>(), Target: NPC.target);
                    NPC.timeLeft = (NPC.timeLeft > 5) ? 1 : NPC.timeLeft-1;
                    defeatTime = 0;
                }
            }
            defeated = true;
            KingdomWorld.downedXionPhases[0] = true;
            NPC.life = 1;
            return false;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }
    }



    [AutoloadBossHead]
    public class xion_secondPhase : ModNPC
    {

        Player player;

        int curAttack = 0;
        int attackSpeed = 1;
        float attackSpeedMult = 1;
        int[] attacksDamage = new int[] { 0, 70, 30, 30, 30 };

        int attackCooldown = 15;
        int nextAttack = 1;

        int keyProj = -1;

        bool defeated;
        int defeatTime = 75;

        ProjectileSource_NPC s;

        void Target()
        {
            player = Main.player[NPC.target];
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Xion");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 3500;
            NPC.damage = 0;
            NPC.defense = 15;
            NPC.knockBackResist = 0;
            NPC.width = 40;
            NPC.height = 60;
            NPC.scale = 1;
            NPC.value = 500;
            NPC.npcSlots = 4;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = false;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Music = MusicLoader.GetMusicSlot("KingdomTerrahearts/Sounds/Music/Vector to the Heaven");

            s = new ProjectileSource_NPC(NPC);
            NPC.ai[0] = 50;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
            for (int i = 0; i < attacksDamage.Length; i++)
            {
                attacksDamage[i] = (int)(attacksDamage[i] * 1.25f * numPlayers);
            }
            NPC.defense = (int)(NPC.defense + numPlayers);
            attackSpeedMult += numPlayers * 0.5f;
        }

        public override void AI()
        {

            NPC.velocity = Vector2.Zero;
            NPC.TargetClosest();
            Target();
            DespawnHandler();

            if (NPC.timeLeft <= 10)
            {
                return;
            }

            if (defeated)
            {
                defeatTime--;
                if (defeatTime == 5)
                {
                    Projectile.NewProjectile(s,NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.darkPortal>(), 0, 0);
                    NPC.NPCLoot();
                }
                else if (defeatTime <= 0)
                {
                    NPC.life = -1;
                    NPC.timeLeft = 1;
                    defeatTime = 1000;
                }

                return;
            }


            if (keyProj == -1)
            {
                NPC.Center = player.Center + new Vector2(0, -300);
                Conversation[] conv = new Conversation[] { new Conversation("Let's get serious!", Color.Yellow, DialogSystem.BOSS_DIALOGTIME, "Xion") };
                DialogSystem.AddConversation(conv);
                keyProj = Projectile.NewProjectile(s,NPC.Center, NPC.velocity, ModContent.ProjectileType<Projectiles.EnemyKingdomKey>(), attacksDamage[curAttack], 1);
                Projectile.NewProjectile(s,NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.darkPortal>(), 0, 0);
            }
            else
            {
                if (Main.projectile[keyProj].type != ModContent.ProjectileType<Projectiles.EnemyKingdomKey>())
                {
                    keyProj = Projectile.NewProjectile(s, NPC.Center, NPC.velocity, ModContent.ProjectileType<Projectiles.EnemyKingdomKey>(), attacksDamage[curAttack], 1);
                }
                Main.projectile[keyProj].timeLeft = 5;
            }

            if (player != null && player.active)
            {
                if (attackCooldown > 0)
                {
                    attackCooldown--;
                    NPC.velocity = Vector2.Zero;
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    Main.projectile[keyProj].Center = new Vector2((NPC.spriteDirection - 1) * 17.5f + NPC.Center.X, NPC.Center.Y + 5);
                    Main.projectile[keyProj].rotation = (NPC.spriteDirection > 0) ? 0.25f : 4.5f;
                }
                else
                {
                    switch (curAttack)
                    {
                        case 0:
                            NPC.velocity = Vector2.Zero;
                            NPC.ai[0] += attackSpeed * attackSpeedMult;
                            if (NPC.ai[0] > 50)
                            {
                                curAttack = nextAttack;
                                nextAttack = Main.rand.Next(1, 5);
                                CheckCurAttack();
                            }
                            else
                            {
                                for (int i = 0; i < Main.rand.Next(0, 2); i++)
                                {
                                    Dust.NewDust(AttackPos(nextAttack), NPC.width, NPC.height, DustID.GoldCoin);
                                }
                            }
                            break;
                        case 1:
                            NPC.velocity = Vector2.Zero;
                            Attack(100);
                            break;
                        case 2:
                            NPC.velocity = new Vector2(30, 0);
                            Attack(100);
                            break;
                        case 3:
                            NPC.velocity = new Vector2(-30, 0);
                            Attack(75);
                            break;
                        case 4:
                            NPC.velocity = new Vector2(0, 30);
                            Attack(100);
                            break;
                    }
                }

            }
        }

        void Attack(int maxAI)
        {
            NPC.ai[1] += attackSpeed * attackSpeedMult;
            if (NPC.ai[1] >= maxAI)
            {
                curAttack = 0;
                CheckCurAttack();
                attackCooldown = 15;
            }
        }

        public Vector2 AttackPos(int attack)
        {
            switch (attack)
            {
                default:
                case 0:
                    return player.Center;
                case 1:
                    return player.Center + new Vector2(0, -300);
                case 2:
                    return player.Center + new Vector2(-500, 0);
                case 3:
                    return player.Center + new Vector2(500, 0);
                case 4:
                    return player.Center + new Vector2(0, -450);
            }
        }

        void CheckCurAttack()
        {
            attackSpeed = 1;
            Main.projectile[keyProj].damage = attacksDamage[curAttack];
            switch (curAttack)
            {
                case 0:
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    NPC.stairFall = false;
                    return;
                case 1:
                    attackSpeed = 2;
                    NPC.Center = AttackPos(curAttack);
                    NPC.stairFall = NPC.noGravity = true;
                    Main.projectile[keyProj].Center = NPC.Center;
                    NPC.ai[1] = 0;
                    return;
                case 2:
                    NPC.Center = AttackPos(curAttack);
                    NPC.stairFall = NPC.noGravity = true;
                    NPC.direction = -1;
                    NPC.ai[1] = 76;
                    return;
                case 3:
                    NPC.Center = AttackPos(curAttack);
                    NPC.stairFall = NPC.noGravity = true;
                    NPC.ai[1] = 51;
                    NPC.direction = 1;
                    return;
                case 4:
                    NPC.Center = AttackPos(curAttack);
                    NPC.stairFall = NPC.noGravity = true;
                    NPC.ai[1] = 76;
                    return;
            }
        }

        void DespawnHandler()
        {
            if (!player.active || player.dead || player.statLife == 0)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead || player.statLife == 0)
                {
                    NPC.velocity = new Vector2(0, 100000);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 1;
                        Conversation[] conv = new Conversation[] { new Conversation("No good", Color.Yellow, DialogSystem.BOSS_DIALOGTIME, "Xion") };
                        DialogSystem.AddConversation(conv);
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            int frame;
            if (curAttack != 0)
            {
                if (NPC.ai[1] > 75)
                    frame = 4;
                else if (NPC.ai[1] > 50)
                    frame = 3;
                else if (NPC.ai[1] > 25)
                    frame = 2;
                else
                    frame = 1;
            }
            else
            {
                if (NPC.velocity.Y != 0)
                    frame = 5;
                else
                    frame = 0;
            }


            if (keyProj != -1 && Main.projectile[keyProj].active)
            {
                Main.projectile[keyProj].spriteDirection = NPC.spriteDirection;
                GrabKey(frame);
            }

            NPC.frame.Y = frame * frameHeight;
        }

        void GrabKey(int frame)
        {
            if (curAttack == 1 && attackCooldown <= 0)
            {
                Main.projectile[keyProj].velocity = new Vector2(0, 10);
                return;
            }
            switch (frame)
            {
                default:
                    Main.projectile[keyProj].Center = new Vector2((NPC.spriteDirection - 1) * 17.5f + NPC.Center.X, NPC.Center.Y + 5);
                    Main.projectile[keyProj].rotation = (NPC.spriteDirection > 0) ? 0.25f : 4.5f;
                    break;
                case 0:
                    Main.projectile[keyProj].Center = new Vector2((NPC.spriteDirection - 1) * 17.5f + NPC.Center.X, NPC.Center.Y + 5);
                    Main.projectile[keyProj].rotation = (NPC.spriteDirection > 0) ? 0.25f : 4.5f;
                    break;
                case 1:
                    Main.projectile[keyProj].Center = NPC.Center;
                    Main.projectile[keyProj].rotation = MathHelp.DegreeToQuat(45);
                    break;
                case 2:
                    Main.projectile[keyProj].Center = NPC.Center;
                    Main.projectile[keyProj].rotation = 0;
                    break;
                case 3:
                    Main.projectile[keyProj].Center = NPC.Center;
                    Main.projectile[keyProj].rotation = 0;
                    break;
                case 4:
                    Main.projectile[keyProj].Center = NPC.Center;
                    Main.projectile[keyProj].rotation = 0;
                    break;
                case 5:
                    Main.projectile[keyProj].Center = new Vector2((NPC.spriteDirection - 1) * 25 + NPC.Center.X + 7, NPC.Center.Y - 30);
                    Main.projectile[keyProj].rotation = (NPC.spriteDirection > 0) ? -MathHelp.DegreeToQuat(90) : MathHelp.DegreeToQuat(180) - 0.75f;
                    break;
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            base.BossLoot(ref name, ref potionType);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

            int[] dropOptions = new int[] { ModContent.ItemType<Items.Weapons.Keyblade_Kingdom>(), ModContent.ItemType<Items.Armor.orgCoat>(), ModContent.ItemType<Items.seasaltIcecream>() };
            npcLoot.Add(ItemDropRule.OneFromOptions(3, dropOptions));


            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.twilightGem>(), 1, 15, 35));
        }

        public override bool CheckDead()
        {
            if (!NPC.downedMoonlord)
            {
                if (NPC.timeLeft > 20 && !defeated)
                {
                    Conversation[] conv = new Conversation[] { new Conversation("Good fight, but I still went easy on you", Color.Yellow, DialogSystem.BOSS_DIALOGTIME, "Xion") };
                    DialogSystem.AddConversation(conv);
                }
            }
            else
            {
                if (!defeated)
                {
                    ProjectileSource_NPC s = new ProjectileSource_NPC(NPC);
                    Projectile.NewProjectile(s, Main.player[NPC.target].Center, Vector2.Zero, ModContent.ProjectileType<FinalXionSpawnProjectile>(), 0, 0);
                    NPC.timeLeft = (NPC.timeLeft > 5) ? 1 : NPC.timeLeft;
                    defeated = true;
                    defeatTime = 0;
                }
            }
            KingdomWorld.downedXionPhases[1] = true;
            defeated = true;
            NPC.life = 1;
            return false;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }
    }


    public class FinalXionSpawnProjectile : ModProjectile
    {

        int target = -1;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Xion Spawn");
        }

        public override void SetDefaults()
        {
            Projectile.damage = 0;
            Projectile.scale = 0.5f;
            Projectile.width = Projectile.height = 150;
            Projectile.timeLeft = 150;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.light = 10;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            if (TargetActive()) {
                GetTarget();
            }

            Projectile.Center = Main.player[target].Center;
            if (Projectile.timeLeft > 25)
            {
                Projectile.alpha = (int)Math.Clamp(Projectile.alpha - 5f, 0f, 255f);

                Projectile.scale = Math.Clamp((150f - Projectile.timeLeft) / 150f * 20 - 0.5f, 0.5f, 15f);
                Projectile.hide = true;
            }
            else
            {
                if (Projectile.ai[0] < 5)
                {
                    SpawnFinalXion();
                    Projectile.ai[0] = 10;
                }
                Projectile.hide = false;
                Projectile.alpha = (int)((50f-Projectile.timeLeft*2f)*25f);
            }

        }

        public void SpawnFinalXion()
        {

            if (TargetActive())
            {
                NPC.NewNPC((int)Main.player[target].Center.X, (int)Main.player[target].Center.Y + 300, ModContent.NPCType<xion_finalPhase>());
            }
            else
            {
                GetTarget();
                if (Main.player[target].active && !Main.player[target].dead)
                {
                    NPC.NewNPC((int)Main.player[target].Center.X, (int)Main.player[target].Center.Y + 300, ModContent.NPCType<xion_finalPhase>());
                }
            }
        }

        public bool TargetActive()
        {
            if (target == -1 || Main.player[target].active && !Main.player[target].dead)
            {
                return true;
            }
            return false;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public void GetTarget()
        {

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                target = i;
                if (TargetActive())
                {
                    return;
                }
            }
            target = -1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.hide)
            {
                Color clr =Projectile.GetAlpha(new Color(1, 1, 1, 0f));
                Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
                Rectangle rect = new Rectangle(0, 0, texture.Width, texture.Height);

                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rect, clr, (float)Math.PI * (Projectile.timeLeft / 150f) * 2f, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, SpriteEffects.None, 1);

                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rect, clr, (float)Math.PI * ((150f - Projectile.timeLeft) / 150f) * 2f, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, SpriteEffects.None, 1);

                return false;
            }
            else {
                return true;
            }
        }

    }


    [AutoloadBossHead]
    public class xion_finalPhase : ModNPC
    {

        Player player;

        int curAttack = 0;
        int attackSpeed = 1;
        float attackSpeedMult = 1;
        int[] attacksDamage = new int[] { 0, 70, 20, 80, 30 };

        int attackCooldown = 15;
        int nextAttack = 1;

        int[] armProj=new int[] { -1,-1 };
        Vector2[] armPivot = new Vector2[] { new Vector2(),new Vector2()};

        bool defeated;
        int defeatTime = 75;

        ProjectileSource_NPC s;

        void Target()
        {
            player = Main.player[NPC.target];
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Xion");
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 35000;
            NPC.damage = 0;
            NPC.defense = 15;
            NPC.knockBackResist = 0;
            NPC.width = 621/2;
            NPC.height = 877;
            NPC.scale = 1;
            NPC.value = 500;
            NPC.npcSlots = 4;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.behindTiles = true;
            NPC.hide = true;
            NPC.DeathSound = SoundID.NPCDeath1;
            Music = MusicLoader.GetMusicSlot("KingdomTerrahearts/Sounds/Music/Vector to the Heaven");

            s = new ProjectileSource_NPC(NPC);
            NPC.ai[0] = 50;
        }

        public override void DrawBehind(int index)
        {
            Main.instance.DrawCacheNPCsMoonMoon.Add(index);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Rectangle rect = new Rectangle((int)(NPC.position.X - Main.screenPosition.X - (NPC.width / 2)), (int)(NPC.position.Y - Main.screenPosition.Y), NPC.width * 2, NPC.height);
            spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("KingdomTerrahearts/NPCs/Bosses/Org13/xion_finalPhase"), rect, Color.White);
            return false;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
            for (int i = 0; i < attacksDamage.Length; i++)
            {
                attacksDamage[i] = (int)(attacksDamage[i] * 1.25f * numPlayers);
            }
            NPC.defense = (int)(NPC.defense + numPlayers);
            attackSpeedMult += numPlayers * 0.5f;
        }

        public override void AI()
        {

            NPC.velocity = Vector2.Zero;
            NPC.TargetClosest();
            Target();
            DespawnHandler();

            if (NPC.timeLeft <= 10)
            {
                return;
            }

            if (defeated)
            {
                NPC.velocity = Vector2.Zero;
                defeatTime--;
                if (defeatTime <= 0)
                {
                    NPC.life = -1;
                    NPC.timeLeft = 1;
                    defeatTime = 1000;
                }

                return;
            }

            for (int i = 0; i < armProj.Length; i++)
            {
                if (armProj[i] == -1)
                {
                    Conversation[] conv = new Conversation[] { new Conversation("NO HOLDING BACK!!!", Color.Yellow, DialogSystem.BOSS_DIALOGTIME, "Xion") };
                    DialogSystem.AddConversation(conv);
                    armProj[i] = Projectile.NewProjectile(s,NPC.Center, NPC.velocity, ModContent.ProjectileType<Projectiles.BossStuff.xion_finalPhase_arms>(), attacksDamage[curAttack], 1);
                    Projectile.NewProjectile(s,NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.darkPortal>(), 0, 0);
                }
                else
                {
                    Main.projectile[armProj[i]].timeLeft = 5;
                    switch (i)
                    {
                        case 0:
                            Main.projectile[armProj[i]].Center = new Vector2(NPC.Center.X - 157f, NPC.Center.Y - 295f);
                            Main.projectile[armProj[i]].spriteDirection = 1;
                            Main.projectile[armProj[i]].rotation = -(float)Math.PI / 2; 
                            break;
                        case 1:
                            Main.projectile[armProj[i]].Center = new Vector2(NPC.Center.X + 157f, NPC.Center.Y - 295f);
                            Main.projectile[armProj[i]].spriteDirection = -1;
                            Main.projectile[armProj[i]].rotation = (float)Math.PI / 2;
                            break;
                    }
                }
            }

            Attacks();

        }

        public void Attacks()
        {
            if (player != null && player.active)
            {
                int proj;

                switch (curAttack)
                {
                    case 0:
                        NPC.velocity = Vector2.Zero;
                        NPC.ai[0] += attackSpeed * attackSpeedMult;
                        if (NPC.ai[0] > 150)
                        {
                            curAttack = nextAttack;
                            nextAttack = Main.rand.Next(1, 4);
                            CheckCurAttack();
                        }
                        else
                        {
                            CoolAnimation(NPC.ai[0]);
                        }
                        break;
                    case 1:

                        NPC.ai[0] += attackSpeed * attackSpeedMult;

                        NPC.Center += new Vector2(0, (float)Math.Sin(NPC.ai[0] / 50));
                        if (NPC.ai[0] < 175f)
                        {
                            for(int i = 0; i < 15; i++)
                            {
                                Dust.NewDust(player.position-new Vector2(player.width/2,player.height/2), player.width*2, player.height * 2, 175);
                            }
                        }else if (NPC.ai[0] >200f && NPC.ai[0]<200f+attackSpeed*attackSpeedMult*2)
                        {
                            proj = Projectile.NewProjectile(s,player.Center, new Vector2(0, 0), ModContent.ProjectileType<Projectiles.Lightning_Spell>(), attacksDamage[curAttack], 1);
                            Main.projectile[proj].hostile = true;
                            Main.projectile[proj].friendly = false;
                        }

                        Main.projectile[armProj[0]].rotation = (float)(Math.PI / 2+Math.PI/16+Math.Sin(NPC.ai[0]/30)/50);
                        Main.projectile[armProj[0]].spriteDirection = -1;
                        Main.projectile[armProj[0]].ai[0] = 2;
                        Main.projectile[armProj[0]].Center+= new Vector2(160,10);
                        Main.projectile[armProj[1]].rotation = -(float)(Math.PI / 2+Math.PI/16 + Math.Sin(NPC.ai[0]/30)/50);
                        Main.projectile[armProj[1]].spriteDirection = 1;
                        Main.projectile[armProj[1]].ai[0] = 2;
                        Main.projectile[armProj[1]].Center -= new Vector2(160,-10);

                        NPC.velocity = Vector2.Zero;
                        Attack(320);

                        break;
                    case 2:

                        NPC.ai[0] += attackSpeed * attackSpeedMult;

                        if (NPC.ai[0] % 20f==0 && NPC.ai[0]>=100 && NPC.ai[0]<200)
                        {
                            Vector2 newPos = new Vector2(NPC.Center.X + Main.rand.NextFloat(-350f, 350f), NPC.Center.Y);
                            proj = Projectile.NewProjectile(s,newPos, new Vector2(0, 0), ModContent.ProjectileType<Projectiles.Lightning_Spell>(), attacksDamage[curAttack], 1);
                            Main.projectile[proj].hostile = true;
                            Main.projectile[proj].friendly = false;
                        }

                        Main.projectile[armProj[0]].rotation = (float)(Math.PI / 2 + Math.PI / 16 + Math.Sin(NPC.ai[0] / 30) / 50);
                        Main.projectile[armProj[0]].spriteDirection = -1;
                        Main.projectile[armProj[0]].ai[0] = 2;
                        Main.projectile[armProj[0]].Center += new Vector2(160, 10);
                        Main.projectile[armProj[1]].rotation = -(float)(Math.PI / 2 + Math.PI / 16 + Math.Sin(NPC.ai[0] / 30) / 50);
                        Main.projectile[armProj[1]].spriteDirection = 1;
                        Main.projectile[armProj[1]].ai[0] = 2;
                        Main.projectile[armProj[1]].Center -= new Vector2(160, -10);

                        //NPC.velocity = new Vector2(30, 0);
                        Attack(250);
                        break;
                    case 3:
                        if (NPC.ai[0]>50 && NPC.ai[0]<50+attackSpeed * attackSpeedMult * 2)
                        {
                            Vector2 newPos = new Vector2(NPC.Center.X, player.Center.Y-650);
                            proj = Projectile.NewProjectile(s,newPos, new Vector2(0, 0), ModContent.ProjectileType<Projectiles.BossStuff.xion_finalPhase_lightBeam>(), attacksDamage[curAttack], 1);
                            Main.projectile[proj].hostile = true;
                            Main.projectile[proj].friendly = false;
                        }

                        Main.projectile[armProj[0]].rotation = (float)(-Math.PI / 4 + Math.PI / 16 + Math.Sin(NPC.ai[0] / 30) / 50);
                        Main.projectile[armProj[0]].spriteDirection = -1;
                        Main.projectile[armProj[0]].frame = 2;
                        Main.projectile[armProj[0]].ai[0] = 2;
                        Main.projectile[armProj[0]].Center += new Vector2(60, 160);

                        Main.projectile[armProj[1]].rotation = -(float)(-Math.PI / 2 + Math.PI / 16 + Math.Sin(NPC.ai[0] / 30) / 50);
                        Main.projectile[armProj[1]].spriteDirection = -1;
                        Main.projectile[armProj[1]].ai[0] = 2;
                        Main.projectile[armProj[1]].Center -= new Vector2(210, -10);

                        NPC.ai[0] += attackSpeed * attackSpeedMult;
                        //NPC.velocity = new Vector2(-30, 0);
                        Attack(300);
                        break;
                    case 4:
                        //NPC.velocity = new Vector2(0, 30);
                        Attack(100);
                        break;
                }

            }
        }

        public void CoolAnimation(float animValue)
        {

            NPC.Center += new Vector2(0, (float)Math.Sin(animValue / 20));

            Main.projectile[armProj[0]].rotation += (float)Math.Sin((animValue - 5) / 20) * MathHelper.Pi / 10;
            Main.projectile[armProj[0]].position -= new Vector2((float)Math.Sin((animValue - 5) / 20) * -45, -(float)Math.Sin((animValue - 5) / 20) *-29);

            Main.projectile[armProj[1]].rotation -= (float)Math.Sin((animValue - 5) / 20) * MathHelper.Pi / 10;
            Main.projectile[armProj[1]].position += new Vector2((float)Math.Sin((animValue - 5) / 20) * -45, (float)Math.Sin((animValue - 5) / 20) * -29);
            //NPC.behindTiles = true;

        }

        void Attack(int maxAI)
        {
            NPC.ai[1] += attackSpeed * attackSpeedMult;
            if (NPC.ai[1] >= maxAI)
            {
                curAttack = 0;
                CheckCurAttack();
                attackCooldown = 60;
            }
        }

        public Vector2 AttackPos(int attack)
        {
            switch (attack)
            {
                default:
                case 0:
                    return player.Center;
                case 1:
                    return player.Center;
                case 2:
                    return player.Center;
                case 3:
                    return player.Center;
                case 4:
                    return player.Center;
            }
        }

        void CheckCurAttack()
        {
            attackSpeed = 1;
            for (int i = 0; i < armProj.Length; i++)
            {
                Main.projectile[armProj[i]].damage = attacksDamage[curAttack];
            }
            NPC.ai[0] = 0;
            NPC.ai[1] = 0;
            switch (curAttack)
            {
                case 0:
                    NPC.stairFall = NPC.noGravity = true;
                    return;
                case 1:
                    attackSpeed = 2;
                    NPC.stairFall = NPC.noGravity = true;
                    return;
                case 2:
                    NPC.stairFall = NPC.noGravity = true;
                    NPC.direction = -1;
                    NPC.ai[1] = 76;
                    return;
                case 3:
                    NPC.stairFall = NPC.noGravity = true;
                    NPC.ai[1] = 51;
                    NPC.direction = 1;
                    return;
                case 4:
                    NPC.stairFall = NPC.noGravity = true;
                    NPC.ai[1] = 76;
                    return;
            }
        }

        void DespawnHandler()
        {
            if (!player.active || player.dead || player.statLife == 0)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead || player.statLife == 0)
                {
                    NPC.velocity = new Vector2(0, 100000);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 1;
                        Conversation[] conv = new Conversation[] { new Conversation("Try harder", Color.Yellow, DialogSystem.BOSS_DIALOGTIME, "Xion") };
                        DialogSystem.AddConversation(conv);
                    }
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            int[] dropOptions = new int[] { ModContent.ItemType<Items.Weapons.Keyblade_FinalXion>(),ModContent.ItemType<Items.Armor.orgCoat>(),ModContent.ItemType<Items.seasaltIcecream>()};
            npcLoot.Add(ItemDropRule.OneFromOptions(3,dropOptions));

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.twilightCrystal>(), 1, 50, 137));

            npcLoot.Add(ItemDropRule.Common(ItemID.BlueSolution, 1, 10, 15));
        }

        public override bool CheckDead()
        {
            if (!defeated)
            {
                //if (!NPC.downedMoonlord)
                {
                    if (NPC.timeLeft > 20 && !defeated)
                    {
                        NPC.NPCLoot();
                        Conversation[] conv = new Conversation[] { new Conversation("Can't... keep... fighting...", Color.Yellow, DialogSystem.BOSS_DIALOGTIME, "Xion") };
                        DialogSystem.AddConversation(conv);
                    }
                }
                KingdomWorld.downedXionPhases[2] = true;
                defeated = true;
                NPC.life = 1;
            }
            return false;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            position = new Vector2(0,1500);
            scale = 1.5f;
            return null;
        }
    }


}
