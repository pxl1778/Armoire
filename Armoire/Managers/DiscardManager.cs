using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Armoire
{
    class DiscardManager
    {
        public struct DiscardedArmor
        {
            public Armor armor;
            public int age;
        }

        private List<DiscardedArmor> discardedArmor;

        public DiscardManager()
        {
            discardedArmor = new List<DiscardedArmor>();
        }

        public void DiscardArmor(Armor armor)
        {
            discardedArmor.Add(new DiscardedArmor { armor = armor, age = 0 });
            MainManager.Instance.gameMan.player.armorLevel--;
        }

        public void Update()
        {
            List<DiscardedArmor> armorToRemove = new List<DiscardedArmor>();
            for(int i = 0; i < discardedArmor.Count; i++)
            {
                DiscardedArmor da = discardedArmor[i];
                da.armor.position.X -= 3;
                //da.armor.position.Y--;
                da.age++;
                if (da.age > 15)
                    armorToRemove.Add(da);
                discardedArmor[i] = da;
            }
            foreach(DiscardedArmor da in armorToRemove)
            {
                discardedArmor.Remove(da);
                MainManager.Instance.gameMan.armorPickups.Add(da.armor);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach(DiscardedArmor da in discardedArmor)
            {
                da.armor.DrawPickup(sb);
            }
        }
    }
}
