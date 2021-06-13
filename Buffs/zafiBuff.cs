using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Buffs
{
    public class zafiBuff:ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Lost Dog");
			Description.SetDefault("\"Come home safe... some day\"");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<SoraPlayer>().hasZafi = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("zafi")] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("zafi"), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}
