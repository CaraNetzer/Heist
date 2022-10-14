using System;

namespace Heist
{
    public class Teammember
    {
        public string Name {get;set;}
        public int SkillLevel {get;set;}
        public double CourageFactor {get;set;}

        public Teammember(string name)
        {
            Name = name;
        }
    }


    public class Hacker : IRobber
    {
        public string Name {get;set;}
        public int SkillLevel {get;set;}
        public int PercentageCut {get;set;}
        public Hacker(string name, int skillLevel, int percentageCut) 
        {
            Name = name;
            SkillLevel = skillLevel;
            PercentageCut = percentageCut;
        }
        public void PerformSKill(Bank b)
        {
            b.AlarmScore -= SkillLevel;
            Console.WriteLine($"\n{Name} is hacking the alarm system. Decreased security by {SkillLevel} points");
            if(b.AlarmScore <= 0)
            {
                Console.WriteLine($"{Name} has disabled the alarm system!");
            } 
        }
    }
    public class Muscle : IRobber
    {
        public string Name {get;set;}
        public int SkillLevel {get;set;}
        public int PercentageCut {get;set;}
        public Muscle(string name, int skillLevel, int percentageCut) 
        {
            Name = name;
            SkillLevel = skillLevel;
            PercentageCut = percentageCut;
        }
        public void PerformSKill(Bank b)
        {
            b.SecurityGaurdScore -= SkillLevel;
            Console.WriteLine($"\n{Name} is dealing with the security gaurds. Decreased security by {SkillLevel} points");
            if(b.SecurityGaurdScore <= 0)
            {
                Console.WriteLine($"{Name} has knocked out the security gaurds!");
            } 
        }
    }
    public class LockSpecialist : IRobber
    {
        public string Name {get;set;}
        public int SkillLevel {get;set;}
        public int PercentageCut {get;set;}
        public LockSpecialist(string name, int skillLevel, int percentageCut) 
        {
            Name = name;
            SkillLevel = skillLevel;
            PercentageCut = percentageCut;
        }
        public void PerformSKill(Bank b)
        {
            b.VaultScore -= SkillLevel;
            Console.WriteLine($"\n{Name} is picking the vault lock. Decreased security by {SkillLevel} points");
            if(b.VaultScore <= 0)
            {
                Console.WriteLine($"{Name} has picked the lock!");
            } 
        }
    }

    public class Bank
    {
        public int CashOnHand {get;set;}
        public int AlarmScore {get;set;}
        public int VaultScore {get;set;}
        public int SecurityGaurdScore {get;set;}
        public bool IsSecure 
        {
            /* get {return IsSecure;}
            set { 
                if(AlarmScore<=0 && VaultScore<=0 && SecurityGaurdScore<=0) 
                {
                    IsSecure = false;
                } 
                else {
                    IsSecure = true;
                }
            } */

            //tommy's version
            get {
                if(AlarmScore<=0 && VaultScore<=0 && SecurityGaurdScore<=0) 
                {
                    return false;
                } 
                else {
                    return true;
                }
            }
        }
        
    }
}