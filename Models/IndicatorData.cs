using System;
using System.Collections.Generic;

namespace Processo_Seletivo.Models {
    public class IndicatorData {
        public DateTime Date { get; set; }
        public string IndicatorType { get; set; }
        public double Value { get; set; }
    }

    public class Rootobject {
        public List<IndicatorData> value { get; set; }
    }
}
