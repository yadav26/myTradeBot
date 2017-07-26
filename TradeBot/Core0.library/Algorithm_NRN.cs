using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core0.library
{
    class Algorithm_NRN
    {
        //How to Find NR7 day..
        //1) Get the High and Low data of last few period
        //2) Calculate the range of each day i.e.high - low) for each day
        //3) Compare the range of a today and previous 6 days range(to get NR7.To get NR4 get last 3 days range)
        //4) If today's range is smallest of all 7 days, then today is NR7 day..else not.
        //It is that simple.
        //This is one of my favorite setup.It gives u a chance to be ahead of trade follower / indicator followers who will jump in the trend after you.
        //One of the easiest way to trade this setup will be to go long above the Day's high of NR7 day with stop at the Day's Low of NR7 day.
        //Or
        //Go short below the Day's Low of NR7 day with stop at the Day's High of NR7 day.


    }
}
