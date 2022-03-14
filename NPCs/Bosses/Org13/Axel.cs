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
    public class Axel_FirstPhase:Base_OrgMember
    {

        public int[] heldProjectiles;

        public override string BossHeadTexture => "KingdomTerrahearts/NPCs/Bosses/Org13/xion_finalPhase_Head_Boss";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Axel");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.lifeMax = 3000;
            NPC.width = 40;
            NPC.height = 60;
            Music = MusicLoader.GetMusicSlot("KingdomTerrahearts/Sounds/Music/The 13th Struggle");
            NPC.friendly = false;
            NPC.boss = true;
            NPC.damage = 40;
            NPC.aiStyle= 0;
            NPC.npcSlots = 4;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            s = new EntitySource_Parent(NPC);

            attacksDamage = new int []{0,10,25,15 };
            weaponType = ModContent.ProjectileType<Projectiles.BossStuff.AxelChakrams>();
            weaponProj = new int[] { -1,-1 };
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

        public override void OnHitEffect()
        {
            NPC.rotation = 0;
        }

        public override void AI()
        {

            hitRecoil--;
            if (hitRecoil > 0)
            {
                for (int i = 0; i < weaponProj.Length; i++)
                {
                    Main.projectile[weaponProj[i]].timeLeft = 0;
                }
                NPC.rotation += 0.3f*-NPC.direction;
                return;
            }else if (hitRecoil <= -90){
                hitCombo = 0;
            }

            NPC.rotation = 0;
            NPC.noTileCollide = false;

            NPC.TargetClosest(true);


            for (int i = 0; i < weaponProj.Length; i++)
            {
                if (weaponProj[i] == -1)
                {
                    NPC.Center = Main.player[NPC.target].Center + new Vector2(0, -300);

                    Conversation[] conv = new Conversation[] { new Conversation("You asked for it", Color.Red, DialogSystem.BOSS_DIALOGTIME, "Axel") };
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
                Projectiles.BossStuff.AxelChakrams chakram = (Projectiles.BossStuff.AxelChakrams)Main.projectile[weaponProj[i]].ModProjectile;
                chakram.bossOwner = NPC.whoAmI;
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
                    NPC.Center =new Vector2(Main.player[NPC.target].Center.X,Main.screenPosition.Y+NPC.height);
                }
                for (int i = 0; i < weaponProj.Length; i++)
                {
                    Main.projectile[weaponProj[i]].damage = attacksDamage[curAttack];
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
                            else{
                                Main.projectile[weaponProj[i]].velocity =Vector2.Lerp(Main.projectile[weaponProj[i]].velocity, MathHelp.Normalize(NPC.Center- Main.projectile[weaponProj[i]].Center) *5,0.5f);
                            }
                            Main.projectile[weaponProj[i]].rotation = 0;
                        }
                        NPC.ai[0]++;
                        if (NPC.ai[0] > 90)
                        {
                            NPC.ai[0] = 0;
                            curAttack = Main.rand.Next(1, 4);
                        }
                        break;
                    case 1:

                        Vector2 offset = Vector2.Zero;

                        //Chakrams
                        if (NPC.ai[1] < 21)
                        {
                            for (int i = 0; i < weaponProj.Length; i++)
                            {
                                Main.projectile[weaponProj[i]].Center = HoldWeaponPoint();
                            }
                        }
                        else
                        {
                            for (int i = 0; i < weaponProj.Length; i++)
                            {
                                if (NPC.ai[1] > 21 + i * 3)
                                {
                                    Main.projectile[weaponProj[i]].velocity = MathHelp.Normalize(Main.player[NPC.target].Center - Main.projectile[weaponProj[i]].Center) * 2;

                                    Main.projectile[weaponProj[i]].rotation += 0.4f;
                                }
                                else
                                {
                                    Main.projectile[weaponProj[i]].Center = HoldWeaponPoint();
                                }
                            }
                        }

                        //Axel himself dashing
                        if (NPC.ai[1] < 15)
                        {
                            if (NPC.ai[1] <= 1 + attackSpeed * attackSpeedMult)
                            {
                                NPC.ai[0] = MathHelp.Sign(Main.player[NPC.target].Center.X - NPC.Center.X);
                            }

                            NPC.noTileCollide = true;

                            offset = Main.player[NPC.target].Center - NPC.Center;
                            offset = offset + new Vector2(0, NPC.height + Main.player[NPC.target].height / 2);

                            NPC.velocity = offset * 0.25f;

                        }
                        else if (NPC.ai[1] < 30)
                        {

                            NPC.noTileCollide = true;

                            offset = Main.player[NPC.target].Center - NPC.Center;
                            offset = offset + new Vector2((NPC.width * 2 + Main.player[NPC.target].width * 2) * NPC.ai[0], -40);

                            NPC.velocity = offset * 0.25f;

                        }
                        else
                        {
                            NPC.noTileCollide = false;
                            NPC.velocity = offset;
                        }

                        //flaming aspect
                        for (int i = 0; i < 2; i++)
                        {
                            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, SpeedY: Main.rand.Next(-5, 5), newColor: Color.Red);
                        }

                        AttackTimerCheck(50);

                        break;
                    case 2:

                        NPC.Center += new Vector2(NPC.direction * 1, 0);

                        if (NPC.ai[1] < 40)
                        {
                            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch);
                            for (int i = 0; i < weaponProj.Length; i++)
                            {
                                if (Vector2.Distance(HoldWeaponPoint(), Main.projectile[weaponProj[i]].Center) > 3)
                                {
                                    Main.projectile[weaponProj[i]].velocity = MathHelp.Normalize(HoldWeaponPoint() - Main.projectile[weaponProj[i]].Center) * 3;
                                }
                            }

                        }
                        else
                        {

                            for (int i = 0; i < weaponProj.Length; i++)
                            {
                                if (NPC.ai[1] % 40 < 20)
                                {
                                    if (NPC.ai[1] % 40 > i * 3)
                                    {
                                        Main.projectile[weaponProj[i]].velocity = MathHelp.Normalize(Main.player[NPC.target].Center - Main.projectile[weaponProj[i]].Center) * 5;
                                        Main.projectile[weaponProj[i]].rotation -= 0.4f;
                                    }
                                    else if (Vector2.Distance(HoldWeaponPoint(), Main.projectile[weaponProj[i]].Center) > 3)
                                    {
                                        Main.projectile[weaponProj[i]].velocity = MathHelp.Normalize(HoldWeaponPoint() - Main.projectile[weaponProj[i]].Center) * 3;
                                    }
                                }
                                else if (Vector2.Distance(HoldWeaponPoint(), Main.projectile[weaponProj[i]].Center) > 3)
                                {
                                    Main.projectile[weaponProj[i]].velocity = MathHelp.Normalize(HoldWeaponPoint() - Main.projectile[weaponProj[i]].Center) * 5;
                                }
                            }
                        }

                        AttackTimerCheck(160);
                        break;
                    case 3:

                        for (int i = 0; i < weaponProj.Length; i++)
                        {
                            if (NPC.ai[1] > i * 3)
                            {
                                Main.projectile[weaponProj[i]].velocity =  MathHelp.Normalize(Main.player[NPC.target].Center + new Vector2(20 * i, 0) - Main.projectile[weaponProj[i]].Center) * 3;
                                Main.projectile[weaponProj[i]].rotation += 0.4f;
                            }
                            else if (Vector2.Distance(HoldWeaponPoint(), Main.projectile[weaponProj[i]].Center) > 3)
                            {
                                Main.projectile[weaponProj[i]].velocity = MathHelp.Normalize(HoldWeaponPoint() - Main.projectile[weaponProj[i]].Center) * 3;
                            }
                        }

                        if (NPC.ai[1] == 40)
                        {

                            for (int i = 0; i < weaponProj.Length; i++)
                            {
                                Main.projectile[weaponProj[i]].velocity = (Main.player[NPC.target].Center + new Vector2(20 * i, 0) - Main.projectile[weaponProj[i]].Center)*2;
                            }
                        }

                        AttackTimerCheck(80);
                        break;
                    case 4:

                        offset = Vector2.Zero;

                        //Chakrams
                        for (int i = 0; i < weaponProj.Length; i++)
                        {
                            if (NPC.ai[1] > i * 3)
                            {
                                Main.projectile[weaponProj[i]].velocity = MathHelp.Normalize(Main.player[NPC.target].Center - Main.projectile[weaponProj[i]].Center) * 3;

                                Main.projectile[weaponProj[i]].rotation += 0.7f;
                            }
                            else if (Vector2.Distance(HoldWeaponPoint(), Main.projectile[weaponProj[i]].Center) > 3)
                            {
                                Main.projectile[weaponProj[i]].velocity = MathHelp.Normalize(HoldWeaponPoint() - Main.projectile[weaponProj[i]].Center) * 3;
                            }
                        }

                        //Axel himself dashing
                        if (NPC.ai[1] < 2)
                        {
                            if (NPC.ai[1] <= 1 + attackSpeed * attackSpeedMult)
                            {
                                NPC.ai[0] = MathHelp.Sign(Main.player[NPC.target].Center.X - NPC.Center.X);
                            }

                            NPC.noTileCollide = true;

                            offset = Main.player[NPC.target].Center - NPC.Center;
                            offset = offset + new Vector2(0, NPC.height + Main.player[NPC.target].height / 2);

                            NPC.velocity = offset * 0.5f;

                        }
                        else if (NPC.ai[1] < 8)
                        {

                            NPC.noTileCollide = true;

                            offset = Main.player[NPC.target].Center - NPC.Center;
                            offset = offset + new Vector2((NPC.width * 2 + Main.player[NPC.target].width * 2) * NPC.ai[0], -40);

                            NPC.velocity = offset * 0.5f;

                        }
                        else
                        {
                            NPC.noTileCollide = false;
                            NPC.velocity = offset;
                        }

                        //flaming aspect
                        for (int i = 0; i < 2; i++)
                        {
                            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, SpeedY: Main.rand.Next(-5, 5), newColor: Color.Red);
                        }

                        AttackTimerCheck(25);

                        break;
                }

            }

        }

        public override void DefeatQuote()
        {
            Conversation[] conv = new Conversation[] { new Conversation("How did this happen...?", Color.Red, DialogSystem.BOSS_DIALOGTIME, "Axel") };
            DialogSystem.AddConversation(conv);
        }

        public override void DespawnQuote()
        {
            Conversation[] conv;

            switch (Main.rand.Next(0, 4))
            {
                default:
                case 0:
                    conv = new Conversation[] { new Conversation("Sorry, kid. No one axes Axel", Color.Red, DialogSystem.BOSS_DIALOGTIME, "Axel") };
                    break;
                case 1:
                    conv = new Conversation[] { new Conversation("Don't say I didn't warn you!", Color.Red, DialogSystem.BOSS_DIALOGTIME, "Axel") };
                    break;
            }
            DialogSystem.AddConversation(conv);
        }

        public override void RevengeValueMet()
        {
            curAttack = 4;
        }

        public override bool SpecialNoHit()
        {
            return curAttack != 4;
        }

        public override Vector2 HoldWeaponPoint()
        {
            return NPC.Center + new Vector2(0, 15);
        }

        public override void FindFrame(int frameHeight)
        {

            int frame=0;
            if (hitRecoil > 0)
            {
                frame = 5;
            }else{
                switch (curAttack)
                {
                    case 0:
                        frame = 0;
                        break;
                    case 1:
                        frame = (NPC.ai[1] < 21) ? 5 : 3;
                        break;
                    case 2:
                        frame = (NPC.ai[1] < 30) ?1:3;
                        break;
                    case 3:
                        frame = (NPC.ai[1]<20)? (int)(NPC.ai[1] / 5)+1 : 0;
                        break;
                }
            }

            NPC.frame.Y = frame * frameHeight;

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
            }

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            int[] dropOptions = new int[] { ModContent.ItemType<Items.Weapons.Keyblade_flameFrolic>(),ModContent.ItemType<Items.Weapons.Org13.Axel.Chacrams_Ashes>(), ModContent.ItemType<Items.Armor.orgCoat>(), ModContent.ItemType<Items.seasaltIcecream>() };
            npcLoot.Add(ItemDropRule.OneFromOptions(1, dropOptions));


            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AxelSpawner>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.blazingShard>(), 1, 5, 15));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.blazingStone>(), 7));
        }

    }

    //===================
    //===== SPAWNER =====
    //===================

    public class AxelSpawner : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Fiery Keyblade");
            Tooltip.SetDefault("Summons the rogue pawn" +
                "\n'Is this supposed to be a Keyblade? Or is it some sort of... joke?");
        }

        public override void SetDefaults()
        {
            Item.width =Item.height= 75;
            Item.useTime = Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.HiddenAnimation;
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 1000;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<Axel_FirstPhase>());
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnBoss((int)player.Center.X, (int)Main.screenPosition.Y, ModContent.NPCType<Axel_FirstPhase>(), player.whoAmI);

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.Materials.blazingShard>(), 10)
            .AddTile(TileID.Anvils)
            .Register();
        }

    }

}
