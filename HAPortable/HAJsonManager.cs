using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace HAPortable
{
    public  class HAJsonManager
    {
        public  HAReport AppointmentFromJson()
        {
            var report = new HAReport();
            // deserialize JSON directly from a file
            using (StreamReader file = new System.IO.StreamReader(LoadResourceStream()))
            {
                var json = file.ReadToEnd();
                report = JsonConvert.DeserializeObject<HAReport>(json);

            }
            return report;
        }

        private   Stream LoadResourceStream()
        {

            #region How to load a text file embedded resource
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(LoadResourceText)).Assembly;
            // file is kept local under root folder
            Stream stream = assembly.GetManifestResourceStream("HAPortable.HealthAssessmentReport_DRCJ1248066_441820.json");
            return stream;
            #endregion
        }

        private  class LoadResourceText
        {
        }

    }
}
