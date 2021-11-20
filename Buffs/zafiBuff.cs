using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Buffs
{
    public class zafiBuff:ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lost Dog");
			Description.SetDefault("\"Come home safe... some day\"");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{

			ProjectileSource_Buff s = new ProjectileSource_Buff(player, 0,buffIndex);

			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<SoraPlayer>().hasZafi = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Pets.zafi>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(s,player.position.X + (player.width / 2f), player.position.Y + (player.height / 2f), 0f, 0f, ModContent.ProjectileType<Projectiles.Pets.zafi>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}
