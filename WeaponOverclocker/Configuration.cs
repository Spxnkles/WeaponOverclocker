using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;

namespace WeaponOverclocker
{
    public class Configuration : IRocketPluginConfiguration
    {
        public List<DamageMultiplier> DamageMultipliers { get; set; }

        public ushort SoundEffectID { get; set; }

        public bool Debug {  get; set; }

        public void LoadDefaults()
        {
            DamageMultipliers = new List<DamageMultiplier>
            {
                new DamageMultiplier{AttachmentID = 1, Multiplier = 1.1f, ChancePercentage = 20},
                new DamageMultiplier{AttachmentID = 2, Multiplier = 1.2f, ChancePercentage = 10},
                new DamageMultiplier{AttachmentID = 3, Multiplier = 1.3f, ChancePercentage = 5}
            };

            SoundEffectID = 51200;

            Debug = false;
        }
    }

    public class DamageMultiplier
    {
        public ushort AttachmentID { get; set; }
        public float Multiplier { get; set; }
        public int ChancePercentage { get; set; }
    }
}
