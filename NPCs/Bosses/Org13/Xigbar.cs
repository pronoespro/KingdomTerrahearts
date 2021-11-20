using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using KingdomTerrahearts.Interface;

namespace KingdomTerrahearts.NPCs.Bosses.Org13
{
    [AutoloadBossHead]
    public class xigbar_FirstPhase : Base_OrgMember
    {

        private int[] attackProjectiles=new int[0];

        public override string BossHeadTexture => "KingdomTerrahearts/NPCs/Bosses/Org13/xion_finalPhase_Head_Boss";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Xigbar");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 1200;
            NPC.width = 40;
            NPC.height = 60;
            Music = MusicLoader.GetMusicSlot("KingdomTerrahearts/Sounds/Music/The 13th Struggle");
            NPC.friendly = false;
            NPC.boss = true;
            NPC.damage = 40;
            NPC.aiStyle = 0;
            NPC.npcSlots = 4;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            s = new ProjectileSource_NPC(NPC);

            attacksDamage = new int[] { 0, 10, 25, 15,15,15,15,15,15 };
            weaponType = ModContent.ProjectileType<Projectiles.BossStuff.sharpShooterProj>();
            weaponProj = new int[] { -1 };
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


            hitRecoil--;
            if (hitRecoil > 0)
            {
                NPC.velocity *= 0.75f;
                NPC.rotation += 0.1f * -NPC.direction;
                return;
            }
            else if (hitRecoil <= -90)
            {
                hitCombo = 0;
            }

            NPC.rotation = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;

            NPC.TargetClosest(true);

            for (int i = 0; i < weaponProj.Length; i++)
            {
                if (weaponProj[i] == -1)
                {
                    NPC.Center = Main.player[NPC.target].Center + new Vector2(0, -300);

                    Conversation[] conv = new Conversation[] { new Conversation("Get a load of this!", Color.Yellow, DialogSystem.BOSS_DIALOGTIME, "Xigbar") };
                    DialogSystem.AddConversation(conv);

                    weaponProj[i] = Projectile.NewProjectile(s, NPC.Center, NPC.velocity, weaponType, attacksDamage[curAttack], 1);
                    Projectile.NewProjectile(s, NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.darkPortal>(), 0, 0);
                }
                else
                {
                    if (Main.projectile[weaponProj[i]].type != weaponType || !Main.projectile[weaponProj[i]].active)
                    {
                        weaponProj[i] = Projectile.NewProjectile(s, NPC.Center, NPC.velocity, weaponType, attacksDamage[curAttack], 1);
                    }
                    Main.projectile[weaponProj[i]].timeLeft = 5;
                }
            }

            if (NPC.target >= 0)
            {
                DespawnHandler(Main.player[NPC.target]);

                if (NPC.timeLeft <= 10)
                {
                    return;
                }

                if (defeated)
                {
                    NPC.velocity = Vector2.Zero;
                    for (int i = 0; i < weaponProj.Length; i++)
                    {
                        Main.projectile[weaponProj[i]].Center = HoldWeaponPoint();
                        Main.projectile[weaponProj[i]].rotation = (float)Math.PI * ((NPC.spriteDirection - 1) / 2);
                    }
                    defeatTime--;
                    if (defeatTime == 5)
                    {
                        Projectile.NewProjectile(s, NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.darkPortal>(), 0, 0);
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

                if (Main.player[NPC.target].Center.Y < NPC.Center.Y - 200)
                {
                    NPC.Center = new Vector2(Main.player[NPC.target].Center.X, Main.screenPosition.Y + NPC.height);
                }

                switch (curAttack)
                {
                    case 0:
                        NPC.velocity = Vector2.Zero;
                        for (int i = 0; i < weaponProj.Length; i++)
                        {
                            if (MathHelp.Magnitude(Main.projectile[weaponProj[i]].Center - NPC.Center) < 5)
                            {
                                Main.projectile[weaponProj[i]].Center = HoldWeaponPoint();
                            }
                            else
                            {
                                Main.projectile[weaponProj[i]].velocity = Vector2.Lerp(Main.projectile[weaponProj[i]].velocity, MathHelp.Normalize(NPC.Center - Main.projectile[weaponProj[i]].Center) * 5, 0.5f);
                            }
                            Main.projectile[weaponProj[i]].rotation = (float)Math.PI * ((NPC.spriteDirection - 1) / 2);
                        }
                        NPC.ai[0]++;
                        if (NPC.ai[0] > 90)
                        {
                            curAttack = Main.rand.Next(1, 5);
                            CheckCurAttack();
                        }
                        break;
                    case 1:

                        if (NPC.ai[1] == 1)
                        {
                            NPC.Center = Main.player[NPC.target].Center + new Vector2(200 * NPC.spriteDirection, -100);
                        }
                        else if (NPC.ai[1] < 20)
                        {
                            for (int i = 0; i < weaponProj.Length; i++)
                            {
                                Main.projectile[weaponProj[i]].Center = HoldWeaponPoint();
                                Main.projectile[weaponProj[i]].rotation = (float)Math.PI * ((NPC.spriteDirection - 1) / 2);
                            }
                        }
                        else
                        {
                            Vector2 playerOffsetToGun;
                            for (int i = 0; i < weaponProj.Length; i++)
                            {
                                playerOffsetToGun = Main.player[NPC.target].Center-Main.projectile[weaponProj[i]].Center;
                                Main.projectile[weaponProj[i]].Center = HoldWeaponPoint();
                                Main.projectile[weaponProj[i]].rotation =MathHelp.DegreeToQuat(Math.Atan2(playerOffsetToGun.Y,playerOffsetToGun.X));
                            }
                            if (NPC.ai[1] > 30 && (NPC.ai[1]) % 10 == 0)
                            {
                                playerOffsetToGun = Main.player[NPC.target].Center - NPC.Center;
                                int proj = Projectile.NewProjectile(s, NPC.Center, MathHelp.Normalize(playerOffsetToGun) * 13, ProjectileID.JestersArrow, attacksDamage[1], 1);
                                Main.projectile[proj].hostile = true;
                                Main.projectile[proj].friendly = false;
                            }
                        }

                        NPC.velocity/=2;
                        NPC.spriteDirection = -NPC.spriteDirection;
                        NPC.rotation = (float)Math.PI;


                        AttackTimerCheck(30+60);
                        break;
                    case 2:


                        if (NPC.ai[1] == 1)
                        {
                            NPC.Center = Main.player[NPC.target].Center + new Vector2(350 * NPC.spriteDirection, 0);
                        }
                        else if (NPC.ai[1] < 20)
                        {
                            for (int i = 0; i < weaponProj.Length; i++)
                            {
                                Main.projectile[weaponProj[i]].Center = HoldWeaponPoint();
                                Main.projectile[weaponProj[i]].rotation = (float)Math.PI * ((NPC.spriteDirection - 1) / 2);
                            }
                        }
                        else
                        {

                            Vector2 playerOffsetToGun;
                            for (int i = 0; i < weaponProj.Length; i++)
                            {
                                playerOffsetToGun = Main.player[NPC.target].Center - Main.projectile[weaponProj[i]].Center;
                                Main.projectile[weaponProj[i]].Center = HoldWeaponPoint();
                                Main.projectile[weaponProj[i]].rotation = Math.Abs(MathHelp.DegreeToQuat(Math.Atan2(playerOffsetToGun.Y, playerOffsetToGun.X)));
                            }
                            if (NPC.ai[1] > 30 && (NPC.ai[1]) % 10 == 0)
                            {
                                playerOffsetToGun = Main.player[NPC.target].Center - NPC.Center;
                                int proj = Projectile.NewProjectile(s, NPC.Center, MathHelp.Normalize(playerOffsetToGun) * 13, ProjectileID.JestersArrow, attacksDamage[1], 1);
                                Main.projectile[proj].hostile = true;
                                Main.projectile[proj].friendly = false;
                            }
                        }

                        NPC.velocity =new Vector2(2.5f,0)*NPC.spriteDirection;
                        NPC.velocity.Y = 0;

                        AttackTimerCheck(100);

                        break;
                    case 3:


                        for (int i = 0; i < weaponProj.Length; i++)
                        {
                            Main.projectile[weaponProj[i]].Center = HoldWeaponPoint();
                            Main.projectile[weaponProj[i]].rotation = 0;
                            Main.projectile[weaponProj[i]].spriteDirection = NPC.spriteDirection;
                        }

                        if (NPC.ai[1] == 1)
                        {
                            NPC.Center = Main.player[NPC.target].Center + new Vector2(-250, -300);

                            bool projectileStillSpawned = false;
                            for (int i = 0; i < Main.maxProjectiles; i++)
                            {
                                if (Main.projectile[i].active && Main.projectile[i].type==ModContent.ProjectileType<Projectiles.BossStuff.xigbar_missile>())
                                {
                                    projectileStillSpawned = true;
                                }
                            }
                            if (projectileStillSpawned)
                            {
                                curAttack = 1;
                                CheckCurAttack();
                                break;
                            }

                            attackProjectiles = new int[] {
                                Projectile.NewProjectile(s,NPC.Center,new Vector2(5*NPC.spriteDirection,0),ModContent.ProjectileType<Projectiles.BossStuff.xigbar_missile>(),attacksDamage[3],2, ai0:NPC.spriteDirection)
                            };
                        }
                        if (NPC.ai[1] < 75)
                        {
                            for(int i = 0; i < attackProjectiles.Length; i++)
                            {
                                Main.projectile[attackProjectiles[i]].Center = HoldWeaponPoint();
                            }
                        }
                        if (NPC.ai[1] == 99)
                        {
                            for (int i = 0; i < weaponProj.Length; i++)
                            {
                                Main.projectile[weaponProj[i]].spriteDirection = 1;
                            }
                        }
                        AttackTimerCheck(100);
                        break;
                    case 4:
                        for (int i = 0; i < weaponProj.Length; i++)
                        {
                            Main.projectile[weaponProj[i]].Center = HoldWeaponPoint();
                            Main.projectile[weaponProj[i]].rotation = (float)Math.PI/2;
                        }

                        if (NPC.ai[1] <= 30)
                        {
                            NPC.Center = Main.player[NPC.target].Center + new Vector2(0, -250);
                        }
                        else{
                            if (NPC.ai[1] % 10 == 0)
                            {
                                int proj = Projectile.NewProjectile(s, NPC.Center, new Vector2(Main.rand.NextFloat(-2f, 2f), 15), ProjectileID.JestersArrow, attacksDamage[4], 1);
                                Main.projectile[proj].hostile = true;
                                Main.projectile[proj].friendly = false;
                            }
                        }

                        AttackTimerCheck(120);
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:
                        break;
                    case 8:
                        break;
                    case 9:

                        NPC.velocity = Vector2.Zero;

                        if (NPC.ai[1] == 1)
                        {
                            NPC.Center = Main.player[NPC.target].Center + new Vector2(350 * -NPC.direction, 150);
                        }

                        AttackTimerCheck(90);
                        break;
                }
            }else{
                NPC.TargetClosest(false);
                if (NPC.target < 0)
                {
                    if (NPC.timeLeft >= 10)
                    {
                        NPC.timeLeft = 1;
                        DespawnQuote();
                    }
                }
            }

        }

        public override void FindFrame(int frameHeight)
        {


            int frame = 0;
            if (hitRecoil > 0)
            {
                frame = 5;
            }
            else
            {
                switch (curAttack)
                {
                    case 0:
                        frame = 0;
                        break;
                    case 1:
                        frame =(NPC.ai[1]<15)?3:2;
                        break;
                    case 2:
                        frame = 2;
                        break;
                    case 3:
                        frame = 2;
                        break;
                    case 4:
                        frame = 1;
                        break;
                }
            }

            NPC.frame.Y = frame * frameHeight;

        }

        public override void DefeatQuote()
        {
            Conversation[] conv = new Conversation[] { new Conversation("If only I had a keyblade...", Color.Yellow, DialogSystem.BOSS_DIALOGTIME, "Xigbar") };
            DialogSystem.AddConversation(conv);
        }

        public override void DespawnQuote()
        {
            Conversation[] conv = new Conversation[] { new Conversation("Shoot you later, kiddo!", Color.Yellow, DialogSystem.BOSS_DIALOGTIME, "Xigbar") };
            DialogSystem.AddConversation(conv);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            if((curAttack==9 && NPC.ai[1] < 40)||(curAttack==1 && NPC.ai[1]<5))
            {
                return false;
            }

            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void CheckCurAttack()
        {
            attackSpeed = 1;
            NPC.damage = attacksDamage[curAttack];
            switch (curAttack)
            {
                case 0:
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    break;
                case 1:
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    break;
                case 2:
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    break;
                case 3:
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    break;
                case 4:
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    break;
                case 9:
                    NPC.ai[0] = 0;
                    NPC.ai[1] = 0;
                    break;
            }
        }

        public override void OnHitEffect()
        {
            if (!defeated)
            {
                NPC.rotation = 0.2f * NPC.direction;
            }
        }

        public override Vector2 HoldWeaponPoint()
        {
            return NPC.Center + new Vector2(0,(Math.Abs(NPC.rotation)>0)?-15: 15);
        }

        public override void RevengeValueMet()
        {
            curAttack = 9;
            CheckCurAttack();
        }

        public override bool SpecialNoHit()
        {
            return curAttack != 9;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            int[] dropOptions = new int[] { ModContent.ItemType<Items.Weapons.Keyblade_Lionheart>(), ModContent.ItemType<Items.Weapons.Org13.Xigbar.sharpshooter>(), ModContent.ItemType<Items.Armor.orgCoat>(), ModContent.ItemType<Items.glide>() };
            npcLoot.Add(ItemDropRule.OneFromOptions(1, dropOptions));


            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<xigbarSpawner>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.twilightShard>(), 1, 5, 15));
        }

    }

    //===================
    //===== SPAWNER =====
    //===================

    public class xigbarSpawner : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Master's Eye");
            Tooltip.SetDefault("Summons the Freeshooter" +
                "\nThis is the arrowgun that will pierce the heavens!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 75;
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.HiddenAnimation;
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 1000;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<xigbar_FirstPhase>());
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnBoss((int)player.Center.X, (int)Main.screenPosition.Y, ModContent.NPCType<xigbar_FirstPhase>(), player.whoAmI);

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.Materials.twilightShard>(), 10)
            .AddIngredient(ItemID.Lens, 1)
            .AddTile(TileID.Anvils)
            .Register();
        }

    }
}
