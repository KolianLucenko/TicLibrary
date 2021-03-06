﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolGameLibrary
{
    [Serializable]
    /// <summary>
    /// Класс для содержания в себе статистики игрока
    /// </summary>
    public class Statistic
    {
        /// <summary>
        /// Побед в легком режиме
        /// </summary>
        public int WinEase { get; set; }
        /// <summary>
        /// Проигрышей в легком режиме
        /// </summary>
        public int OverEase { get; set; }
        /// <summary>
        /// Ничья в легком режиме
        /// </summary>
        public int DrawEase { get; set; }
        /// <summary>
        /// Побед в среднем режиме
        /// </summary>
        public int WinNormal { get; set; }
        /// <summary>
        /// Проигрышей в среднем режиме
        /// </summary>
        public int OverNormal { get; set; }
        /// <summary>
        /// Ничья в среднем режиме
        /// </summary>
        public int DrawNormal { get; set; }
        /// <summary>
        /// Побед в сложном режиме
        /// </summary>
        public int WinHard { get; set; }
        /// <summary>
        /// Проигрышей в сложном режиме
        /// </summary>
        public int OverHard { get; set; }
        /// <summary>
        /// Ничья в тяжелом режиме
        /// </summary>
        public int DrawHard { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public Statistic()
        {

        }

        public override string ToString()
        {
            return string.Format("----------Легкоий режим----------\nПобед:               {0}\nПоражений:         {1}\nНичья:               {2}\n----------Cредний режим----------\nПобед:               {3}\nПоражений:         {4}\nНичья:               {5}\n----------Тяжелый режим----------\nПобед:               {6}\nПоражений:         {7}\nНичья:               {8}",
                this.WinEase,this.OverEase,this.DrawEase,this.WinNormal,this.OverNormal,this.DrawNormal,this.WinHard,this.OverHard,this.DrawHard);
        }
    }
}
