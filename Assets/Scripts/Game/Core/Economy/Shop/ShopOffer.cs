using System;
using Game.Data.Base;

namespace Game.Core.Economy.Shop
{
    // Numbers match the level JSON ("Kind": 1, "Currency": 3, ...).
    public enum ShopOfferKind { Cat = 0, RandomCat = 1 } // RandomCat draws from the level's CatPool
    public enum ShopCurrency { Gems = 0, Coins = 1, Free = 2, RewardedAd = 3 }

    // One entry of a level's ShopPool.
    [Serializable]
    public struct ShopOffer
    {
        public ShopOfferKind Kind;
        public CatType Cat; // ignored when Kind is RandomCat
        public int Level; // merge level the bought gear starts at, 1-3
        public ShopCurrency Currency;
        public int Price; // 0 for Free and RewardedAd
        public float Weight; // how likely this entry is picked when the tray fills its slots
        public int MaxPerLevel; // 0 = no limit
    }
}
