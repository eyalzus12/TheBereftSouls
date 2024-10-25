using Terraria.ModLoader;

namespace TheBereftSouls
{
    public class TheBereftSouls : Mod
    {
        // Public variables for easy accses to mod reference
        public static Mod CalamityMod;
        public static Mod BossChecklist;
        public static Mod ThoriumMod;
        public static Mod SpiritMod;
        public static Mod FargosSoulMod;
        public static Mod FargowiltasMod;
        public static Mod GensokyoMod;
        public static Mod SOTS;
        public static Mod CalamityRangerExpansion;

        public override void Load()
        {
            ModLoader.TryGetMod("CalamityMod", out CalamityMod);
            ModLoader.TryGetMod("ThoriumMod", out ThoriumMod);
            ModLoader.TryGetMod("BossChecklist", out BossChecklist);
            ModLoader.TryGetMod("FargowiltasSouls", out FargosSoulMod);
            ModLoader.TryGetMod("Fargowiltas", out FargowiltasMod);
            ModLoader.TryGetMod("SpiritMod", out SpiritMod);
            ModLoader.TryGetMod("Gensokyo", out GensokyoMod);
            ModLoader.TryGetMod("SOTS", out SOTS);
            ModLoader.TryGetMod("CalamityAmmo", out CalamityRangerExpansion);
        }
    }
}