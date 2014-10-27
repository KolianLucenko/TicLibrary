using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolGameLibrary
{
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
        /// Побед в среднем режиме
        /// </summary>
        public int WinNormal { get; set; }
        /// <summary>
        /// Проигрышей в среднем режиме
        /// </summary>
        public int OverNormal { get; set; }
        /// <summary>
        /// Побед в сложном режиме
        /// </summary>
        public int WinHard { get; set; }
        /// <summary>
        /// Проигрышей в сложном режиме
        /// </summary>
        public int OverHard { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public Statistic()
        {

        }

        public override string ToString()
        {
            return string.Format("Побед в легком режиме: {0}\nПроигрышей в легком режиме : {1}\n-----------------------------\nПобед в среднем режиме: {2}\nПроигрышей в среднем режиме режиме : {2}\n-----------------------------\nПобед в тяжелом режиме: {4}\nПроигрышей в тяжелом режиме : {5}\n-----------------------------\n",
                this.WinEase,this.OverEase,this.WinNormal,this.OverNormal,this.WinHard,this.OverHard);
        }
    }
}
