using WalletAppication.Interfaces;
using WalletAppication.Repositories;

namespace WalletAppication.Services
{
    public class CurrencyService : ICurrencyService
    {

        private readonly ICurrencyRateRepository _currencyRateRepository;

        public CurrencyService(ICurrencyRateRepository currencyRateRepository)
        {
            _currencyRateRepository = currencyRateRepository;
        }

        public bool IsCurrencyValid(string currency)
        {
            var currencies = _currencyRateRepository.GetAllCurrencies().Result;
            if (currencies.FirstOrDefault(y => y.Currency.ToLower() == currency.ToLower()) == null)
            {
                return false;
            }
            return true;
        }  
    }
}
