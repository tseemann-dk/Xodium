using System;
using System.Linq;
using System.Text.RegularExpressions;
using Xodium.Geography;

namespace Xodium.Utilities
{
    public class DirectionsUrlBuilder
    {
        private readonly IDirectionsUrlProvider urlProvider;

        public DirectionsUrlBuilder(IDirectionsUrlProvider urlProvider)
        {
            this.urlProvider = urlProvider ?? throw new ArgumentNullException(nameof(urlProvider));
        }

        public string Build(GeoPosition position, string address)
        {
            if (IsValidPosition(position))
            {
                return IsValidAddress(address)
                    ? urlProvider.GetUrl(position, address)
                    : urlProvider.GetUrl(position);
            }
            else
            {
                return IsValidAddress(address)
                    ? urlProvider.GetUrl(address)
                    : string.Empty;
            }
        }
    
        private static bool IsValidPosition(GeoPosition position)
        {
            return !GeoPosition.IsNullOrEmpty(position) && !position.IsEmpty;
        }

        private static bool IsValidAddress(string adrStr)
        {
            if (string.IsNullOrEmpty(adrStr))
                return false;

            var split = adrStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            return IsFullAddress(split) || IsStreetAndNumber(split); // || IsStreetOrCity(split);
        }

        private static string GetSplitValue(string[] split, int index)
        {
            if (index < split.Length)
                return split[index].Trim();

            return string.Empty;
        }

        private static bool IsFullAddress(string[] split)
        {
            // Assume format: street number, city
            if (split.Length > 2)
                return false;

            var streetAndNumber = GetStreetAndNumber(GetSplitValue(split, 0));
            var street = streetAndNumber[0];
            var number = streetAndNumber[1];
            var city = GetSplitValue(split, 1);

            return IsStreet(street) && IsStreetNumber(number) && IsCity(city);
        }

        private static bool IsStreetAndNumber(string[] split)
        {
            // Assume format: street number
            if (split.Length > 1)
                return false;

            var streetAndNumber = GetStreetAndNumber(GetSplitValue(split, 0));
            var street = streetAndNumber[0];
            var number = streetAndNumber[1];

            return IsStreet(street) && IsStreetNumber(number);
        }

        private static string[] GetStreetAndNumber(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new string[] { string.Empty, string.Empty };

            var index = str.LastIndexOf(' ');

            if (index < 0)
                return new string[] { string.Empty, string.Empty };

            var street = str.Substring(0, index);
            var number = str.Substring(index, str.Length - index);

            return new string[] { street, number };
        }

        private static bool IsStreetNumber(string streetnumber)
        {
            return IsType<int>(streetnumber);
        }

        private static bool IsStreet(string street)
        {
            return IsType<string>(street) && !ContainsNumber(street) && !ContainsSpecialChars(street);
        }

        private static bool IsCity(string city)
        {
            return IsType<string>(city) && !ContainsNumber(city) && !ContainsSpecialChars(city);
        }

        private static bool IsStreetCityAddress(string[] split)
        {
            // Assume format: street, city
            if (split.Length > 2)
                return false;

            var street = GetSplitValue(split, 0);
            var city = GetSplitValue(split, 1);

            return IsStreet(street) && IsCity(city);
        }

        private static bool IsStreetOrCity(string[] split)
        {
            // Assume format: street/city
            if (split.Length > 1)
                return false;

            var streetOrCity = GetSplitValue(split, 0);

            return IsCity(streetOrCity);
        }

        private static bool ContainsNumber(string value)
        {
            return value.ToCharArray().Any(char.IsDigit);
        }

        private static bool ContainsSpecialChars(string value)
        {
            var regexItem = new Regex("^[a-zæøåA-ZÆØÅ0-9 ]*$");

            return !regexItem.IsMatch(value);
        }

        private static bool IsType<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            if (typeof(T) == typeof(string))
                return true;

            if (typeof(T) == typeof(int))
                return int.TryParse(value, out var result);

            return false;
        }
    }
}
