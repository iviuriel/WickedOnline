using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BroadcastName
{
    public class Card
    {
        public static string prefix = "Card_";

        public static string OnHeroDefeated = prefix + "OnHeroDefeated";

        public static string OnCardPlayed = prefix + "OnCardPlayed";
        public static string OnCardDiscard = prefix + "OnCardDiscard";
        public static string OnCardMoved = prefix + "OnCardMoved";

        public static string OnItemAttachedToCard = prefix + "OnItemAttachedToCard";

    }

    public class GameFlow
    {
        public static string prefix = "GameFlow_";

        public static string OnActionStarted = prefix + "OnActionStarted";
        public static string OnActionCompleted = prefix + "OnActionStarted";
        public static string OnActionCancelled = prefix + "OnActionStarted";

        public static string OnActionCardSelected = prefix + "OnActionCardSelected";
        public static string OnActionCardDeselected = prefix + "OnActionCardDeselected";



    }
}

