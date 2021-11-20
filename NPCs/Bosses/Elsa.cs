using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;

namespace KingdomTerrahearts.NPCs.Bosses
{
    public class Elsa : ModNPC
    {

        public int curAttack = 0;
        public int[] curProjs = new int[30];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Elsa");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 2500;
            NPC.damage = 30;
            NPC.defense = 15;
            NPC.knockBackResist = 0;
            NPC.width = 250;
            NPC.height = 1260/6;
            NPC.value = 15000;
            NPC.npcSlots = 5;
            NPC.boss = true;
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.behindTiles = true;
            Music = MusicLoader.GetMusicSlot("KingdomTerrahearts/Sounds/Music/The 13th Struggle");
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.75f);
            NPC.defense = (int)(NPC.defense + numPlayers);
            NPC.scale = 1 + (0.5f * numPlayers);
        }

        public override void AI()
        {
            ProjectileSource_NPC s = new ProjectileSource_NPC(NPC);

            NPC.TargetClosest();
            DespawnHandler();

            Vector2 desPosition;
            Vector2 velocity;

            switch (curAttack)
            {
                case 0:

                    if (curProjs[0] != 0)
                    {
                        for (int i = 0; i < curProjs.Length; i++)
                        {
                            curProjs[i] = -1;
                        }
                    }

                    desPosition = Main.player[NPC.target].Center + new Vector2(0, VerticalDirection(50, 0.04f) - 250);
                    velocity = desPosition - NPC.Center;

                    NPC.Center += velocity / 5;

                    CheckAttack(120);

                    break;
                case 1:
                    desPosition = Main.player[NPC.target].Center + new Vector2((NPC.ai[0]-140)/240*650, -350);
                    velocity = desPosition - NPC.Center;

                    NPC.Center += velocity / 10;

                    if (NPC.ai[0] % 20 == 0 && NPC.ai[0]>=40)
                    {
                        int proj=Projectile.NewProjectile(s,NPC.Center, new Vector2(0, 50),ProjectileID.IceSpike, 80, 1);
                        Main.projectile[proj].scale = 1.25f;
                    }

                    CheckAttack(240);

                    break;
                case 2:
                    desPosition = Main.player[NPC.target].Center + new Vector2((NPC.ai[0] - 140) / 240 * -650, -350);
                    velocity = desPosition - NPC.Center;

                    NPC.Center += velocity / 10;

                    if (NPC.ai[0] % 20 == 0 && NPC.ai[0] >= 40)
                    {
                        int proj = Projectile.NewProjectile(s,NPC.Center, new Vector2(0, 50), ProjectileID.IceSpike, 80, 1);
                        Main.projectile[proj].scale = 1.25f;
                    }

                    CheckAttack(240);
                    
                    break;
                case 3:

                    desPosition = Main.player[NPC.target].Center + new Vector2(0, -400);
                    velocity = desPosition - NPC.Center;

                    NPC.Center += velocity / 5;

                    if (NPC.ai[0] < 320)
                    {
                        if (NPC.ai[0] == 0)
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                curProjs[i] = Projectile.NewProjectile(s,NPC.Center,Vector2.Zero,ProjectileID.IceBolt,150,1);
                                Main.projectile[curProjs[i]].hostile = true;
                                Main.projectile[curProjs[i]].friendly = false;
                                Main.projectile[curProjs[i]].tileCollide= false;

                            }
                        }

                        if (curProjs[0] != -1)
                        {
                            if (NPC.ai[0] >= 300)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    Main.projectile[curProjs[i]].tileCollide = true;
                                    switch (i % 5)
                                    {
                                        case 0:
                                            Main.projectile[curProjs[i]].velocity = new Vector2(0, -50);
                                            break;
                                        case 1:
                                            Main.projectile[curProjs[i]].velocity = new Vector2(-15, -15);
                                            break;
                                        case 2:
                                            Main.projectile[curProjs[i]].velocity = new Vector2(15, -15);
                                            break;
                                        case 3:
                                            Main.projectile[curProjs[i]].velocity = new Vector2(-35, 35);
                                            break;
                                        case 4:
                                            Main.projectile[curProjs[i]].velocity = new Vector2(35, 35);
                                            break;
                                    }
                                    if (i % 10 > 4)
                                    {
                                        Main.projectile[curProjs[i]].velocity = -Main.projectile[curProjs[i]].velocity;
                                    }
                                    curProjs[i] = -1;
                                }
                            }
                            else
                            {
                                for(int i = 0; i < 20; i++)
                                {

                                    Vector2 offset;

                                    switch (i % 5)
                                    {
                                        default:
                                        case 0:
                                            offset = new Vector2(0, -50);
                                            break;
                                        case 1:
                                            offset = new Vector2(-15, -15);
                                            break;
                                        case 2:
                                            offset = new Vector2(15, -15);
                                            break;
                                        case 3:
                                            offset = new Vector2(-35, 35);
                                            break;
                                        case 4:
                                            offset = new Vector2(35, 35);
                                            break;
                                    }

                                    if (i % 10 > 4)
                                    {
                                        offset = -offset;
                                    }
                                    if (i >= 10)
                                    {
                                        offset *= 2;
                                    }

                                    Main.projectile[curProjs[i]].Center = NPC.Center+offset;

                                }
                            }
                        }

                    }

                    CheckAttack(340);

                    break;
                case 4:

                    if (NPC.ai[0] > 9)
                    {
                        if (NPC.ai[0] == 10)
                        {
                            curProjs[0] = Projectile.NewProjectile(s,NPC.Center, new Vector2(0, -15f), ProjectileID.IceBolt, 10, 2);
                            Main.projectile[curProjs[0]].tileCollide = false;
                            Main.projectile[curProjs[0]].hostile = true;
                        }

                        if (NPC.ai[0] > 40)
                        {
                            Main.projectile[curProjs[0]].velocity=Vector2.Lerp(Main.projectile[curProjs[0]].velocity,MathHelp.Normalize(Main.player[NPC.target].Center-Main.projectile[curProjs[0]].Center)*15, 0.2f);
                            Main.projectile[curProjs[0]].velocity = MathHelp.Normalize(Main.projectile[curProjs[0]].velocity) * 15;
                        }
                        if (NPC.ai[0] == 150)
                        {
                            Main.projectile[curProjs[0]].tileCollide = true;
                            Main.projectile[curProjs[0]].velocity*=2;

                        }

                    }

                    CheckAttack(150);
                    break;
            }

        }

        public void CheckAttack(int attackChangeTimer)
        {
            NPC.ai[0]+= (NPC.ai[0] > 2500 / 2)?2:1;
            if (NPC.ai[0] >= attackChangeTimer)
            {
                NPC.ai[0] = 0;
                if (curAttack == 0)
                {
                    curAttack = Main.rand.Next(1, 5);
                }
                else
                {
                    curAttack = 0;
                }
            }
        }

        public float VerticalDirection(float magnitude=1,float speed=1)
        {
            return (float)Math.Sin(NPC.ai[0]*speed)*magnitude;
        }

        void DespawnHandler()
        {
            if (!Main.player[NPC.target].active || Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(false);
                Main.player[NPC.target] = Main.player[NPC.target];
                if (!Main.player[NPC.target].active || Main.player[NPC.target].dead)
                {
                    NPC.velocity = new Vector2(0, 100000);
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                        for (int i = 0; i < curProjs.Length; i++)
                        {
                            if (Main.projectile[curProjs[i]].active)
                            {
                                Main.projectile[curProjs[i]].timeLeft = 1;
                            }
                        }
                    }
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<elsasHeart>(), 1, 2, 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Keyblade_ice>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.frostShard>(), 1, 15, 50));
        }

        public override void FindFrame(int frameHeight)
        {
            int frame=0;

            switch (curAttack)
            {
                case 0:
                    frame = 0;
                    break;
                case 1:
                    frame = 2;
                    break;
                case 2:
                    frame = 2;
                    break;
                case 3:
                    frame = 1;
                    break;
            }

            NPC.frame.Y = frame * frameHeight;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 4f;
            return null;
        }

        public override void DrawEffects(ref Color drawColor)
        {
            for (int i = 0; i < 4; i++)
            {
                if (Main.rand.Next(10) <= 3)
                {
                    int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Ice, 0, 2f);
                }
            }

            base.DrawEffects(ref drawColor);
        }

        public override bool CheckDead()
        {

            if (curProjs != null)
            {
                for (int i = 0; i < curProjs.Length; i++)
                {
                    if (curProjs[i]<Main.maxProjectiles && Main.projectile[curProjs[i]].active)
                    {
                        Main.projectile[curProjs[i]].timeLeft = 1;
                    }
                }
            }

            return base.CheckDead();
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Ice, 2 * hitDirection, -2f);
        }


    }

    public class elsasHeart : ModItem
    {


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Elsa's heart");
            Tooltip.SetDefault("Summons the true queen of the ice and snow" +
                "\nIce Queen? Nah, Elsa.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.value = 10;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 40;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            bool alreadySpawned = NPC.AnyNPCs(ModContent.NPCType<Elsa>());
            return !alreadySpawned;
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Elsa>());
            SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.IceBlock, 3)
            .AddIngredient(ItemID.SnowBlock, 3)
            .AddTile(TileID.WorkBenches)
            .Register();

        }


    }

}
