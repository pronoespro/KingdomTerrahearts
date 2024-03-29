﻿using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Items
{
    public class Invincible : ModItem
    {


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Angel Feather");
            Tooltip.SetDefault("Gatito brand" +
                "\nOur buyer used divine means to acquire this mysterious feather" +
                "\nIt was once in the possession of Shibuya's Composer" +
                "\nMakes you as invulnerable and mobile as an angel");
        }

        public override void SetDefaults()
        {

            Item.accessory = true;
            Item.width = 26;
            Item.height = 26;

        }


        public override void UpdateEquip(Player player)
        {
            SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
            sp.invincible = true;

            sp.canDash = true;
            sp.canDashMidAir = true;
            sp.dashSpeed = 200;
            sp.ChangeDashReload(1);

            sp.canDoubleJump = true;
            sp.doubleJumpQuantity = 400;
            sp.doubleJumpHeight = 100;
            
            sp.canGlide = true;
            sp.glideTime = 10000;

            sp.glideFallSpeed = 0;
            sp.enlightened = true;
            
            sp.reviveTime = 0;

            for (int i = 0; i < player.buffImmune.Length; i++)
            {
                #region goodBuffs
                switch (i)
                {
                    case BuffID.AmmoBox:
                    case BuffID.AmmoReservation:
                    case BuffID.Archery:
                    case BuffID.BabyDinosaur:
                    case BuffID.BabyEater:
                    case BuffID.BabyFaceMonster:
                    case BuffID.BabyGrinch:
                    case BuffID.BabyHornet:
                    case BuffID.BabyPenguin:
                    case BuffID.BabySkeletronHead:
                    case BuffID.BabySlime:
                    case BuffID.BabySnowman:
                    case BuffID.BabyTruffle:
                    case BuffID.BasiliskMount:
                    case BuffID.Battle:
                    case BuffID.BeeMount:
                    case BuffID.BeetleEndurance1:
                    case BuffID.BeetleEndurance2:
                    case BuffID.BeetleEndurance3:
                    case BuffID.BeetleMight1:
                    case BuffID.BeetleMight2:
                    case BuffID.BeetleMight3:
                    case BuffID.Bewitched:
                    case BuffID.BlackCat:
                    case BuffID.Builder:
                    case BuffID.BunnyMount:
                    case BuffID.Calm:
                    case BuffID.Campfire:
                    case BuffID.Clairvoyance:
                    case BuffID.CompanionCube:
                    case BuffID.Crate:
                    case BuffID.CrimsonHeart:
                    case BuffID.CuteFishronMount:
                    case BuffID.Dangersense:
                    case BuffID.Daybreak:
                    case BuffID.DeadlySphere:
                    case BuffID.DrillMount:
                    case BuffID.DryadsWard:
                    case BuffID.Endurance:
                    case BuffID.EyeballSpring:
                    case BuffID.FairyBlue:
                    case BuffID.FairyGreen:
                    case BuffID.FairyRed:
                    case BuffID.Featherfall:
                    case BuffID.Fishing:
                    case BuffID.Flipper:
                    case BuffID.Gills:
                    case BuffID.Gravitation:
                    case BuffID.HeartLamp:
                    case BuffID.Heartreach:
                    case BuffID.Honey:
                    case BuffID.HornetMinion:
                    case BuffID.IceBarrier:
                    case BuffID.Hunter:
                    case BuffID.Inferno:
                    case BuffID.Invisibility:
                    case BuffID.Ironskin:
                    case BuffID.Lifeforce:
                    case BuffID.Lovestruck:
                    case BuffID.MagicLantern:
                    case BuffID.MagicPower:
                    case BuffID.ManaRegeneration:
                    case BuffID.Midas:
                    case BuffID.Mining:
                    case BuffID.NebulaUpDmg1:
                    case BuffID.NebulaUpDmg2:
                    case BuffID.NebulaUpDmg3:
                    case BuffID.NebulaUpLife1:
                    case BuffID.NebulaUpLife2:
                    case BuffID.NebulaUpLife3:
                    case BuffID.NebulaUpMana1:
                    case BuffID.NebulaUpMana2:
                    case BuffID.NebulaUpMana3:
                    case BuffID.NightOwl:
                    case BuffID.ObsidianSkin:
                    case BuffID.ParryDamageBuff:
                    case BuffID.PeaceCandle:
                    case BuffID.PetBunny:
                    case BuffID.PetDD2Dragon:
                    case BuffID.PetDD2Gato:
                    case BuffID.PetDD2Ghost:
                    case BuffID.PetLizard:
                    case BuffID.PetParrot:
                    case BuffID.PetSapling:
                    case BuffID.PetSpider:
                    case BuffID.PetTurtle:
                    case BuffID.PigronMount:
                    case BuffID.PirateMinion:
                    case BuffID.Puppy:
                    case BuffID.Pygmies:
                    case BuffID.Rage:
                    case BuffID.RapidHealing:
                    case BuffID.Ravens:
                    case BuffID.Regeneration:
                    case BuffID.Rudolph:
                    case BuffID.ScutlixMount:
                    case BuffID.ShadowDodge:
                    case BuffID.ShadowOrb:
                    case BuffID.SharknadoMinion:
                    case BuffID.Sharpened:
                    case BuffID.Shine:
                    case BuffID.SlimeMount:
                    case BuffID.SolarShield1:
                    case BuffID.SolarShield2:
                    case BuffID.SolarShield3:
                    case BuffID.Sonar:
                    case BuffID.Spelunker:
                    case BuffID.SpiderMinion:
                    case BuffID.StardustDragonMinion:
                    case BuffID.StardustGuardianMinion:
                    case BuffID.StardustMinion:
                    case BuffID.StarInBottle:
                    case BuffID.SugarRush:
                    case BuffID.Summoning:
                    case BuffID.Sunflower:
                    case BuffID.SuspiciousTentacle:
                    case BuffID.Swiftness:
                    case BuffID.Thorns:
                    case BuffID.TikiSpirit:
                    case BuffID.Titan:
                    case BuffID.TurtleMount:
                    case BuffID.TwinEyesMinion:
                    case BuffID.UFOMinion:
                    case BuffID.UFOMount:
                    case BuffID.UnicornMount:
                    case BuffID.Warmth:
                    case BuffID.WaterCandle:
                    case BuffID.WaterWalking:
                    case BuffID.WeaponImbueConfetti:
                    case BuffID.WeaponImbueCursedFlames:
                    case BuffID.WeaponImbueFire:
                    case BuffID.WeaponImbueGold:
                    case BuffID.WeaponImbueIchor:
                    case BuffID.WeaponImbueNanites:
                    case BuffID.WeaponImbuePoison:
                    case BuffID.WeaponImbueVenom:
                    case BuffID.WellFed:
                    case BuffID.Werewolf:
                    case BuffID.Wisp:
                    case BuffID.Wrath:
                    case BuffID.ZephyrFish:
                    case BuffID.BabyBird:
                    case BuffID.BabyRedPanda:
                    case BuffID.BabyWerewolf:
                    case BuffID.BatOfLight:
                    case BuffID.BrainOfCthulhuPet:
                    case BuffID.CatBast:
                    case BuffID.CoolWhipPlayerBuff:
                    case BuffID.DarkHorseMount:
                    case BuffID.DarkMageBookMount:
                    case BuffID.DestroyerPet:
                    case BuffID.DukeFishronPet:
                    case BuffID.DynamiteKitten:
                    case BuffID.EaterOfWorldsPet:
                    case BuffID.EmpressBlade:
                    case BuffID.EverscreamPet:
                    case BuffID.EyeOfCthulhuPet:
                    case BuffID.FairyQueenPet:
                    case BuffID.FennecFox:
                    case BuffID.Flamingo:
                    case BuffID.FlinxMinion:
                    case BuffID.GlitteryButterfly:
                    case BuffID.GolemPet:
                    case BuffID.GolfCartMount:
                    case BuffID.IceQueenPet:
                    case BuffID.ImpMinion:
                    case BuffID.KingSlimePet:
                    case BuffID.LavaSharkMount:
                    case BuffID.LeafCrystal:
                    case BuffID.LilHarpy:
                    case BuffID.Lucky:
                    case BuffID.LunaticCultistPet:
                    case BuffID.MajesticHorseMount:
                    case BuffID.MartianPet:
                    case BuffID.Merfolk:
                    case BuffID.MiniMinotaur:
                    case BuffID.MonsterBanner:
                    case BuffID.MoonLordPet:
                    case BuffID.PaintedHorseMount:
                    case BuffID.PaladinsShield:
                    case BuffID.PirateShipMount:
                    case BuffID.PlanteraPet:
                    case BuffID.Plantero:
                    case BuffID.PogoStickMount:
                    case BuffID.PumpkingPet:
                    case BuffID.QueenBeePet:
                    case BuffID.QueenSlimeMount:
                    case BuffID.QueenSlimePet:
                    case BuffID.SantankMount:
                    case BuffID.SharkPup:
                    case BuffID.SkeletronPet:
                    case BuffID.SkeletronPrimePet:
                    case BuffID.SpookyWoodMount:
                    case BuffID.Squashling:
                    case BuffID.StormTiger:
                    case BuffID.SugarGlider:
                    case BuffID.TwinsPet:
                    case BuffID.UpbeatStar:
                    case BuffID.VampireFrog:
                    case BuffID.VoltBunny:
                    case BuffID.WallOfFleshGoatMount:
                    case BuffID.WellFed2:
                    case BuffID.WellFed3:
                    case BuffID.WitchBroom:
                        continue;
                }
                if(i==ModContent.BuffType<Buffs.EnlightenedBuff>()|| i == ModContent.BuffType<Buffs.mewwowBuff>()||i==ModContent.BuffType<Buffs.zafiBuff>())
                    continue;
                #endregion

                #region badBuffs
                switch (i)
                {
                    case BuffID.NoBuilding:
                    case BuffID.Horrified:
                        continue;
                }
                #endregion
                player.buffImmune[i] = true;
            }

            player.GetDamage(DamageClass.Generic)+=1000000;
            
            player.pickSpeed = int.MaxValue;
            player.breath = player.breathMax;
            player.gills = true;
            player.waterWalk = true;
            player.moveSpeed= 3;
            player.blockRange = 1000000;
            player.maxRunSpeed = 10000;
            player.maxFallSpeed = 10000;
            //player.HeldItem.consumable = false;
        }
    }
}
