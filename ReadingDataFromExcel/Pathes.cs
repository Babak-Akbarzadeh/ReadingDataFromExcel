using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ReadingDataFromExcel
{
    public class Pathes
    {
        public string OutPutLocation;
        public string OutPutGr;
        public string ColumnLoc;

        public string InstanceLocation;
        public string ExprimentLocation;
        public string InsGroupLocation;
        public string CurrentDir;
        public Pathes() { }
        public Pathes(string Expriment, string Methodology, string InsGroupName)
        {

            CurrentDir = System.IO.Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + Expriment + Path.DirectorySeparatorChar;

            InstanceLocation = CurrentDir + "Instance" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(InstanceLocation))
            {
                Directory.CreateDirectory(InstanceLocation);
            }
            if (InsGroupName != "")
            {
                InsGroupLocation = CurrentDir + "Instance" + Path.DirectorySeparatorChar + InsGroupName + Path.DirectorySeparatorChar;
                if (!Directory.Exists(InsGroupLocation))
                {
                    Directory.CreateDirectory(InsGroupLocation);
                }
            }
            if (Methodology != "")
            {
                ExprimentLocation = CurrentDir + Methodology + Path.DirectorySeparatorChar;
                if (!Directory.Exists(ExprimentLocation))
                {
                    Directory.CreateDirectory(ExprimentLocation);
                }
                OutPutLocation = ExprimentLocation + "Result" + Path.DirectorySeparatorChar;
                if (!Directory.Exists(OutPutLocation))
                {
                    Directory.CreateDirectory(OutPutLocation);
                }
                OutPutGr = ExprimentLocation + InsGroupName + Path.DirectorySeparatorChar;
                if (!Directory.Exists(OutPutLocation))
                {
                    Directory.CreateDirectory(OutPutGr);
                }
                ColumnLoc = ExprimentLocation + InsGroupName + Path.DirectorySeparatorChar + "Column" + Path.DirectorySeparatorChar;
                if (!Directory.Exists(ColumnLoc))
                {
                    Directory.CreateDirectory(ColumnLoc);
                }
            }

        }
    }
}
