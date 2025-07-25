using System.Globalization;
using System.Text.RegularExpressions;

namespace GameApi.Utils
{
    public class PrecioHelper
    {
        public static int ConvertirPrecioATotalPesos(string precioTexto)
        {
            if (string.IsNullOrWhiteSpace(precioTexto))
                return 0;

            string limpio = Regex.Replace(precioTexto, @"[^\d,\.]", "").Replace(".", "").Replace(",", ".");

            return decimal.TryParse(limpio, NumberStyles.Any, CultureInfo.InvariantCulture, out var valorDecimal)
                ? (int)Math.Floor(valorDecimal): 0;
        }

        public static int ConvertirDescuento(string input)
        {
            var match = Regex.Match(input, @"-?\d+");
            return match.Success && int.TryParse(match.Value, out var numero) ? Math.Abs(numero) : 0;
        }
    }
}
