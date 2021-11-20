using System.Collections.Generic;
using Terraria.ModLoader;

namespace KingdomTerrahearts.Extra
{
	public class KeybladeDamage : DamageClass
	{
		public override void SetStaticDefaults()
		{
			// Make weapons with this damage type have a tooltip of 'X example damage'.
			ClassName.SetDefault("Keyblade damage");
		}


		protected override float GetBenefitFrom(DamageClass damageClass)
		{
			// Make this damage class not benefit from any otherclass stat bonuses by default, but still benefit from universal/all-class bonuses.
			if (damageClass == Generic)
				return 1f;


			if (damageClass == Melee)
				return 0.25f;

			return 0f;

		}

		public override bool CountsAs(DamageClass damageClass)
		{
			// Make this damage class not benefit from any otherclass effects (e.g. Spectre bolts, Magma Stone) by default.
			// Note that unlike GetBenefitFrom, you do not need to account for universal bonuses in this method.
			return false;
		}

	}
}
