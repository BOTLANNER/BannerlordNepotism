using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Helpers;

using TaleWorlds.AchievementSystem;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.PlatformService.Steam;

using static TaleWorlds.Library.CommandLineFunctionality;

namespace BannerlordNepotism
{
    class ConsoleCommands
    {

        [CommandLineArgumentFunction("help", "nepotism_mod")]
        public static string Help(List<string> args)
        {
            string helpResponse = @"
Commands:

nepotism_mod.help                                            - Shows console command usage
nepotism_mod.generate_heirs                                  - Generates an heir of each gender for each kingdom
nepotism_mod.list_kingdoms                                   - List all supported kingdom names for use with nepotism_mod.generate_heir
nepotism_mod.generate_heir       kingdomName gender?         - Generates an heir for the specified kingdom name (Call nepotism_mod.list_kingdoms for all supported kingdom names), optionally with the specified gender if provided (either male or female)


";
            /*
or nepotism_mod.abdicate
nepotism_mod.abdicate      kingdomName                       - Triggers an abdication for the specified kingdom name (Call nepotism_mod.list_kingdoms for all supported kingdom names)
             */

            return helpResponse;
        }

        [CommandLineArgumentFunction("generate_heirs", "nepotism_mod")]
        public static string GenerateHeirs(List<string> args)
        {
            try
            {
                if (Campaign.Current == null || Campaign.Current.Kingdoms == null || Campaign.Current.Kingdoms.Count <= 0)
                {
                    return "Must be in a campaign or sandbox session!";
                }


                string result = "";
                foreach (var kingdom in Campaign.Current.Kingdoms)
                {
                    if (kingdom.IsEliminated || kingdom.Leader == null)
                    {
                        continue;
                    }

                    result += "\r\n";
                    bool success = GenerateHeirInternal(kingdom, true, out string output);
                    result += kingdom.InformalName.ToString() + " / " + kingdom.Name.ToString() + " / " + kingdom.GetName().ToString() + " (female) - " + output;
                    result += "\r\n";
                    success = GenerateHeirInternal(kingdom, false, out output);
                    result += kingdom.InformalName.ToString() + " / " + kingdom.Name.ToString() + " / " + kingdom.GetName().ToString() + " (male) - " + output;
                }
                return result;
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }


        }

        [CommandLineArgumentFunction("list_kingdoms", "nepotism_mod")]
        public static string ListKingdoms(List<string> args)
        {
            try
            {
                if (Campaign.Current == null || Campaign.Current.Kingdoms == null || Campaign.Current.Kingdoms.Count <= 0)
                {
                    return "Must be in a campaign or sandbox session!";
                }


                string result = "";
                foreach (var kingdom in Campaign.Current.Kingdoms)
                {
                    if (kingdom.IsEliminated || kingdom.Leader == null)
                    {
                        continue;
                    }
                    result += "\r\n";
                    result += kingdom.InformalName.ToString() + " or " + kingdom.Name.ToString() + " or " + kingdom.GetName().ToString();
                }
                return result;
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }


        }

        [CommandLineArgumentFunction("generate_heir", "nepotism_mod")]
        public static string GenerateHeir(List<string> args)
        {
            try
            {
                if (args.Count > 0)
                {
                    string kingdom = ArgsToString(args).Replace("\"", "").ToLower();
                    bool female = true;
                    if (kingdom.EndsWith(" male"))
                    {
                        kingdom = kingdom.Replace(" male", "");
                        female = false;
                    }
                    else if (kingdom.EndsWith(" female"))
                    {
                        kingdom = kingdom.Replace(" female", "");
                    }

                    if (Campaign.Current == null || Campaign.Current.Kingdoms == null || Campaign.Current.Kingdoms.Count <= 0)
                    {
                        return "Must be in a campaign or sandbox session!";
                    }


                    foreach (var k in Campaign.Current.Kingdoms)
                    {
                        var name = k.GetName().ToString().ToLower();
                        var name2 = k.Name.ToString().ToLower();
                        var name3 = k.InformalName.ToString().ToLower();
                        if (name == kingdom || name2 == kingdom || name3 == kingdom)
                        {
                            if (k.IsEliminated || k.Leader == null)
                            {
                                continue;
                            }
                            bool result = GenerateHeirInternal(k, female, out string output);
                            return output;
                        }
                    }
                    return "Kingdom not found!";
                }
                else
                {
                    return Help(args);
                }
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }


        }

        //[CommandLineArgumentFunction("abdicate", "nepotism_mod")]
        //public static string Abdicate(List<string> args)
        //{
        //    try
        //    {
        //        if (args.Count > 0)
        //        {
        //            string kingdom = ArgsToString(args).Replace("\"", "").ToLower();
        //            bool female = true;
        //            if (kingdom.EndsWith(" male"))
        //            {
        //                kingdom = kingdom.Replace(" male", "");
        //                female = false;
        //            }
        //            else if (kingdom.EndsWith(" female"))
        //            {
        //                kingdom = kingdom.Replace(" female", "");
        //            }

        //            if (Campaign.Current == null || Campaign.Current.Kingdoms == null || Campaign.Current.Kingdoms.Count <= 0)
        //            {
        //                return "Must be in a campaign or sandbox session!";
        //            }


        //            foreach (var k in Campaign.Current.Kingdoms)
        //            {
        //                var name = k.GetName().ToString().ToLower();
        //                var name2 = k.Name.ToString().ToLower();
        //                var name3 = k.InformalName.ToString().ToLower();
        //                if (name == kingdom || name2 == kingdom || name3 == kingdom)
        //                {
        //                    if (k.IsEliminated || k.Leader == null)
        //                    {
        //                        continue;
        //                    }
        //                    var leader = k.Leader;
        //                    Campaign.Current.KingdomManager.AbdicateTheThrone(k);
        //                    return $"{leader.Name} has abdicated";
        //                }
        //            }
        //            return "Kingdom not found!";
        //        }
        //        else
        //        {
        //            return Help(args);
        //        }
        //    }
        //    catch (System.Exception e)
        //    {
        //        return e.ToString();
        //    }


        //}

        internal static bool GenerateHeirInternal(Kingdom kingdom, bool female, out string output)
        {
            try
            {
                if (kingdom.IsEliminated || kingdom.Leader == null)
                {
                    output = "Kingdom is not valid!";
                    return false;
                }

                Hero mother;
                Hero father;
                if (kingdom.Leader.IsFemale)
                {
                    mother = kingdom.Leader;
                    father = kingdom.Leader.Spouse;
                }
                else
                {
                    father = kingdom.Leader;
                    mother = kingdom.Leader.Spouse;
                }

                if (mother != null && father != null)
                {
                    var hero = HeroCreator.DeliverOffSpring(mother, father, female);
                    int heroChildOfAge = Campaign.Current.Models.AgeModel.BecomeChildAge;
                    hero.SetBirthDay(HeroHelper.GetRandomBirthDayForAge(heroChildOfAge));
                    CampaignEventDispatcher.Instance.OnHeroGrowsOutOfInfancy(hero);

                    int heroComesOfAge = Campaign.Current.Models.AgeModel.HeroComesOfAge;
                    hero.SetBirthDay(HeroHelper.GetRandomBirthDayForAge(heroComesOfAge));
                    CampaignEventDispatcher.Instance.OnHeroComesOfAge(hero);

                    output = $"Created {hero.Name}";
                    return true;
                } 
                else
                {
                    var template = father;
                    if (female && mother != null)
                    {
                        template = mother;
                    }
                    else if (!female && father == null)
                    {
                        template = mother;
                    }

                    if (template == null)
                    {
                        throw new System.ArgumentNullException("Kingdom leader not found!");
                    }

                    Hero hero = HeroCreator.CreateSpecialHero(template.CharacterObject, template.CurrentSettlement ?? template.HomeSettlement ?? template.BornSettlement, template.Clan, null, Campaign.Current.Models.AgeModel.HeroComesOfAge);
                    hero.SetNewOccupation(template.Occupation);
                    hero.CharacterObject.IsFemale = female;
                    hero.GetType().GetProperty("IsFemale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public).GetSetMethod(true).Invoke(hero, new object[] { female });

                    NameGenerator.Current.GenerateHeroNameAndHeroFullName(hero, out TextObject textObject, out TextObject textObject1, false);
                    hero.SetName(textObject1, textObject);
                    typeof(HeroCreator).GetMethod("ModifyAppearanceByTraits", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).Invoke(null,new object[] { hero });

                    hero.Mother = mother;
                    hero.Father = father;

                    hero.ChangeState(Hero.CharacterStates.Active);

                    output = $"Created {hero.Name}";
                    return true;
                }
            }
            catch (System.Exception e)
            {
                output = e.ToString();
                return false;
            }
        }

        private static string ArgsToString(List<string> args)
        {
            return string.Join(" ", args).Trim();
        }
    }
}
