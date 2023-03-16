using System.Collections.Generic;
using System.Linq;

using TaleWorlds.CampaignSystem;

namespace BannerlordNepotism
{
    public static class HeroExtensions
    {
        public static List<Hero> Relatives(this Hero hero)
        {
            var foundRelatives = new List<Hero>();
            hero.AllFamilyInternal(ref foundRelatives);
            return foundRelatives;
        }

        private static void AllFamilyInternal(this Hero hero,ref List<Hero> foundRelatives)
        {
            if (hero.Children != null && hero.Children.Count > 0)
            {
                foreach (var child in hero.Children)
                {
                    if (foundRelatives.Contains(child))
                    {
                        continue;
                    }
                    foundRelatives.Add(child);
                    child.AllFamilyInternal(ref foundRelatives);

                }
            }
            if (hero.Siblings != null)
            {
                foreach (var sibling in hero.Siblings)
                {
                    if (foundRelatives.Contains(sibling))
                    {
                        continue;
                    }
                    foundRelatives.Add(sibling);
                    sibling.AllFamilyInternal(ref foundRelatives);

                }
            }

            if (hero.Mother != null && hero.Mother != hero && !foundRelatives.Contains(hero.Mother))
            {
                foundRelatives.Add(hero.Mother);
                hero.Mother.AllFamilyInternal(ref foundRelatives);
            }
            if (hero.Father != null && hero.Father != hero && !foundRelatives.Contains(hero.Father))
            {
                foundRelatives.Add(hero.Father);
                hero.Father.AllFamilyInternal(ref foundRelatives);
            }
        }

        public static bool IsRelatedTo(this Hero hero,Hero other)
        {
            return hero.Relatives().Contains(other);
        }

        public static bool IsFamilyOf(this Hero current, Hero other)
        {
            bool flag;
            foreach (Hero child in current.Children)
            {
                if (child == other)
                {
                    flag = true;
                    return flag;
                }
            }
            foreach (Hero hero in other.Children)
            {
                if (hero == current)
                {
                    flag = true;
                    return flag;
                }
            }
            using (IEnumerator<Hero> enumerator = current.Siblings.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Hero sibling = enumerator.Current;
                    if (other == sibling)
                    {
                        flag = true;
                        return flag;
                    }
                    else if ((other.Mother == null ? false : other.Mother == sibling))
                    {
                        flag = true;
                        return flag;
                    }
                    else if ((other.Father == null ? false : other.Father == sibling))
                    {
                        flag = true;
                        return flag;
                    }
                }
                goto Label1;
            }
            return flag;
        Label1:
            if (current.Mother != null)
            {
                foreach (Hero uncle in current.Mother.Siblings)
                {
                    if (other == uncle)
                    {
                        flag = true;
                        return flag;
                    }
                }
            }
            if (current.Father != null)
            {
                foreach (Hero uncle in current.Father.Siblings)
                {
                    if (other == uncle)
                    {
                        flag = true;
                        return flag;
                    }
                }
            }
            flag = false;
            return flag;
        }
    }
}
