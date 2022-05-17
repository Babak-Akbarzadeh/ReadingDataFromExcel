using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingDataFromExcel
{
    public class AlgorithmicSettings
    {
        // basic MIP
        public int timeLimit_MIP;


        // overall 
        public double epsilon;
        public double bigM;

        // fix and relax
        public double RF_magnifier;
        public int RF_windowSize; // size 
        public string RF_windowType; // how to select between x and y
        public double RF_overlap;
        public string RF_selection; // which type of decomposition
        public string RF_fixation;
        public int RF_timeLimit;
        public int RF_SubTimeLimit;




        // fix and optimize 
        public double FO_magnifier;
        public int FO_windowSize;
        public string FO_windowType;
        public double FO_overlap;
        public string FO_selection;
        public string FO_fixation;
        public int FO_timeLimit;
        public int FO_SubTimeLimit;


        // local branching
        public int LB_k;

        public AlgorithmicSettings() { }

        public AlgorithmicSettings(InstanceSettings settings)
        {
            // basic MIP
            timeLimit_MIP = settings.index_J * settings.index_K * settings.index_A;

            // overall
            epsilon = 0.0000000001;
            bigM = settings.index_J * settings.totalRegTimePerRoom;

            // fix and relax
            setRF_Knobes(settings, 0.8, 0.5);
            setRF_Settings("YthenX", "WL", "All0");

            // fix and optimize 
            setFO_Knobes(settings, 0.8, 0.5);
            setFO_Settings("YthenX", "Vertical", "All1");

            // local branching
            LB_k = settings.index_J / settings.index_A;
        }

        public void setRF_Knobes(InstanceSettings settings, double RF_magnifier, double RF_overlap)
        {
            this.RF_magnifier = RF_magnifier;
            RF_windowSize = (int)(RF_magnifier * settings.index_J / settings.index_A);
            this.RF_overlap = RF_overlap;
            RF_timeLimit = settings.index_J * settings.index_K;
            RF_SubTimeLimit = settings.index_J * settings.index_K / 10;
        }

        public void setRF_Settings(string RF_windowType, string RF_selection, string RF_fixation)
        {
            this.RF_windowType = RF_windowType;
            this.RF_selection = RF_selection;
            this.RF_fixation = RF_fixation;
        }

        public void setFO_Knobes(InstanceSettings settings, double FO_magnifier, double FO_overlap)
        {
            this.FO_magnifier = FO_magnifier;
            FO_windowSize = (int)(FO_magnifier * settings.index_J / settings.index_A);
            this.FO_overlap = FO_overlap;
            FO_timeLimit = settings.index_J * settings.index_K;
            FO_SubTimeLimit = settings.index_J * settings.index_K / 10;
        }

        public void setFO_Settings(string FO_windowType, string FO_selection, string FO_fixation)
        {
            this.FO_windowType = FO_windowType;
            this.FO_selection = FO_selection;
            this.FO_fixation = FO_fixation;
        }

    }
}
