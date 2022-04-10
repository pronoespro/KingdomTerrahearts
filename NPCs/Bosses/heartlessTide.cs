using KingdomTerrahearts.Extra;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Creative;
using System.Collections.Generic;
using Terraria.GameContent.Bestiary;

namespace KingdomTerrahearts.NPCs.Bosses
{




    public class ShadowHeartlessPosition
    {
        public Vector2 position;
        public Vector2 velocity;

        public float curRotation;
        public float rotationVel;

        public ShadowHeartlessPosition() { }
        public ShadowHeartlessPosition(Vector2 pos) { position = pos; }

        public void Update()
        {
            position = (MathHelp.Magnitude(position + velocity) > 1) ? MathHelp.Normalize(position + velocity) : position + velocity;
            velocity = Vector2.Lerp(velocity, MathHelp.Normalize(new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1))), 0.05f);

            curRotation += rotationVel;
            rotationVel = MathHelp.Lerp(rotationVel, Main.rand.NextFloat(-1, 1), 0.05f);
        }

    }



    [AutoloadBossHead]
    public class heartlessTower : ModNPC
    {

        public override string BossHeadTexture => "KingdomTerrahearts/NPCs/Bosses/HeartlessSigil";
        public override string Texture => "KingdomTerrahearts/NPCs/Bosses/heartlessBall";

        public string shadowTexture = "KingdomTerrahearts/NPCs/Bosses/shadowHeartless_dark";

        public int curAttack;
        public float attackSpeed;
        public Vector2 desVel;
        public bool setNewVel;
        public int[] spawnedHeartlesses;
        public int heartlessToSpawn;
        public List<Vector2> positions;
        public int maxPositions;
        public ShadowHeartlessPosition[,] heartlessPos;
        public int heartlessDensity;
        public bool resetPositionsToCurrent;
        public int[] attacksDmg = { 0, 60, 0, 100 };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demon Tower");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 10000;
            NPC.damage = 30;
            NPC.defense = 10;
            NPC.knockBackResist = 0;
            NPC.width = NPC.height = 50;
            NPC.scale = 2f;
            NPC.value = 5000;
            NPC.npcSlots = 5;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Music = MusicLoader.GetMusicSlot("KingdomTerrahearts/Sounds/Music/Wave of Darkness");

            attackSpeed = 1;
            heartlessToSpawn = 20;
            heartlessDensity = 20;
            positions = new List<Vector2>();
            resetPositionsToCurrent = false;
            maxPositions = 40;
            heartlessPos = new ShadowHeartlessPosition[maxPositions + 4, heartlessDensity];

            for (int i = 0; i < heartlessPos.GetLength(0); i++)
            {
                for (int j = 0; j < heartlessPos.GetLength(1); j++)
                {
                    heartlessPos[i, j] = new ShadowHeartlessPosition();
                    heartlessPos[i, j].position = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
                    heartlessPos[i, j].velocity = MathHelp.Normalize(new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)));
                    if (MathHelp.Magnitude(heartlessPos[i, j].position) > 1f)
                    {
                        heartlessPos[i, j].position = MathHelp.Normalize(heartlessPos[i, j].position);
                    }
                    heartlessPos[i, j].curRotation = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                    heartlessPos[i, j].rotationVel = Main.rand.NextFloat(-3, 3);
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("A dark swarm of Shadows, stacked into a looming spire." +
                "\n\nThe fact that its foul ilk has been seen not just in the realm of darkness but in the realm of light is surely a harbinger of some coming evil.")
            });
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.75f);
            NPC.defense = (int)(NPC.defense + numPlayers);
            NPC.scale = 1 + (0.5f * numPlayers);
            attackSpeed *= 1.5f;
            heartlessToSpawn += 5 * numPlayers;
            heartlessDensity += numPlayers;
            maxPositions += numPlayers * 2;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 2f;
            if (NPC.hide)
            {
                return false;
            }
            return null;
        }

        public override void AI()
        {

            if (resetPositionsToCurrent)
            {
                positions.Add(NPC.Center);
                if (positions.Count > maxPositions)
                {
                    positions.RemoveAt(0);
                }
            }

            NPC.TargetClosest(false);
            resetPositionsToCurrent = true;
            if (NPC.target >= 0 && Main.player[NPC.target].active)
            {
                Player p = Main.player[NPC.target];

                if (NPC.damage > 0)
                {
                    CheckPositionAttacks(p);
                }

                switch (curAttack)
                {
                    default:
                        resetPositionsToCurrent = true;
                        float desX = (Math.Abs(p.Center.X - NPC.Center.X) > 400) ? Math.Sign(p.Center.X - NPC.Center.X) * 30 : (float)Math.Sin(Main.time / 20) * 5;
                        float desY = -200;
                        while (desY < p.Center.Y + 500 - (NPC.Center.Y) && !Collision.IsWorldPointSolid(NPC.Center + new Vector2(desX, desY + 10 * maxPositions)))
                        {
                            desY++;
                        }

                        desVel = Vector2.Lerp(desVel, new Vector2(desX, desY / 10), 0.5f);
                        for (int i = 0; i < positions.Count; i++)
                        {
                            positions[i] = new Vector2(MathHelp.Lerp(positions[i].X, NPC.Center.X, 0.01f), NPC.Center.Y + 10 * (positions.Count - i));
                        }
                        CheckAttack(200);
                        break;
                    case 1:
                        resetPositionsToCurrent = false;
                        if (NPC.ai[0] < 2)
                        {
                            NPC.Center = Vector2.Lerp(NPC.Center, p.Center + new Vector2(Math.Sign(NPC.Center.X - p.Center.X) * 300f, -150), 0.3f);
                            desVel = new Vector2(Math.Sign(p.Center.X - NPC.Center.X) * 6, 0);
                        }

                        for (int i = 0; i < positions.Count; i++)
                        {
                            positions[i] = Vector2.Lerp(positions[i], NPC.Center + new Vector2((float)Math.Sin(Main.time / 5f + i / 10f) * 150f * (i / (float)positions.Count), 10f * (positions.Count - i)), 0.1f);
                        }

                        CheckAttack(200);

                        break;
                    case 2:

                        if (NPC.ai[0] < 50)
                        {
                            spawnedHeartlesses = new int[0];
                            NPC.ai[1] = NPC.lifeMax / 10f;
                            desVel = new Vector2(0, Math.Clamp((NPC.ai[0] - 30) * 0.5f, -100, 0));
                        }
                        else if (NPC.ai[0] < 110)
                        {
                            desVel = new Vector2(0, 30);
                        }
                        else if (NPC.ai[0] < 400)
                        {
                            desVel = Vector2.Zero;
                        }
                        else
                        {

                            desVel = (p.Center + new Vector2(150, -150) - NPC.Center) / 2;
                            for (int i = 0; i < positions.Count; i++)
                            {
                                positions[i] += new Vector2(0, 5);
                            }
                        }

                        if (NPC.Center.Y > Main.player[NPC.target].Center.Y + 100 && NPC.ai[0] < 300)
                        {
                            if (NPC.hide && NPC.immortal)
                            {
                                bool deffeatedHeartlesses = true;
                                for (int i = 0; i < spawnedHeartlesses.Length; i++)
                                {
                                    if (Main.npc[spawnedHeartlesses[i]].active && Main.npc[spawnedHeartlesses[i]].type == ModContent.NPCType<shadowHeartless>())
                                    {
                                        deffeatedHeartlesses = false;
                                    }
                                }
                                if (deffeatedHeartlesses)
                                {
                                    NPC.ai[0] = 400;
                                    NPC.Center = p.Center + new Vector2(0, -150);
                                }
                            }
                            else
                            {
                                NPC.hide = true;
                                NPC.immortal = true;

                                desVel = Main.player[NPC.target].Center + new Vector2(150, 100) - NPC.Center;
                                if (spawnedHeartlesses.Length == 0)
                                {
                                    EntitySource_Parent s = new EntitySource_Parent(NPC);
                                    spawnedHeartlesses = new int[heartlessToSpawn];
                                    for (int i = 0; i < spawnedHeartlesses.Length; i++)
                                    {
                                        spawnedHeartlesses[i] = NPC.NewNPC(s,(int)(NPC.Center.X + Main.rand.Next(-50, 50)), (int)(p.Center.Y + Main.rand.Next(-70, 0)), ModContent.NPCType<shadowHeartless>(), Target: NPC.target);
                                        Main.npc[spawnedHeartlesses[i]].velocity = new Vector2(Main.rand.NextFloat(-10, 10), Main.rand.NextFloat(-5, 0));
                                    }
                                }
                            }
                        }
                        else if (NPC.hide)
                        {
                            positions.Clear();
                            if (NPC.ai[0] > 400)
                            {
                                NPC.immortal = false;
                                NPC.hide = false;
                                if (spawnedHeartlesses.Length > 0)
                                {
                                    float aliveHeartlesses = 0;
                                    for (int i = 0; i < spawnedHeartlesses.Length; i++)
                                    {
                                        if (Main.npc[spawnedHeartlesses[i]].active &&
                                            Main.npc[spawnedHeartlesses[i]].type == ModContent.NPCType<shadowHeartless>())
                                        {
                                            aliveHeartlesses++;
                                            Main.npc[spawnedHeartlesses[i]].life = 0;
                                        }
                                    }
                                    NPC.life -= (int)(NPC.ai[1] / heartlessToSpawn * (heartlessToSpawn - aliveHeartlesses));
                                    NPC.checkDead();
                                    spawnedHeartlesses = new int[0];
                                }
                            }
                        }

                        CheckAttack(450);

                        break;
                    case 3:
                        resetPositionsToCurrent = true;

                        if (NPC.ai[0] % 100 < 40)
                        {
                            desVel = Vector2.Lerp(desVel, p.Center + new Vector2(450 * Math.Sign(NPC.Center.X - p.Center.X), -500) - NPC.Center, 0.3f);
                        }
                        else if (NPC.ai[0] % 100 < 43)
                        {
                            desVel = MathHelp.Normalize(p.Center - NPC.Center) * 30;
                        }


                        CheckAttack(500);

                        break;
                }
            }
            NPC.velocity = desVel;
        }

        public void CheckPositionAttacks(Player p)
        {
            PlayerDeathReason reason = PlayerDeathReason.ByNPC(NPC.type);
            for (int i = 0; i < positions.Count; i++)
            {
                if (Vector2.Distance(p.Center, positions[i]) < 30)
                {
                    p.Hurt(reason, NPC.damage, 0);
                }
            }
        }

        public void CheckAttack(int attackMax = 100)
        {
            NPC.ai[0] += attackSpeed;
            if (NPC.ai[0] > attackMax)
            {
                curAttack = (curAttack == 0) ? Main.rand.Next(1, 4) : 0;
                NPC.ai[0] = 0;
            }
            NPC.damage = attacksDmg[curAttack];
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            for (int i = 0; i < heartlessPos.GetLength(0); i++)
            {
                for (int j = 0; j < heartlessPos.GetLength(1); j++)
                {
                    heartlessPos[i, j].Update();
                }
            }
            if (!NPC.hide)
            {
                Texture2D heartlessTexture = ModContent.Request<Texture2D>(shadowTexture).Value;
                if (heartlessTexture != null)
                {
                    Rectangle rect = new Rectangle(0, 0, heartlessTexture.Width, heartlessTexture.Height);


                    for (int i = 4; i < positions.Count; i++)
                    {
                        DrawBall(spriteBatch, heartlessTexture, positions[i], rect, drawColor, i + 4, (int)(30 + i / 5f));
                    }
                    DrawBall(spriteBatch, heartlessTexture, NPC.Center, rect, drawColor, 0, 50);
                    DrawBall(spriteBatch, heartlessTexture, NPC.Center, rect, drawColor, 1, 50);
                    if (positions.Count > 2 && curAttack < 2)
                    {
                        for (int i = -5; i < 5; i++)
                        {
                            DrawBall(spriteBatch, heartlessTexture, positions[0] + new Vector2(10 * i, 0), rect, drawColor, (int)((i + 5f) / 10f * (positions.Count - 1)), 30);
                        }
                    }
                }
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public void DrawBall(SpriteBatch batch, Texture2D texture, Vector2 position, Rectangle rect, Color drawColor, int heartlessWave, int radius = 30)
        {
            Vector2 newPos;
            bool flip;
            for (int i = 0; i < heartlessDensity; i++)
            {
                newPos = position + heartlessPos[heartlessWave, i].position * radius;
                flip = Main.rand.NextBool();

                batch.Draw(texture, newPos - Main.screenPosition, rect, drawColor, heartlessPos[heartlessWave, i].curRotation, Vector2.Zero, 1, (flip) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeartlessTowerSpawner>(), 1, 2, 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lucidStone>(), 1, 5, 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Keyblade_KingdomD>(), 3));
        }

    }


    public class HeartlessTowerSpawner : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Kingdom Key D");
            Tooltip.SetDefault("Summons the amalgamation of darkness");
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
            bool alreadySpawned = NPC.AnyNPCs(ModContent.NPCType<heartlessTower>());
            return !alreadySpawned;
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<heartlessTower>());
            SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.Materials.writhingShard>(), 5)
            .AddIngredient(ModContent.ItemType<Items.Materials.writhingStone>(), 2)
            .AddTile(TileID.WorkBenches)
            .Register();

        }
    }

    [AutoloadBossHead]
    public class heartlessTide : ModNPC
    {

        public override string BossHeadTexture => "KingdomTerrahearts/NPCs/Bosses/HeartlessSigil";
        public override string Texture => "KingdomTerrahearts/NPCs/Bosses/heartlessBall";

        public string shadowTexture = "KingdomTerrahearts/NPCs/Bosses/shadowHeartless_dark";

        public int curAttack;
        public float attackSpeed;
        public Vector2 desVel;
        public bool setNewVel;
        public int[] spawnedHeartlesses;
        public int heartlessToSpawn;
        public List<Vector2> positions;
        public int maxPositions;
        public ShadowHeartlessPosition[,] heartlessPos;
        public int heartlessDensity;
        public bool resetPositionsToCurrent;
        public int[] attacksDmg = { 0, 100, 150, 120 };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demon Tide");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 18000;
            NPC.damage = 30;
            NPC.defense = 40;
            NPC.knockBackResist = 0;
            NPC.width = NPC.height = 50;
            NPC.scale = 2f;
            NPC.value = 5000;
            NPC.npcSlots = 5;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Music = MusicLoader.GetMusicSlot("KingdomTerrahearts/Sounds/Music/Wave of Darkness");

            attackSpeed = 1;
            heartlessToSpawn = 50;
            heartlessDensity = 30;
            positions = new List<Vector2>();
            resetPositionsToCurrent = false;
            maxPositions = 75;
            heartlessPos = new ShadowHeartlessPosition[maxPositions + 4, heartlessDensity];

            for (int i = 0; i < heartlessPos.GetLength(0); i++)
            {
                for (int j = 0; j < heartlessPos.GetLength(1); j++)
                {
                    heartlessPos[i, j] = new ShadowHeartlessPosition();
                    heartlessPos[i, j].position = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
                    heartlessPos[i, j].velocity = MathHelp.Normalize(new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)));
                    if (MathHelp.Magnitude(heartlessPos[i, j].position) > 1f)
                    {
                        heartlessPos[i, j].position = MathHelp.Normalize(heartlessPos[i, j].position);
                    }
                    heartlessPos[i, j].curRotation = Main.rand.NextFloat(-(float)Math.PI, (float)Math.PI);
                    heartlessPos[i, j].rotationVel = Main.rand.NextFloat(-3, 3);
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("A dark swarm of Shadows, stacked into a massive, seething cloud." +
                "\n\nThe fact that its foul ilk has been seen not just in the realm of darkness but in the realm of light is surely a harbinger of some coming evil.")
            });
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.75f);
            NPC.defense = (int)(NPC.defense + numPlayers);
            NPC.scale = 1 + (0.5f * numPlayers);
            attackSpeed *= 1.5f;
            heartlessToSpawn += 5 * numPlayers;
            heartlessDensity += numPlayers;
            maxPositions += numPlayers * 2;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 2f;
            if (NPC.hide)
            {
                return false;
            }
            return null;
        }

        public override void AI()
        {

            if (resetPositionsToCurrent)
            {
                positions.Add(NPC.Center);
                if (positions.Count > maxPositions)
                {
                    positions.RemoveAt(0);
                }
            }

            NPC.TargetClosest(false);
            resetPositionsToCurrent = true;
            if (NPC.target >= 0 && Main.player[NPC.target].active)
            {
                Player p = Main.player[NPC.target];

                if (NPC.damage > 0)
                {
                    CheckPositionAttacks(p);
                }

                switch (curAttack)
                {
                    default:
                        desVel = (p.Center + new Vector2((float)Math.Sin(Main.time / 20) * 450, -200 + (float)Math.Sin(Main.time / 10) * 50) - NPC.Center) / 5;
                        CheckAttack(200);
                        break;
                    case 1:
                        if (NPC.ai[0] < 3)
                        {
                            for (int i = 0; i < positions.Count; i++)
                            {
                                positions[i] = Vector2.Lerp(positions[i], NPC.Center, 0.4f);
                            }
                        }
                        if (NPC.ai[0] % 100 < 50)
                        {
                            if (NPC.Center.Y > p.Center.Y - 150)
                            {
                                desVel = new Vector2(0, -40);
                            }
                            else
                            {
                                desVel = new Vector2((p.Center.X - NPC.Center.X) / 5, desVel.Y * 0.99f);
                            }
                        }
                        else if (NPC.ai[0] % 100 < 100)
                        {
                            if (NPC.Center.Y < p.Center.Y + 150)
                            {
                                desVel = new Vector2(0, 40);
                            }
                            else
                            {
                                desVel = new Vector2((p.Center.X - NPC.Center.X) / 5, desVel.Y * 0.99f);
                            }
                        }
                        NPC.velocity = desVel;

                        CheckAttack(300);

                        break;
                    case 2:

                        if (NPC.ai[0] < 10)
                        {
                            desVel = Vector2.Lerp(desVel, (p.Center + new Vector2(0, -200)+p.velocity - NPC.Center) / 20, 0.8f);
                        }
                        else
                        {
                            desVel = Vector2.Zero;
                        }
                        for (int i = 4; i < positions.Count; i++)
                        {
                            positions[i] = Vector2.Lerp(positions[i], new Vector2(NPC.Center.X + (((250f - NPC.ai[0]) / 10f) * (int)(i % 2 * 2 - 1)) * (7f * (int)(i / 2)),
                                NPC.Center.Y + (int)(400f - (int)(i / 2) * 20f)),
                                0.5f);
                        }

                        CheckAttack(250);

                        break;
                    case 3:

                        desVel = Vector2.Zero;
                        if (NPC.ai[0] < 5)
                        {
                            NPC.Center = Vector2.Lerp(NPC.Center, p.Center + new Vector2(0, -300), 0.1f);
                            for (int i = 0; i < positions.Count; i++)
                            {
                                positions[i] = Vector2.Lerp(positions[i], NPC.Center, 0.4f);
                            }
                        }
                        resetPositionsToCurrent = false;

                        if (NPC.ai[0] > 100)
                        {
                            int selectedPos = (int)(positions.Count - (NPC.ai[0] - 100) / 300f * (positions.Count - 5));

                            positions[selectedPos] = Vector2.Lerp(positions[selectedPos], p.Center, 0.1f);
                            for (int i = 0; i < selectedPos; i++)
                            {
                                positions[i] = positions[selectedPos];
                            }
                        }

                        CheckAttack(400);

                        break;
                }
            }
            NPC.velocity = desVel;
        }

        public void CheckPositionAttacks(Player p)
        {
            PlayerDeathReason reason = PlayerDeathReason.ByNPC(NPC.type);
            for (int i = 0; i < positions.Count; i++)
            {
                if (Vector2.Distance(p.Center, positions[i]) < 30)
                {
                    p.Hurt(reason, NPC.damage, 0);
                }
            }
        }

        public void CheckAttack(int attackMax = 100)
        {
            NPC.ai[0] += attackSpeed;
            if (NPC.ai[0] > attackMax)
            {
                curAttack = (curAttack == 0) ? Main.rand.Next(1, 4) : 0;
                NPC.ai[0] = 0;
            }
            NPC.damage = attacksDmg[curAttack];
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            for (int i = 0; i < heartlessPos.GetLength(0); i++)
            {
                for (int j = 0; j < heartlessPos.GetLength(1); j++)
                {
                    heartlessPos[i, j].Update();
                }
            }
            if (!NPC.hide)
            {
                Texture2D heartlessTexture = ModContent.Request<Texture2D>(shadowTexture).Value;
                if (heartlessTexture != null)
                {
                    Rectangle rect = new Rectangle(0, 0, heartlessTexture.Width, heartlessTexture.Height);


                    for (int i = 4; i < positions.Count; i++)
                    {
                        DrawBall(spriteBatch, heartlessTexture, positions[i], rect, drawColor, i + 4, (int)(20 + i / 2f));
                    }
                    DrawBall(spriteBatch, heartlessTexture, NPC.Center, rect, drawColor, 0, 30);
                    DrawBall(spriteBatch, heartlessTexture, NPC.Center, rect, drawColor, 1, 30);
                    DrawBall(spriteBatch, heartlessTexture, NPC.Center, rect, drawColor, 2, 30);
                    DrawBall(spriteBatch, heartlessTexture, NPC.Center, rect, drawColor, 3, 30);
                }
            }
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public void DrawBall(SpriteBatch batch, Texture2D texture, Vector2 position, Rectangle rect, Color drawColor, int heartlessWave, int radius = 30)
        {
            Vector2 newPos;
            bool flip;
            for (int i = 0; i < heartlessDensity; i++)
            {
                newPos = position + heartlessPos[heartlessWave, i].position * radius;
                flip = Main.rand.NextBool();

                batch.Draw(texture, newPos - Main.screenPosition, rect, drawColor, heartlessPos[heartlessWave, i].curRotation, Vector2.Zero, 1, (flip) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeartlessTideSpawner>(), 1, 2, 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.lucidGem>(), 1, 5, 10));
            npcLoot.Add(ItemDropRule.Common(ItemID.DarkShard, 1, 1, 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Keyblade_KingdomD>(), 3));
        }



    }

    public class HeartlessTideSpawner : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Keyhole");
            Tooltip.SetDefault("Summons the congregation of darkness");
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
            bool alreadySpawned = NPC.AnyNPCs(ModContent.NPCType<heartlessTide>());
            return !alreadySpawned;
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<heartlessTide>());
            SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Items.Materials.writhingGem>(), 1)
            .AddIngredient(ModContent.ItemType<Items.Materials.writhingStone>(), 5)
            .AddTile(TileID.WorkBenches)
            .Register();

        }
    }











    public class HeartlessXeanorthSpawner : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Master's Dark figure");
            Tooltip.SetDefault("Summons the silhouette of darkness");
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
            bool alreadySpawned = NPC.AnyNPCs(ModContent.NPCType<heartlessXeanorth>());
            return !alreadySpawned;
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<heartlessXeanorth>());
            SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }
    }

    public class HeartlessXeanorthWeapons : ModProjectile
    {
        public override string Texture => "KingdomTerrahearts/NPCs/Bosses/heartlessXeanorth_weapons";

        int weaponType=-1;
        Vector2 initPos, desvel;
        float initRot=-666;
        Vector2 targetPosition;
        int shakeTimer = 5;
        int target;

        public void ChangeWeapon(int type)
        {
            weaponType = type;

        }

        public void SetInitRotation(float rot)
        {
            initRot = rot;
            Projectile.rotation = rot;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demonic Xeanorth's power");
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 250;
            Projectile.height = 75;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 100;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {

            if((Projectile.timeLeft==100 && Projectile.ai[0] == 0) || Projectile.timeLeft == 1){
                for(int i = 0; i < 50; i++){
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, GetDustID());
                }
            }
            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(Projectile.position, Projectile.width/2, Projectile.height, GetDustID());
            }

            initPos = (initPos == Vector2.Zero) ? Projectile.Center : initPos;
            desvel = (desvel == Vector2.Zero) ? Projectile.velocity : desvel;
            if (weaponType >= 0)
            {
                switch (weaponType)
                {
                    default:
                        break;
                    case 0:
                        if (Projectile.timeLeft ==100)
                        {
                            SoundEngine.PlaySound(SoundID.Item18,Projectile.Center);
                        }
                        if (initRot != -666)
                        {
                            Projectile.ai[0] += ((500 - Vector2.Distance(initPos, Projectile.Center)) / 500f) * 10f;

                            Projectile.Center = initPos + new Vector2(MathF.Cos((float)Main.time / 40 + (initRot)) * Projectile.ai[0], MathF.Sin((float)Main.time / 40 + (initRot)) * Projectile.ai[0]);

                            Projectile.rotation = (float)Math.Atan2(initPos.Y - Projectile.Center.Y, initPos.X - Projectile.Center.X);
                        }

                        break;
                    case 1:

                        desvel = new Vector2((float)Math.Cos(Projectile.rotation + (float)Math.PI / 2f), (float)Math.Sin(Projectile.rotation + (float)Math.PI / 2f));

                        if (targetPosition == Vector2.Zero)
                        {
                            Projectile.velocity = desvel;
                            target = LineOfSightToPlayers(0.5f);

                            if (target >= 0 && Main.player[target].active && !Main.player[target].dead)
                            {
                                targetPosition = Main.player[target].Center;
                            }
                        }

                        if (Projectile.timeLeft > 60)
                        {
                            Projectile.velocity = (Vector2.Distance(targetPosition, Projectile.Center) < MathHelp.Magnitude(Projectile.velocity)) ? Vector2.Zero : desvel * -4;
                        }
                        else if (Projectile.timeLeft > 10)
                        {
                            if (Vector2.Distance(targetPosition, Projectile.Center) < (Projectile.height / 2 - 7) || Projectile.velocity == Vector2.Zero)
                            {
                                if (shakeTimer >= 0 && target!=-1 && Main.player[target].active && !Main.player[target].dead)
                                {
                                    if (shakeTimer == 5)
                                    {
                                        SoundEngine.PlaySound(SoundID.NPCDeath46, Projectile.Center);
                                        SoundEngine.PlaySound(SoundID.NPCDeath14, Projectile.Center);
                                        SoundEngine.PlaySound(SoundID.Research, Projectile.Center);
                                    }
                                    shakeTimer--;

                                    SoraPlayer sp = Main.player[target].GetModPlayer<SoraPlayer>();
                                    sp.ModifyCutsceneCamera(Vector2.Zero, shakeForce: 15, shakeSpeed: 10, camPercentChange: 100);
                                }

                                Projectile.velocity = Vector2.Zero;
                                Projectile.Center = targetPosition - MathHelp.Normalize(desvel) * (Projectile.height / 2-7);
                            }
                            else
                            {
                                Projectile.velocity = desvel * 15;
                            }
                        }
                        else
                        {
                            Projectile.velocity = Vector2.Zero;
                        }

                        break;
                    case 2:
                        if (Projectile.timeLeft == 100)
                        {
                            SoundEngine.PlaySound(SoundID.Item1,Projectile.Center);
                        }

                        if (Projectile.timeLeft > 75)
                        {
                            Projectile.velocity = MathHelp.Normalize(desvel) * -5;
                            Projectile.rotation = (float)((Projectile.timeLeft - 75)*Math.PI/2/3)+(float)(MathF.Atan2(-desvel.Y,-desvel.X));
                        }else if (Projectile.timeLeft > 50)
                        {
                            Projectile.velocity = Vector2.Zero;
                            Projectile.rotation = (float)(MathF.Atan2(-desvel.Y, -desvel.X));
                        }
                        else
                        {
                            if (Projectile.velocity == Vector2.Zero)
                            {
                                SoundEngine.PlaySound(SoundID.Item20.SoundId, x: (int)Projectile.Center.X, y: (int)Projectile.Center.Y, volumeScale: 1.5f, pitchOffset: 1f);
                            }
                            Projectile.velocity = MathHelp.Normalize(desvel) * 25;
                        }

                        break;
                    case 3:
                        if (Projectile.timeLeft == 100 && Projectile.ai[0]==0)
                        {
                            Projectile.timeLeft = 150;
                            Projectile.ai[0]++;
                            SoundEngine.PlaySound(SoundID.Item23,Projectile.Center);
                        }
                        if (Projectile.timeLeft % 35 == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item22, Projectile.Center);
                        }
                        Projectile.velocity = Vector2.Zero;
                        Projectile.rotation += MathF.PI / 16/4;

                        Projectile.Center = initPos - new Vector2(MathF.Cos(Projectile.rotation), MathF.Sin(Projectile.rotation))*(Projectile.width/2-8);

                        break;
                }
                Projectile.frame = weaponType;
            }

        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.expertMode)
            {
                int buffType;
                switch (weaponType)
                {
                    default:
                        buffType = -1;
                        break;
                    case 0:
                        buffType = BuffID.OnFire3;
                        break;
                    case 1:
                        buffType = BuffID.Ichor;
                        break;
                    case 2:
                        buffType = BuffID.BrokenArmor;
                        break;
                    case 3:
                        buffType = BuffID.Poisoned;
                        break;
                }
                if (buffType >= 0)
                {
                    target.AddBuff(buffType, 60 * 4);
                }
            }
        }

        public int GetDustID()
        {
            switch (weaponType)
            {
                case 0:
                    return DustID.Adamantite;
                case 1:
                    return DustID.GemAmber;
                case 2:
                    return DustID.GemSapphire;
                case 3:
                    return DustID.GemEmerald;
            }
            return DustID.Asphalt;
        }

        public int LineOfSightToPlayers(float dotMinimum=0.05f)
        {
            int chosenPlayer = -1;
            foreach(Player p in Main.player)
            {
                if (Vector2.Dot(Projectile.velocity, p.Center - Projectile.Center) > dotMinimum)
                {
                    chosenPlayer = 
                        (chosenPlayer == -1 ||
                        Vector2.Distance(Projectile.Center,p.Center)<
                        Vector2.Distance(Projectile.Center,Main.player[chosenPlayer].Center))
                        ?p.whoAmI : chosenPlayer;
                }
            }
            return chosenPlayer;
        }

    }

    [AutoloadBossHead]
    public class heartlessXeanorth : ModNPC
    {

        public override string BossHeadTexture => "KingdomTerrahearts/NPCs/Bosses/HeartlessSigil";
        public override string Texture => "KingdomTerrahearts/NPCs/Bosses/heartlessXeanorth";

        public int curAttack;
        public float attackSpeed;
        public Vector2 desVel;
        public int[] attacksDmg = { 70, 100, 50, 50};

        public Vector2 armRotations;
        public float tentacleRot=0;

        private int lastAttack;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demonic Xeanorth");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 1300000;
            NPC.damage = 0;
            NPC.defense = 13;
            NPC.knockBackResist = 0;
            NPC.scale = 0.75f;
            NPC.width = (int)(953*0.75f);
            NPC.height = (int)(1175);
            NPC.value = 50000;
            NPC.npcSlots = 5;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Music = MusicLoader.GetMusicSlot("KingdomTerrahearts/Sounds/Music/Heartless Made Xeanorth");

            curAttack = -1;
            attackSpeed = 1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("A dark swarm of Shadows, taking the shape of their master" +
                "\n\nLittle is known about this form they took, except that it is extemelly dangerous.")
            });
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
            NPC.defense = (int)(NPC.defense + numPlayers);
            NPC.scale = 1 + (0.5f * numPlayers);
            attackSpeed *= 1.5f;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 5f;
            if (NPC.hide)
            {
                return false;
            }
            return null;
        }

        public override void AI()
        {

            armRotations.X = MathF.Sin((float)Main.time / 20) * MathF.PI / 16;
            armRotations.Y = -MathF.Sin((float)Main.time / 20) * MathF.PI / 16;
            tentacleRot += MathF.PI/175;

            NPC.TargetClosest(false);
            NPC.damage = 0;
            float rotation = 0;

            if (NPC.target >= 0 && Main.player[NPC.target].active)
            {
                Player p = Main.player[NPC.target];

                desVel = Vector2.Zero;

                switch (curAttack)
                {
                    case -1:
                        NPC.Center = p.Center - new Vector2(0, NPC.height / 2 - 100);

                        Conversation[] spawnDialog = new Conversation[] { new Conversation("You are most impressive, my child", Color.Yellow, 600, NPC.GivenOrTypeName), new Conversation("A pitty you will have to be dispossed of", Color.Yellow, 600, NPC.GivenOrTypeName) };
                        Interface.DialogSystem.AddConversation(spawnDialog);
                        CheckAttack(-1);

                        break;
                    case 0:

                        foreach (Player play in Main.player)
                        {
                            if (play.active && !play.dead)
                            {
                                SoraPlayer sora = play.GetModPlayer<SoraPlayer>();
                                if (sora != null)
                                {
                                    sora.SetTrapLimits(NPC.Center - new Vector2(NPC.width / 2f, NPC.height / 2f), NPC.Center + new Vector2(NPC.width / 2f, NPC.height / 2f));
                                    sora.fightingInArena = true;
                                }
                            }
                        }

                        if (NPC.ai[0] % 30 < attackSpeed)
                        {
                            if (GetBossPhase() > 0)
                            {
                                rotation += MathF.PI / 32 * (int)(NPC.ai[0] / 30);
                            }
                            CreateWeapon(new Vector2(0,15),rotation);
                            CreateWeapon(new Vector2(0,-15),rotation+(float)Math.PI);
                        }

                        CheckAttack(200);
                        break;
                    case 1:

                        if (NPC.ai[0] % 50 < attackSpeed)
                        {
                            if (GetBossPhase() > 0)
                            {
                                rotation += MathF.PI / 4 * (int)(NPC.ai[0] / 50);
                            }
                            CreateWeapon(p.Center - NPC.Center + new Vector2(MathF.Cos(rotation-MathF.PI/2f)+0,MathF.Sin(rotation - MathF.PI / 2f))*150,rotation);
                            CreateWeapon(p.Center - NPC.Center + new Vector2(MathF.Cos(rotation - MathF.PI / 2f) +0,MathF.Sin(rotation - MathF.PI / 2f))*-150, rotation+(float)Math.PI);
                        }

                        CheckAttack((GetBossPhase()>1)?300:150);
                        break;
                    case 2:

                        if (NPC.ai[0] % 25 < attackSpeed)
                        {
                            Vector2 desPos = Main.rand.NextVector2Unit();
                            CreateWeapon(desPos);

                            if (GetBossPhase() > 0)
                            {
                                float desProjRotation =MathF.Atan2(p.Center.Y-NPC.Center.Y, p.Center.X - NPC.Center.X)+MathF.PI/16;
                                CreateWeapon(desPos,desVelX: MathF.Cos(desProjRotation), desVelY: MathF.Sin(desProjRotation));

                                desProjRotation = MathF.Atan2(p.Center.Y - NPC.Center.Y, p.Center.X - NPC.Center.X) - MathF.PI / 16;
                                CreateWeapon(desPos, desVelX: MathF.Cos(desProjRotation), desVelY: MathF.Sin(desProjRotation));

                                if (GetBossPhase() > 1)
                                {
                                    desProjRotation = MathF.Atan2(p.Center.Y - NPC.Center.Y, p.Center.X - NPC.Center.X) + MathF.PI / 8;
                                    CreateWeapon(desPos, desVelX: MathF.Cos(desProjRotation), desVelY: MathF.Sin(desProjRotation));

                                    desProjRotation = MathF.Atan2(p.Center.Y - NPC.Center.Y, p.Center.X - NPC.Center.X) - MathF.PI / 8;
                                    CreateWeapon(desPos, desVelX: MathF.Cos(desProjRotation), desVelY: MathF.Sin(desProjRotation));
                                }
                            }

                        }

                        CheckAttack(150);
                        break;
                    case 3:

                        if (GetBossPhase() > 0)
                        {
                            foreach (Player play in Main.player)
                            {
                                if (play.active && !play.dead)
                                {
                                    SoraPlayer sora = play.GetModPlayer<SoraPlayer>();
                                    if (sora != null)
                                    {
                                        sora.SetTrapLimits(NPC.Center - new Vector2(NPC.width / 2f, NPC.height / 2f), NPC.Center + new Vector2(NPC.width / 2f, NPC.height / 2f));
                                        sora.fightingInArena = true;
                                    }
                                }
                            }
                        }

                        if (NPC.ai[0] % 200 < attackSpeed)
                        {

                            float distanceToCreate = (GetBossPhase() > 0) ? -400 : -500;

                            Vector2 desPos = p.Center - NPC.Center + new Vector2(MathF.Cos(0), MathF.Sin(0)) * distanceToCreate;
                            CreateTripleSpikes(desPos);

                            desPos = p.Center - NPC.Center + new Vector2(MathF.Cos(MathF.PI * 2 / 3), MathF.Sin(MathF.PI * 2 / 3)) * distanceToCreate;
                            CreateTripleSpikes(desPos);

                            desPos = p.Center - NPC.Center + new Vector2(MathF.Cos(MathF.PI * 2 / 3 * 2), MathF.Sin(MathF.PI * 2 / 3 * 2)) * distanceToCreate;
                            CreateTripleSpikes(desPos);

                            if (GetBossPhase() > 1)
                            {

                                desPos = p.Center - NPC.Center + new Vector2(MathF.Cos(MathF.PI / 3), MathF.Sin(MathF.PI / 3)) * distanceToCreate;
                                CreateTripleSpikes(desPos);

                                desPos = p.Center - NPC.Center + new Vector2(MathF.Cos(MathF.PI * 2 / 3 + MathF.PI / 3), MathF.Sin(MathF.PI * 2 / 3 + MathF.PI / 3)) * distanceToCreate;
                                CreateTripleSpikes(desPos);

                                desPos = p.Center - NPC.Center + new Vector2(MathF.Cos(MathF.PI * 2 / 3 * 2 + MathF.PI / 3), MathF.Sin(MathF.PI * 2 / 3 * 2 + MathF.PI / 3)) * distanceToCreate;
                                CreateTripleSpikes(desPos);
                            }

                        }

                        CheckAttack(200);
                        break;
                    case 4:
                        foreach (Player play in Main.player)
                        {
                            if (play.active && !play.dead)
                            {
                                SoraPlayer sora = play.GetModPlayer<SoraPlayer>();
                                if (sora != null)
                                {
                                    sora.fightingInArena = false;
                                }
                            }
                        }
                        CheckAttack(150);
                        break;
                    case 5:

                        if (NPC.ai[0] == 0)
                        {
                            attackSpeed *= 1.5f;
                            SoundEngine.PlaySound(SoundID.Roar, Main.player[NPC.target].Center);
                        }

                        SoraPlayer sp = Main.player[NPC.target].GetModPlayer<SoraPlayer>();
                        sp.ModifyCutsceneCamera(Vector2.Zero, shakeForce: 15, shakeSpeed: 10, camPercentChange: 100);

                        CheckAttack(50);
                        break;
                    default:
                        CheckAttack(0);
                        break;
                }
            }

            NPC.velocity = desVel;
        }

        public int GetBossPhase()
        {
            int phase=0;

            if (NPC.life < NPC.lifeMax / 4*3)
            {
                phase++;
            }
            if (NPC.life < NPC.lifeMax / 2)
            {
                phase++;
            }

            return phase;
        }

        public void CreateWeapon(Vector2 offsetPosition,float rot = 0,float desVelX=0,float desVelY=0)
        {

            SoundEngine.PlaySound(SoundID.NPCHit52, NPC.Center);

            EntitySource_BossSpawn s = new EntitySource_BossSpawn(NPC);

            int proj=Projectile.NewProjectile(s, NPC.Center+offsetPosition, MathHelp.Normalize((Main.player[NPC.target].Center - NPC.Center)) * 5, ModContent.ProjectileType<HeartlessXeanorthWeapons>(), attacksDmg[curAttack], 5);
            if (desVelX != 0 || desVelY != 0)
            {
                Main.projectile[proj].velocity = new Vector2(desVelX, desVelY);
            }
            Main.projectile[proj].friendly= false;
            Main.projectile[proj].hostile= true;
            
            HeartlessXeanorthWeapons w=(HeartlessXeanorthWeapons)Main.projectile[proj].ModProjectile;
            w.ChangeWeapon(curAttack);
            w.SetInitRotation(rot);
        }

        public void CreateTripleSpikes(Vector2 desPos)
        {
            CreateWeapon(desPos);
            CreateWeapon(desPos, MathF.PI * 2 / 3);
            CreateWeapon(desPos, MathF.PI * 2 / 3 * 2);
        }

        public void CheckAttack(int attackMax = 100)
        {
            NPC.ai[0] += attackSpeed;
            if (NPC.ai[0] > attackMax)
            {
                if (curAttack < 4)
                {
                    curAttack = 4;

                }
                else
                {
                    SoundEngine.PlaySound(SoundID.Item4, NPC.Center);
                    curAttack =  Main.rand.Next(0, 4);

                    while (curAttack == lastAttack)
                    {
                        curAttack = Main.rand.Next(0, 4);
                    }
                    
                    lastAttack = curAttack;
                }
                NPC.ai[0] = 0;

                if(NPC.ai[1]<1 && NPC.life < NPC.lifeMax / 4*3)
                {
                    curAttack = 5;
                    NPC.ai[1] = 2;
                }else if(NPC.ai[1]<5 && NPC.life < NPC.lifeMax / 4)
                {
                    curAttack = 5;
                    NPC.ai[1] = 6;
                }
            }
        }

        public override bool CheckDead()
        {
            foreach (Player play in Main.player)
            {
                if (play.active && !play.dead)
                {
                    SoraPlayer sora = play.GetModPlayer<SoraPlayer>();
                    if (sora != null)
                    {
                        sora.SetTrapLimits(Vector2.Zero, Vector2.Zero);
                        sora.fightingInArena = false;
                    }
                }
            }
            return base.CheckDead();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D neededTexture = ModContent.Request<Texture2D>("KingdomTerrahearts/NPCs/Bosses/heartlessXeanorth_darkTentacle").Value;

            if (neededTexture != null)
            {
                Rectangle rect = new Rectangle(0, 0, neededTexture.Width, neededTexture.Height / 4);
                Vector2 pivot = new Vector2(0, neededTexture.Height / 4 / 2);

                for (int i = 0; i < 10; i++)
                {
                    spriteBatch.Draw(neededTexture, NPC.Center - Main.screenPosition + new Vector2(150, -285) * NPC.scale, rect, drawColor, i * (float)Math.PI / 5 - tentacleRot, pivot, NPC.scale * 2, SpriteEffects.None, 0);
                    spriteBatch.Draw(neededTexture, NPC.Center - Main.screenPosition + new Vector2(-150, -285) * NPC.scale, rect, drawColor, i * (float)Math.PI / 5 + tentacleRot, pivot, NPC.scale * 2, SpriteEffects.None, 0);

                    spriteBatch.Draw(neededTexture, NPC.Center - Main.screenPosition, rect, drawColor, i * (float)Math.PI / 5 + tentacleRot/4, pivot, NPC.scale * 5, SpriteEffects.None, 0);
                    spriteBatch.Draw(neededTexture, NPC.Center - Main.screenPosition, rect, drawColor, i * (float)Math.PI / 5 - tentacleRot/3, pivot, NPC.scale * 4, SpriteEffects.None, 0);
                }
            }



            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            Texture2D neededTexture = ModContent.Request<Texture2D>("KingdomTerrahearts/NPCs/Bosses/heartlessXeanorth_arm").Value;

            if (neededTexture != null)
            {
                Rectangle rect = new Rectangle(0, 0, neededTexture.Width, neededTexture.Height);
                Vector2 pivot = new Vector2(43, 67);

                spriteBatch.Draw(neededTexture, NPC.Center - Main.screenPosition + new Vector2(130, -285) * NPC.scale, rect, drawColor, armRotations.X, pivot, NPC.scale, SpriteEffects.None, 0);

                pivot.X = neededTexture.Width - pivot.X;

                spriteBatch.Draw(neededTexture, NPC.Center - Main.screenPosition + new Vector2(-130, -285) * NPC.scale, rect, drawColor, armRotations.Y, pivot, NPC.scale, SpriteEffects.FlipHorizontally, 0);
            }

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeartlessXeanorthSpawner>(), 1, 2, 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Keyblade_Eye>(), 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Materials.writhingCrystal>(), 1, 5, 10));
        }



    }

}
