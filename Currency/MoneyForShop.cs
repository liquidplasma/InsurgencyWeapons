using Terraria.GameContent.UI;

namespace InsurgencyWeapons.Currency
{
    public class MoneyForShop : CustomCurrencySingleCoin
    {
        public MoneyForShop(int coinItemID, long currencyCap, string CurrencyTextKey) : base(coinItemID, currencyCap)
        {
            this.CurrencyTextKey = CurrencyTextKey;
            CurrencyTextColor = Color.Green;
        }
    }
}