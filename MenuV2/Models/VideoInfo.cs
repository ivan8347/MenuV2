using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuV2.Models
{
    public class VideoInfo
    {
        // youtube / vk / instagram
        public string Platform { get; set; }

        // Заголовок видео (если есть)
        public string Title { get; set; }

        // Описание (VK и YouTube дают, Instagram — нет)
        public string Description { get; set; }

        // Прямая ссылка на видео (mp4 или player)
        public string VideoUrl { get; set; }

        // Превью (thumbnail)
        public string ImageUrl { get; set; }
    }
}
