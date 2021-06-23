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

public enum blockingType
{
	none,
	normal,
	reflect,
	reversal
}
namespace KingdomTerrahearts.Items.Weapons
{
    public abstract class Keyblade : ModItem
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
			honey
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
			skates,
			hammer,
			guns,
			cannon,
			swords,
			dual,
			staff,
			shield,
			drill,
			nanoArms
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

		//Magic attack atributes
		public int magicCost=10;
		public keyMagic magic;

		//Drive/Keyblade transformations atributes
		public keyDriveForm[] formChanges=new keyDriveForm[] { keyDriveForm.anti,keyDriveForm.element,keyDriveForm.guardian,keyDriveForm.limit,keyDriveForm.master,keyDriveForm.second,keyDriveForm.ultimate,keyDriveForm.valor,keyDriveForm.wisdom,keyDriveForm.ultimate,keyDriveForm.second,keyDriveForm.limit};

		public keyTransformation[] keyTransformations=new keyTransformation []{keyTransformation.flag,keyTransformation.yoyo,keyTransformation.cannon,keyTransformation.drill,keyTransformation.dual,keyTransformation.guns,keyTransformation.hammer,keyTransformation.nanoArms,keyTransformation.shield,keyTransformation.skates,keyTransformation.staff,keyTransformation.swords};

		public string[] transSprites= { };

		public int curTransformation=-1;
		public int curForm = -1;
		public Color curFormColor = Color.White;

		//Tooltips
		public TooltipLine transTooltip;
		public TooltipLine levelTooltip;

		//Summon atributes
		public summonType keySummon=summonType.ultimateKeys;
		public bool curSummoning;

		//Blocking attributes
		public blockingType guardType=blockingType.normal;

		//Level atributes
		//0:Damage   1:knockback   2:Max combo   3:???   4:???   5:???
		public float[] keyAtributes=new float[5];
		public int keyLevel=0;

		public bool enlightened;
		public float damageMult = 1;

		Player wielder;

		public void SaveAtributes()
        {
			keyAtributes[0] = item.damage;
			keyAtributes[1] = item.knockBack;
			keyAtributes[2] = comboMax;
        }

        public override bool AltFunctionUse(Player player)
        {
			return true;
        }

        public override bool CanUseItem(Player player)
		{
			item.pick = 0;

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

				if ((curTransformation == -1 || (keyTransformations.Length>0 && keyTransformations[curTransformation] == keyTransformation.none)) && CommandLogic.instance.selectedCommand==0)
				{
					for (int i = 0; i < Main.projectile.Length; ++i)
					{
						if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot && Main.projectile[i].timeLeft > projectileTime)
						{
							if (!canShootAgain)
								return false;
							Main.projectile[i].timeLeft = projectileTime;
						}
					}
				}

				if (animationTimes.Length > 0 && keyTransformations.Length>0)
				{
					item.useTime =item.useAnimation= animationTimes[(curTransformation+1<keyTransformations.Length)?curTransformation+1:keyTransformations.Length-1];
				}

				SoraPlayer sp = wielder.GetModPlayer<SoraPlayer>();

				if (sp.isBlocking())
				{
					if (!sp.blockedAttack)
						return false;
					sp.StopBlocking();
					return BlockCounterattack(sp);
				}
				else
				{
					switch (CommandLogic.instance.selectedCommand)
					{
						case 0:
							return MeleeAttack();
						case 1:
							return MagicAttack();
						case 2:
							return TransformKey();
						case 3:
							return SummonAttack();
						default:
							return MeleeAttack();
					}
				}
			}

		}

		public bool BlockCounterattack(SoraPlayer sp)
        {
			if (!sp.blockedAttack) return false;
			int projType = mod.ProjectileType("Vergil_Bubble");
			item.mana = 0;

			switch (sp.guardType)
            {
				case blockingType.normal:
					item.shoot = ProjectileID.None;
					item.useStyle = ItemUseStyleID.SwingThrow;
					break;
				case blockingType.reflect:
					item.shoot = projType;
					break;
				case blockingType.reversal:
					Projectile.NewProjectile(sp.player.Center, Vector2.Zero, projType, item.damage, item.knockBack);
					break;
            }
			return true;
        }

		public bool MeleeAttack()
		{

			item.mana = 0;

			if (curTransformation == -1 || (keyTransformations.Length>0 && keyTransformations[curTransformation]==keyTransformation.none))
			{
				switch (combo)
				{
					case 0:
						item.shoot = ProjectileID.None;
						item.useStyle = ItemUseStyleID.SwingThrow;
						break;
					case 1:
						item.shoot = ProjectileID.None;
						item.useStyle = ItemUseStyleID.Stabbing;
						break;
					case 2:
						ShootmagicProjectile();
						break;
					case 3:
						item.shoot = ProjectileID.None;
						item.useStyle = ItemUseStyleID.Stabbing;
						break;
					case 4:
						ComboPlus(combo);
						break;
					case 5:
						ComboPlus(combo);
						break;
					default:
						item.shoot = ProjectileID.None;
						item.useStyle = ItemUseStyleID.SwingThrow;
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

				TransformedKeyAttacks();

				combo++;
				if (combo >= comboMax)
				{
					combo = 0;
				}

			}


			return true;
		}

		void TransformedKeyAttacks()
		{
			switch (keyTransformations[curTransformation])
			{
				case keyTransformation.cannon:
					item.shoot = ProjectileID.CannonballFriendly;
					item.shootSpeed = 10;
					item.useStyle = ItemUseStyleID.HoldingOut;
					break;
				case keyTransformation.guns:
					item.shoot = ProjectileID.JestersArrow;
					item.shootSpeed = 50;
					item.useStyle = ItemUseStyleID.HoldingOut;
					break;
				case keyTransformation.staff:
					item.shoot =ProjectileID.MagicMissile;
					item.shootSpeed = 25;
					item.useStyle = ItemUseStyleID.HoldingOut;
					break;
				case keyTransformation.yoyo:
					item.useStyle = ItemUseStyleID.HoldingOut;
					break;
				case keyTransformation.swords:
					UltimateSwordsCombo();
					break;
				case keyTransformation.skates:
					break;
				case keyTransformation.shield:
					item.useStyle = ItemUseStyleID.SwingThrow;
					break;
				case keyTransformation.nanoArms:
					break;
				case keyTransformation.hammer:
					item.useStyle = ItemUseStyleID.SwingThrow;
					break;
				case keyTransformation.flag:
					item.useStyle = ItemUseStyleID.HoldingOut;
					break;
				case keyTransformation.dual:
					DualSwordsCombo();
					break;
				case keyTransformation.drill:
					item.pick = 100;
					item.useStyle = ItemUseStyleID.HoldingOut;
					break;
			}
		}

        #region Transformed combos
        void UltimateSwordsCombo()
        {
            switch (combo)
			{
				case 0:
					item.shoot = ProjectileID.None;
					item.useStyle = ItemUseStyleID.Stabbing;
					break;
				case 1:
					item.shoot = ProjectileID.None;
					item.useStyle = ItemUseStyleID.Stabbing;
					break;
				case 2:
					ShootmagicProjectile();
					break;
				case 3:
					item.shoot = ProjectileID.SwordBeam;
					item.useStyle = ItemUseStyleID.Stabbing;
					break;
				case 4:
					item.shoot = ProjectileID.None;
					item.useStyle = ItemUseStyleID.Stabbing;
					break;
				case 5:
					item.shoot = ProjectileID.None;
					item.useStyle = ItemUseStyleID.Stabbing;
					break;
            }
            if (combo >= comboMax - 2)
			{
				item.useStyle = 100;
				item.channel = true;
				SummonUltimateBlades();
			}
        }

		void DualSwordsCombo()
		{
			switch (combo)
			{
				case 0:
					item.shoot = ProjectileID.None;
					item.useStyle = ItemUseStyleID.SwingThrow;
					break;
				case 1:
					item.shoot = ProjectileID.None;
					item.useStyle = ItemUseStyleID.Stabbing;
					break;
				case 2:
					ShootmagicProjectile();
					break;
				case 3:
					item.shoot = mod.ProjectileType("roxasLightBeam");
					item.shootSpeed = 15;
					item.useStyle = ItemUseStyleID.SwingThrow;
					break;
				case 4:
					item.shoot = ProjectileID.None;
					item.useStyle = ItemUseStyleID.Stabbing;
					break;
				case 5:
					item.shoot = ProjectileID.None;
					item.useStyle = ItemUseStyleID.SwingThrow;
					break;
			}
		}

        #endregion

        public void SummonUltimateBlades()
		{
			for(int i = 0; i < SoraPlayer.summonProjectiles.Length; i++)
            {
				SoraPlayer.summonProjectiles[i].timeLeft = 0;
            }
			SoraPlayer.summonProjectiles = new Projectile[6];
			Vector2 vel;
			for (int i = 0; i < 6; i++)
			{
				Vector2 posOffset = new Vector2(Math.Abs(i - 3) * 5f, 20);
				vel = new Vector2(wielder.direction * 25, -15);
				SoraPlayer.summonProjectiles[i] = Main.projectile[Projectile.NewProjectile(wielder.Center + posOffset, vel, mod.ProjectileType("ultimate_projectile"), (int)(item.damage * 1.5f), 3, Owner: wielder.whoAmI, i / 2f)];
			}
			for (int i = 0; i < 6; i++)
			{
				Vector2 posOffset = new Vector2(Math.Abs(i - 3) * 5f, 20);
				vel = new Vector2(wielder.direction * 25, 15);
				SoraPlayer.summonProjectiles[i] = Main.projectile[Projectile.NewProjectile(wielder.Center + posOffset, vel, mod.ProjectileType("ultimate_projectile"), (int)(item.damage * 1.5f), 3, Owner: wielder.whoAmI,2+i/2f)];
			}
		}

		public void ChangeForm()
        {
			if (curForm == -1)
			{
				return;
			}

            switch (formChanges[curForm])
            {
				case keyDriveForm.anti:
					wielder.yoraiz0rDarkness = true;
					curFormColor = Color.Gray;
					break;
				case keyDriveForm.blitz:
					curFormColor =Color.LightGreen;
					break;
				case keyDriveForm.element:
					curFormColor =Color.Blue;
					break;
				case keyDriveForm.final:
					curFormColor =Color.White;
					break;
				case keyDriveForm.guardian:
					curFormColor =Color.Yellow;
					break;
				case keyDriveForm.limit:
					curFormColor = new Color(1, 0, 0, 0.5f);
					break;
				case keyDriveForm.master:
					curFormColor =Color.Yellow;
					break;
				case keyDriveForm.none:
					break;
				case keyDriveForm.rage:
					curFormColor = Color.DarkGray;
					break;
				case keyDriveForm.second:
					curFormColor =new Color(0,0,0,0.5f);
					break;
				case keyDriveForm.strike:
					curFormColor =Color.Red;
					break;
				case keyDriveForm.ultimate:
					curFormColor =Color.White;
					break;
				case keyDriveForm.valor:
					curFormColor =Color.Red;
					break;
				case keyDriveForm.wisdom:
					curFormColor = Color.Blue;
					break;
				case keyDriveForm.dark:
					curFormColor = Color.Gray;
					break;
				case keyDriveForm.light:
					curFormColor = Color.White;
					break;
				case keyDriveForm.dual:
					curFormColor = Color.White;
					break;
			}
		}

		public bool MagicAttack()
		{

			item.mana = magicCost;
            if (wielder.statMana <= item.mana)
			{
				CommandLogic.instance.ChangeCommand(0);
			}

            switch (magic)
            {
				case keyMagic.fire:
					item.shoot = ProjectileID.BallofFire;
					item.shootSpeed = 15;
					break;
				case keyMagic.ice:
					item.shoot = ProjectileID.IceBolt;
					item.shootSpeed = 15;
					break;
				case keyMagic.lightning:
					item.useStyle = 100;
					item.channel = true;
					item.shoot = mod.ProjectileType("Lightning_Spell");
					break;
				case keyMagic.water:
					item.shoot = ProjectileID.WaterBolt;
					item.shootSpeed = 15;
					break;
				case keyMagic.wind:
					item.shootSpeed = 15;
					break;
				case keyMagic.magnet:
					item.shootSpeed = 0;
					item.shoot = mod.ProjectileType("magnet");
					return !KingdomTerrahearts.instance.AnyProjectile(mod.ProjectileType("magnet"));
				case keyMagic.reflect:
					wielder.GetModPlayer<SoraPlayer>().AddInvulnerability(60);
					item.shootSpeed = 5;
					break;
				case keyMagic.gravity:
					item.useStyle = 100;
					item.channel = true;
					break;
				case keyMagic.stop:
					item.useStyle = 100;
					item.channel = true;
					break;
				case keyMagic.slow:
					item.useStyle = 100;
					item.channel = true;
					break;
				case keyMagic.confuse:
					item.shoot = ProjectileID.NanoBullet;
					item.shootSpeed = 5;
					break;
				case keyMagic.zeroGrav:
					item.useStyle = 100;
					item.channel = true;
					break;
				case keyMagic.sleep:
					item.useStyle = 100;
					item.channel = true;
					break;
				case keyMagic.poison:
					item.shoot = ProjectileID.PoisonedKnife;
					item.shootSpeed = 15;
					break;
				case keyMagic.balloon:
					item.shoot = ProjectileID.BeachBall;
					item.shootSpeed = 15;
					break;

			}

			return true;
        }

        public override TagCompound Save()
        {
			return new TagCompound
			{
				{"keyLevel",keyLevel }
			};
        }

        public override void Load(TagCompound tag)
        {
			keyLevel = tag.GetInt("keyLevel");
        }

        public bool TransformKey()
		{

			if (lastUsedTime > 2)
			{
				curTransformation = (++curTransformation >= keyTransformations.Length) ? -1 : curTransformation;
				curForm = (++curForm >= formChanges.Length) ? -1 : curForm;
				CommandLogic.instance.UseReaction();
				CommandLogic.instance.ChangeCommand(0);

				for (int i = 0; i < 20; i++)
				{
					Rectangle rect = wielder.getRect();
					int newDust = Dust.NewDust(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, DustID.SilverCoin);
					Main.dust[newDust].color = Color.White;
					Main.dust[newDust].noGravity = true;
				}
			}

			lastUsedTime = 0;

			if (animationTimes.Length > 0)
			{
				item.useTime = item.useAnimation = animationTimes[0];
			}

			return false;
        }

		public bool SummonAttack()
		{
			if (lastUsedTime > item.useTime/2 || !curSummoning)
			{
				if (!curSummoning)
				{
					if (wielder.statMana >= wielder.statManaMax)
					{
						wielder.statMana = 0;
						CommandLogic.instance.ChangeCommand(0);

						switch (keySummon)
                        {
							case summonType.ultimateKeys:
								SummonUltimateBlades();
								break;
							case summonType.dualKeys:
								SummonUltimateBlades();
								break;
							case summonType.mushu:
								SoraPlayer.summonProjectiles = new Projectile[1];
								SoraPlayer.summonProjectiles[0] = Main.projectile[Projectile.NewProjectile(wielder.Center, Vector2.Zero, mod.ProjectileType("Mushu"), item.damage, item.knockBack,item.owner)];
								break;
							case summonType.bambi:
								SoraPlayer.summonProjectiles = new Projectile[1];
								SoraPlayer.summonProjectiles[0] = Main.projectile[Projectile.NewProjectile(wielder.Center, Vector2.Zero, mod.ProjectileType("Bambi"), 0, item.knockBack, item.owner)];
								break;
							case summonType.simba:
								SoraPlayer.summonProjectiles = new Projectile[1];
								SoraPlayer.summonProjectiles[0] = Main.projectile[Projectile.NewProjectile(wielder.Center, Vector2.Zero, mod.ProjectileType("Simba"), item.damage, item.knockBack, item.owner)];
								break;
							case summonType.dumbo:
								SoraPlayer.summonProjectiles = new Projectile[1];
								SoraPlayer.summonProjectiles[0] = Main.projectile[Projectile.NewProjectile(wielder.Center, Vector2.Zero, mod.ProjectileType("Dumbo"), item.damage, item.knockBack, item.owner)];
								break;
							case summonType.chickenLittle:
								SoraPlayer.summonProjectiles = new Projectile[1];
								SoraPlayer.summonProjectiles[0] = Main.projectile[Projectile.NewProjectile(wielder.Center, Vector2.Zero, mod.ProjectileType("ChickenLittle"), item.damage, item.knockBack, item.owner)];
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
				item.useTime=item.useAnimation = animationTimes[0];
            }

			return false;
        }

        public override void HoldItem(Player player)
		{
			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
			sp.lastHeldKeyblade = 2;
			sp.guardType = guardType;

            if (player.itemAnimation <=0)
            {
				item.useStyle = ItemUseStyleID.SwingThrow;
            }

		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			if (curTransformation != -1 && keyTransformations[curTransformation]!=keyTransformation.none)
			{
				switch (keyTransformations[curTransformation]) {
					case keyTransformation.cannon:
						spriteBatch.Draw(mod.GetTexture("KupoCoin"),item.position,lightColor);
						break;
					case keyTransformation.dual:
						Rectangle r = item.getRect();
						spriteBatch.Draw(mod.GetTexture("Keyblade_oath"), r,r, lightColor,0, item.position,SpriteEffects.None,0);

						spriteBatch.Draw(mod.GetTexture("Keyblade_oblivion"), r,r,lightColor,(float)Math.PI/2, item.position,SpriteEffects.None,15);
						break;
				}

				return false;
			}
			else
			{
				return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
			}
		}

        public abstract void ChangeKeybladeValues();

		void ComboPlus(int comboMoment)
		{
			switch (keyComboType) {
				case KeyComboType.normal:
					switch (comboMoment)
					{
						case 4:
							item.shoot = ProjectileID.None;
							item.useStyle = ItemUseStyleID.SwingThrow;
							break;
						case 5:
							item.shoot = ProjectileID.None;
							item.useStyle = 100;
							item.channel = true;
							break;
					}
				break;
				case KeyComboType.magic:
					switch (comboMoment)
					{
						case 4:
							ShootmagicProjectile();
							break;
						case 5:
							item.shoot = ProjectileID.None;
							item.useStyle = ItemUseStyleID.Stabbing;
							break;
					}
					break;
				case KeyComboType.dual:
					switch (comboMoment)
					{
						case 4:
							ShootmagicProjectile();
							break;
						case 5:
							item.shoot = ProjectileID.None;
							item.useStyle = ItemUseStyleID.Stabbing;
							break;
					}
					break;
				case KeyComboType.stabbing:

					switch (comboMoment)
					{
						case 4:
							item.shoot = ProjectileID.None;
							item.useStyle = ItemUseStyleID.Stabbing;
							break;
						case 5:
							item.shoot = ProjectileID.None;
                            item.useStyle = 100;
							item.channel = true;
							break;
					}
					break;
			}

		}

		public override void HoldStyle(Player player)
		{
			Vector2 position = GetLightPosition(player);
			SoraPlayer sp = player.GetModPlayer<SoraPlayer>();
			if (sp.guardTime > 0)
			{
				player.itemLocation.X = player.Center.X + 30f * player.direction;
				player.itemLocation.Y = player.position.Y + 23f + 23f * player.gravDir + player.mount.PlayerOffsetHitbox;
				player.itemRotation = (player.direction == 1) ? MathHelper.TwoPi - MathHelper.Pi / 2 : MathHelper.Pi / 2;

			}
            else { 
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

        public override bool HoldItemFrame(Player player)
        {

			Vector2 position = GetLightPosition(player);
			if (position.Y <= player.Center.Y || Math.Abs(position.Y - player.Center.Y) < 2)
			{
				player.bodyFrame.Y =0;
			}
			else
			{
				player.bodyFrame.Y = player.bodyFrame.Height * 1;
			}

			return true;
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
				spriteBatch.Draw(mod.GetTexture(transSprites[curTransformation]), position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0);
				return false;
            }
        }

        public override bool UseItem(Player player)
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
					CommandLogic.instance.ChangeCommand(0);
					return true;
				}
			}
			return false;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			int proj=Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, item.owner);
			Main.projectile[proj].friendly = true;
			Main.projectile[proj].hostile = false;
            return false;
        }

        public void ShootmagicProjectile()
		{

			if(manaConsumed>0)
			{
				if (wielder.statMana >= manaConsumed)
				{
					item.mana = 20;
					wielder.statMana -= manaConsumed;
				}
				else
				{
					item.shoot = ProjectileID.None;
					item.useStyle = ItemUseStyleID.Stabbing;
					return;
				}
			}

			switch (keybladeElement)
			{
				case keyType.fire:
					item.shoot = ProjectileID.Flamelash;
					item.shootSpeed = 3;
					Main.projectile[item.shoot].timeLeft = projectileTime;
					break;
				case keyType.dark:
					item.shoot = ProjectileID.DemonScythe;
					item.shootSpeed = 1;
					break;
				case keyType.light:
					item.shoot = ProjectileID.AmethystBolt;
					item.shootSpeed = 4;
					break;
				case keyType.jungle:
					item.shoot = ProjectileID.SporeCloud;
					item.shootSpeed = 1;
					break;
				case keyType.digital:
					item.shoot = mod.ProjectileType("tronDisk");
					item.shootSpeed = 7;
					break;
				case keyType.destiny:
					item.shoot = mod.ProjectileType("teleportThrownKey");
					item.shootSpeed = 15;
					break;
				case keyType.star:
					item.shoot = ProjectileID.FallingStar;
					item.shootSpeed = 25;
					break;
				case keyType.honey:
					item.shoot = ProjectileID.Bee;
					item.shootSpeed = 15;
					break;
			}
			item.useStyle = ItemUseStyleID.HoldingOut;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			//Transformation tooltip
			if (transTooltip == null)
			{
				transTooltip = new TooltipLine(mod, "Transformations", (keyTransformations.Length > 0 && formChanges.Length > 0)
					? "This keyblade can transform itself and you" :
					(keyTransformations.Length > 0) ? "This keyblade can transform itself" :
					(formChanges.Length > 0) ? "This keyblade can transform you" : "This Keyblade can't transform itself or you");
			}
			transTooltip.overrideColor = Color.LightBlue;
			if (!tooltips.Contains(transTooltip))
			{
				tooltips.Add(transTooltip);
			}
            //Level up tooltip
            if (levelTooltip == null)
            {
				levelTooltip = new TooltipLine(mod, "Level", "Level of keyblade: "+keyLevel);
			}
			levelTooltip.overrideColor = Color.Yellow;
			if (!tooltips.Contains(levelTooltip))
			{
				tooltips.Add(levelTooltip);
			}
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

			ChangeKeybladeValues();

            if (transTooltip == null)
			{
				transTooltip = new TooltipLine(mod, "Transformations", (keyTransformations.Length>0 && formChanges.Length>0)
					?"This keyblade can transform itself and you":
					(keyTransformations.Length>0)?"This keyblade can transform itself":
					(formChanges.Length>0)?"This keyblade can transform you":"This Keyblade can't transform itself or you");
			}
			transTooltip.text = (keyTransformations.Length > 0 && formChanges.Length > 0)
					? "This keyblade can transform itself and you" :
					(keyTransformations.Length > 0) ? "This keyblade can transform itself" :
					(formChanges.Length > 0) ? "This keyblade can transform you" : "This Keyblade can't transform itself or you";

			if (levelTooltip == null)
			{
				levelTooltip = new TooltipLine(mod, "Level", "Level of keyblade: " + keyLevel);
			}
			levelTooltip.text= "Level of keyblade: " + keyLevel;

			item.prefix = 0;

			if (SoraPlayer.summonProjectiles.Length > 0)
				CheckSummonProjectiles();

			ChangeForm();
			SoraPlayer sora = player.GetModPlayer<SoraPlayer>();

			if (sora.lastHeldItem == item)
            {
                if (player.HeldItem == item)
                {
					sora.usingForm = (curForm >= 0);
					sora.formColor = curFormColor;
				}
                else
                {
					sora.usingForm = false;
					sora.formColor = Color.White;
                }
            }

			enlightened = player.HasBuff(mod.BuffType("EnlightenedBuff"));

			item.damage = Math.Max(1,(int)(item.damage/damageMult));

			damageMult = 1;
			damageMult += (enlightened) ? 0.5f : 0;

			item.damage = (int)(item.damage * damageMult);
			item.color = (enlightened) ? Color.Blue : Color.White;

			if ((player.selectedItem>=0 && player.selectedItem<player.inventory.Length 
				&& player.inventory[player.selectedItem] == item)
				|| player.HeldItem==item)
			{
				lastUsedTime++;
				if (lastUsedTime > item.useTime * 1.5f && combo > 0)
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

		public void LevelUp()
        {
			item.damage = (int)keyAtributes[0];
			item.knockBack = (int)keyAtributes[1];
			for(int i = 0; i < keyLevel; i++)
            {
				item.damage += (int)(keyAtributes[0] * 2);
			}
        }

    }
}
