using GameApi.Models;
using GameApi.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace GameApi.Services
{
    public class ScrapperGameService
    {
        public List<Game> ObtenerJuegos()
        {
            const string url = "https://www.xbox.com/es-AR/games/all-games/pc?PlayWith=PC";
            const string gameItemSelector = "div.ProductCard-module__cardWrapper___6Ls86.shadow";
            const string titleSelector = "span.ProductCard-module__title___nHGIp.typography-module__xdsBody2___RNdGY";
            const string discountTagSelector = "div.ProductCard-module__discountTag___OjGFy.typography-module__xdsBody2___RNdGY";
            const string normalPrice = "span.Price-module__originalPrice___XNCxs";
            const string originalPriceSelector = "span.Price-module__boldText___1i2Li.Price-module__moreText___sNMVr.ProductCard-module__price___cs1xr";
            const string precompraTitle = "span.ProductCard-module__singleLineTitle___32jUF.typography-module__xdsBody2___RNdGY";

           
            new DriverManager().SetUpDriver(new ChromeConfig());
            var options = new ChromeOptions();
            options.AddArgument("--headless");

            using var driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl(url);

            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(30));
            wait.Until(d => d.FindElements(By.CssSelector(gameItemSelector)).Count > 0);

            int clicks = 0, maxClicks = 3;

            while (clicks < maxClicks)
            {
                try
                {
                    var button = driver.FindElement(By.XPath("//button[contains(., 'Cargar más')]"));
                    button.Click();
                    wait.Until(d => d.FindElements(By.CssSelector(gameItemSelector)).Count > (25 * (clicks + 1)));
                    Thread.Sleep(new Random().Next(1000, 2000));
                    clicks++;
                }
                catch { break; }
            }

            var juegos = new List<Game>();
            var elementos = driver.FindElements(By.CssSelector(gameItemSelector));

            foreach (var elemento in elementos)
            {
                try
                {
                    var imagenUrl = elemento.FindElement(By.CssSelector("img")).GetAttribute("src");
                    var linkGameXbox = elemento.FindElement(By.CssSelector("a")).GetAttribute("href");
                    string nombre = "";
                    bool esPrecompra = false;

                    try
                    {
                        var precompraText = elemento.FindElement(By.XPath("//span[contains(text(),'PEDIR POR ADELANTADO')]")).Text;
                        if (precompraText.Contains("ADELANTADO"))
                        {
                            nombre = elemento.FindElement(By.CssSelector(precompraTitle)).Text;
                            esPrecompra = true;
                        }
                    }
                    catch
                    {
                        nombre = elemento.FindElement(By.CssSelector(titleSelector)).Text;
                    }

                    int precio = 0, descuento = 0;

                    try
                    {
                        var descuentoText = elemento.FindElement(By.CssSelector(discountTagSelector)).Text;
                        descuento = PrecioHelper.ConvertirDescuento(descuentoText);

                        if (descuento > 0)
                        {
                            var precioOriginalText = elemento.FindElement(By.CssSelector(normalPrice)).Text;
                            precio = PrecioHelper.ConvertirPrecioATotalPesos(precioOriginalText);
                        }
                        else
                        {
                            var precioText = elemento.FindElement(By.CssSelector(originalPriceSelector)).Text;
                            precio = PrecioHelper.ConvertirPrecioATotalPesos(precioText);
                        }
                    }
                    catch
                    {
                        var precioText = elemento.FindElement(By.CssSelector(originalPriceSelector)).Text;
                        precio = PrecioHelper.ConvertirPrecioATotalPesos(precioText);
                    }

                    juegos.Add(new Game
                    {
                        Name = nombre,
                        Image = imagenUrl,
                        Link = linkGameXbox,
                        Price = precio,
                        Discount = descuento,
                        EsPrecompra = esPrecompra
                    });
                }
                catch
                {
                    continue;
                }
            }

            return juegos;
        }
    }
}
