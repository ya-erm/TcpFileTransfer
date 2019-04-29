using System;
using System.Linq;
using System.Collections.Generic;

namespace TCPClient.Models
{
    public class SpeedModel : AModel
    {
        /// <summary>
        /// Значение скорости (байт/сек)
        /// </summary>
        public double Value { get => Get<double>(); private set => Set(value); }

        /// <summary>
        /// Значение для отображения (Кб/сек, Мб/сек, и т.д.)
        /// </summary>
        [DependsOn(nameof(Value))]
        public string PrettyValue {
            get {
                int pow = 0;
                var value = Value;
                while (value > 1024)
                {
                    value /= 1024;
                    pow++;
                }
                return $"{value:0.00} {SpeedLabels[pow]}/сек";
            }
        }

        static readonly string[] SpeedLabels = { "байт", "Кб", "Мб", "Гб", "Тб", "Пб" };

        /// <summary>
        /// Добавить точку расчета
        /// </summary>
        /// <param name="time">Время</param>
        /// <param name="value">Кол-во байт</param>
        public void AddPoint(TimeSpan time, long value)
        {
            values.Add((time, value));

            const int n = 5;
            var lasts = values.Skip(Math.Max(0, values.Count - n)).ToArray();
            if (lasts.Length > 1)
            {
                var diffs = lasts.Select((x, i) => i > 0 ? x.Item2 - lasts[i - 1].Item2 : 0).Skip(1).ToArray();
                Value = diffs.Average() / (lasts.Last().Item1 - lasts.First().Item1).TotalSeconds;
            }
            else
            {
                Value = value;
            }
        }

        /// <summary>
        /// Сбросить расчёт
        /// </summary>
        public void Clear()
        {
            values.Clear();
            Value = 0;
        }

        List<(TimeSpan, long)> values = new List<(TimeSpan, long)>();

    }
}
