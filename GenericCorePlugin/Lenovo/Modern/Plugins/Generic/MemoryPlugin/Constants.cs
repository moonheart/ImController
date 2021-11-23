using System;
using System.Collections.Generic;

namespace Lenovo.Modern.Plugins.Generic.MemoryPlugin
{
	// Token: 0x02000002 RID: 2
	public class Constants
	{
		// Token: 0x04000001 RID: 1
		public static readonly IReadOnlyDictionary<string, string> MemSlotLocDictionary = new Dictionary<string, string>
		{
			{ "0", "Reserved" },
			{ "1", "Other" },
			{ "2", "Unknown" },
			{ "3", "Motherboard" },
			{ "4", "ISA add-on card" },
			{ "5", "EISA add-on card" },
			{ "6", "PCI add-on card" },
			{ "7", "MCA add-on card" },
			{ "8", "PCMCIA add-on card" },
			{ "9", "Proprietary add-on card" },
			{ "10", "NuBus" },
			{ "11", "PC-98/C20 add-on card" },
			{ "12", "PC-98/C24 add-on card" },
			{ "13", "PC-98/E add-on card" },
			{ "14", "PC-98/Local bus add-on card" }
		};

		// Token: 0x04000002 RID: 2
		public static readonly IReadOnlyDictionary<string, string> MemFormFactorDictionary = new Dictionary<string, string>
		{
			{ "0", "Unknown" },
			{ "1", "Other" },
			{ "2", "SIP" },
			{ "3", "DIP" },
			{ "4", "ZIP" },
			{ "5", "SOJ" },
			{ "6", "Proprietary" },
			{ "7", "SIMM" },
			{ "8", "DIMM" },
			{ "9", "TSOP" },
			{ "10", "PGA" },
			{ "11", "RIMM" },
			{ "12", "SODIMM" },
			{ "13", "SRIMM" },
			{ "14", "SMD" },
			{ "15", "SSPM" },
			{ "16", "QFP" },
			{ "17", "TQFP" },
			{ "18", "SOIC" },
			{ "19", "LCC" },
			{ "20", "PLCC" },
			{ "21", "BGA" },
			{ "22", "FPBGA" },
			{ "23", "LGA" }
		};

		// Token: 0x04000003 RID: 3
		public static readonly IReadOnlyDictionary<string, string> MemTypeDictionary = new Dictionary<string, string>
		{
			{ "0", "Unknown" },
			{ "1", "Other" },
			{ "2", "DRAM" },
			{ "3", "Synchronous DRAM" },
			{ "4", "Cache DRAM" },
			{ "5", "EDO" },
			{ "6", "EDRAM" },
			{ "7", "VRAM" },
			{ "8", "SRAM" },
			{ "9", "RAM" },
			{ "10", "ROM" },
			{ "11", "Flash" },
			{ "12", "EEPROM" },
			{ "13", "FEPROM" },
			{ "14", "EPROM" },
			{ "15", "CDRAM" },
			{ "16", "3DRAM" },
			{ "17", "SDRAM" },
			{ "18", "SGRAM" },
			{ "19", "RDRAM" },
			{ "20", "DDR" },
			{ "21", "DDR2" },
			{ "22", "DDR2" },
			{ "23", "DDR2 FB-DIMM" },
			{ "24", "DDR3" },
			{ "25", "FBD2" }
		};
	}
}
