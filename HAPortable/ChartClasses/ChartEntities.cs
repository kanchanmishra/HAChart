using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HAPortable
{
    public class X
    {
        public string name { get; set; }
        public bool scale { get; set; }
        public bool labels { get; set; }
        public int major { get; set; }
    }

    public class Y
    {
        public string name { get; set; }
        public bool labels { get; set; }
        public bool scale { get; set; }
        public int major { get; set; }
        public object minor { get; set; }
    }

    public class Axes
    {
        public X x { get; set; }
        public Y y { get; set; }
    }

    public class YRange
    {
        public string name { get; set; }
        public string min { get; set; }
        public string max { get; set; }
        public string color { get; set; }
    }

    public class XRange
    {
        public string name { get; set; }
        public double min { get; set; }
        public int max { get; set; }
    }

    public class Ranges
    {
        public IList<YRange> y { get; set; }
        public IList<XRange> x { get; set; }
    }

    public class Datum
    {
        public int x { get; set; }
        public string y { get; set; }
        public string label { get; set; }
    }

    public class Annotation
    {
        public string label { get; set; }
        public string text { get; set; }
        public int is_value_annotation { get; set; }
    }

    public class Historic
    {
        public string name { get; set; }
        public string report_question_name { get; set; }
        public string graph { get; set; }
        public string latest_value { get; set; }
        public Axes axes { get; set; }
        public Ranges ranges { get; set; }
        public IList<Datum> data { get; set; }
        public IList<Annotation> annotations { get; set; }
        public string data_source_form_question_version_id { get; set; }
        public int type_of_graph { get; set; }
        public IList<string> legend { get; set; }
        public string annotation { get; set; }
    }

    public class BodyFat
    {
        public Historic historic { get; set; }
    }

    public class Bmi
    {
        public Historic historic { get; set; }
    }

    public class BodyComposition
    {
        [JsonProperty("body_fat")]
        public BodyFat body_fat { get; set; }
        [JsonProperty("bmi")]
        public Bmi bmi { get; set; }
    }

    public class Graphs
    {
        [JsonProperty("body_composition")]
        public BodyComposition body_composition { get; set; }

        [Newtonsoft.Json.Serialization.OnError]
        internal void OnError(System.Runtime.Serialization.StreamingContext context, Newtonsoft.Json.Serialization.ErrorContext errorContext)
        {
            errorContext.Handled = true;
        }
    }

    public class HAReport
    {
        [JsonProperty("graphs")]
        public Graphs graphs { get; set; }

        [Newtonsoft.Json.Serialization.OnError]
        internal void OnError(System.Runtime.Serialization.StreamingContext context, Newtonsoft.Json.Serialization.ErrorContext errorContext)
        {
            errorContext.Handled = true;
        }
    }

}
