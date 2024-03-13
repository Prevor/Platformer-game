using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Weapons.Damage
{
    internal class DurationDamage : DamageDecorator
    {
        public DurationDamage(IBaseDamage baseDamage) : base(baseDamage)
        {


        }
    }
}
