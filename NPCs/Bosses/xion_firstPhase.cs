using KingdomTerrahearts.Extra;
using KingdomTerrahearts.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.NPCs.Bosses
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

        void Target()
        {
            player = Main.player[npc.target];
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Xion");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 1500;
            npc.damage = 0;
            npc.defense = 15;
            npc.knockBackResist = 0;
            npc.width = 40;
            npc.height = 60;
            npc.scale = 1;
            npc.value = 500;
            npc.npcSlots = 4;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = false;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Vector to the Heaven");

            npc.ai[0] = 50;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
            for(int i = 0; i < attacksDamage.Length; i++)
            {
                attacksDamage[i]=(int)(attacksDamage[i]* 1.25f * numPlayers);
            }
            npc.defense = (int)(npc.defense + numPlayers);
            attackSpeedMult += numPlayers*0.5f;
        }

        public override void AI()
        {

            npc.velocity = Vector2.Zero;
            npc.TargetClosest();
            Target();
            DespawnHandler();

            if (npc.timeLeft <= 10)
            {
                return;
            }

            if (defeated)
            {
                defeatTime--;
                if (defeatTime == 5)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("darkPortal"), 0, 0);
                }else if (defeatTime <= 0)
                {
                    npc.life = -1;
                    npc.timeLeft = 1;
                    defeatTime = 1000;
                }

                return;
            }


            if (keyProj == -1)
            {
                npc.Center= player.Center + new Vector2(0, -300);
                Conversation[] conv =new Conversation[] { new Conversation("NO HOLDING BACK!",Color.Yellow,50000,"Xion") };

                if (KingdomWorld.downedXionPhases[0])
                    conv[0].dialog = "Let's go again!";
                else if (KingdomWorld.downedXionPhases[1])
                    conv[0].dialog = "Come on!";

                DialogSystem.AddConversation(conv);
                keyProj = Projectile.NewProjectile(npc.Center, npc.velocity, mod.ProjectileType("EnemyKingdomKey"), attacksDamage[curAttack], 1);
                Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("darkPortal"), 0, 0);
            }
            else
            {
                Main.projectile[keyProj].timeLeft = 5;
            }

            if (player!=null && player.active)
            {
                if (attackCooldown > 0)
                {
                    attackCooldown--;
                    npc.velocity = Vector2.Zero;
                    npc.ai[0] =0;
                    npc.ai[1] = 0;
                    Main.projectile[keyProj].Center = new Vector2((npc.spriteDirection - 1) * 17.5f + npc.Center.X, npc.Center.Y + 5);
                    Main.projectile[keyProj].rotation = (npc.spriteDirection > 0) ? 0.25f : 4.5f;
                }
                else
                {
                    switch (curAttack)
                    {
                        case 0:
                            npc.velocity = Vector2.Zero;
                            npc.ai[0] += attackSpeed* attackSpeedMult;
                            if (npc.ai[0] > 50)
                            {
                                curAttack = nextAttack;
                                nextAttack = Main.rand.Next(1, 5);
                                CheckCurAttack();
                            }
                            else
                            {
                                for (int i = 0; i < Main.rand.Next(0, 2); i++)
                                {
                                    Dust.NewDust(AttackPos(nextAttack), npc.width, npc.height, DustID.GoldCoin);
                                }
                            }
                            break;
                        case 1:
                            npc.velocity = Vector2.Zero;
                            Attack(100);
                            break;
                        case 2:
                            npc.velocity = new Vector2(30, 0);
                            Attack(100);
                            break;
                        case 3:
                            npc.velocity = new Vector2(-30, 0);
                            Attack(75);
                            break;
                        case 4:
                            npc.velocity = new Vector2(0, 30);
                            Attack(100);
                            break;
                    }
                }

            }
        }

        void Attack(int maxAI)
        {
            npc.ai[1] += attackSpeed * attackSpeedMult;
            if (npc.ai[1] >= maxAI)
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
                    npc.ai[0] = 0;
                    npc.ai[1] = 0;
                    npc.stairFall = false;
                    return;
                case 1:
                    attackSpeed = 2;
                    npc.Center =AttackPos(curAttack);
                    npc.stairFall  = npc.noGravity = true;
                    Main.projectile[keyProj].Center = npc.Center;
                    npc.ai[1] = 0;
                    return;
                case 2:
                    npc.Center = AttackPos(curAttack) ;
                    npc.stairFall = npc.noGravity = true;
                    npc.direction = -1;
                    npc.ai[1] = 76;
                    return;
                case 3:
                    npc.Center = AttackPos(curAttack);
                    npc.stairFall  = npc.noGravity = true;
                    npc.ai[1] = 51;
                    npc.direction = 1;
                    return;
                case 4:
                    npc.Center = AttackPos(curAttack);
                    npc.stairFall =npc.noGravity = true;
                    npc.ai[1] = 76;
                    return;
            }
        }

        void DespawnHandler()
        {
            if (!player.active || player.dead || player.statLife==0)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead || player.statLife == 0)
                {
                    npc.velocity = new Vector2(0, 100000);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 1;
                        Conversation[] conv = new Conversation[] { new Conversation("Try again", Color.Yellow, 50000, "Xion") };
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
                if (npc.ai[1] > 75)
                    frame = 4;
                else if (npc.ai[1] > 50)
                    frame = 3;
                else if (npc.ai[1] > 25)
                    frame = 2;
                else
                    frame = 1;
            }
            else
            {
                if (npc.velocity.Y != 0)
                    frame = 5;
                else
                    frame = 0;
            }


            if (keyProj != -1 && Main.projectile[keyProj].active)
            {
                Main.projectile[keyProj].spriteDirection = npc.spriteDirection;
                GrabKey(frame);
            }

            npc.frame.Y = frame * frameHeight;
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
                    Main.projectile[keyProj].Center = new Vector2((npc.spriteDirection - 1) * 17.5f + npc.Center.X, npc.Center.Y + 5);
                    Main.projectile[keyProj].rotation = (npc.spriteDirection > 0) ? 0.25f : 4.5f;
                    break;
                case 0:
                    Main.projectile[keyProj].Center = new Vector2((npc.spriteDirection-1)*17.5f+ npc.Center.X, npc.Center.Y+5);
                    Main.projectile[keyProj].rotation = (npc.spriteDirection>0)?0.25f:4.5f;
                    break;
                case 1:
                    Main.projectile[keyProj].Center = npc.Center;
                    Main.projectile[keyProj].rotation = MathHelp.DegreeToQuat(45);
                    break;
                case 2:
                    Main.projectile[keyProj].Center = npc.Center;
                    Main.projectile[keyProj].rotation = 0;
                    break;
                case 3:
                    Main.projectile[keyProj].Center = npc.Center;
                    Main.projectile[keyProj].rotation = 0;
                    break;
                case 4:
                    Main.projectile[keyProj].Center = npc.Center;
                    Main.projectile[keyProj].rotation = 0;
                    break;
                case 5:
                    Main.projectile[keyProj].Center = new Vector2((npc.spriteDirection - 1) * 25 + npc.Center.X+7, npc.Center.Y-30);
                    Main.projectile[keyProj].rotation = (npc.spriteDirection > 0) ? -MathHelp.DegreeToQuat(90) : MathHelp.DegreeToQuat(180)-0.75f;
                    break;
            }
        }

        public override void NPCLoot()
        {

            Item staritem = new Item();
            staritem.SetDefaults(mod.ItemType("twilightShard"));
            staritem.stack = Main.rand.Next(5, 25);
            player.GetItem(15, staritem);

            int randItem = Main.rand.Next(3);
            switch (randItem)
            {
                default:
                    break;
                case 0:
                    Item.NewItem(npc.getRect(), mod.ItemType("Keyblade_Kingdom"));
                    break;
                case 1:
                    Item.NewItem(npc.getRect(), mod.ItemType("orgCoat"));
                    break;
                case 2:
                    Item.NewItem(npc.getRect(), mod.ItemType("seasaltIcecream"));
                    break;

            }

            Conversation[] conv = new Conversation[] { new Conversation("Maybe I'll fight you for real next time.", Color.Yellow, 50000, "Xion") };
            DialogSystem.AddConversation(conv);

        }

        public override bool CheckDead()
        {
            if (!defeated)
            {
                if (!Main.hardMode)
                {
                    if (npc.timeLeft > 20)
                    {
                        NPCLoot();
                    }
                }
                else
                {
                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("xion_secondPhase"), Target: npc.target);
                    npc.timeLeft = (npc.timeLeft > 5) ? 1 : npc.timeLeft-1;
                    defeatTime = 0;
                }
            }
            defeated = true;
            KingdomWorld.downedXionPhases[0] = true;
            npc.life = 1;
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

        void Target()
        {
            player = Main.player[npc.target];
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Xion");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 3500;
            npc.damage = 0;
            npc.defense = 15;
            npc.knockBackResist = 0;
            npc.width = 40;
            npc.height = 60;
            npc.scale = 1;
            npc.value = 500;
            npc.npcSlots = 4;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = false;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Vector to the Heaven");

            npc.ai[0] = 50;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
            for (int i = 0; i < attacksDamage.Length; i++)
            {
                attacksDamage[i] = (int)(attacksDamage[i] * 1.25f * numPlayers);
            }
            npc.defense = (int)(npc.defense + numPlayers);
            attackSpeedMult += numPlayers * 0.5f;
        }

        public override void AI()
        {

            npc.velocity = Vector2.Zero;
            npc.TargetClosest();
            Target();
            DespawnHandler();

            if (npc.timeLeft <= 10)
            {
                return;
            }

            if (defeated)
            {
                defeatTime--;
                if (defeatTime == 5)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("darkPortal"), 0, 0);
                }
                else if (defeatTime <= 0)
                {
                    npc.life = -1;
                    npc.timeLeft = 1;
                    defeatTime = 1000;
                }

                return;
            }


            if (keyProj == -1)
            {
                npc.Center = player.Center + new Vector2(0, -300);
                Conversation[] conv = new Conversation[] { new Conversation("Let's get serious!", Color.Yellow, 50000, "Xion") };
                DialogSystem.AddConversation(conv);
                keyProj = Projectile.NewProjectile(npc.Center, npc.velocity, mod.ProjectileType("EnemyKingdomKey"), attacksDamage[curAttack], 1);
                Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("darkPortal"), 0, 0);
            }
            else
            {
                Main.projectile[keyProj].timeLeft = 5;
            }

            if (player != null && player.active)
            {
                if (attackCooldown > 0)
                {
                    attackCooldown--;
                    npc.velocity = Vector2.Zero;
                    npc.ai[0] = 0;
                    npc.ai[1] = 0;
                    Main.projectile[keyProj].Center = new Vector2((npc.spriteDirection - 1) * 17.5f + npc.Center.X, npc.Center.Y + 5);
                    Main.projectile[keyProj].rotation = (npc.spriteDirection > 0) ? 0.25f : 4.5f;
                }
                else
                {
                    switch (curAttack)
                    {
                        case 0:
                            npc.velocity = Vector2.Zero;
                            npc.ai[0] += attackSpeed * attackSpeedMult;
                            if (npc.ai[0] > 50)
                            {
                                curAttack = nextAttack;
                                nextAttack = Main.rand.Next(1, 5);
                                CheckCurAttack();
                            }
                            else
                            {
                                for (int i = 0; i < Main.rand.Next(0, 2); i++)
                                {
                                    Dust.NewDust(AttackPos(nextAttack), npc.width, npc.height, DustID.GoldCoin);
                                }
                            }
                            break;
                        case 1:
                            npc.velocity = Vector2.Zero;
                            Attack(100);
                            break;
                        case 2:
                            npc.velocity = new Vector2(30, 0);
                            Attack(100);
                            break;
                        case 3:
                            npc.velocity = new Vector2(-30, 0);
                            Attack(75);
                            break;
                        case 4:
                            npc.velocity = new Vector2(0, 30);
                            Attack(100);
                            break;
                    }
                }

            }
        }

        void Attack(int maxAI)
        {
            npc.ai[1] += attackSpeed * attackSpeedMult;
            if (npc.ai[1] >= maxAI)
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
                    npc.ai[0] = 0;
                    npc.ai[1] = 0;
                    npc.stairFall = false;
                    return;
                case 1:
                    attackSpeed = 2;
                    npc.Center = AttackPos(curAttack);
                    npc.stairFall = npc.noGravity = true;
                    Main.projectile[keyProj].Center = npc.Center;
                    npc.ai[1] = 0;
                    return;
                case 2:
                    npc.Center = AttackPos(curAttack);
                    npc.stairFall = npc.noGravity = true;
                    npc.direction = -1;
                    npc.ai[1] = 76;
                    return;
                case 3:
                    npc.Center = AttackPos(curAttack);
                    npc.stairFall = npc.noGravity = true;
                    npc.ai[1] = 51;
                    npc.direction = 1;
                    return;
                case 4:
                    npc.Center = AttackPos(curAttack);
                    npc.stairFall = npc.noGravity = true;
                    npc.ai[1] = 76;
                    return;
            }
        }

        void DespawnHandler()
        {
            if (!player.active || player.dead || player.statLife == 0)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead || player.statLife == 0)
                {
                    npc.velocity = new Vector2(0, 100000);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 1;
                        Conversation[] conv = new Conversation[] { new Conversation("No good", Color.Yellow, 50000, "Xion") };
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
                if (npc.ai[1] > 75)
                    frame = 4;
                else if (npc.ai[1] > 50)
                    frame = 3;
                else if (npc.ai[1] > 25)
                    frame = 2;
                else
                    frame = 1;
            }
            else
            {
                if (npc.velocity.Y != 0)
                    frame = 5;
                else
                    frame = 0;
            }


            if (keyProj != -1 && Main.projectile[keyProj].active)
            {
                Main.projectile[keyProj].spriteDirection = npc.spriteDirection;
                GrabKey(frame);
            }

            npc.frame.Y = frame * frameHeight;
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
                    Main.projectile[keyProj].Center = new Vector2((npc.spriteDirection - 1) * 17.5f + npc.Center.X, npc.Center.Y + 5);
                    Main.projectile[keyProj].rotation = (npc.spriteDirection > 0) ? 0.25f : 4.5f;
                    break;
                case 0:
                    Main.projectile[keyProj].Center = new Vector2((npc.spriteDirection - 1) * 17.5f + npc.Center.X, npc.Center.Y + 5);
                    Main.projectile[keyProj].rotation = (npc.spriteDirection > 0) ? 0.25f : 4.5f;
                    break;
                case 1:
                    Main.projectile[keyProj].Center = npc.Center;
                    Main.projectile[keyProj].rotation = MathHelp.DegreeToQuat(45);
                    break;
                case 2:
                    Main.projectile[keyProj].Center = npc.Center;
                    Main.projectile[keyProj].rotation = 0;
                    break;
                case 3:
                    Main.projectile[keyProj].Center = npc.Center;
                    Main.projectile[keyProj].rotation = 0;
                    break;
                case 4:
                    Main.projectile[keyProj].Center = npc.Center;
                    Main.projectile[keyProj].rotation = 0;
                    break;
                case 5:
                    Main.projectile[keyProj].Center = new Vector2((npc.spriteDirection - 1) * 25 + npc.Center.X + 7, npc.Center.Y - 30);
                    Main.projectile[keyProj].rotation = (npc.spriteDirection > 0) ? -MathHelp.DegreeToQuat(90) : MathHelp.DegreeToQuat(180) - 0.75f;
                    break;
            }
        }

        public override void NPCLoot()
        {

            Item staritem = new Item();
            staritem.SetDefaults(mod.ItemType("twilightShard"));
            staritem.stack = Main.rand.Next(5, 25);
            player.GetItem(15, staritem);

            int randItem = Main.rand.Next(3);
            switch (randItem)
            {
                default:
                    break;
                case 0:
                    Item.NewItem(npc.getRect(), mod.ItemType("Keyblade_Kingdom"));
                    break;
                case 1:
                    Item.NewItem(npc.getRect(), mod.ItemType("orgCoat"));
                    break;
                case 2:
                    Item.NewItem(npc.getRect(), mod.ItemType("seasaltIcecream"));
                    break;

            }

            Conversation[] conv = new Conversation[] { new Conversation("Good fight, but I still went easy on you", Color.Yellow, 50000, "Xion") };
            DialogSystem.AddConversation(conv);

        }

        public override bool CheckDead()
        {
            if (!NPC.downedMoonlord)
            {
                if (npc.timeLeft > 20 && !defeated)
                {
                    NPCLoot();
                }
            }
            else
            {
                if (!defeated)
                {
                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("xion_finalPhase"), Target: npc.target);
                    npc.timeLeft = (npc.timeLeft > 5) ? 1 : npc.timeLeft;
                    defeated = true;
                    defeatTime = 0;
                }
            }
            KingdomWorld.downedXionPhases[1] = true;
            defeated = true;
            npc.life = 1;
            return false;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
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

        void Target()
        {
            player = Main.player[npc.target];
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Xion");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 35000;
            npc.damage = 0;
            npc.defense = 15;
            npc.knockBackResist = 0;
            npc.width = 621/2;
            npc.height = 877;
            npc.scale = 1;
            npc.value = 500;
            npc.npcSlots = 4;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.behindTiles = true;
            npc.hide = true;
            npc.DeathSound = SoundID.NPCDeath1;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Vector to the Heaven");

            npc.ai[0] = 50;
        }

        public override void DrawBehind(int index)
        {
            Main.instance.DrawCacheNPCsMoonMoon.Add(index);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Rectangle rect = new Rectangle((int)(npc.position.X-Main.screenPosition.X-(npc.width/2)),(int)( npc.position.Y-Main.screenPosition.Y), npc.width*2, npc.height);
            spriteBatch.Draw(mod.GetTexture("NPCs/Bosses/xion_finalPhase"), rect, Color.White);
            return false;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
            for (int i = 0; i < attacksDamage.Length; i++)
            {
                attacksDamage[i] = (int)(attacksDamage[i] * 1.25f * numPlayers);
            }
            npc.defense = (int)(npc.defense + numPlayers);
            attackSpeedMult += numPlayers * 0.5f;
        }

        public override void AI()
        {

            npc.velocity = Vector2.Zero;
            npc.TargetClosest();
            Target();
            DespawnHandler();

            if (npc.timeLeft <= 10)
            {
                return;
            }

            if (defeated)
            {
                npc.velocity = Vector2.Zero;
                defeatTime--;
                if (defeatTime <= 0)
                {
                    npc.life = -1;
                    npc.timeLeft = 1;
                    defeatTime = 1000;
                }

                return;
            }

            for (int i = 0; i < armProj.Length; i++)
            {
                if (armProj[i] == -1)
                {
                    npc.Center = player.Center + new Vector2(0, -300);
                    Conversation[] conv = new Conversation[] { new Conversation("Let's get serious!", Color.Yellow, 50000, "Xion") };
                    DialogSystem.AddConversation(conv);
                    armProj[i] = Projectile.NewProjectile(npc.Center, npc.velocity, mod.ProjectileType("xion_finalPhase_arms"), attacksDamage[curAttack], 1);
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("darkPortal"), 0, 0);
                }
                else
                {
                    Main.projectile[armProj[i]].timeLeft = 5;
                    switch (i)
                    {
                        case 0:
                            Main.projectile[armProj[i]].Center = new Vector2(npc.Center.X - 157f, npc.Center.Y - 295f);
                            Main.projectile[armProj[i]].spriteDirection = 1;
                            Main.projectile[armProj[i]].rotation = -(float)Math.PI / 2; 
                            break;
                        case 1:
                            Main.projectile[armProj[i]].Center = new Vector2(npc.Center.X + 157f, npc.Center.Y - 295f);
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
                        npc.velocity = Vector2.Zero;
                        npc.ai[0] += attackSpeed * attackSpeedMult;
                        if (npc.ai[0] > 150)
                        {
                            curAttack = nextAttack;
                            nextAttack = Main.rand.Next(1, 4);
                            CheckCurAttack();
                        }
                        else
                        {
                            CoolAnimation(npc.ai[0]);
                        }
                        break;
                    case 1:

                        npc.ai[0] += attackSpeed * attackSpeedMult;

                        npc.Center += new Vector2(0, (float)Math.Sin(npc.ai[0] / 50));
                        if (npc.ai[0] < 175f)
                        {
                            for(int i = 0; i < 15; i++)
                            {
                                Dust.NewDust(player.position-new Vector2(player.width/2,player.height/2), player.width*2, player.height * 2, 175);
                            }
                        }else if (npc.ai[0] >200f && npc.ai[0]<200f+attackSpeed*attackSpeedMult*2)
                        {
                            proj = Projectile.NewProjectile(player.Center, new Vector2(0, 0), mod.ProjectileType("Lightning_Spell"), attacksDamage[curAttack], 1);
                            Main.projectile[proj].hostile = true;
                            Main.projectile[proj].friendly = false;
                        }

                        Main.projectile[armProj[0]].rotation = (float)(Math.PI / 2+Math.PI/16+Math.Sin(npc.ai[0]/30)/50);
                        Main.projectile[armProj[0]].spriteDirection = -1;
                        Main.projectile[armProj[0]].ai[0] = 2;
                        Main.projectile[armProj[0]].Center+= new Vector2(160,10);
                        Main.projectile[armProj[1]].rotation = -(float)(Math.PI / 2+Math.PI/16 + Math.Sin(npc.ai[0]/30)/50);
                        Main.projectile[armProj[1]].spriteDirection = 1;
                        Main.projectile[armProj[1]].ai[0] = 2;
                        Main.projectile[armProj[1]].Center -= new Vector2(160,-10);

                        npc.velocity = Vector2.Zero;
                        Attack(320);

                        break;
                    case 2:

                        npc.ai[0] += attackSpeed * attackSpeedMult;

                        if (npc.ai[0] % 20f==0 && npc.ai[0]>=100 && npc.ai[0]<200)
                        {
                            Vector2 newPos = new Vector2(npc.Center.X + Main.rand.NextFloat(-350f, 350f), npc.Center.Y);
                            proj = Projectile.NewProjectile(newPos, new Vector2(0, 0), mod.ProjectileType("Lightning_Spell"), attacksDamage[curAttack], 1);
                            Main.projectile[proj].hostile = true;
                            Main.projectile[proj].friendly = false;
                        }

                        Main.projectile[armProj[0]].rotation = (float)(Math.PI / 2 + Math.PI / 16 + Math.Sin(npc.ai[0] / 30) / 50);
                        Main.projectile[armProj[0]].spriteDirection = -1;
                        Main.projectile[armProj[0]].ai[0] = 2;
                        Main.projectile[armProj[0]].Center += new Vector2(160, 10);
                        Main.projectile[armProj[1]].rotation = -(float)(Math.PI / 2 + Math.PI / 16 + Math.Sin(npc.ai[0] / 30) / 50);
                        Main.projectile[armProj[1]].spriteDirection = 1;
                        Main.projectile[armProj[1]].ai[0] = 2;
                        Main.projectile[armProj[1]].Center -= new Vector2(160, -10);

                        //npc.velocity = new Vector2(30, 0);
                        Attack(250);
                        break;
                    case 3:
                        if (npc.ai[0]>50 && npc.ai[0]<50+attackSpeed * attackSpeedMult * 2)
                        {
                            Vector2 newPos = new Vector2(npc.Center.X, player.Center.Y-650);
                            proj = Projectile.NewProjectile(newPos, new Vector2(0, 0), mod.ProjectileType("xion_finalPhase_lightBeam"), attacksDamage[curAttack], 1);
                            Main.projectile[proj].hostile = true;
                            Main.projectile[proj].friendly = false;
                        }

                        Main.projectile[armProj[0]].rotation = (float)(-Math.PI / 4 + Math.PI / 16 + Math.Sin(npc.ai[0] / 30) / 50);
                        Main.projectile[armProj[0]].spriteDirection = -1;
                        Main.projectile[armProj[0]].frame = 2;
                        Main.projectile[armProj[0]].ai[0] = 2;
                        Main.projectile[armProj[0]].Center += new Vector2(60, 160);

                        Main.projectile[armProj[1]].rotation = -(float)(-Math.PI / 2 + Math.PI / 16 + Math.Sin(npc.ai[0] / 30) / 50);
                        Main.projectile[armProj[1]].spriteDirection = -1;
                        Main.projectile[armProj[1]].ai[0] = 2;
                        Main.projectile[armProj[1]].Center -= new Vector2(210, -10);

                        npc.ai[0] += attackSpeed * attackSpeedMult;
                        //npc.velocity = new Vector2(-30, 0);
                        Attack(300);
                        break;
                    case 4:
                        //npc.velocity = new Vector2(0, 30);
                        Attack(100);
                        break;
                }

            }
        }

        public void CoolAnimation(float animValue)
        {

            npc.Center += new Vector2(0, (float)Math.Sin(animValue / 20));

            Main.projectile[armProj[0]].rotation += (float)Math.Sin((animValue - 5) / 20) * MathHelper.Pi / 10;
            Main.projectile[armProj[0]].position -= new Vector2((float)Math.Sin((animValue - 5) / 20) * -45, -(float)Math.Sin((animValue - 5) / 20) *-29);

            Main.projectile[armProj[1]].rotation -= (float)Math.Sin((animValue - 5) / 20) * MathHelper.Pi / 10;
            Main.projectile[armProj[1]].position += new Vector2((float)Math.Sin((animValue - 5) / 20) * -45, (float)Math.Sin((animValue - 5) / 20) * -29);
            //npc.behindTiles = true;

        }

        void Attack(int maxAI)
        {
            npc.ai[1] += attackSpeed * attackSpeedMult;
            if (npc.ai[1] >= maxAI)
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
            npc.ai[0] = 0;
            npc.ai[1] = 0;
            switch (curAttack)
            {
                case 0:
                    npc.stairFall = npc.noGravity = true;
                    return;
                case 1:
                    attackSpeed = 2;
                    npc.stairFall = npc.noGravity = true;
                    return;
                case 2:
                    npc.stairFall = npc.noGravity = true;
                    npc.direction = -1;
                    npc.ai[1] = 76;
                    return;
                case 3:
                    npc.stairFall = npc.noGravity = true;
                    npc.ai[1] = 51;
                    npc.direction = 1;
                    return;
                case 4:
                    npc.stairFall = npc.noGravity = true;
                    npc.ai[1] = 76;
                    return;
            }
        }

        void DespawnHandler()
        {
            if (!player.active || player.dead || player.statLife == 0)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead || player.statLife == 0)
                {
                    npc.velocity = new Vector2(0, 100000);
                    if (npc.timeLeft > 10)
                    {
                        npc.timeLeft = 1;
                        Conversation[] conv = new Conversation[] { new Conversation("Try harder", Color.Yellow, 50000, "Xion") };
                        DialogSystem.AddConversation(conv);
                    }
                }
            }
        }

        public override void NPCLoot()
        {

            Item staritem = new Item();
            staritem.SetDefaults(mod.ItemType("twilightShard"));
            staritem.stack = Main.rand.Next(5, 25);
            player.GetItem(15, staritem);

            int randItem = Main.rand.Next(3);
            switch (randItem)
            {
                default:
                    break;
                case 0:
                    Item.NewItem(npc.getRect(), mod.ItemType("Keyblade_FinalXion"));
                    break;
                case 1:
                    Item.NewItem(npc.getRect(), mod.ItemType("orgCoat"));
                    break;
                case 2:
                    Item.NewItem(npc.getRect(), mod.ItemType("seasaltIcecream"));
                    break;

            }

            Conversation[] conv = new Conversation[] { new Conversation("Can't... keep... fighting...", Color.Yellow, 50000, "Xion") };
            DialogSystem.AddConversation(conv);

        }

        public override bool CheckDead()
        {
            if (!defeated)
            {
                //if (!NPC.downedMoonlord)
                {
                    if (npc.timeLeft > 20 && !defeated)
                    {
                        NPCLoot();
                    }
                }
                KingdomWorld.downedXionPhases[1] = true;
                defeated = true;
                npc.life = 1;
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
