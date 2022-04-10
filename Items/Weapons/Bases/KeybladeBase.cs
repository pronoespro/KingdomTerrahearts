using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using KingdomTerrahearts.Extra;
using Terraria.ModLoader.IO;
using System;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using KingdomTerrahearts.Projectiles.Weapons;
using Terraria.Audio;
using System.IO;
using KingdomTerrahearts.Logic;

public enum keybladeBlockingType
{
	none,
	normal,
	reflect,
	reversal,
    special
}
namespace KingdomTerrahearts.Items.Weapons
{
    public abstract class KeybladeBase : ModItem
    {

		public enum keyType
		{
			light,
			fire,
			dark,
			jungle,
			digital,
			destiny,
			star,
			honey,
			ice
		}

		public enum KeyComboType
		{
			normal,
			magic,
			stabbing,
			dual
		}

		public enum keyMagic
        {
			fire,
			ice,
			lightning,
			water,
			wind,
			magnet,
			reflect,
			gravity,
			stop,
			slow,
			confuse,
			zeroGrav,
			sleep,
			poison,
			balloon
        }

		public enum keyTransformation
		{
			none,
			yoyo,
			flag,
			spear,
			skates,
			hammer,
			guns,
			cannon,
			swords,
			dual,
			staff,
			shield,
			drill,
			nanoArms,
			claws
		}

		public enum keyDriveForm
		{
			none,
			valor,
			wisdom,
			master,
			final,
			limit,
			anti,
			second,
			strike,
			element,
			guardian,
			blitz,
			rage,
			ultimate,
			dark,
			light,
			dual
		}

		public enum summonType
        {
			none,
			simba,
			ariel,
			bambi,
			mushu,
			genie,
			dualKeys,
			ultimateKeys,
			mewwow,
			Ralph,
			tinkerBell,
			dumbo,
			chickenLittle,
			stitch,
			peterPan,
			soraAndKairi,
			soraAndRiku
        }

		//Normal attack atributes
		public int targetDistance = 300;
		public int forwardMovement=5;
		public int initDamage;
		public int combo = 0;
        public int comboMax = 3;
		public int projectileTime = 10;
		public int lastUsedTime = 0;
		public int extraShootTimes = 0;
		public keyType keybladeElement=keyType.light;
		public KeyComboType keyComboType = KeyComboType.normal;
		public bool canShootAgain = true;
		public int manaConsumed = 0;
		public int[] animationTimes=new int[0];
		public float[] transformationsDamageMultiplier = new float[] { 1f, 1f, 1f, 1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f,1f };
		public int shootProjectile = -1;
		public float shootSpeed = 10;

		//Magic attack atributes
		public int magicCost=10;
		public keyMagic magic;

		//Drive/Keyblade transformations atributes
		public keyDriveForm[] formChanges=new keyDriveForm[] { keyDriveForm.anti,keyDriveForm.element,keyDriveForm.guardian,keyDriveForm.limit,keyDriveForm.master,keyDriveForm.second,keyDriveForm.ultimate,keyDriveForm.valor,keyDriveForm.wisdom,keyDriveForm.ultimate,keyDriveForm.second,keyDriveForm.limit};

		public keyTransformation[] keyTransformations=new keyTransformation []{keyTransformation.flag,keyTransformation.yoyo,keyTransformation.cannon,keyTransformation.drill,keyTransformation.dual,keyTransformation.guns,keyTransformation.hammer,keyTransformation.nanoArms,keyTransformation.shield,keyTransformation.skates,keyTransformation.staff,keyTransformation.swords};

		public string[] transSprites= { };

		public int[] transProj = { };
		public float[] transShootSpeed = { };
		public LegacySoundStyle[] transSounds = { SoundID.Item1 };

		public int curTransformation=-1;
		public int curForm = -1;
		public int weaponDisplayProjectile;
        public float weaponDisplayRotation;

		public int[] formchangeCutscenes = new int[] { 4,4,4,4,4,4,4,4,4,4,4,4,4 };

        //Tooltips
        public TooltipLine transTooltip;
		public TooltipLine levelTooltip;

		//Summon atributes
		public summonType keySummon=summonType.ultimateKeys;
		public bool curSummoning;

		//Blocking attributes
		public keybladeBlockingType guardType = keybladeBlockingType.normal;

		//Level atributes
		//0:Damage   1:knockback   2:Max combo   3:???   4:???   5:???
		public float[] keyAtributes=new float[5];
		public int keyLevel=0;

		public bool enlightened;
		public float damageMult = 1;

		Player wielder;

        public override string Texture => GetCurrentTexture();

		public string GetCurrentTexture()
        {
            if (keyTransformations!=null &&
				keyTransformations.Length>0 &&
				keyTransformations[Math.Clamp(curTransformation,0,keyTransformations.Length-1)]!=keyTransformation.none && curTransformation>=0)
			{
				return "KingdomTerrahearts/" + transSprites[Math.Clamp(curTransformation, 0, keyTransformations.Length - 1)];
			}
            else
			{
				return base.Texture;
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
			writer.Write(combo);
        }

        public override void NetReceive(BinaryReader reader)
        {
			combo=reader.ReadInt32();
        }

        public void SaveAtributes()
        {
			keyAtributes[0] = Item.damage;
			keyAtributes[1] = Item.knockBack;
			keyAtributes[2] = comboMax;
        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

        public override void SetDefaults()
        {
			Item.DamageType = ModContent.GetInstance<KeybladeDamage>();
			Item.shootSpeed = 50;
			base.SetDefaults();
        }

        public override bool AltFunctionUse(Player player)
        {
			return true;
        }

		public override bool CanUseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				Item.pick = 0;
				Item.noMelee = false;
				Item.channel = false;
				Item.noUseGraphic = false;

				if (player.altFunctionUse == 2)
				{
					if (lastUsedTime > 2)
					{
						CommandLogic.instance.MoveCommandCursor();
					}
					lastUsedTime = 0;
					return false;
				}
				else
				{

					wielder = player;


					if ((curTransformation == -1 || (keyTransformations.Length > 0 && keyTransformations[curTransformation] == keyTransformation.none)) && CommandLogic.instance.selectedCommand == 0)
					{

						for (int i = 0; i < Main.projectile.Length; ++i)
						{
							if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot && Main.projectile[i].timeLeft > projectileTime)
							{
								if (!canShootAgain)
								{
									return false;
								}
							}
						}
					}

					if (animationTimes.Length > 0 && keyTransformations.Length > 0)
					{
						Item.useTime = Item.useAnimation = animationTimes[(curTransformation + 1 < keyTransformations.Length) ? curTransformation + 1 : keyTransformations.Length - 1];
					}

					SoraPlayer sp = wielder.GetModPlayer<SoraPlayer>();

					Item.useTime = Item.useAnimation = animationTimes[curTransformation + 1];


					if (sp.isBlocking())
					{
						if (!sp.blockedAttack)
							return false; 
						
						BlockCounterattack(sp);
						sp.StopBlocking();
						return true;
					}
					else
					{
						switch (CommandLogic.instance.selectedCommand)
						{
							default:
							case 0:
								Item.autoReuse = true;
								return MeleeAttack(player);
							case 1:
								Item.autoReuse = true;
								player.GetModPlayer<SoraPlayer>().SetContactinvulnerability((int)(Item.useTime * 2f));
								return MagicAttack();
							case 2:
								Item.autoReuse = false;
								return TransformKey();
							case 3:
								Item.autoReuse = false;
								return SummonAttack();
						}
					}
				}

			}
			return base.CanUseItem(player);
		}

		public void AttackDisplay(bool show)
		{
			if (show)
			{
				Item.noMelee = true;
				if (weaponDisplayProjectile >= 0 && Main.projectile[weaponDisplayProjectile].active && Main.projectile[weaponDisplayProjectile].type == ModContent.ProjectileType<KeybladeHoldDisplay>())
				{

					KeybladeHoldDisplay display = (KeybladeHoldDisplay)Main.projectile[weaponDisplayProjectile].ModProjectile;
					if (display != null)
					{
						display.Attack(Item.useTime+5, Main.player[Item.playerIndexTheItemIsReservedFor], keyTransformations[curTransformation]);

						display.SetComboAttack(combo);
						Main.projectile[weaponDisplayProjectile].damage = Item.damage;
						Main.projectile[weaponDisplayProjectile].knockBack = Item.knockBack;
					}
				}
				else
				{
					EntitySource_ItemUse s = new EntitySource_ItemUse(Main.player[Item.playerIndexTheItemIsReservedFor], Item);

					weaponDisplayProjectile = Projectile.NewProjectile(s, Main.player[Item.playerIndexTheItemIsReservedFor].position, Vector2.Zero, ModContent.ProjectileType<KeybladeHoldDisplay>(), 0, 0, Item.playerIndexTheItemIsReservedFor);

					KeybladeHoldDisplay display = (KeybladeHoldDisplay)Main.projectile[weaponDisplayProjectile].ModProjectile;
					if (display != null)
					{
						display.Attack(Item.useTime+5, Main.player[Item.playerIndexTheItemIsReservedFor], keyTransformations[curTransformation]);
					}
				}

				Main.projectile[weaponDisplayProjectile].spriteDirection = Main.player[Item.playerIndexTheItemIsReservedFor].direction;
				Main.projectile[weaponDisplayProjectile].scale = Item.scale;
				Main.projectile[weaponDisplayProjectile].damage = Item.damage;
			}
			else
			{
				if (Main.projectile[weaponDisplayProjectile].timeLeft > 1)
				{
					Main.projectile[weaponDisplayProjectile].timeLeft = 1;
				}
			}
		}

		public bool BlockCounterattack(SoraPlayer sp)
        {

			int projType = ModContent.ProjectileType<Projectiles.ScepTend.Vergil_Bubble>();
			Item.mana = 0;

			EntitySource_ItemUse source = new EntitySource_ItemUse(wielder, Item);

			switch (sp.guardType)
            {
				case keybladeBlockingType.normal:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Swing;
					GetCloserToEnemy(sp.Player);
					break;
				case keybladeBlockingType.reflect:
					int guardProj=Projectile.NewProjectile(source, sp.Player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.guardExpandProjectile>(), Item.damage*2, Item.knockBack*10);
					Main.projectile[guardProj].owner = Item.playerIndexTheItemIsReservedFor;

					Item.useStyle = ItemUseStyleID.Swing;
					Item.noMelee = true;
					sp.AddInvulnerability(15);
					break;
				case keybladeBlockingType.reversal:
					Projectile.NewProjectile(source,sp.Player.Center, Vector2.Zero, projType, Item.damage, Item.knockBack);
					break;
            }
			return true;
        }

		public bool MeleeAttack(Player player)
		{
			Item.UseSound = (transSounds.Length > 0) ? 
				transSounds[Math.Clamp(curTransformation, 0, transSounds.Length-1)] 
				: SoundID.Item1;

			Item.shoot = ProjectileID.None;
			if (curTransformation >= 0 && keyTransformations[curTransformation] != keyTransformation.none && keyTransformations[curTransformation] != keyTransformation.yoyo)
			{
				Item.noUseGraphic = true;
				AttackDisplay(true);
			}
			else
			{
				Item.noUseGraphic = false;
				AttackDisplay(false);
			}

			Item.mana = 0;

			if (curTransformation == -1 || (keyTransformations.Length>0 && keyTransformations[curTransformation]==keyTransformation.none))
			{
				GetCloserToEnemy(player);
				switch (combo)
				{
					case 0:
						Item.shoot = ProjectileID.None;
						Item.useStyle = ItemUseStyleID.Swing;
						break;
					case 1:
						Item.shoot = ProjectileID.None;
						Item.useStyle = ItemUseStyleID.Thrust;
						break;
					case 2:
						ShootMagicProjectile();
						break;
					case 3:
						Item.shoot = ProjectileID.None;
						Item.useStyle = ItemUseStyleID.Thrust;
						break;
					default:
					case 4:
					case 5:
						ComboPlus(combo);
						break;
				}

				combo++;
				if (combo >= comboMax)
				{
					combo = 0;
				}

			}
            else
            {

				Item.reuseDelay = 0;

				TransformedKeyAttacks(player);

				combo++;
				if (combo >= comboMax)
				{
					combo = 0;
				}

			}


			return true;
		}

		public void GetCloserToEnemy(Player player)
		{
			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
			int closest = sp.GetClosestEnemy(targetDistance);
			if (closest != -1 && Vector2.Distance(Main.npc[closest].Center, player.Center) > forwardMovement + (Main.npc[closest].width / 2 + Main.npc[closest].height / 2) / 2)
			{
				player.velocity = MathHelp.Normalize(Main.npc[closest].Center - player.Center) * forwardMovement;
				player.direction = (int)MathHelp.Sign((Main.npc[closest].Center - player.Center).X);
				sp.SetContactinvulnerability(Item.useTime / 5);
			}
		}

		void ComboPlus(int comboMoment)
		{
			switch (keyComboType)
			{
				default:
				case KeyComboType.normal:
					switch (comboMoment)
					{
						case 4:
							Item.shoot = ProjectileID.None;
							Item.useStyle = ItemUseStyleID.Swing;
							break;
						default:
						case 5:
							Item.shoot = ProjectileID.None;
							Item.useStyle = 100;
							Item.channel = true;
							break;
					}
					break;
				case KeyComboType.magic:
					switch (comboMoment)
					{
						case 4:
							ShootMagicProjectile();
							break;
						default:
						case 5:
							Item.shoot = ProjectileID.None;
							Item.useStyle = ItemUseStyleID.Thrust;
							break;
					}
					break;
				case KeyComboType.dual:
					switch (comboMoment)
					{
						case 4:
							ShootMagicProjectile();
							break;
						default:
						case 5:
							Item.shoot = ProjectileID.None;
							Item.useStyle = ItemUseStyleID.Thrust;
							break;
					}
					break;
				case KeyComboType.stabbing:

					switch (comboMoment)
					{
						case 4:
							Item.shoot = ProjectileID.None;
							Item.useStyle = ItemUseStyleID.Thrust;
							break;
						default:
						case 5:
							Item.shoot = ProjectileID.None;
							Item.useStyle = ItemUseStyleID.Thrust;
							Item.channel = true;
							break;
					}
					break;
			}

		}

		void TransformedKeyAttacks(Player player)
		{
			switch (keyTransformations[curTransformation])
			{
				case keyTransformation.cannon:
					TransformedShoot(ProjectileID.CannonballFriendly, 10);
					Item.useStyle = ItemUseStyleID.Rapier;
					break;
				case keyTransformation.guns:
					TransformedShoot(ProjectileID.JestersArrow, 30);
					Item.useStyle = ItemUseStyleID.Rapier;
					break;
				case keyTransformation.staff:
					TransformedShoot(ProjectileID.MagicMissile, 20);
					Item.useStyle = ItemUseStyleID.RaiseLamp;
					break;
				case keyTransformation.yoyo:
					Item.noMelee = true;
					TransformedShoot(ProjectileID.RedsYoyo,Item.shootSpeed);
					Item.channel = true;
					Item.noUseGraphic = true;
					break;
				case keyTransformation.swords:
					GetCloserToEnemy(player);
					UltimateSwordsCombo();
					break;
				case keyTransformation.skates:
					GetCloserToEnemy(player);
					break;
				case keyTransformation.shield:
					Item.channel = true;
					TransformedShoot(ModContent.ProjectileType<shieldGuardProjectile>(),Item.shootSpeed);
					GetCloserToEnemy(player);
					Item.useStyle = ItemUseStyleID.Swing;
					break;
				case keyTransformation.nanoArms:
					GetCloserToEnemy(player);
					break;
				case keyTransformation.hammer:
					GetCloserToEnemy(player);
					Item.useStyle = ItemUseStyleID.Swing;
					break;
				case keyTransformation.flag:
					FlagCombo();
					GetCloserToEnemy(player);
					Item.useStyle = ItemUseStyleID.Rapier;
					break;
				case keyTransformation.dual:
					GetCloserToEnemy(player);
					DualSwordsCombo();
					break;
				case keyTransformation.drill:
					GetCloserToEnemy(player);
					Item.pick = 100;
					Item.useStyle = ItemUseStyleID.Rapier;
					break;
			}
		}

		#region Transformed combos

		void TransformedShoot(int defaultProj,float defaultSpeed)
		{
			if (transProj.Length > curTransformation && transProj[curTransformation] > 0)
			{
				Item.shoot = transProj[curTransformation];
			}
			else
			{
				Item.shoot = defaultProj;
			}
			if(transShootSpeed.Length>curTransformation && transShootSpeed[curTransformation] >= 0)
            {
				Item.shootSpeed = transShootSpeed[curTransformation];
            }
            else
			{
				Item.shootSpeed = defaultSpeed;
			}
		}

        void UltimateSwordsCombo()
        {
            switch (combo)
			{
				case 0:
					Item.shoot = ProjectileID.None;
                    Item.useStyle = ItemUseStyleID.Thrust;
					SummonUltimateBladesSlice();
                    break;
				case 1:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Thrust;
					SummonUltimateBladesSlice();
					break;
				case 2:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Thrust;
					SummonUltimateBladesStab();
					break;
				case 3:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Thrust;
					SummonUltimateBladesSlice();
					break;
				case 4:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Thrust;
					SummonUltimateBladesSlice();
					break;
				case 5:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Thrust;
					SummonUltimateBladesStab();
					break;
            }
        }

		void DualSwordsCombo()
		{
			switch (combo)
			{
				case 0:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Swing;
					break;
				case 1:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Thrust;
					break;
				case 2:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Swing;
					break;
				case 3:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Swing;
					break;
				case 4:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Thrust;
					break;
				case 5:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Swing;
					break;
			}
		}

		public void FlagCombo()
		{
			switch (combo)
			{
				case 0:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Swing;
					break;
				case 1:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Thrust;
					break;
				case 2:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Swing;
					break;
				case 3:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Swing;
					break;
				case 4:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Thrust;
					break;
				case 5:
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Swing;
					break;
			}
		}

        public void SummonUltimateBladesStab()
		{
			for(int i = 0; i < SoraPlayer.summonProjectiles.Length; i++)
            {
				SoraPlayer.summonProjectiles[i].timeLeft = 0;
            }
			SoraPlayer.summonProjectiles = new Projectile[6];

			EntitySource_ItemUse s = new EntitySource_ItemUse(wielder, Item);
			for (int i = 0; i < 6; i++)
			{
				SoraPlayer.summonProjectiles[i] = Main.projectile[Projectile.NewProjectile(s,wielder.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.ultimate_projectile_stab>(), (int)(Item.damage/2f), 3, Item.playerIndexTheItemIsReservedFor, i*10f,Item.useAnimation)];
			}
		}

		public void SummonUltimateBladesSlice()
		{
			for (int i = 0; i < SoraPlayer.summonProjectiles.Length; i++)
			{
				SoraPlayer.summonProjectiles[i].timeLeft = 0;
			}
			SoraPlayer.summonProjectiles = new Projectile[6];

			EntitySource_ItemUse s = new EntitySource_ItemUse(wielder, Item);
			for (int i = 0; i < 6; i++)
			{
				SoraPlayer.summonProjectiles[i] = Main.projectile[Projectile.NewProjectile(s, wielder.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.ultimate_projectile_slice>(), (int)(Item.damage), 3, Item.playerIndexTheItemIsReservedFor, Main.rand.NextFloat(-2f*(float)Math.PI,2f* (float)Math.PI), Item.useAnimation)];
			}
		}

		#endregion

		public void ChangeForm()
        {
			if (curForm == -1)
			{
				return;
			}
			SoraPlayer sp = wielder.GetModPlayer<SoraPlayer>();

            switch (formChanges[curForm])
            {
				case keyDriveForm.anti:
					wielder.yoraiz0rDarkness = true;
					break;
				case keyDriveForm.blitz:
					break;
				case keyDriveForm.element:
					break;
				case keyDriveForm.final:
					break;
				case keyDriveForm.guardian:
					break;
				case keyDriveForm.limit:
					sp.curCostume = 2;
					break;
				case keyDriveForm.master:
					break;
				case keyDriveForm.none:
					break;
				case keyDriveForm.rage:
					break;
				case keyDriveForm.second:
					sp.curCostume = 2;
					break;
				case keyDriveForm.strike:
					break;
				case keyDriveForm.ultimate:
					break;
				case keyDriveForm.valor:
					break;
				case keyDriveForm.wisdom:
					break;
				case keyDriveForm.dark:
					break;
				case keyDriveForm.light:
					break;
				case keyDriveForm.dual:
					break;
			}
		}

		public bool MagicAttack()
		{

			Item.useStyle = ItemUseStyleID.Thrust;

			Item.mana = magicCost;
            if (Main.player[Item.playerIndexTheItemIsReservedFor].statMana < magicCost)
			{
				CommandLogic.instance.ChangeCommand(0);
			}

            switch (magic)
            {
				case keyMagic.fire:
					Item.shoot = ProjectileID.Fireball;
					Item.shootSpeed = 15;
					break;
				case keyMagic.ice:
					Item.shoot = ProjectileID.IceBolt;
					Item.shootSpeed = 15;
					break;
				case keyMagic.lightning:
					Item.useStyle = 100;
					Item.channel = true;
					Item.shoot = ModContent.ProjectileType<Projectiles.Magic.Lightning_Spell>();
					break;
				case keyMagic.water:
					Item.shoot = ProjectileID.WaterBolt;
					Item.shootSpeed = 15;
					break;
				case keyMagic.wind:
					Item.shootSpeed = 15;
					break;
				case keyMagic.magnet:
					Item.shootSpeed = 0;
					Item.shoot = ModContent.ProjectileType<Projectiles.Magic.magnet>();
					return !KingdomTerrahearts.instance.AnyProjectile(ModContent.ProjectileType<Projectiles.Magic.magnet>());
				case keyMagic.reflect:
					Item.shootSpeed = 0;
					wielder.GetModPlayer<SoraPlayer>().AddInvulnerability(1);
					Item.shoot = ModContent.ProjectileType<Projectiles.reflectProjectile>();
					Item.useStyle = 100;
					break;
				case keyMagic.gravity:
					Item.useStyle = 100;
					Item.channel = true;
					break;
				case keyMagic.stop:
					Item.useStyle = 100;
					Item.shootSpeed = 0;
					Item.shoot = ModContent.ProjectileType<Projectiles.Magic.stop>();
					break;
				case keyMagic.slow:
					Item.useStyle = 100;
					Item.channel = true;
					break;
				case keyMagic.confuse:
					Item.shoot = ProjectileID.NanoBullet;
					Item.shootSpeed = 5;
					break;
				case keyMagic.zeroGrav:
					Item.useStyle = 100;
					Item.channel = true;
					break;
				case keyMagic.sleep:
					Item.useStyle = 100;
					Item.channel = true;
					break;
				case keyMagic.poison:
					Item.shoot = ProjectileID.PoisonedKnife;
					Item.shootSpeed = 15;
					break;
				case keyMagic.balloon:
					Item.shoot = ProjectileID.BeachBall;
					Item.shootSpeed = 15;
					break;

			}

			return true;
        }

        public bool TransformKey()
		{

			if (keyTransformations!=null && keyTransformations.Length > 0)
			{

				if (lastUsedTime > 2)
				{

					curTransformation = (++curTransformation >= keyTransformations.Length) ? -1 : curTransformation;
					curForm = (++curForm >= formChanges.Length) ? -1 : curForm;

					for (int i = 0; i < 20; i++)
					{
						Rectangle rect = wielder.getRect();
						int newDust = Dust.NewDust(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, DustID.SilverCoin);
						Main.dust[newDust].color = Color.White;
						Main.dust[newDust].noGravity = true;
					}

					SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/keybladeTransform"));

					if (curForm >= 0 && curForm < formchangeCutscenes.Length && formchangeCutscenes[curForm] >= 0)
					{
						CutsceneLogic.instance.ChangeCutscene(formchangeCutscenes[curForm], initDamage);
					}
				}

				lastUsedTime = 0;
				if (animationTimes.Length > 0)
				{
					Item.useTime = Item.useAnimation = animationTimes[0];
				}


			}

			CommandLogic.instance.UseReaction();
			CommandLogic.instance.ChangeCommand(0);

			return false;
        }

		public bool SummonAttack()
		{
			if (lastUsedTime > Item.useTime/2 || !curSummoning)
			{
				if (!curSummoning)
				{
					if (wielder.statMana >= wielder.statManaMax)
					{
						wielder.statMana = 0;
						CommandLogic.instance.ChangeCommand(0);

						EntitySource_ItemUse s = new EntitySource_ItemUse(wielder, Item);

						switch (keySummon)
                        {
							default:
							case summonType.ultimateKeys:
							case summonType.dualKeys:
								SummonUltimateBladesSlice();
								break;
							case summonType.mushu:
								SoraPlayer.summonProjectiles = new Projectile[1];
								SoraPlayer.summonProjectiles[0] = Main.projectile[Projectile.NewProjectile(s,wielder.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Summons.Mushu>(), Item.damage, Item.knockBack,wielder.whoAmI)];
								break;
							case summonType.bambi:
								SoraPlayer.summonProjectiles = new Projectile[1];
								SoraPlayer.summonProjectiles[0] = Main.projectile[Projectile.NewProjectile(s,wielder.Center, Vector2.Zero, ModContent.ProjectileType < Projectiles.Summons.Bambi>(), 0, Item.knockBack, wielder.whoAmI)];
								break;
							case summonType.simba:
								SoraPlayer.summonProjectiles = new Projectile[1];
								SoraPlayer.summonProjectiles[0] = Main.projectile[Projectile.NewProjectile(s,wielder.Center, Vector2.Zero, ModContent.ProjectileType < Projectiles.Summons.Simba>(), Item.damage, Item.knockBack, wielder.whoAmI)];
								break;
							case summonType.dumbo:
								SoraPlayer.summonProjectiles = new Projectile[1];
								SoraPlayer.summonProjectiles[0] = Main.projectile[Projectile.NewProjectile(s,wielder.Center, Vector2.Zero, ModContent.ProjectileType < Projectiles.Summons.Dumbo>(), Item.damage, Item.knockBack, wielder.whoAmI)];
								break;
							case summonType.chickenLittle:
								SoraPlayer.summonProjectiles = new Projectile[1];
								SoraPlayer.summonProjectiles[0] = Main.projectile[Projectile.NewProjectile(s,wielder.Center, Vector2.Zero, ModContent.ProjectileType < Projectiles.Summons.ChickenLittle>(), Item.damage, Item.knockBack, wielder.whoAmI)];
								break;
						}
						curSummoning = true;

					}
                }
                else
                {
					for(int i = 0; i < SoraPlayer.summonProjectiles.Length; i++)
                    {
                        if (SoraPlayer.summonProjectiles[i].active)
                        {
							SoraPlayer.summonProjectiles[i].timeLeft = 0;
                        }
                    }
					SoraPlayer.summonProjectiles = new Projectile[0];
					curSummoning = false;
                }
			}
			lastUsedTime = 0;

			if (animationTimes.Length > 0)
			{
				Item.useTime=Item.useAnimation = animationTimes[0];
            }

			return false;
		}

		public override void SaveData(TagCompound tag)
		{
			tag["keyLevel"] = keyLevel;
		}

		public override void LoadData(TagCompound tag)
		{
			keyLevel = tag.GetInt("keyLevel");
		}

		public override void HoldItem(Player player)
		{

			if (player.whoAmI == Main.myPlayer)
			{
				SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
				sp.lastHeldKeyblade = 2;
				sp.guardType = guardType;

                if (sp.midCutscene)
                {
					Item.noUseGraphic = true;
					return;
                }

				if (player.itemAnimation <= 0)
				{
					Item.useStyle = ItemUseStyleID.Swing;
				}
			}

			Item.holdStyle = -1;

			if (curTransformation >= 0 && keyTransformations[curTransformation] != keyTransformation.none)
			{
				if (keyTransformations[curTransformation] == keyTransformation.yoyo)
				{
					Item.noUseGraphic = true;
					ChangeDisplay(false);
				}
				else
				{
					Item.noUseGraphic = true;
					ChangeDisplay(true);
				}
			}
			else
			{
				Item.noUseGraphic = false;
				ChangeDisplay(false);
			}

			if (curTransformation >= 0)
			{
				Vector2 mouseDir = MathHelp.Normalize(Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - player.Center);
				switch (keyTransformations[curTransformation])
				{
					case keyTransformation.guns:
					case keyTransformation.cannon:
					case keyTransformation.drill:
					case keyTransformation.shield:
						player.direction = (mouseDir.X >= 0) ? 1 : -1;
						break;
				}
			}
		}

		public void ChangeDisplay(bool show)
		{
			if (show)
			{
				if (weaponDisplayProjectile >= 0 && Main.projectile[weaponDisplayProjectile].active && Main.projectile[weaponDisplayProjectile].type == ModContent.ProjectileType<KeybladeHoldDisplay>())
				{
					Main.projectile[weaponDisplayProjectile].scale = Item.scale;

					KeybladeHoldDisplay display = (KeybladeHoldDisplay)Main.projectile[weaponDisplayProjectile].ModProjectile;
					if (display != null)
					{
						display.SetSprite(transSprites[Math.Clamp(curTransformation, 0, keyTransformations.Length)], Main.player[Item.playerIndexTheItemIsReservedFor], keyTransformations[Math.Clamp(curTransformation, 0, keyTransformations.Length)]);
					}
				}
				else
				{
					EntitySource_ItemUse s = new EntitySource_ItemUse(Main.player[Item.playerIndexTheItemIsReservedFor], Item);

					weaponDisplayProjectile = Projectile.NewProjectile(s, Main.player[Item.playerIndexTheItemIsReservedFor].position, Vector2.Zero, ModContent.ProjectileType<KeybladeHoldDisplay>(), 0, 0, Item.playerIndexTheItemIsReservedFor);
					Main.projectile[weaponDisplayProjectile].scale = Item.scale;
				}

				Main.projectile[weaponDisplayProjectile].spriteDirection = Item.direction;
			}
			else
			{
				if (Main.projectile[weaponDisplayProjectile].active
					 && Main.projectile[weaponDisplayProjectile].type == ModContent.ProjectileType<KeybladeHoldDisplay>() && Main.projectile[weaponDisplayProjectile].timeLeft > 1)
				{
					Main.projectile[weaponDisplayProjectile].timeLeft = 1;
				}
			}
		}


		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			if (whoAmI == Main.myPlayer)
			{
				if (curTransformation != -1 && keyTransformations[curTransformation] != keyTransformation.none)
				{
					Rectangle r = Item.getRect();
					switch (keyTransformations[curTransformation])
					{
						case keyTransformation.cannon:
							spriteBatch.Draw(ModContent.Request<Texture2D>("Items/KupoCoin").Value, Item.position, lightColor);
							break;
						case keyTransformation.dual:
							spriteBatch.Draw(ModContent.Request<Texture2D>("Items/Weapons/Keyblade_oath").Value, r, r, lightColor, 0, Item.position, SpriteEffects.None, 0);

							spriteBatch.Draw(ModContent.Request<Texture2D>("Items/Weapons/Keyblade_oblivion").Value, r, r, lightColor, (float)Math.PI / 2, Item.position, SpriteEffects.None, 15);
							break;
						case keyTransformation.yoyo:
							spriteBatch.Draw(ModContent.Request<Texture2D>("Items/Weapons/Keyblade_oblivion").Value, r, r, lightColor, (float)Math.PI / 2, Item.position, SpriteEffects.None, 15);
							break;
					}

					return false;
				}
			}
			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}

        public abstract void ChangeKeybladeValues();

		public override void HoldStyle(Player player, Rectangle heldItemFrame)
		{
			Vector2 position = GetLightPosition(player);
			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
			if (sp.guardTime > 0)
			{
				player.itemLocation.X = player.Center.X + 30f * player.direction;
				player.itemLocation.Y = player.position.Y + 23f + 23f * player.gravDir + player.mount.PlayerOffsetHitbox;
				player.itemRotation = (player.direction == 1) ? MathHelper.TwoPi - MathHelper.Pi / 2 : MathHelper.Pi / 2;

			}
			else
			{
				if (Math.Abs(position.Y - player.Center.Y) < 2)
				{
					player.itemLocation.X = player.Center.X - 25f * player.direction;
					player.itemLocation.Y = player.position.Y + 3f + 23f * player.gravDir + player.mount.PlayerOffsetHitbox;
					player.itemRotation = (player.direction == 1) ? MathHelper.Pi / 3 : MathHelper.TwoPi - MathHelper.Pi / 3;
				}
				else if (position.Y < player.Center.Y)
				{
					player.itemLocation.X = player.Center.X - 25f * player.direction;
					player.itemLocation.Y = player.position.Y + 10f + 23f * player.gravDir + player.mount.PlayerOffsetHitbox;
					player.itemRotation = (player.direction == 1) ? MathHelper.Pi / 4 : MathHelper.TwoPi - MathHelper.Pi / 4;
				}
				else
				{
					player.itemLocation.X = player.Center.X;
					player.itemLocation.Y = player.position.Y + 21f - 3f * player.gravDir + player.mount.PlayerOffsetHitbox;
					player.itemRotation = (player.direction == 1) ? MathHelper.TwoPi - MathHelper.Pi / 2 : MathHelper.Pi / 2;
				}
			}

		}

        public override void HoldItemFrame(Player player)
		{
			Vector2 position = GetLightPosition(player);
			if (curTransformation >= 0 && keyTransformations[curTransformation]!=keyTransformation.none)
			{
				Vector2 mouseDir = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - player.Center;
				float mouseUp = Vector2.Dot(MathHelp.Normalize(mouseDir), new Vector2(0, -1));
                switch (keyTransformations[curTransformation])
                {
					case keyTransformation.cannon:
					case keyTransformation.drill:
					case keyTransformation.guns:
					case keyTransformation.shield:

						if (mouseUp > 0.75f)
						{
							player.bodyFrame.Y = player.bodyFrame.Height;
						}else if (mouseUp>0)
						{
							player.bodyFrame.Y = player.bodyFrame.Height*2;
						}
						else if (mouseUp > -0.5f)
						{
							player.bodyFrame.Y = player.bodyFrame.Height*3;
						}
                        else
						{
							player.bodyFrame.Y = player.bodyFrame.Height*4;
						}

						break;
					case keyTransformation.staff:
					case keyTransformation.spear:

						player.bodyFrame.Y = player.bodyFrame.Height*3;

						break;
					case keyTransformation.swords:
					case keyTransformation.dual:
						player.bodyFrame.Y = player.bodyFrame.Height * 4;
						break;
					case keyTransformation.flag:
						player.bodyFrame.Y = player.bodyFrame.Height * 3;
						break;
                }
			}
			else
			{
				if (position.Y <= player.Center.Y)
				{
					player.bodyFrame.Y = 0;
				}
				else
				{
					player.bodyFrame.Y = player.bodyFrame.Height;
				}
			}
		}

        private Vector2 GetLightPosition(Player player)
		{
			Vector2 position = player.Center;
			position.Y += player.velocity.Y;
			return position;
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
			if (curTransformation == -1)
			{
				return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
			}
			else
			{
				spriteBatch.Draw(ModContent.Request < Texture2D > ("KingdomTerrahearts/"+transSprites[curTransformation]).Value, position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0);
				return false;
            }
        }

        public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				if (player.altFunctionUse == 2)
				{
					return false;
				}
				else
				{
					if ((player.whoAmI == Main.myPlayer && Main.netMode == NetmodeID.MultiplayerClient) || Main.netMode == NetmodeID.SinglePlayer)
					{
						lastUsedTime = 0;
						if (CommandLogic.instance.selectedCommand > 1)
						{
							CommandLogic.instance.ChangeCommand(0);
						}
						else if (Main.player[Item.playerIndexTheItemIsReservedFor].statMana < magicCost)
						{
							CommandLogic.instance.ChangeCommand(0);
						}
						return true;
					}
				}
			}
			return false;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.whoAmI == Main.myPlayer && lastUsedTime < Item.useTime)
			{
				if ((curTransformation == -1 || keyTransformations[curTransformation] == keyTransformation.none) && CommandLogic.instance.selectedCommand == 0)
				{
					Vector2 vel = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - player.Center;
					vel = MathHelp.Normalize(vel) * Item.shootSpeed;
					int proj = Projectile.NewProjectile(source, position, vel, type, damage, knockback, Item.playerIndexTheItemIsReservedFor);
					Main.projectile[proj].timeLeft = projectileTime;
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].hostile = false;
					Item.shoot = ProjectileID.None;
					return false;
				}
				if (curTransformation >= 0 && keyTransformations[curTransformation] == keyTransformation.cannon && CommandLogic.instance.selectedCommand == 0)
				{
					Vector2 vel = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - player.Center;
					vel = MathHelp.Normalize(vel) * Item.shootSpeed;
					int proj = Projectile.NewProjectile(source, position, vel, type, damage * 3, knockback * 3, Item.playerIndexTheItemIsReservedFor);
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].hostile = false;
					return false;
				}
				if (CommandLogic.instance.selectedCommand == 1)
				{
					Vector2 vel = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - player.Center;
					vel = MathHelp.Normalize(vel) * Item.shootSpeed;
					int proj = Projectile.NewProjectile(source, position, vel, type, damage * 3, knockback * 3, Item.playerIndexTheItemIsReservedFor);
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].hostile = false;
					return false;
				}
			}
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public void ShootMagicProjectile()
		{

			if(manaConsumed>0)
			{
				if (wielder.statMana >= manaConsumed)
				{
					Item.mana = 20;
					wielder.statMana -= manaConsumed;
				}
				else
				{
					Item.shoot = ProjectileID.None;
					Item.useStyle = ItemUseStyleID.Thrust;
					return;
				}
			}

			if (shootProjectile == -1)
			{
				switch (keybladeElement)
				{
					case keyType.fire:
						Item.shoot = ProjectileID.Fireball;
						Item.shootSpeed = 3;
						break;
					case keyType.dark:
						Item.shoot = ProjectileID.DemonScythe;
						Item.shootSpeed = 1;
						break;
					case keyType.light:
						Item.shoot = ProjectileID.MagicMissile;
						Item.shootSpeed = 4;
						break;
					case keyType.jungle:
						Item.shoot = ProjectileID.SporeCloud;
						Item.shootSpeed = 1;
						break;
					case keyType.digital:
						Item.shoot = ModContent.ProjectileType<Projectiles.tronDisk>();
						Item.shootSpeed = 7;
						break;
					case keyType.destiny:
						Item.shoot = ModContent.ProjectileType<Projectiles.teleportThrownKey>();
						Item.shootSpeed = 15;
						break;
					case keyType.star:
						Item.shoot = ProjectileID.FallingStar;
						Item.shootSpeed = 25;
						break;
					case keyType.honey:
						Item.shoot = ProjectileID.Bee;
						Item.shootSpeed = 15;
						break;
				}
            }
            else
			{
				Item.shoot = shootProjectile;
				Item.shootSpeed =shootSpeed;
			}
			Item.useStyle = ItemUseStyleID.Shoot;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			//Transformation tooltip
			if (transTooltip == null)
			{
				transTooltip = new TooltipLine(Mod, "Transformations", GetTransformationsText());
			}
			transTooltip.OverrideColor = Color.LightBlue;
			if (!tooltips.Contains(transTooltip))
			{
				tooltips.Add(transTooltip);
			}
            //Level up tooltip
            if (levelTooltip == null)
            {
				levelTooltip = new TooltipLine(Mod, "Level", "Level of keyblade: "+keyLevel);
			}
			levelTooltip.OverrideColor = Color.Yellow;
			if (!tooltips.Contains(levelTooltip))
			{
				tooltips.Add(levelTooltip);
			}
		}

		string GetTransformationsText()
        {
			string text="";

			for(int i = 0; i < keyTransformations.Length; i++)
            {
				if(i>0 && keyTransformations[0] != keyTransformation.none)
                {
					text += ", ";
                }
                switch (keyTransformations[i])
                {
					case keyTransformation.none:
						break;
					case keyTransformation.cannon:
						text += "cannon";
						break;
					case keyTransformation.claws:
						text += "claws";
						break;
					case keyTransformation.drill:
						text += "drill";
						break;
					case keyTransformation.dual:
						text += "dual";
						break;
					case keyTransformation.flag:
						text += "flag";
						break;
					case keyTransformation.guns:
						text+="guns";
						break;
					case keyTransformation.hammer:
						text += "hammer";
						break;
					case keyTransformation.nanoArms:
						text += "nano arms";
						break;
					case keyTransformation.shield:
						text += "shield";
						break;
					case keyTransformation.skates:
						text += "skates";
						break;
					case keyTransformation.spear:
						text += "spear";
						break;
					case keyTransformation.staff:
						text += "staff";
						break;
					case keyTransformation.swords:
						text += "ultimate swords";
						break;
					case keyTransformation.yoyo:
						text += "yoyos";
						break;
				}
            }

			return (text.Length == 0 ? "This key has no transformations" :"Key transformations: "+text);
        }

		void CheckSummonProjectiles()
		{
			for (int i = 0; i < SoraPlayer.summonProjectiles.Length; i++)
			{
				if (SoraPlayer.summonProjectiles[i].active)
				{
					curSummoning = true;
					return;
				}
			}

			SoraPlayer.summonProjectiles = new Projectile[0];
			curSummoning = false;

		}

        public override bool AllowPrefix(int pre)
        {
			return pre==0;
        }

        public override void UpdateInventory(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{

				ChangeKeybladeValues();

				if (transTooltip == null)
				{
					transTooltip = new TooltipLine(Mod, "Transformations", GetTransformationsText());
				}
				transTooltip.Text = GetTransformationsText();

				if (levelTooltip == null)
				{
					levelTooltip = new TooltipLine(Mod, "Level", "Level of keyblade: " + keyLevel);
				}
				levelTooltip.Text = "Level of keyblade: " + keyLevel;

				Item.prefix = 0;

				if (SoraPlayer.summonProjectiles.Length > 0)
				{
					CheckSummonProjectiles();
				}

				SoraPlayer sora = player.GetModPlayer<SoraPlayer>();

				if (sora.lastHeldItem == Item)
				{
					if (player.HeldItem == Item)
					{
						sora.usingForm = (curForm >= 0);
						ChangeForm();
					}
					else
					{
						sora.usingForm = false;
					}
                }
                else
                {
                    if (player.HeldItem == Item)
                    {
						sora.StopBlocking();
                    }
                }

				enlightened = player.HasBuff(ModContent.BuffType<Buffs.EnlightenedBuff>());

				Item.damage = Math.Max(1, (int)(Item.damage / damageMult));

				damageMult = 1;
				damageMult += (enlightened) ? 0.5f : 0;

				Item.damage = (int)(Item.damage * damageMult);
				Item.color = (enlightened) ? Color.Blue : Color.White;

				if ((player.selectedItem >= 0 && player.selectedItem < player.inventory.Length
					&& player.inventory[player.selectedItem] == Item)
					|| player.HeldItem == Item)
				{
					lastUsedTime++;
					if (lastUsedTime > Item.useTime * 1.5f && combo > 0)
					{
						combo = 0;
						lastUsedTime = 0;
					}

				}
				else
				{
					combo = 0;
					lastUsedTime = 0;
				}
			}
		}

		public void LevelUp()
        {
			Item.damage = (int)keyAtributes[0];
			Item.knockBack = (int)keyAtributes[1];
			for(int i = 0; i < keyLevel; i++)
            {
				Item.damage += (int)(keyAtributes[0] * 2);
			}
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
			player.GetModPlayer<SoraPlayer>().SetContactinvulnerability((int)(Item.useTime*3f));
            base.OnHitNPC(player, target, damage, knockBack, crit);
        }

    }
}
