using System;
using System.Collections.Generic;

namespace Heist
{
    public interface IRobber
    {
        public string Name {get;set;}
        public int SkillLevel {get;set;}
        public int PercentageCut {get;set;}
        public void PerformSKill(Bank b);

    }
}