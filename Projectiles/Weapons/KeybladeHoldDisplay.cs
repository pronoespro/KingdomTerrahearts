using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static KingdomTerrahearts.Items.Weapons.KeybladeBase;
using KingdomTerrahearts.Extra;
using Terraria.DataStructures;
using Terraria.Audio;

namespace KingdomTerrahearts.Projectiles.Weapons
{
    public class KeybladeHoldDisplay:ModProjectile
    {

        public string sprite = "None";
        public Player player;
        public bool midAttack;
        public keyTransformation transform;
        public bool flipVertically;
        public int attackType = 0;
        public Vector2 initMouseDir;
        public int target=-1;
        public Vector2 specialPos;
        public Vector2 lastSpecialPos;
        public float desRotation=0;
        public float cannonScale=1.25f;

        public bool opositeSides;
        public bool createdProjectile;

        public int[] attackProjectiles=new int[0];

        public enum weaponSpriteType
        {
            normal,
            guns,
            dual
        }
        public weaponSpriteType spriteType;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Keyblade");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 50;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.hide = true;
            Projectile.hostile = false;
            Projectile.friendly = true;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }

        public override bool? CanDamage()
        {
            return Projectile.ai[0] > 0;
        }

        public override void AI()
        {

            Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
            if (midAttack)
            {

                if (player != null)
                {
                    Projectile.Center = player.Center;
                    Vector2 mouseDir = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - player.Center;
                    Vector2 weaponDir;
                    float percentageDone = Projectile.ai[0] / Projectile.ai[1];
                    int proj;

                    SoraPlayer sp = player.GetModPlayer<SoraPlayer>();

                    switch (transform)
                    {
                        default:
                        case keyTransformation.none:
                            Projectile.Center = Vector2.Zero;
                            break;
                        case keyTransformation.hammer:

                            player.direction = initMouseDir.X < 0 ? -1 : 1;

                            percentageDone = percentageDone * 4 - 2.5f;
                            if (createdProjectile || sp.Grounded())
                            {
                                percentageDone = Math.Clamp(percentageDone, 0.6f, 1);
                                if (percentageDone == 0.6f)
                                {
                                    player.velocity = Vector2.Zero;
                                }
                            }

                            if (player.direction < 0)
                            {
                                weaponDir = new Vector2((float)Math.Cos((percentageDone * 4 + 1.5f)),
                                    (float)Math.Sin((percentageDone * 4 + 1.5f)));
                                Projectile.rotation = (float)Math.Atan2(weaponDir.Y, weaponDir.X);

                                weaponDir = new Vector2((float)Math.Cos((percentageDone * 4 + 0.75f)),
                                   (float)Math.Sin((percentageDone * 4 + 0.75f)));

                                Projectile.position = player.Center + weaponDir * 50 + new Vector2(20 * -Projectile.spriteDirection, 0);
                            }
                            else
                            {
                                weaponDir = new Vector2((float)Math.Cos(-(percentageDone * 4 - 1.5f)),
                                    -(float)Math.Sin((percentageDone * 4 - 1.5f)));
                                Projectile.rotation = (float)Math.Atan2(weaponDir.Y, weaponDir.X) + (float)(Math.PI * 0.5f);

                                weaponDir = new Vector2(-(float)Math.Cos((percentageDone * 4 + 0.75f)),
                                   (float)Math.Sin((percentageDone * 4 + 0.75f)));

                                Projectile.position = player.Center + weaponDir * 50 + new Vector2(20 * -Projectile.spriteDirection + 15, 0);
                            }

                            if (sp.Grounded())
                            {
                                player.fullRotation = 0;
                                player.legRotation = 0;

                                if (!createdProjectile)
                                {
                                    if (percentageDone <= 0.7f)
                                    {

                                        ProjectileSource_ProjectileParent s = new ProjectileSource_ProjectileParent(Projectile);

                                        proj = Projectile.NewProjectile(s, Projectile.position, Vector2.Zero, ModContent.ProjectileType<GroundPound>(), Projectile.damage * 2, Projectile.knockBack * 2, player.whoAmI);

                                        SoundEngine.PlaySound(SoundID.Dig, player.position);

                                        Main.projectile[proj].friendly = true;
                                        Main.projectile[proj].hostile = false;
                                        Main.projectile[proj].scale *= 0.25f;

                                        createdProjectile = true;
                                    }
                                }
                            }
                            else
                            {
                                if (percentageDone > -1.5f)
                                {
                                    player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                                    player.fullRotation = Projectile.rotation;
                                    player.legRotation = -(float)Math.PI / 2f;

                                    sp.SetContactinvulnerability(3);
                                }
                                else
                                {
                                    player.fullRotation = 0;
                                    player.legRotation = 0;
                                }
                            }

                            break;
                        case keyTransformation.swords:

                            player.direction = initMouseDir.X < 0 ? -1 : 1;

                            switch (attackType)
                            {
                                default:
                                case 0:
                                case 4:
                                    percentageDone = Math.Clamp(percentageDone * 1.2f,0,1);
                                    if (!createdProjectile)
                                    {
                                        CreateSliceProjectile("Wide", 25, (float)Math.PI * (player.direction < 0 ? -1 : 0 - 0.125f), 3);
                                        SliceProjectile slice = (SliceProjectile)Main.projectile[attackProjectiles[0]].ModProjectile;
                                        if (slice != null && player.direction < 0) { slice.FlipVertically(); }
                                        createdProjectile = true;
                                    }
                                    break;
                                case 1:
                                case 3:
                                    percentageDone = Math.Clamp(1 - percentageDone,0,1);
                                    if (!createdProjectile)
                                    {
                                        CreateSliceProjectile("Wide", 25, (float)Math.PI * (player.direction < 0 ? -1 : 0 - 0.125f), 3);
                                        SliceProjectile slice = (SliceProjectile)Main.projectile[attackProjectiles[0]].ModProjectile;
                                        if (slice != null && player.direction >= 0) { slice.FlipVertically(); }
                                        createdProjectile = true;
                                    }
                                    break;
                                case 2:
                                case 5:
                                    percentageDone = 0.6f;
                                    break;
                            }
                            if (player.direction < 0)
                            {
                                weaponDir = new Vector2((float)Math.Cos((percentageDone * 4 + 1.5f)),
                                    (float)Math.Sin((percentageDone * 4 + 1.5f)));
                                Projectile.rotation = (float)Math.Atan2(weaponDir.Y, weaponDir.X);

                                weaponDir = new Vector2((float)Math.Cos((percentageDone * 4 + 0.75f)),
                                   (float)Math.Sin((percentageDone * 4 + 0.75f)));

                                Projectile.position = player.Center + weaponDir * 50 + new Vector2(20 * -Projectile.spriteDirection, 0);
                            }
                            else
                            {
                                weaponDir = new Vector2((float)Math.Cos(-(percentageDone * 4 - 1.5f)),
                                    -(float)Math.Sin((percentageDone * 4 - 1.5f)));
                                Projectile.rotation = (float)Math.Atan2(weaponDir.Y, weaponDir.X) + (float)(Math.PI * 0.5f);

                                weaponDir = new Vector2(-(float)Math.Cos((percentageDone * 4 + 0.75f)),
                                   (float)Math.Sin((percentageDone * 4 + 0.75f)));

                                Projectile.position = player.Center + weaponDir * 50 + new Vector2(20 * -Projectile.spriteDirection + 15, 0);
                            }

                            player.direction = initMouseDir.X < 0 ? -1 : 1;

                            break;
                        case keyTransformation.staff:
                            Projectile.Center =player.Center+ new Vector2(player.width/2+15 +15 * player.direction, player.height/2-10);
                            Projectile.spriteDirection = player.direction;

                            if (player.direction < 0)
                            {
                                Projectile.rotation = +(float)Math.PI / 4f;
                            }
                            else
                            {
                                Projectile.rotation = -(float)Math.PI / 4f;
                            }
                            break;
                        case keyTransformation.cannon:
                        case keyTransformation.drill:

                            flipVertically = mouseDir.X < 0;
                            Projectile.Center = player.Center + new Vector2(player.width, player.height / 2 + 5);
                            Projectile.Center += MathHelp.Normalize(mouseDir)*15;

                            player.bodyFrame.Y = player.bodyFrame.Height*3;

                            Projectile.rotation = (float)Math.Atan2(mouseDir.Y, mouseDir.X) + (float)Math.PI / 4;

                            Projectile.scale = cannonScale;

                            break;
                        case keyTransformation.guns:

                            player.bodyFrame.Y = player.bodyFrame.Height * 3;

                            flipVertically = mouseDir.X < 0;
                            Projectile.Center = player.Center + new Vector2(5, player.height / 2 + 5);

                            if (flipVertically)
                            {
                                Projectile.rotation = (float)Math.Atan2(mouseDir.Y, mouseDir.X) - (float)Math.PI / 4;
                            }
                            else
                            {
                                Projectile.rotation = (float)Math.Atan2(mouseDir.Y, mouseDir.X) + (float)Math.PI / 4;
                            }
                            break;
                        case keyTransformation.shield:
                            switch (attackType)
                            {
                                default:
                                case 0:

                                    percentageDone = Math.Clamp(percentageDone * 2-1, -1, 1);
                                    if (percentageDone > -0.75f)
                                    {
                                        sp.SetContactinvulnerability(4);
                                    }

                                    Slash(percentageDone);
                                    Projectile.rotation -= (float)Math.PI / 4f;
                                    Projectile.Center += (player.Center - Projectile.Center)/2+new Vector2(0,player.height/2);

                                    break;
                                case 1:
                                    if (percentageDone == 1)
                                    {
                                        target = sp.GetClosestEnemy(500);
                                    }
                                    if(target!=-1 && Main.npc[target].active)
                                    {
                                        Vector2 directionToEnemy=Main.npc[target].Center-player.Center;

                                        flipVertically = directionToEnemy.X < 0;
                                        player.direction = (flipVertically) ? -1 : 1;
                                        Projectile.rotation = (float)Math.Atan2(directionToEnemy.Y,directionToEnemy.X);
                                        Projectile.Center = player.Center+directionToEnemy*Math.Clamp(1-percentageDone*1.2f,0,1) + new Vector2(0, player.height / 2);
                                    }
                                    else
                                    {
                                        flipVertically = initMouseDir.X < 0;
                                        player.direction = (flipVertically) ? -1 : 1;
                                        Projectile.rotation = 0;
                                        Projectile.Center = player.Center + new Vector2(300 * (1 - percentageDone)*player.direction, player.height/2);
                                    }
                                    player.velocity = Vector2.Zero;

                                    break;
                                case 2:
                                    sp.SetContactinvulnerability(4);
                                    percentageDone = Math.Clamp(percentageDone * 2 - 0.5f, 0f, 1f);

                                    player.velocity = initMouseDir * percentageDone * 15f;

                                    Projectile.Center = player.Center + new Vector2(0, player.height / 2);
                                    Projectile.Center += MathHelp.Normalize(Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - player.Center) * 5;

                                    Projectile.rotation = (float)Math.Atan2(mouseDir.Y, mouseDir.X) + (float)Math.PI / 4 * (mouseDir.X < 0 ? 0 : 0);

                                    flipVertically = mouseDir.X < 0;
                                    player.noFallDmg = true;

                                    if (percentageDone > 0)
                                    {
                                        sp.SetContactinvulnerability(5);
                                    }
                                    break;
                            }

                            break;
                        case keyTransformation.dual:
                            opositeSides = false;
                            switch (attackType)
                            {
                                default:
                                case 0:

                                    sp.SetContactinvulnerability(5);
                                    Projectile.Center = player.Center;

                                    if (percentageDone > 0.5f)
                                    {
                                        Slash(percentageDone*2 + 0.75f);
                                        Projectile.rotation += (float)Math.PI / 4 * (player.direction < 0 ? -2 : 0);
                                        if (!createdProjectile)
                                        {
                                            createdProjectile = true;
                                            CreateDoubleSliceProjectile("Wide", 15, (float)Math.PI * (player.direction < 0 ? 1 : 0), 3,secondSliceDistance:30);
                                            SliceProjectile slice = (SliceProjectile)Main.projectile[attackProjectiles[0]].ModProjectile;
                                            if (slice != null) { slice.FlipVertically(player.direction < 0); }
                                        }
                                    }
                                    else
                                    {
                                        Vector2 newWeaponDir = MathHelp.Normalize(mouseDir) * (cannonScale - percentageDone*3) * 40;
                                        Projectile.Center += newWeaponDir;
                                        newWeaponDir = MathHelp.Normalize(mouseDir);
                                        Projectile.rotation = (float)Math.Atan2(newWeaponDir.Y, newWeaponDir.X) -((float)Math.PI/2*player.direction*(0.5f-(1-percentageDone*2)));
                                        if (createdProjectile)
                                        {
                                            createdProjectile = false;
                                            CreateDoubleSliceProjectile("Horizontal", 15,(float)Math.Atan2(MathHelp.Normalize(initMouseDir).Y,MathHelp.Normalize(initMouseDir).X) , 3,secondSliceDistance:30);
                                        }
                                    }

                                    break;
                                case 1:
                                    sp.SetContactinvulnerability(5);
                                    opositeSides = true;

                                    percentageDone = percentageDone * 2 % 1;

                                    player.direction = (percentageDone < 0.5f) ? 1 : -1;
                                    player.bodyFrame.Y = player.bodyFrame.Height * 3;

                                    Projectile.Center = player.Center + new Vector2(Projectile.width / 2, Projectile.height / 2);

                                    Projectile.Center += new Vector2(40 + percentageDone * 15, 0) * player.direction;

                                    Projectile.rotation += (float)Math.PI / 4 * (initMouseDir.X < 0 ? 
                                        (player.direction<0?-5:-1) : 
                                        (player.direction<0?-3: 1));

                                    player.velocity = new Vector2(MathHelp.Sign(initMouseDir.X) * 10,0);

                                    if (percentageDone>0.5f)
                                    {
                                        if (!createdProjectile)
                                        {
                                            CreateSliceProjectile("Horizontal_left", 10,0,3,0,1,true);
                                            CreateSliceProjectile("Horizontal_right", 10, (float)Math.PI, 3, 0, 1, true,30f);
                                            createdProjectile = true;
                                        }
                                    }
                                    else
                                    {
                                        if (createdProjectile)
                                        {
                                            CreateSliceProjectile("Horizontal_left", 10, (float)Math.PI, 3, 0, 1, true);
                                            CreateSliceProjectile("Horizontal_right", 10, 0, 3, 0, 1, true, -30f);
                                            createdProjectile = false;
                                        }
                                    }

                                    break;
                                case 2:
                                    Projectile.Center = player.Center;

                                    Slash(Math.Clamp((1-percentageDone)*2f,0,1));
                                    if (!createdProjectile)
                                    {
                                        createdProjectile = true;
                                        CreateDoubleSliceProjectile(timeLeft: 25, rotation: (player.direction < 0 ? (float)Math.PI : 0),scale:4,secondSliceDistance:40);
                                        for (int i = 0; i < 2; i++)
                                        {
                                            SliceProjectile slice = (SliceProjectile)Main.projectile[attackProjectiles[i]].ModProjectile;
                                            if (slice != null) { slice.FlipVertically(player.direction >= 0); }
                                        }
                                    }

                                    Projectile.Center += new Vector2(0,player.Center.Y-Projectile.Center.Y)*0.75f;

                                    Projectile.rotation += (float)Math.PI / 4 * (player.direction < 0 ? -2 : 0);

                                    player.velocity = VelToEnemyIfAny(Math.Clamp(percentageDone * 1.2f, 0, 1) * 50);
                                    sp.SetContactinvulnerability(5);
                                    
                                    break;
                                case 3:
                                    opositeSides = true;
                                    player.direction = ((percentageDone * 2 % 1) < 0.5f) ? 1 : -1;
                                    player.bodyFrame.Y = player.bodyFrame.Height * 3;

                                    Projectile.Center = player.Center + new Vector2(Projectile.width / 2, Projectile.height / 2);

                                    Projectile.Center += new Vector2(40 + percentageDone * 15 * player.direction, 0);

                                    Projectile.rotation += (float)Math.PI / 4 * (initMouseDir.X < 0 ?
                                        (player.direction < 0 ? -5 : -1) :
                                        (player.direction < 0 ? -3 : 1));

                                    player.velocity = new Vector2(MathHelp.Sign(initMouseDir.X)*15, 0);

                                    player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                                    player.fullRotation =(percentageDone>0.2f)?(float)Math.PI:0;
                                    sp.SetContactinvulnerability(5);

                                    if ((percentageDone*2%1) > 0.5f)
                                    {
                                        if (!createdProjectile)
                                        {
                                            CreateSliceProjectile("Horizontal_right", 10, 0, 3, 0, 1, true);
                                            CreateSliceProjectile("Horizontal_left", 10, (float)Math.PI, 3, 0, 1, true, 30f);
                                            createdProjectile = true;
                                        }
                                    }
                                    else
                                    {
                                        if (createdProjectile)
                                        {
                                            CreateSliceProjectile("Horizontal_right", 10, (float)Math.PI, 3, 0, 1, true);
                                            CreateSliceProjectile("Horizontal_left", 10, 0, 3, 0, 1, true, -30f);
                                            createdProjectile = false;
                                        }
                                    }
                                    break;
                                case 4:

                                    
                                    sp.SetContactinvulnerability(5);
                                    opositeSides = true;
                                    percentageDone = percentageDone * 3;

                                    Projectile.rotation = 0;

                                    Slash(percentageDone);

                                    Projectile.rotation +=(float)Math.PI/4* (player.direction<0?-2:0);

                                    if (percentageDone%1f > 0.75f)
                                    {
                                        player.bodyFrame.Y = player.bodyFrame.Height;
                                    }
                                    else if (percentageDone%1f > 0.5)
                                    {
                                        player.bodyFrame.Y = player.bodyFrame.Height * 2;
                                    }
                                    else if (percentageDone%1f > 0.25f)
                                    {
                                        player.bodyFrame.Y = player.bodyFrame.Height * 3;
                                    }
                                    else
                                    {
                                        player.bodyFrame.Y = player.bodyFrame.Height * 4;
                                    }

                                    if (percentageDone%1f < 0.5f)
                                    {
                                        if (createdProjectile)
                                        {
                                            CreateDoubleSliceProjectile(timeLeft: 5, rotation: (player.direction < 0 ? (float)Math.PI:0), scale: 3,additive:true,secondSliceDistance:30f);
                                            createdProjectile = false;
                                            for (int i = 0; i < 2; i++)
                                            {
                                                SliceProjectile slice = (SliceProjectile)Main.projectile[attackProjectiles[attackProjectiles.Length - 2 + i]].ModProjectile;
                                                if (slice != null) { slice.FlipVertically(player.direction < 0); }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!createdProjectile)
                                        {
                                            CreateDoubleSliceProjectile(timeLeft: 5,rotation:(player.direction<0?0:(float)Math.PI), scale: 3, additive: true, secondSliceDistance: 30f);
                                            createdProjectile = true;
                                            for (int i = 0; i < 2; i++)
                                            {
                                                SliceProjectile slice = (SliceProjectile)Main.projectile[attackProjectiles[attackProjectiles.Length-2+ i]].ModProjectile;
                                                if (slice != null) { slice.FlipVertically(player.direction >= 0); }
                                            }
                                        }
                                    }

                                    break;
                                case 5:
                                    sp.SetContactinvulnerability(5);

                                    player.direction = (initMouseDir.X < 0) ? -1 : 1;

                                    Projectile.Center += new Vector2(player.width / 2, player.height / 2) + MathHelp.Normalize(initMouseDir) * (15+(1-percentageDone) * 20);

                                    if (player.direction > 0)
                                    {
                                        flipVertically = false;
                                        Projectile.rotation = (float)Math.Atan2(initMouseDir.Y, initMouseDir.X) + (float)Math.PI / 4;
                                    }
                                    else
                                    {
                                        flipVertically = true;
                                        Projectile.rotation = (float)Math.Atan2(initMouseDir.Y, initMouseDir.X) - (float)Math.PI / 4;
                                    }
                                    if (!createdProjectile)
                                    {
                                        CreateDoubleSliceProjectile("Horizontal", timeLeft: 20, (float)Math.Atan2(MathHelp.Normalize(initMouseDir).Y, MathHelp.Normalize(initMouseDir).X), scale: 3, secondSliceDistance: 30);
                                        createdProjectile = true;
                                    }

                                    break;
                            }
                            break;
                        case keyTransformation.flag:
                            switch (attackType)
                            {
                                default:
                                case 0:

                                    player.direction = initMouseDir.X < 0 ? -1 : 1;

                                    Slash(percentageDone * 3);

                                    player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                                    player.fullRotation = (percentageDone > 0.2f)?
                                        Projectile.rotation - ((float)Math.PI * (player.direction < 0 ? -3 : 1))
                                        :0;

                                    target = sp.GetClosestEnemy(500);

                                    player.gravity = 0;
                                    if (target >= 0 && Main.npc[target].active) {
                                        player.velocity = MathHelp.Normalize(Main.npc[target].Center-player.Center) * 25*percentageDone;
                                    }
                                    else
                                    {
                                        player.velocity = new Vector2(player.direction * 40 * percentageDone,0);
                                    }

                                    SetSpecialPos();
                                    break;
                                case 1:
                                    Projectile.Center = player.Center + new Vector2(Projectile.width / 2, 0) * (player.direction < 0 ? 0 : 2)+new Vector2(0,player.height/2);
                                    Projectile.rotation = (float)Math.PI * 2 * (player.direction < 0 ? 1 - percentageDone : percentageDone);
                                    if (percentageDone > 0.25f)
                                    {
                                        player.velocity = new Vector2(25 * percentageDone*player.direction, 0);
                                        sp.SetContactinvulnerability(5);
                                    }
                                    SetSpecialPos(0.5f);
                                    break;
                                case 2:
                                    if (Projectile.ai[0] == Projectile.ai[1])
                                    {
                                        player.direction = -player.direction;
                                    }
                                    Projectile.Center = player.Center + new Vector2(Projectile.width / 2, 0) * (player.direction < 0 ? 0 : 2) + new Vector2(0, player.height / 2);
                                    Projectile.rotation = (float)Math.PI * 2 * (player.direction >= 0 ? 1 - percentageDone : percentageDone);
                                    if (percentageDone > 0.25f)
                                    {
                                        player.velocity = new Vector2(25 * percentageDone * -player.direction, 0);
                                        sp.SetContactinvulnerability(5);
                                    }
                                    SetSpecialPos(0.5f);
                                    break;
                                case 3:
                                    player.direction = ((percentageDone * 2 % 1) < 0.5f) ? 1 : -1;
                                    player.bodyFrame.Y = player.bodyFrame.Height * 3;

                                    Projectile.Center = player.Center + new Vector2(Projectile.width / 2, Projectile.height / 2);

                                    Projectile.Center += new Vector2(40 + percentageDone * 15, 0) * player.direction;

                                    Projectile.rotation += (float)Math.PI / 4 * (player.direction < 0 ? -3 : 1);

                                    SetSpecialPos();
                                    break;
                                case 4:
                                    target = sp.GetClosestEnemy(500);

                                    player.bodyFrame.Y = player.bodyFrame.Height * (int)(5 - (1 - percentageDone) * 3f);

                                    if (target >= 0 && Main.npc[target].active)
                                    {
                                        player.velocity = MathHelp.Normalize(Main.npc[target].Center - player.Center) * 15;
                                        sp.SetContactinvulnerability(5);
                                    }

                                    player.direction = (initMouseDir.X < 0) ? -1 : 1;

                                    Projectile.Center = player.Center + new Vector2(player.width / 2, player.height / 2);

                                    percentageDone = Math.Clamp((1 - percentageDone) * 1.2f, 0f, 0.8f);

                                    Slash(percentageDone);

                                    Projectile.Center += (player.Center - Projectile.Center) / 2 + new Vector2(Projectile.width / 2 * (player.direction <= 0 ? 0 : 1), 0);

                                    SetSpecialPos(0.5f);

                                    break;
                            }
                            break;
                        case keyTransformation.spear:

                            switch (attackType)
                            {
                                default:
                                case 0:
                                    player.direction = (initMouseDir.X < 0) ? -1 : 1;

                                    Projectile.Center = player.Center + new Vector2(player.width / 2, player.height / 2);


                                    if (percentageDone == 1)
                                    {
                                        target = sp.GetClosestEnemy(500);

                                        if (target != -1 && Main.npc[target].active)
                                            specialPos = MathHelp.Normalize(Main.npc[target].Center + Main.npc[target].velocity - player.Center);
                                    }

                                    if (target != -1 && Main.npc[target].active)
                                    {

                                        player.direction = (specialPos.X < 0 ? -1 : 1);

                                        percentageDone = Math.Clamp(2.5f - (percentageDone * 4), -(1 - percentageDone) / 2, 2);

                                        Projectile.rotation = (float)Math.Atan2(specialPos.Y, specialPos.X) + (specialPos.X < 0 ? (float)Math.PI : 0);

                                        Projectile.Center += specialPos * 30 * percentageDone;

                                        if (percentageDone > 0)
                                        {
                                            player.velocity = MathHelp.Normalize(specialPos) * (2 - percentageDone) / 2 * 30;
                                            player.bodyFrame.Y = player.bodyFrame.Height * 3;
                                        }
                                        else
                                        {
                                            player.velocity = Vector2.Zero;
                                            player.bodyFrame.Y = player.bodyFrame.Height * 6;
                                        }
                                    }
                                    else
                                    {
                                        percentageDone = Math.Clamp(2.5f - (percentageDone * 4), -(1 - percentageDone) / 2, 2);
                                        Projectile.rotation = 0;
                                        Projectile.Center += new Vector2(percentageDone * player.direction, 0) * 30;

                                        if (percentageDone > 0)
                                        {
                                            player.velocity = new Vector2(50 * (2 - percentageDone) / 2 * player.direction, 0);
                                            player.bodyFrame.Y = player.bodyFrame.Height * 3;
                                        }
                                        else
                                        {
                                            player.velocity = Vector2.Zero;
                                            player.bodyFrame.Y = player.bodyFrame.Height * 6;
                                        }
                                    }
                                    Projectile.rotation += (float)Math.PI / 4 * (player.direction < 0 ? -3 : 1);

                                    sp.SetContactinvulnerability(5);
                                    break;
                                case 1:

                                    target = sp.GetClosestEnemy(500);

                                    player.bodyFrame.Y = player.bodyFrame.Height*(int)(5-(1-percentageDone)*3f);

                                    if(target>=0 && Main.npc[target].active)
                                    {
                                        player.velocity = MathHelp.Normalize(Main.npc[target].Center - player.Center)*15;
                                        sp.SetContactinvulnerability(5);
                                    }

                                    player.direction = (initMouseDir.X < 0) ? -1 : 1;

                                    Projectile.Center = player.Center + new Vector2(player.width / 2, player.height / 2);

                                    percentageDone = Math.Clamp( (1 - percentageDone)*1.2f,0f,0.8f);

                                    Slash(percentageDone);

                                    Projectile.Center += (player.Center - Projectile.Center) / 2+new Vector2(Projectile.width/2*(player.direction<=0?0:1),0);

                                    break;
                                case 2:

                                    player.direction = ((percentageDone * 2 % 1) < 0.5f) ? 1 : -1;
                                    player.bodyFrame.Y = player.bodyFrame.Height *3;

                                    Projectile.Center = player.Center + new Vector2(Projectile.width / 2, Projectile.height / 2);

                                    Projectile.Center += new Vector2(40+ percentageDone*15,0)*player.direction;

                                    Projectile.rotation += (float)Math.PI / 4 * (player.direction < 0 ? -3 : 1);

                                    break;
                                case 3:

                                    sp.SetContactinvulnerability(5);
                                    player.direction = (initMouseDir.X < 0) ? -1 : 1;

                                    Projectile.Center = player.Center + new Vector2(player.width / 2, player.height / 2);

                                    Slash(percentageDone*3);

                                    Projectile.Center += new Vector2(player.width / 2,0);

                                    player.velocity = Vector2.Zero;
                                    player.gravity = 0;

                                    break;
                                case 4:

                                    if (percentageDone == 1)
                                    {
                                        target = sp.GetClosestEnemy((int)(500 * 1.5f));
                                        if (target != -1 && Main.npc[target].active && Main.npc[target].life > 0)
                                        {
                                            player.Center = Main.npc[target].Center + new Vector2(0, -500);
                                        }
                                        else
                                        {
                                            player.Center = player.Center + new Vector2(0, -500);
                                        }
                                    }

                                    if (!createdProjectile)
                                    {
                                        if (sp.Grounded())
                                        {
                                            createdProjectile = true;


                                            ProjectileSource_ProjectileParent s = new ProjectileSource_ProjectileParent(Projectile);

                                            proj = Projectile.NewProjectile(s, Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GroundPound>(), Projectile.damage * 2, Projectile.knockBack * 2, player.whoAmI);
                                            proj = Projectile.NewProjectile(s, Projectile.Center + new Vector2(50, 0), Vector2.Zero, ModContent.ProjectileType<GroundPound>(), Projectile.damage * 2, Projectile.knockBack * 2, player.whoAmI);
                                            proj = Projectile.NewProjectile(s, Projectile.Center - new Vector2(50, 0), Vector2.Zero, ModContent.ProjectileType<GroundPound>(), Projectile.damage * 2, Projectile.knockBack * 2, player.whoAmI);
                                        }
                                        else
                                        {
                                            Vector2 tpDir = new Vector2(0, 75);
                                            if (!Collision.IsWorldPointSolid(player.position + tpDir + new Vector2(0, player.height)))
                                            {
                                                player.Center += tpDir;
                                            }
                                            else
                                            {
                                                tpDir = new Vector2(0, 35);
                                                if (!Collision.IsWorldPointSolid(player.position + tpDir + new Vector2(0, player.height)))
                                                {
                                                    player.Center += tpDir;
                                                }
                                            }
                                            player.velocity = new Vector2(0, 25);
                                            player.gravity = 0;
                                            sp.SetContactinvulnerability(5);
                                        }
                                    }

                                    Projectile.Center = player.Center + player.velocity + new Vector2(Projectile.width / 2, 0);
                                    Projectile.rotation = (float)Math.PI / 4 * 3;

                                    break;
                            }

                            break;
                        case keyTransformation.nanoArms:
                            break;
                        case keyTransformation.claws:
                            Projectile.Center = player.Center;
                            switch (attackType)
                            {
                                case 0:
                                    player.direction = initMouseDir.X < 0 ? -1 : 1;

                                    player.velocity = VelToEnemyIfAny(15)*percentageDone;

                                    sp.SetContactinvulnerability(5);
                                    if (percentageDone < 0.35f)
                                    {
                                        player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                                        player.fullRotation = (float)Math.PI /2*percentageDone*-player.direction;
                                        if (createdProjectile)
                                        {
                                            createdProjectile = false;

                                            CreateSliceProjectile(timeLeft: 20, ammount: 3, rotation: (player.direction < 0 ? (float)Math.PI * -1 : 0));
                                            Main.projectile[attackProjectiles[0]].scale = 1.25f;
                                            Main.projectile[attackProjectiles[1]].scale = 1.75f;
                                            Main.projectile[attackProjectiles[2]].scale = 1.25f;

                                            SliceProjectile slice;
                                            for (int i = 0; i < attackProjectiles.Length; i++)
                                            {
                                                if (attackProjectiles[i] >= 0 && Main.projectile[attackProjectiles[i]].active)
                                                {
                                                    slice = (SliceProjectile)Main.projectile[attackProjectiles[i]].ModProjectile;
                                                    if (slice != null)
                                                    {

                                                        slice.ChangeOffset(new Vector2(0, player.height / 4));
                                                        slice.ChangeDistanceToPlayer(0);
                                                        slice.FlipVertically();

                                                        if (i == 2)
                                                        {
                                                            slice.ChangeDistanceToPlayer(15);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (percentageDone < 0.75f)
                                    {
                                        player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                                        player.fullRotation = (float)Math.PI / 2*(percentageDone-0.35f)*player.direction;
                                        if (!createdProjectile)
                                        {
                                            createdProjectile = true;
                                            CreateSliceProjectile("Wide", timeLeft: 20, ammount: 3,rotation:(player.direction<0?(float)Math.PI*-1:0));
                                            Main.projectile[attackProjectiles[0]].scale = 1.5f;
                                            Main.projectile[attackProjectiles[1]].scale = 2f;
                                            Main.projectile[attackProjectiles[2]].scale = 1.5f;

                                            SliceProjectile slice = (SliceProjectile)Main.projectile[attackProjectiles[2]].ModProjectile;
                                            if (slice != null)
                                            {
                                                slice.ChangeDistanceToPlayer(40);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                                        player.fullRotation = (float)Math.PI * 2 * (1 - percentageDone) / 0.25f*player.direction;
                                    }
                                    
                                    if (percentageDone > 0.5f)
                                    {
                                        player.bodyFrame.Y = player.bodyFrame.Height * (int)((1-percentageDone)*8f+1);
                                    }
                                    else
                                    {
                                        player.bodyFrame.Y = player.bodyFrame.Height * (int)(percentageDone * 16f+1);
                                    }

                                    break;
                                default:
                                case 1:

                                    sp.SetContactinvulnerability(5);
                                    player.velocity = new Vector2(MathHelp.Sign(initMouseDir.X) * 15*percentageDone, 0);

                                    Projectile.Center = player.Center + new Vector2(player.width / 2, player.height / 2);
                                    player.bodyFrame.Y = player.bodyFrame.Height * 3;

                                    bool flipped = ((percentageDone * 5f) % 1f < 0.5f);

                                    player.direction = ((percentageDone * 5f) % 1f < 0.5f)?-1:1;
                                    Projectile.direction = player.direction;

                                    if (createdProjectile==flipped)
                                    {
                                        CreateSliceProjectile("Horizontal", timeLeft: 15, additive: true, rotation: (float)Math.PI / 4 * (player.direction < 0 ? -4 : 0), scale: 3f, offsetY: (flipped ? -6:6));

                                        SliceProjectile slice =(SliceProjectile) Main.projectile[attackProjectiles[attackProjectiles.Length - 1]].ModProjectile;

                                        if (slice != null)
                                        {
                                            slice.ChangeDistanceToPlayer(10);
                                        }

                                        createdProjectile = !createdProjectile;
                                    }

                                    break;
                                case 2:

                                    player.direction = (initMouseDir.X < 0) ? -1 : 1;

                                    if (percentageDone > 0.75f)
                                    {
                                        player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                                        player.fullRotation = (float)Math.PI * 1.5f * (1f- (percentageDone-0.75f)*4)*-player.direction;

                                        player.velocity = VelToEnemyIfAny(20)* Math.Clamp(-1 + (1 - percentageDone) / 0.25f * 2, (percentageDone - 1) / 0.25f, 1);

                                    }else if (percentageDone > 0.5f)
                                    {
                                        sp.SetContactinvulnerability(5);
                                        if (!createdProjectile)
                                        {
                                            CreateSliceProjectile("Horizontal", 20, (player.direction < 0 ?-3:0), 3f,0,1,false,0,20);
                                            CreateSliceProjectile("Horizontal", 20, (player.direction < 0 ? -3 : 0), 2f, 0, 1, true, 0, 25);
                                            CreateSliceProjectile("Horizontal", 20, (player.direction < 0 ? -3 : 0), 2f, 0, 1, true, 0, 15);

                                            createdProjectile = true;
                                        }
                                        player.fullRotation = (float)Math.PI * 2.5f*player.direction;
                                        player.velocity = new Vector2(20 * player.direction, 0);
                                    }
                                    else
                                    {
                                        sp.SetContactinvulnerability(5);
                                        if (percentageDone<0.3f && createdProjectile)
                                        {
                                            CreateSliceProjectile("Wide", 15, (player.direction < 0 ? -3 : 0), 3f, 0, 1, false, 0, -25);
                                            SliceProjectile slice=(SliceProjectile) Main.projectile[attackProjectiles[0]].ModProjectile;
                                            if (slice != null)
                                            {
                                                slice.FlipVertically(player.direction>=0);
                                            }
                                            for(int i = 0; i < 2; i++)
                                            {
                                                CreateSliceProjectile("Wide", 15, (player.direction < 0 ? -3 : 0), 2f, 0,2, true, (i*2-1)*10, -25);
                                                slice = (SliceProjectile)Main.projectile[attackProjectiles[i+1]].ModProjectile;
                                                if (slice != null)
                                                {
                                                    slice.FlipVertically(player.direction >= 0);
                                                }
                                            }

                                            createdProjectile = false;
                                        }
                                        player.fullRotation = (float)Math.PI*0.5f*(percentageDone*2f)*player.direction;
                                        player.velocity = new Vector2(percentageDone * 40*player.direction, (0.5f - percentageDone)*-30);
                                    }

                                    break;
                                case 3:

                                    player.direction = (int)(MathHelp.Sign(initMouseDir.X) * (percentageDone < 0.75f ?  1:-1 ));

                                    if (percentageDone < 0.5f)
                                    {
                                        player.fullRotation =(percentageDone<0.1f)?0: (float)Math.PI/2*(1-percentageDone)*player.direction;

                                        if (percentageDone<0.25f && createdProjectile)
                                        {
                                            player.velocity = VelToEnemyIfAny(10);

                                            createdProjectile = true;
                                            CreateSliceProjectile("Horizontal", 10, (player.direction < 0 ? -3 : 0), 3f, 0, 1, false, 0, 0);
                                            SliceProjectile slice = (SliceProjectile)Main.projectile[attackProjectiles[0]].ModProjectile;
                                            if (slice != null)
                                            {
                                                slice.FlipVertically(player.direction >= 0);
                                            }
                                            for (int i = 0; i < 2; i++)
                                            {
                                                CreateSliceProjectile("Horizontal", 10, (player.direction < 0 ? -3 : 0), 2f, 0, 2, true, (i * 2 - 1) * 10, 0);
                                                slice = (SliceProjectile)Main.projectile[attackProjectiles[i + 1]].ModProjectile;
                                                if (slice != null)
                                                {
                                                    slice.FlipVertically(player.direction >= 0);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (percentageDone < 0.75f && !createdProjectile)
                                        {
                                            TripleSlice();
                                        }

                                        player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);

                                        percentageDone = (percentageDone > 0.75f) ?(1-percentageDone)/0.25f:(0.75f-percentageDone)/0.25f;

                                        player.fullRotation = (percentageDone > 0.75f) ? percentageDone * (float)Math.PI / 2 * player.direction : (float)Math.PI * (percentageDone * 2) * -player.direction;


                                    }

                                    break;
                                case 4:

                                    if (percentageDone < 0.5f)
                                    {
                                        percentageDone *= 2;

                                        player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                                        player.fullRotation =(percentageDone<0.15f)?0: (float)Math.PI * 2 * (1 - percentageDone) * player.direction;

                                        player.velocity =VelToEnemyIfAny(15)*percentageDone;

                                        if (percentageDone < 0.25f)
                                        {
                                            if (!createdProjectile)
                                            {
                                                CreateSliceProjectile("Wide", 15, (float)Math.PI * (player.direction < 0 ? -3 : 0), 3f, 0, 1, true,-player.width*player.direction);
                                                createdProjectile = true;
                                            }
                                        }else if (percentageDone < 0.45f)
                                        {
                                            if (createdProjectile)
                                            {
                                                CreateSliceProjectile("Wide", 15,(float)Math.PI* (player.direction < 0 ? 0 : -3), 3f, 0, 1, true,player.width*player.direction);
                                                createdProjectile = false;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (!createdProjectile && percentageDone<0.75f)
                                        {
                                            TripleSlice();
                                            createdProjectile = true;
                                        }
                                    }
                                    break;
                            }
                            break;
                        case keyTransformation.skates:
                            Projectile.Center = player.Center;
                            switch (attackType)
                            {
                                default:
                                case 0:

                                    if (percentageDone > 0.5f)
                                    {
                                        player.direction = (initMouseDir.X < 0) ? -1 : 1;
                                        player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                                        player.fullRotation = Math.Clamp(-(float)Math.PI * 4f * (1 - percentageDone), -(float)Math.PI * 2, percentageDone * -0.25f) * player.direction;

                                        player.bodyFrame.Y = player.bodyFrame.Height * 5;
                                        player.legFrame.Y = player.legFrame.Height * 5;

                                        player.velocity = VelToEnemyIfAny(30) * (percentageDone-0.5f)*2f;
                                        player.gravity = 0;


                                        if (percentageDone < 0.75f)
                                        {
                                            if (createdProjectile)
                                            {
                                                CreateSliceProjectile("Wide", 10, (float)Math.PI * (player.direction < 0 ? 0 : -3), 3f, 0, 1, player.direction >= 0, player.width * player.direction);
                                                createdProjectile = false;
                                            }
                                        }
                                        else
                                        {
                                            if (!createdProjectile)
                                            {
                                                CreateSliceProjectile("Wide", 10, (float)Math.PI * (player.direction < 0 ? -3 : 0), 3f, 0, 1, player.direction >= 0, -player.width * player.direction);
                                                createdProjectile = true;
                                            }
                                        }
                                    }
                                    else
                                    {


                                        if (percentageDone < 0.05f)
                                        {
                                            player.fullRotation = percentageDone;
                                            player.direction = initMouseDir.X < 0 ? -1 : 1;
                                        }
                                        else
                                        {
                                            player.velocity = VelToEnemyIfAny(0);

                                            player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                                            player.fullRotation = (float)Math.PI / 2 * (initMouseDir.X < 0 ? -1 : 1);

                                            percentageDone = (percentageDone * 3f) % 1f;

                                            player.direction = (percentageDone < 0.5f) ? -1 : 1;

                                            if (percentageDone < 0.5f)
                                            {
                                                if (createdProjectile)
                                                {
                                                    CreateSliceProjectile("Horizontal", 10, (float)Math.PI / 4 * -2, 3f, 0, 1, false, 10);
                                                    createdProjectile = false;
                                                }
                                            }
                                            else
                                            {
                                                if (!createdProjectile)
                                                {
                                                    CreateSliceProjectile("Horizontal", 10, (float)Math.PI / 4 * 2, 3f, 0, 1, false, 0);
                                                    createdProjectile = true;
                                                }
                                            }
                                        }
                                    }

                                    sp.SetContactinvulnerability(5);

                                    break;
                                case 1:

                                    if (percentageDone < 0.6f)
                                    {
                                        if (percentageDone < 0.1f)
                                        {
                                            player.fullRotation = 0;
                                            player.direction = (initMouseDir.X < 0 ? -1 : 1);
                                        }
                                        else
                                        {

                                            percentageDone = (percentageDone/0.6f * 2f) % 1f;
                                            player.direction = (percentageDone < 0.5f) ? -1 : 1;

                                            player.fullRotation = (player.direction < 0) ?
                                                ((float)Math.PI / 4 * (initMouseDir.X < 0 ? -1 : 1))
                                                : ((float)Math.PI / 4 * -(initMouseDir.X < 0 ? -1 : 1));

                                            if (percentageDone < 0.5f)
                                            {
                                                if (!createdProjectile)
                                                {

                                                    createdProjectile = true;
                                                    CreateSliceProjectile("Horizontal", 10, (player.direction < 0 ? -3 : 0), 3f, 0, 1, false, 0, -5);
                                                    SliceProjectile slice = (SliceProjectile)Main.projectile[attackProjectiles[0]].ModProjectile;
                                                }
                                            }
                                            else
                                            {
                                                if (createdProjectile)
                                                {

                                                    createdProjectile = false;
                                                    CreateSliceProjectile("Horizontal", 10, (player.direction < 0 ? -3 : 0), 3f, 0, 1, false, 0, 5);
                                                    SliceProjectile slice = (SliceProjectile)Main.projectile[attackProjectiles[0]].ModProjectile;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {

                                        player.bodyFrame.Y = player.bodyFrame.Height*(percentageDone < 0.5f ?5:6);
                                        player.legFrame.Y = player.legFrame.Height * (percentageDone < 0.5f ? 5 : 7);

                                        percentageDone = (percentageDone - 0.6f) / 0.4f;
                                        
                                        player.velocity = new Vector2(Math.Clamp(1-percentageDone*2, -percentageDone, 10-percentageDone*10)*15*(initMouseDir.X < 0 ? -1 : 1),0);

                                        player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);

                                        desRotation = (1-(Math.Clamp(percentageDone, 0.35f, 1) - 0.35f)/(1-0.35f));
                                        player.fullRotation =desRotation*(float)Math.PI/4*- (initMouseDir.X < 0 ? -1 : 1);

                                        player.direction = (percentageDone < 0.75f && percentageDone > 0.5f) ? -(initMouseDir.X < 0 ? -1 : 1) : (initMouseDir.X < 0 ? -1 : 1);

                                    }

                                    break;
                                case 2:

                                    float yOffset = percentageDone*player.height-player.height/2;
                                    player.velocity = new Vector2(Math.Clamp(percentageDone - 0.5f, 0, 1) * 15 * (initMouseDir.X < 0 ? -1 : 1), (1-percentageDone)*-5);

                                    percentageDone = percentageDone * 5f % 1f;

                                    player.direction = percentageDone < 0.5f ? -1 : 1;


                                    if (percentageDone < 0.5f)
                                    {
                                        if (!createdProjectile)
                                        {

                                            createdProjectile = true;
                                            CreateSliceProjectile("Horizontal", 10, (player.direction < 0 ? -3 : 0), 3f, 0, 1, false, 0, -5+yOffset);
                                            SliceProjectile slice = (SliceProjectile)Main.projectile[attackProjectiles[0]].ModProjectile;
                                        }
                                    }
                                    else
                                    {
                                        if (createdProjectile)
                                        {

                                            createdProjectile = false;
                                            CreateSliceProjectile("Horizontal", 10, (player.direction < 0 ? -3 : 0), 3f, 0, 1, false, 0, 5 + yOffset);
                                            SliceProjectile slice = (SliceProjectile)Main.projectile[attackProjectiles[0]].ModProjectile;
                                        }
                                    }

                                    break;
                                case 3:

                                    player.velocity = new Vector2(Math.Clamp(2 - percentageDone * 3, -percentageDone, 2) * 15 * (initMouseDir.X < 0 ? -1 : 1), 0);

                                    if (percentageDone < 0.5f)
                                    {
                                        if (percentageDone < 0.1f)
                                        {
                                            player.fullRotation = 0;
                                            player.direction = initMouseDir.X < 0 ? -1 : 1;
                                        }
                                        else
                                        {
                                            player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                                            player.fullRotation = (float)Math.PI / 2 * (initMouseDir.X < 0 ? -1 : 1);

                                            percentageDone = (1 - percentageDone) * 2 * 5f % 1f;
                                            player.direction = (percentageDone < 0.5f) ? -1 : 1;

                                            float dirToSpin = -35f;

                                            if (percentageDone < 0.5f)
                                            {
                                                if (createdProjectile)
                                                {
                                                    createdProjectile = false;

                                                    CreateSliceProjectile("Wide", 10, (float)Math.PI * (initMouseDir.X < 0 ? -1 : 0), 2f, 0, 1, true, MathHelp.Sign(initMouseDir.X) * (-10+dirToSpin),10*MathHelp.Sign(initMouseDir.X));

                                                    CreateSliceProjectile("", 7, (float)Math.PI * (initMouseDir.X < 0 ? 0 : -1), 1.5f, 0, 1, true, MathHelp.Sign(initMouseDir.X) * (65+dirToSpin));

                                                    CreateSliceProjectile("Horizontal", 5, (float)Math.PI * (initMouseDir.X < 0 ? -1 : 0), 1f, 0, 1, true, MathHelp.Sign(initMouseDir.X) * (70+dirToSpin), 5 * MathHelp.Sign(initMouseDir.X));

                                                }
                                            }
                                            else
                                            {
                                                if (!createdProjectile)
                                                {
                                                    createdProjectile = true;

                                                    CreateSliceProjectile("Wide", 10, (float)Math.PI * (initMouseDir.X < 0 ? 0 : -1), 2f, 0, 1, true, MathHelp.Sign(initMouseDir.X) *(30+dirToSpin),-10 * MathHelp.Sign(initMouseDir.X));

                                                    CreateSliceProjectile("", 7, (float)Math.PI * (initMouseDir.X < 0 ? -1 : 0), 1.5f, 0, 1, true, MathHelp.Sign(initMouseDir.X) * (40+dirToSpin), 5*MathHelp.Sign(initMouseDir.X));

                                                    CreateSliceProjectile("Horizontal", 5, (float)Math.PI * (initMouseDir.X < 0 ? -1 : 0), 1.5f, 0, 1, true, MathHelp.Sign(initMouseDir.X) *(60+dirToSpin),5*MathHelp.Sign(initMouseDir.X));

                                                }
                                            }

                                        }
                                        sp.SetContactinvulnerability(5);

                                    }
                                    else
                                    {

                                        float rotPercent = Math.Clamp((1 - percentageDone)*2, 0, 1);

                                        player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                                        player.fullRotation = (float)Math.PI / 2 * (initMouseDir.X < 0 ? -Math.Clamp(rotPercent*4,0,3) : Math.Clamp(rotPercent*4,0,3));

                                        percentageDone = (1-percentageDone) * 5 % 1;
                                        player.direction = percentageDone < 0.5f ? -1 : 1;

                                    }

                                    break;
                                case 4:

                                    player.velocity = VelToEnemyIfAny(Math.Clamp(2 - percentageDone * 3, 0, 2) * 30 * (initMouseDir.X < 0 ? -1 : 1));

                                    if (percentageDone < 0.5f)
                                    {
                                        if (percentageDone < 0.05f)
                                        {
                                            player.fullRotation = 0;
                                            player.velocity = Vector2.Zero;
                                            player.direction = (int)MathHelp.Sign(initMouseDir.X);
                                        }
                                        else
                                        {
                                            sp.SetContactinvulnerability(5);

                                            percentageDone = Math.Clamp((0.5f - percentageDone) * 4f, 0, 1);

                                            player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                                            player.fullRotation = (float)Math.PI / 2 * (3 + (percentageDone * (initMouseDir.X < 0 ? 5 : -5)));

                                            if (percentageDone < 0.2f && !createdProjectile)
                                            {
                                                createdProjectile = true;

                                                CreateSliceProjectile("Wide", 20, (float)Math.PI * (initMouseDir.X < 0 ? 1 : 0), 3);
                                                SliceProjectile slice = (SliceProjectile)Main.projectile[attackProjectiles[0]].ModProjectile;
                                                if (slice != null && initMouseDir.X >= 0)
                                                {
                                                    slice.FlipVertically();
                                                }

                                            }
                                        }

                                    }
                                    else
                                    {

                                        float rotPercent = Math.Clamp((1 - percentageDone) * 2, 0, 1);

                                        player.fullRotationOrigin = new Vector2(player.width / 2, player.height / 2);
                                        player.fullRotation = (float)Math.PI / 2 * (initMouseDir.X < 0 ? -Math.Clamp(rotPercent * 4, 0, 3) : Math.Clamp(rotPercent * 4, 0, 3));

                                        percentageDone = (1 - percentageDone) * 5 % 1;
                                        player.direction = percentageDone < 0.5f ? -1 : 1;

                                    }

                                    break;
                            }
                            break;
                    }

                }

                Projectile.ai[0]--;
                if (Projectile.ai[0] < -1)
                {
                    midAttack = false;
                    player = null;
                    Projectile.ai[0]= Projectile.ai[1] = 0;
                    transform = keyTransformation.none;
                }
            }
        }

        public void Slash(float percentageDone)
        {
            Vector2 weaponDir;
            if (player.direction < 0)
            {
                weaponDir = new Vector2((float)Math.Cos((percentageDone * 4 + 1.5f)),
                    (float)Math.Sin((percentageDone * 4 + 1.5f)));
                Projectile.rotation = (float)Math.Atan2(weaponDir.Y, weaponDir.X);

                weaponDir = new Vector2((float)Math.Cos((percentageDone * 4 + 0.75f)),
                   (float)Math.Sin((percentageDone * 4 + 0.75f)));

                Projectile.position = player.Center + weaponDir * 50 + new Vector2(20 * -Projectile.spriteDirection, 0);
            }
            else
            {
                weaponDir = new Vector2((float)Math.Cos(-(percentageDone * 4 - 1.5f)),
                    -(float)Math.Sin((percentageDone * 4 - 1.5f)));
                Projectile.rotation = (float)Math.Atan2(weaponDir.Y, weaponDir.X) + (float)(Math.PI * 0.5f);

                weaponDir = new Vector2(-(float)Math.Cos((percentageDone * 4 + 0.75f)),
                   (float)Math.Sin((percentageDone * 4 + 0.75f)));

                Projectile.position = player.Center + weaponDir * 50 + new Vector2(20 * -Projectile.spriteDirection + 15, 0);
            }

            player.direction = initMouseDir.X < 0 ? -1 : 1;

        }

        public Vector2 VelToEnemyIfAny(float speed)
        {
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            target = sp.GetClosestEnemy(500);

            if(target>=0 && Main.npc[target].active)
            {
                return MathHelp.Normalize(Main.npc[target].Center - player.Center) * speed;
            }else{
                return new Vector2(player.direction * speed,0);
            }

        }

        public void TripleSlice()
        {
            createdProjectile = true;
            CreateSliceProjectile("Wide", 10, (player.direction < 0 ? -3 : 0), 3f, 0, 1, false, 0, -25);
            SliceProjectile slice = (SliceProjectile)Main.projectile[attackProjectiles[0]].ModProjectile;
            if (slice != null)
            {
                slice.FlipVertically(player.direction >= 0);
            }
            for (int i = 0; i < 2; i++)
            {
                CreateSliceProjectile("Wide", 10, (player.direction < 0 ? -3 : 0), 2f, 0, 2, true, (i * 2 - 1) * 10, -25);
                slice = (SliceProjectile)Main.projectile[attackProjectiles[i + 1]].ModProjectile;
                if (slice != null)
                {
                    slice.FlipVertically(player.direction >= 0);
                }
            }
        }

        public void CreateSliceProjectile(string type="",int timeLeft = 10, float rotation = 666f, float scale = 0f, int initProjectile = 0, int ammount = 1, bool additive = false,float offsetX=0f,float offsetY=0f)
        {
            int startProj = initProjectile + (additive ? attackProjectiles.Length : 0);
            int ammountToCreate = startProj+ ammount;
            ProjectileSource_ProjectileParent s = new ProjectileSource_ProjectileParent(Projectile);
            RescaleProjectileArray(ammountToCreate);
            for (int i = 0; i < ammount; i++)
            {
                attackProjectiles[startProj + i] = Projectile.NewProjectile(s, player.Center + new Vector2(player.direction * 15, 0), Vector2.Zero, ModContent.ProjectileType<SliceProjectile>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
                Main.projectile[attackProjectiles[startProj + i]].timeLeft = timeLeft;

                if (scale > 0)
                {
                    Main.projectile[attackProjectiles[startProj + i]].scale =scale;
                }
                Main.projectile[attackProjectiles[startProj + i]].rotation = ((rotation == 666f) ? (float)Math.Atan2(initMouseDir.Y, initMouseDir.X) : rotation);
                Main.projectile[attackProjectiles[startProj + i]].direction = player.direction;

                SliceProjectile slice = (SliceProjectile)Main.projectile[attackProjectiles[startProj+i]].ModProjectile;

                if (slice != null)
                {
                    slice.ChangeTexture(sprite + "_slice"+type);
                    slice.ChangeOffset(new Vector2(offsetX, offsetY));
                }
            }
        }

        public void CreateDoubleSliceProjectile(string type = "", int timeLeft = 10, float rotation = 666f, float scale = 0f, int initProjectile = 0, int ammount = 1, bool additive = false, float offsetX = 0f, float offsetY = 0f,float secondSliceDistance=0f)
        {
            CreateSliceProjectile(type+"_right", timeLeft, rotation, scale, initProjectile, ammount, additive, offsetX, offsetY);
            CreateSliceProjectile(type+"_left", timeLeft, rotation, scale, initProjectile, ammount, true, offsetX+(float)Math.Sin(-rotation)*secondSliceDistance, offsetY+(float)Math.Cos(-rotation)*secondSliceDistance);
        }

        public void RescaleProjectileArray(int ammountOfProjectiles)
        {
            if (attackProjectiles!=null && attackProjectiles.Length > 0)
            {
                int[] newProjectiles = new int[ammountOfProjectiles];
                for (int i = 0; i < ammountOfProjectiles; i++)
                {
                    if (i < attackProjectiles.Length)
                    {
                        newProjectiles[i] = attackProjectiles[i];
                    }
                    else
                    {
                        newProjectiles[i] = -1;
                    }
                }
                attackProjectiles = newProjectiles;

            }
            else
            {
                attackProjectiles = new int[ammountOfProjectiles];
            }
        }

        public void SetSprite(string spriteToUse,Player p,keyTransformation transformation)
        {
            player = p;
            sprite = spriteToUse;
            Projectile.position = p.Center;
            Projectile.rotation = 0;
            Projectile.timeLeft = 3;
            transform = transformation;

            if (!midAttack)
            {
                Projectile.damage = 0;
                Projectile.knockBack = 0;

                flipVertically = false;
                Projectile.direction = player.direction;
                ResetPositionAndRotation(transformation);
            }
        }

        public void Attack(int duration, Player p, keyTransformation tranformation)
        {
            player = p;
            Projectile.position = p.Center;
            Projectile.rotation = 0;
            if (!midAttack)
            {
                transform = tranformation;
                Projectile.ai[0] = Projectile.ai[1] = duration;
                Projectile.timeLeft = duration;
                midAttack = true;
                initMouseDir = MathHelp.Normalize(Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - player.Center);
                flipVertically = false;
                Projectile.direction = player.direction;
                ResetPositionAndRotation(tranformation);
                createdProjectile = false;
            }
        }

        public void SetComboAttack(int atkType = 0)
        {
            attackType = atkType;
        }

        public void ResetPositionAndRotation(keyTransformation transformation)
        {
            Vector2 mouseDir= MathHelp.Normalize(Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - player.Center);
            switch (transformation)
            {
                case keyTransformation.cannon:
                case keyTransformation.drill:
                    Projectile.Center += MathHelp.Normalize(Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - player.Center) * 5;

                    Projectile.rotation = (float)Math.Atan2(-mouseDir.Y, -mouseDir.X) + (float)Math.PI / 4*(flipVertically?1:-3);
                    Projectile.scale = cannonScale;

                    break;
                case keyTransformation.shield:
                    Projectile.Center = player.Center+new Vector2(0,player.height/2);
                    Projectile.Center += MathHelp.Normalize(Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - player.Center) ;

                    Projectile.rotation = (float)Math.Atan2(mouseDir.Y, mouseDir.X);

                    flipVertically = mouseDir.X < 0;

                    break;
                case keyTransformation.guns:

                    flipVertically = Vector2.Dot(new Vector2(1, 0),
                        mouseDir)
                        <0;
                    Projectile.direction = mouseDir.X < 0 ? -1 : 1;

                    Projectile.Center = player.Center+new Vector2(5,player.height/2+5);

                    if (flipVertically)
                    {
                        Projectile.rotation = (float)Math.Atan2(mouseDir.Y, mouseDir.X) - (float)Math.PI / 4;
                    }
                    else
                    {
                        Projectile.rotation = (float)Math.Atan2(mouseDir.Y, mouseDir.X) + (float)Math.PI / 4;
                    }

                    break;
                case keyTransformation.flag:

                    player.direction = (mouseDir.X < 0) ? -1 : 1;
                    Projectile.Center = player.Center;
                    Projectile.Center += new Vector2(Projectile.width * (mouseDir.X < 0 ? -0.2f : 1.2f), 0);
                    Projectile.rotation = (float)Math.PI / 4f * (mouseDir.X < 0 ? -2 : 0);
                    Projectile.direction = (mouseDir.X < 0) ? -1 : 1;
                    flipVertically = mouseDir.X < 0;

                    SetSpecialPos(0.25f);

                    break;
                case keyTransformation.hammer:

                    player.direction = (mouseDir.X < 0) ? -1 : 1;
                    Projectile.Center = player.Center;
                    Projectile.Center += new Vector2(Projectile.width * (mouseDir.X < 0 ? 1 :0), 0);
                    Projectile.direction = (mouseDir.X < 0) ? -1 : 1;
                    flipVertically = mouseDir.X < 0;

                    Projectile.rotation = (float)Math.PI / 4f * (mouseDir.X < 0 ? 0 : -2);

                    break;
                case keyTransformation.nanoArms:
                    break;
                case keyTransformation.staff:
                    Projectile.Center += new Vector2(15 * player.direction, 0);
                    Projectile.direction = player.direction;

                    Projectile.rotation = -(float)Math.PI / 4f;
                    break;
                case keyTransformation.swords:
                    player.direction = (mouseDir.X < 0) ? -1 : 1;
                    Projectile.Center = player.Center + new Vector2(0, player.height / 2 + Projectile.height / 4);
                    Projectile.Center += new Vector2(25 + Projectile.width * (mouseDir.X < 0 ? -1 : 1), 0);

                    Projectile.direction = (mouseDir.X < 0) ? -1 : 1;
                    flipVertically = mouseDir.X < 0;

                    Projectile.rotation = (float)Math.PI / 4f * (mouseDir.X < 0 ? -3 : 1);
                    player.bodyFrame.Y = player.bodyFrame.Height * 4;
                    break;
                case keyTransformation.yoyo:
                    break;
                case keyTransformation.dual:
                    opositeSides = false;
                    player.direction = (mouseDir.X < 0) ? -1:1 ;
                    Projectile.Center = player.Center+new Vector2(0,player.height/2+Projectile.height/4);
                    Projectile.Center += new Vector2(5+Projectile.width * (mouseDir.X < 0 ? -1 : 1), 0);
                    
                    Projectile.direction= (mouseDir.X < 0) ? -1 : 1;
                    flipVertically = mouseDir.X < 0;

                    Projectile.rotation = (float)Math.PI / 4f*(mouseDir.X<0?-5:1);
                    player.bodyFrame.Y = player.bodyFrame.Height * 4;
                    break;
                case keyTransformation.spear:
                    player.direction = (mouseDir.X < 0) ? -1 : 1;
                    Projectile.Center = player.Center + new Vector2(player.width*(player.direction<0?0.5f:2f), player.height / 2 + Projectile.height / 4);

                    Projectile.direction = (mouseDir.X < 0) ? -1 : 1;
                    flipVertically = mouseDir.X < 0;

                    Projectile.rotation = (float)Math.PI / 4f * (mouseDir.X < 0 ? -3 : 1);
                    player.bodyFrame.Y = player.bodyFrame.Height * 4;
                    break;
                case keyTransformation.claws:
                case keyTransformation.skates:
                    Projectile.Center = player.Center;
                    Projectile.direction = player.direction;
                    break;

            }
        }

        public void SetSpecialPos(float speed=1)
        {
            switch (transform)
            {
                case keyTransformation.flag:

                    float projRot = -Projectile.rotation + (float)Math.PI-(float)Math.PI/4;

                    Vector2 newSpecialPos= Projectile.position + new Vector2((float)Math.Sin(projRot), (float)Math.Cos(projRot)) * (Projectile.width);

                    if (specialPos == Vector2.Zero)
                    {
                        lastSpecialPos=specialPos = newSpecialPos;
                    }
                    else
                    {
                        if (lastSpecialPos == specialPos)
                        {
                            lastSpecialPos += new Vector2(0, 1);
                        }
                        else
                        {
                            lastSpecialPos = (lastSpecialPos == Vector2.Zero) ? newSpecialPos : specialPos;
                            specialPos = newSpecialPos;
                        }
                    }

                    newSpecialPos = MathHelp.Normalize(lastSpecialPos - specialPos);
                    
                    float newdesRot= (MathHelp.Magnitude(newSpecialPos) > 0)? (float)Math.Atan2(newSpecialPos.Y, newSpecialPos.X):(float)Math.PI/2f;

                    if (newdesRot != float.NaN && newdesRot!=float.NegativeInfinity && newdesRot!=float.PositiveInfinity)
                    {
                        newdesRot = MathHelp.Lerp(desRotation, newdesRot, speed);
                        desRotation = (newdesRot==float.NaN)?desRotation:newdesRot;
                    }
                    break;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture;

            Vector2 pos = Projectile.position;
            float rot = Projectile.rotation;
            if (player != null && opositeSides)
            {
                pos = player.Center + player.Center - Projectile.position - new Vector2(player.width, 0);
                rot = Projectile.rotation + (float)Math.PI;
            }

            Rectangle rect = new Rectangle();

            switch (transform)
            {
                default:
                    if (sprite != "None" && sprite != "")
                    {
                        texture = ModContent.Request<Texture2D>("KingdomTerrahearts/" + sprite).Value;
                        rect = new Rectangle(0, 0, texture.Width, texture.Height);
                        Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition, rect, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, (Projectile.spriteDirection == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1);
                    }
                    break;
                case keyTransformation.guns:
                case keyTransformation.dual:
                    if (sprite != "None" && sprite != "")
                    {
                        Vector2 mouseDir = MathHelp.Normalize(Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - player.Center);
                        if (mouseDir.X>=0) {
                            texture = ModContent.Request<Texture2D>("KingdomTerrahearts/" + sprite + "_left").Value;
                            if (texture != null)
                            {
                                rect = new Rectangle(0, 0, texture.Width, texture.Height);

                                Main.spriteBatch.Draw(texture,
                                    Projectile.position - Main.screenPosition + new Vector2(25 * Projectile.spriteDirection, 0)
                                    , rect, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale,
                                    (flipVertically) ? SpriteEffects.FlipVertically : (Projectile.spriteDirection == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                                    1);
                            }
                            texture = ModContent.Request<Texture2D>("KingdomTerrahearts/" + sprite + "_right").Value;
                            if (texture != null)
                            {
                                rect = new Rectangle(0, 0, texture.Width, texture.Height);


                                Main.spriteBatch.Draw(texture,
                                    pos - Main.screenPosition + new Vector2(10 * Projectile.spriteDirection, 0)
                                    , rect, lightColor, rot, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale,
                                    (flipVertically) ? SpriteEffects.FlipVertically : (Projectile.spriteDirection == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                                    1);
                            }
                        }
                        else
                        {
                            texture = ModContent.Request<Texture2D>("KingdomTerrahearts/" + sprite + "_right").Value;
                            if (texture != null)
                            {
                                rect = new Rectangle(0, 0, texture.Width, texture.Height);

                                Main.spriteBatch.Draw(texture,
                                    Projectile.position - Main.screenPosition + new Vector2(10 * Projectile.spriteDirection, 0)
                                    , rect, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale,
                                    (flipVertically) ? SpriteEffects.FlipVertically : (Projectile.spriteDirection == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                                    1);
                            }
                            texture = ModContent.Request<Texture2D>("KingdomTerrahearts/" + sprite + "_left").Value;
                            if (texture != null)
                            {
                                rect = new Rectangle(0, 0, texture.Width, texture.Height);

                                Main.spriteBatch.Draw(texture,
                                    pos - Main.screenPosition + new Vector2(25 * Projectile.spriteDirection, 0)
                                    , rect, lightColor, rot, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale,
                                    (flipVertically) ? SpriteEffects.FlipVertically : (Projectile.spriteDirection == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                                    1);
                            }
                        }
                    }
                    break;
                case keyTransformation.shield:
                    texture = ModContent.Request<Texture2D>("KingdomTerrahearts/" + sprite + "_shield").Value;
                    if (texture != null)
                    {
                        rect = new Rectangle(0, 0, texture.Width, texture.Height);

                        Main.spriteBatch.Draw(texture,
                            Projectile.position - Main.screenPosition + new Vector2(25 * Projectile.spriteDirection, 0)
                            , rect, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale,
                            (flipVertically) ? SpriteEffects.FlipVertically : (Projectile.spriteDirection == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                            1);
                    }
                    break;
                case keyTransformation.flag:
                    if (sprite != "None" && sprite != "")
                    {
                        texture = ModContent.Request<Texture2D>("KingdomTerrahearts/" + sprite).Value;
                        rect = new Rectangle(0, 0, texture.Width, texture.Height);
                        Main.spriteBatch.Draw(texture, Projectile.position - Main.screenPosition, rect, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), Projectile.scale, (Projectile.spriteDirection == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1);
                    }
                    texture = ModContent.Request<Texture2D>("KingdomTerrahearts/" + sprite + "_flag").Value;
                    if (texture != null)
                    {
                        rect = new Rectangle(0, 0, texture.Width, texture.Height);

                        float flagScale = Projectile.scale * 2;
                        Vector2 texturePos = specialPos - Main.screenPosition;

                        Main.spriteBatch.Draw(texture,texturePos, rect, lightColor,desRotation, new Vector2(0,texture.Height/2), flagScale,
                            SpriteEffects.None,
                            1);
                    }
                    break;
                case keyTransformation.skates:
                case keyTransformation.claws:
                    
                    texture = ModContent.Request<Texture2D>("KingdomTerrahearts/" + sprite + "_claws").Value;


                    Vector2 textureOffset=Vector2.Zero;
                    if (texture != null)
                    {
                        rect = new Rectangle(0, player.bodyFrame.Y, texture.Width, player.bodyFrame.Height);
                        textureOffset = new Vector2(-1f,-3);

                        Main.spriteBatch.Draw(texture, player.Center+textureOffset - Main.screenPosition, rect, lightColor, player.fullRotation, new Vector2(texture.Width/2, player.bodyFrame.Height/2),Projectile.scale,
                            (player.direction<0)?SpriteEffects.FlipHorizontally:SpriteEffects.None,
                            1);
                    }

                    if (transform == keyTransformation.skates)
                    {

                        texture = ModContent.Request<Texture2D>("KingdomTerrahearts/" + sprite + "_skates").Value;

                        if (texture != null)
                        {
                            rect = new Rectangle(0, player.legFrame.Y, texture.Width, player.bodyFrame.Height);

                            Main.spriteBatch.Draw(texture, player.Center + textureOffset - Main.screenPosition, rect, lightColor, player.fullRotation, new Vector2(texture.Width / 2, player.bodyFrame.Height / 2), Projectile.scale,
                                (player.direction < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                                1);
                        }
                    }

                    break;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (player!=null)
            {
                SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
                sp.AddInvulnerability(Projectile.timeLeft+3);
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

    }
}
