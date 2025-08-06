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

            int clicks = 0, maxClicks = 160;

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

        /*
         // Selectores
            const string gameItemSelector = "div.ProductCard-module__cardWrapper___6Ls86.shadow";
            const string titleSelector = "span.ProductCard-module__title___nHGIp.typography-module__xdsBody2___RNdGY";
            const string discountTagSelector = "div.ProductCard-module__discountTag___OjGFy.typography-module__xdsBody2___RNdGY";

            //precio cuando hay descuento
            const string normalPrice = "span.Price-module__originalPrice___XNCxs";
            //precio cuando no hay descuento
            const string originalPriceSelector = "span.Price-module__boldText___1i2Li.Price-module__moreText___sNMVr.ProductCard-module__price___cs1xr";
            //pre compra
            const string precompraTitle = "span.ProductCard-module__singleLineTitle___32jUF.typography-module__xdsBody2___RNdGY";

            new DriverManager().SetUpDriver(new ChromeConfig());
            var options = new ChromeOptions();
            //options.AddExcludedArgument("enable-logging");  // Desactiva logs de USB
            //options.AddArgument("--disable-blink-features=AutomationControlled");
            // Comentá esto si querés ver la ventana del navegador
            // options.AddArgument("--headless");
            using var driver = new ChromeDriver(options);
            

            driver.Navigate().GoToUrl("https://www.xbox.com/es-AR/games/all-games/pc?PlayWith=PC");


            // Esperar al menos un producto
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(d => d.FindElements(By.CssSelector(gameItemSelector)).Count > 0);
            var juegos = new List<Game>();

            //al tener que precionar un boton para ver mas juegos creamos esta parte
            //preguta si existe el boton, en caso de que exista se lo aprieta y esperamos a que carguen los juegos 
            //despues contamos la cantidad de juegos totales visibles
            int clicksRealizados = 0;
            int maxClicks = 1; // 12,000 juegos / 25 por clic = ~480 clics necesarios

            while (clicksRealizados < maxClicks)
            {
                try
                {
                    // Localizar el botón "Cargar más" con espera explícita

 
                    var button = driver.FindElement(By.XPath("//button[contains(., 'Cargar más')]"));
                    button.Click(); // Intenta primero el método estándar

                    clicksRealizados++;
                    Console.WriteLine($"Clic #{clicksRealizados} - Total juegos: {driver.FindElements(By.CssSelector(gameItemSelector)).Count}");

                    // Esperar a que nuevos juegos se agreguen al DOM
                    wait.Until(d =>
                    {
                        int juegosVisibles = d.FindElements(By.CssSelector(gameItemSelector)).Count;
                        return juegosVisibles >= 25 + (25 * clicksRealizados); // 25 iniciales + 25 por clic
                    });

                    // Pausa aleatoria entre 1-3 segundos para evitar bloqueos
                    Thread.Sleep(new Random().Next(1000, 3000));
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("¡Botón 'Ver más' no encontrado! Fin de la carga.");
                    break;
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("Timeout esperando nuevos juegos. Continuando...");
                    break;
                }
            }
            

            var elementosJuegos = driver.FindElements(By.CssSelector(gameItemSelector));

            foreach (var elemento in elementosJuegos)
            {
                try
                {
                    var imagenUrl = elemento.FindElement(By.CssSelector("img")).GetAttribute("src");
                    
                    var gameLink = elemento.FindElement(By.CssSelector("a")).GetAttribute("href");
                    
                    int precio = 0, precioConDescuento = 0, descuento = 0;
                    bool preCompra = false;
                    string nombre = "";

                    try
                    {
                        var precompraText = driver.FindElement(By.XPath("//span[contains(text(),'PEDIR POR ADELANTADO')]")).Text;


                        if(precompraText.Equals("PEDIR POR ADELANTADO"))
                        {
                           // Console.WriteLine("SI HAY PRECOMPRA");
                            preCompra = true;
                            nombre = elemento.FindElement(By.CssSelector(precompraTitle)).Text;
                        }

                    }
                    catch (NoSuchElementException)
                    {
                         nombre = elemento.FindElement(By.CssSelector(titleSelector)).Text;
                         preCompra = false;
                    }



                    try
                    {
                        var precioOriginalText = elemento.FindElement(By.CssSelector(originalPriceSelector)).Text;
                        precio = ConvertirPrecioATotalPesos(precioOriginalText);
                    }
                    catch (NoSuchElementException)
                    {
                        
                    }

                    try
                    {

                        var descuentoText = elemento.FindElement(By.CssSelector(discountTagSelector)).Text;
                        descuento = ConvertirDescuento(descuentoText);

                        if (descuento > 0)
                        {

                            var precioOriginalText = elemento.FindElement(By.CssSelector(normalPrice)).Text;
                            precio = ConvertirPrecioATotalPesos(precioOriginalText);

                            var precioDescuentoText = elemento.FindElement(By.CssSelector(originalPriceSelector)).Text;
                            precioConDescuento = ConvertirPrecioATotalPesos(precioDescuentoText);

                        }

                    }
                    catch (NoSuchElementException)
                    { 
                        
                    }              

                    var game = new Game
                    {
                        Nombre = nombre,
                        ImagenUrl = imagenUrl,
                        Link = gameLink,
                        Precio = precio,
                        Descuento = descuento,
                        EsPrecompra = preCompra
                    };

                    juegos.Add(game);

                    Console.WriteLine($"Nombre: {nombre} | Precio: {precio}$ | Descuento: {descuento}% | Con Descuento: {precioConDescuento} | Es pre-compra: {gameLink} ");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al procesar un producto: " + ex.Message);
                } 
           }
         */
    }
}
