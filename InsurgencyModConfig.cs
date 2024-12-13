using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace InsurgencyWeapons
{
    public class InsurgencyModConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public static InsurgencyModConfig Instance => ModContent.GetInstance<InsurgencyModConfig>();

        [Header("$Mods.InsurgencyWeapons.Configs.Config")]
        [DefaultValue(false)]
        [LabelKey("$Mods.InsurgencyWeapons.Configs.LiteModeDesc")]
        [TooltipKey("$Mods.InsurgencyWeapons.Configs.LiteModeLabel")]
        public bool LiteMode { get; set; }

        [Header("$Mods.InsurgencyWeapons.Configs.Gameplay")]
        [DefaultValue(false)]
        [LabelKey("$Mods.InsurgencyWeapons.Configs.HideWhenNotInUseDesc")]
        [TooltipKey("$Mods.InsurgencyWeapons.Configs.HideWhenNotInUseLabel")]
        public bool HideWhenNotInUse { get; set; }

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

        [Header("$Mods.InsurgencyWeapons.Configs.Misc")]
        [DefaultValue(true)]
        [LabelKey("$Mods.InsurgencyWeapons.Configs.DamageScalingLabel")]
        [TooltipKey("$Mods.InsurgencyWeapons.Configs.DamageScalingDesc")]
        public bool DamageScaling { get; set; }
    }

    public class InsurgencyModConfigClient : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public static InsurgencyModConfigClient Instance => ModContent.GetInstance<InsurgencyModConfigClient>();

        [Header("$Mods.InsurgencyWeapons.Configs.ShowCrosshair")]
        [DefaultValue(true)]
        public bool ShowCrosshair { get; set; }

        [Header("$Mods.InsurgencyWeapons.Configs.ConfigClient")]
        [DefaultValue(typeof(Color), "0, 128, 0, 255"), SliderColor(0, 128, 0)]
        public Color CrosshairBorderColor { get; set; }

        [DefaultValue(typeof(Color), "255, 0, 0, 255"), SliderColor(255, 0, 0)]
        public Color CrosshairColor { get; set; }
    }
}