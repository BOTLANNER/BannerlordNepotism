using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace BannerlordNepotism
{
    public static class CreateClanAction
    {
        public static Clan Apply(Kingdom? fromKingdom = null)
        {
            Kingdom kingdom = fromKingdom ?? Kingdom.All.GetRandomElement<Kingdom>();
            CultureObject culture = kingdom.Culture;
            Settlement settlement = kingdom.Settlements.FirstOrDefault<Settlement>((Settlement x) => x.IsTown) ?? kingdom.Settlements.GetRandomElement<Settlement>();
            TextObject textObject = NameGenerator.Current.GenerateClanName(culture, settlement);
            Clan clan = Clan.CreateClan($"no_clan_{Clan.All.Count}");
            TextObject textObject1 = new TextObject("{=!}informal", null);
            CultureObject cultureObject = Kingdom.All.GetRandomElement<Kingdom>().Culture;
            Banner banner = Banner.CreateRandomClanBanner(-1);
            Vec2 vec2 = new Vec2();

            clan.ChangeClanName(textObject, textObject);
            clan.Culture = cultureObject;
            clan.Banner = banner;
            clan.SetInitialHomeSettlement(settlement);

            //clan.InitializeClan(textObject, textObject1, cultureObject, banner, vec2, false);
            CharacterObject characterObject = culture.LordTemplates.FirstOrDefault<CharacterObject>((CharacterObject x) => x.Occupation == Occupation.Lord);
            Settlement randomElement = kingdom.Settlements.GetRandomElement<Settlement>();
            Hero hero = HeroCreator.CreateSpecialHero(characterObject ?? kingdom.Leader.CharacterObject, randomElement, clan, null, MBRandom.RandomInt(18, 36));
            hero.ChangeState(Hero.CharacterStates.Active);
            clan.SetLeader(hero);
            ChangeKingdomAction.ApplyByJoinToKingdom(clan, kingdom, showNotification: false);

            return clan;
        }
    }
}
