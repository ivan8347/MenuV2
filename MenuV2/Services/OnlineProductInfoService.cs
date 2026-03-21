using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuV2.Core;

namespace MenuV2.Services
{
    public static class OnlineProductInfoService
    {
        // Основной метод: получить продукт из интернета или кэша
        public static async Task<Product> GetProductInfoAsync(string name)
        {
            // 1. Проверяем кэш
            var cached = ProductStorage.Find(name);
            if (cached != null && ProductStorage.IsFresh(cached)) 
            { 
                return cached; 
            }

            // 2. Ищем в интернете
            var online = await FetchFromInternetAsync(name);
            if (online != null)
            {
                online.UpdatedAt = DateTime.Now;
                ProductStorage.AddOrUpdate(online);
                return online;
            }
            // 3. Если ничего не нашли — возвращаем null
            return null;
        }

            // Заглушка — здесь будет интернет‑логика
        private static async Task<Product> FetchFromInternetAsync(string name)
        {
            await Task.Delay(200); // имитация запроса

            // Пока возвращаем null — позже добавим реальный парсинг
            return null;
        }

    }

}

