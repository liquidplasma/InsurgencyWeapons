using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace InsurgencyWeapons
{
    internal class InsurgencyModConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public static InsurgencyModConfig Instance => ModContent.GetInstance<InsurgencyModConfig>();

        [Header("$Mods.InsurgencyWeapons.Configs.Config")]
        [DefaultValue(true)]
        [LabelKey("$Mods.InsurgencyWeapons.Configs.DropCasingLabel")]
        [TooltipKey("$Mods.InsurgencyWeapons.Configs.DropCasingDesc")]
        public bool DropCasing { get; set; }

        [DefaultValue(true)]
        [LabelKey("$Mods.InsurgencyWeapons.Configs.DropMagazineLabel")]
        [TooltipKey("$Mods.InsurgencyWeapons.Configs.DropMagazineDesc")]
        public bool DropMagazine { get; set; }

        [Range(1, 100)]
        [Increment(1)]
        [DefaultValue(15)]
        [LabelKey("$Mods.InsurgencyWeapons.Configs.CasingLifeTimeLabel")]
        [TooltipKey("$Mods.InsurgencyWeapons.Configs.CasingLifeTimeDesc")]
        public int CasingLifeTime { get; set; }

        [DefaultValue(true)]
        [LabelKey("$Mods.InsurgencyWeapons.Configs.DamageScalingLabel")]
        [TooltipKey("$Mods.InsurgencyWeapons.Configs.DamageScalingDesc")]
        public bool DamageScaling { get; set; }
    }
}