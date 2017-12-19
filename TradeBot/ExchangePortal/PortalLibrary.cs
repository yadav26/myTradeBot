using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangePortal
{
    public static class PortalLibrary
    {
        public static Random rand = new Random();

        public static float GetPrice( float cp )
        {

            float retval = rand.Next( (int)(cp -10), (int)(cp +10) );
            //float retval = (float)(float.MaxValue * 2.0 * (rand.NextDouble() - 0.5));

            return retval;
        }
    }
}
