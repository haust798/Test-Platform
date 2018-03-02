﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Resources;

namespace TestPlatform.Models
{
    /*
     * Class that represents results found while executing a test program (ReactionProgram) and stores data from a partincipant test
     *
     */
    class ReactionTest
    {
        private static String headerOutputFileText;
        private Char mark;
        private ReactionProgram programInUse = new ReactionProgram();
        private string participantName;
        private DateTime initialTime;
        private DateTime expositionTime;
        private List<string> output = new List<string>();
        // properties used to localize strings during runtime
        private ResourceManager LocRM = new ResourceManager("TestPlatform.Resources.Localizations.LocalizedResources", typeof(FormMain).Assembly);
        private CultureInfo currentCulture = CultureInfo.CurrentUICulture;
        private string currentResponse;

        public ReactionTest()
        {
            headerOutputFileText = LocRM.GetString("reactionTestHeader", currentCulture);
        }

        public string ParticipantName
        {
            get
            {
                return participantName;
            }

            set
            {
                participantName = value;
            }
        }

        public DateTime InitialTime
        {
            get
            {
                return initialTime;
            }

            set
            {
                initialTime = value;
            }
        }

        public List<string> Output
        {
            get
            {
                return output;
            }

            set
            {
                output = value;
            }
        }

        public static string HeaderOutputFileText
        {
            get
            {
                return headerOutputFileText;
            }

            set
            {
                headerOutputFileText = value;
            }
        }

        internal ReactionProgram ProgramInUse
        {
            get
            {
                return programInUse;
            }

            set
            {
                programInUse = value;
            }
        }

        public char Mark
        {
            get
            {
                return mark;
            }

            set
            {
                mark = value;
            }
        }

        public DateTime ExpositionTime
        {
            get
            {
                return expositionTime;
            }

            set
            {
                expositionTime = value;
            }
        }

        public string CurrentResponse
        {
            get
            {
                return currentResponse;
            }

            set
            {
                currentResponse = value;
            }
        }

        public void setProgramInUse(string path, string prgName)
        {
            string programFile = path + prgName + ".prg";
            if (File.Exists(programFile))
            {
                ProgramInUse = new ReactionProgram(programFile);
            }
            else
            {
                throw new Exception(LocRM.GetString("file", currentCulture) + ProgramInUse.ProgramName + ".prg" +
                                    LocRM.GetString("notFoundIn", currentCulture) + Path.GetDirectoryName(path + "/prg/"));
            }
        }

        private string expositionTypeOutput()
        {
            switch (ProgramInUse.ExpositionType)
            {
                case "shapes":
                    return LocRM.GetString("shape", currentCulture);
                case "words":
                    return LocRM.GetString("word", currentCulture);
                case "images":
                    return LocRM.GetString("image", currentCulture);
                case "imageAndWord":
                    return LocRM.GetString("imageAndWord", currentCulture);
                case "wordWithAudio":
                    return LocRM.GetString("wordWithAudio", currentCulture);
                case "imageWithAudio":
                    return LocRM.GetString("imageWithAudio", currentCulture);
                default:
                    return LocRM.GetString("invalidExpoType", currentCulture);
            }
        }

        public void writeLineOutput(long intervalTime, long intervalShouldBe, long reactTime, int currentExposition, long expositionAccumulative, string[] currentStimuli, string position, bool beeped, string currentColor)
        {
            /* This variable keeps data from an exposition to only one stimulus, being them:
             * programa\tparticipante\tdata\thorarioInicial\thorarioExposicao\ttr(ms)\tintervalo(ms)\tintervaloestimado(ms)\texposicaoTempo(ms)\texposicao(ms)\tsequencia\tpos\trespostaUsuario\ttipoEstimulo\tEstimulo\tCordoEstimulo\tBeep
             * program\tparticipant\tdate\tInitialTime\texpositionTime\treactionTime(ms)\tInterval(ms)\testimatedInterval(ms)\texpositionDuration(ms)\texposition(ms)\tsenquency\tpos\tuserResponse\tstimulusType\tstimulus\tstimulusColor\tBeep
             * program  name    participant     name    date    hour    exposition hour    hit time(ms) interval(ms)  interval should be(ms)  
             * exposition accumulative timeexposition time(ms)  number of sequency  position on screen  user response   type of stimulus    stimulus1 stimulus2   
             * stimulus color */
            var text = ProgramInUse.ProgramName + "\t" + participantName + "\t" + initialTime.Day + "/" +
                       initialTime.Month + "/" + initialTime.Year + "\t" + initialTime.Hour + ":" + initialTime.Minute +
                       ":" + initialTime.Second + ":" + initialTime.Millisecond.ToString() + "\t" + ExpositionTime.Hour + ":" + ExpositionTime.Minute +
                       ":" + ExpositionTime.Second + ":" + ExpositionTime.Millisecond.ToString() + "\t" + reactTime.ToString() +
                        "\t" + intervalTime.ToString() + "\t" + intervalShouldBe.ToString() + "\t" + expositionAccumulative + "\t" +
                        ProgramInUse.ExpositionTime +  "\t" + currentExposition + "\t" + position + "\t" +  CurrentResponse  + "\t"+ expositionTypeOutput() + "\t" +
                        currentStimuli[0] + "\t" + currentStimuli[1] + "\t" + currentColor + "\t" + beeped.ToString();
             Output.Add(text); 
             
        }


    }
}
