using System.Runtime.InteropServices;
using System.Text;

namespace MMSINC.GIS
{
    public class CorpsCon
    {
        public int InUnits
        {
            get { return GetInUnits(); }
            set { SetInUnits(value); }
        }

        public CorpsCon()
        {

        }

        public static string getErrorMessage(int error_code)
        {
            StringBuilder msg = new StringBuilder();
            msg.Length = 2000; //This is not right. Will error if returned values is more than 2000 characters.
            GetErrorMessage(error_code, msg);
            corpscon_clean_up();
            return msg.ToString();
         }

        public double[] convertNJPoint(double Northing, double Easting)
        {
            double[] dblResults = new double[3];
            corpscon_default_config();
            SetNadconPath("c:\\program files\\Corpscon6x\\Nadcon\\".ToCharArray());
            SetVertconPath("c:\\program files\\Corpscon6x\\vertcon\\".ToCharArray());
            SetGeoidPath("c:\\program files\\Corpscon6x\\Geoid\\".ToCharArray());
            
            SetInSystem(2); 
            SetInDatum(1983);
            SetInUnits(1);
            SetInZone(2900);

            SetOutSystem(1);
            SetOutDatum(1983);
            SetOutUnits(3);
            SetOutZone(18);

            corpscon_initialize_convert();

            SetXIn(Easting);
            SetYIn(Northing);
            int result = corpscon_convert();

            dblResults[0] = GetYOut();
            dblResults[1] = GetXOut();
            dblResults[2] = result;
            corpscon_clean_up();
            return dblResults;
        }

        [DllImport("corpscon_v6.dll")]
        static extern int corpscon_default_config();

        [DllImport("corpscon_v6.dll")]
        static extern int SetInSystem(int val);
        [DllImport("corpscon_v6.dll")]
        static extern int SetOutSystem(int val);

        [DllImport("corpscon_v6.dll")]
        static extern int SetInDatum(int val);
        [DllImport("corpscon_v6.dll")]
        static extern int SetOutDatum(int val);

        [DllImport("corpscon_v6.dll")]
        static extern int SetInUnits(int val);
        [DllImport("corpscon_v6.dll")]
        static extern int SetOutUnits(int val);

        [DllImport("corpscon_v6.dll")]
        static extern int SetInZone(int val);
        [DllImport("corpscon_v6.dll")]
        static extern int SetOutZone(int val);

        [DllImport("corpscon_v6.dll")]
        static extern int SetInVDatum(int val);
        [DllImport("corpscon_v6.dll")]
        static extern int SetOutVDatum(int val);

        [DllImport("corpscon_v6.dll")]
        static extern int SetInVUnits(int val);
        [DllImport("corpscon_v6.dll")]
        static extern int SetOutVUnits(int val);

        [DllImport("corpscon_v6.dll")]
        static extern int SetOutUSNGDigits(int val);

        [DllImport("corpscon_v6.dll")]
        static extern int SetNadconPath(char[] path);

        [DllImport("corpscon_v6.dll")]
        static extern int SetInHPGNArea(char[] area);
        [DllImport("corpscon_v6.dll")]
        static extern int SetOutHPGNArea(char[] area);

        [DllImport("corpscon_v6.dll")]
        static extern int SetVertconPath(char[] path);

        [DllImport("corpscon_v6.dll")]
        static extern int SetUseVertconCustomAreas(int opt);

        [DllImport("corpscon_v6.dll")]
        static extern int SetVertconCustomAreaListFile(char[] file);

        [DllImport("corpscon_v6.dll")]
        static extern int SetGeoidPath(char[] path);
        [DllImport("corpscon_v6.dll")]
        static extern int SetGeoidCodeBase(int val);
        [DllImport("corpscon_v6.dll")]
        static extern int SetUseGeoidCustomAreas(int opt);
        [DllImport("corpscon_v6.dll")]
        static extern int SetGeoidCustomAreaListFile(char[] file);
        [DllImport("corpscon_v6.dll")]
        static extern int corpscon_initialize_convert();
        [DllImport("corpscon_v6.dll")]

        static extern int SetXIn(double val);
        [DllImport("corpscon_v6.dll")]
        static extern int SetYIn(double val);
        [DllImport("corpscon_v6.dll")]
        static extern int SetZIn(double val);

        [DllImport("corpscon_v6.dll")]
        static extern int SetUSNGIn(char[] val);
        [DllImport("corpscon_v6.dll")]
        static extern int corpscon_convert();
        [DllImport("corpscon_v6.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        static extern int GetErrorMessage(int err_code, StringBuilder msg);
        [DllImport("corpscon_v6.dll")]

        static extern double GetXOut();
        [DllImport("corpscon_v6.dll")]
        static extern double GetYOut();
        [DllImport("corpscon_v6.dll")]
        static extern double GetZOut();

        [DllImport("corpscon_v6.dll")]
        static extern int GetUSNGOut(char[] val);
        [DllImport("corpscon_v6.dll")]
        static extern int GetCorpsconValue(int code, double[] val);
        [DllImport("corpscon_v6.dll")]
        static extern int corpscon_clean_up();

        
        [DllImport("corpscon_v6.dll")]
        static extern int GetInSystem();
        [DllImport("corpscon_v6.dll")]
                static extern int GetOutSystem();
        [DllImport("corpscon_v6.dll")]
                static extern int GetInDatum();
        [DllImport("corpscon_v6.dll")]
                static extern int GetOutDatum();
        [DllImport("corpscon_v6.dll")]
                static extern int GetInUnits();
        [DllImport("corpscon_v6.dll")]
                static extern int GetOutUnits();
        [DllImport("corpscon_v6.dll")]
                static extern int GetInZone();
        [DllImport("corpscon_v6.dll")]
                static extern int GetOutZone();
        [DllImport("corpscon_v6.dll")]
                static extern int GetInVDatum();
        [DllImport("corpscon_v6.dll")]
                static extern int GetInVUnits();
        [DllImport("corpscon_v6.dll")]
                static extern int GetOutVUnits();
        [DllImport("corpscon_v6.dll")]
                static extern int GetOutUSNGDigits();
        [DllImport("corpscon_v6.dll")]
                static extern int GetNadconPath(char[] path);
        [DllImport("corpscon_v6.dll")]
                static extern int GetInHPGNArea(char[] area);
        [DllImport("corpscon_v6.dll")]
                static extern int GetOutHPGNArea(char[] area);
        [DllImport("corpscon_v6.dll")]
                static extern int GetVertconPath(char[] path);
        [DllImport("corpscon_v6.dll")]
                static extern int GetVertconCustomAreaListFile(char[] filename);
        [DllImport("corpscon_v6.dll")]
                static extern int GetUseVertconCustomAreas();
        [DllImport("corpscon_v6.dll")]
                static extern int GetGeoidCodeBase();
        [DllImport("corpscon_v6.dll")]
                static extern int GetGeoidPath(char[] path);
        [DllImport("corpscon_v6.dll")]
                static extern int GetGeoidCustomAreaListFile(char[] filename);
        [DllImport("corpscon_v6.dll")]
                static extern int GetUseGeoidCustomAreas();

    }
}
