using Terraria;

namespace TheConfectionRebirth {
	public struct TimerData
    {
        public uint endTime;
        public int value;
        public TimerDataType type;

        public TimerData(int value, uint duration, TimerDataType type)
        {
            this.type = type;
            endTime = Main.GameUpdateCount + duration;
            this.value = value;
        }
        public static bool Comparer(TimerData first, TimerData second) {

            //overflow checks
            if (Main.GameUpdateCount > first.endTime && Main.GameUpdateCount < second.endTime) return true;
            if (Main.GameUpdateCount > second.endTime && Main.GameUpdateCount < first.endTime) return false;
            return first.endTime <= second.endTime;
        }
    }
}
