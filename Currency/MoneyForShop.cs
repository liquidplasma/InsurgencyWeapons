using Terraria.GameContent.UI;

namespace InsurgencyWeapons.Currency
{
    internal class MoneyForShop : CustomCurrencySingleCoin
    {
        public MoneyForShop(int coinItemID, long currencyCap, string CurrencyTextKey) : base(coinItemID, currencyCap)
        {
            this.CurrencyTextKey = CurrencyTextKey;
            CurrencyTextColor = Color.Green;
        }
    }
}