using System;
using System.IO;

namespace SearchEngineCompanySort {

    public class ProgramArgs { 
        public string[] FileNames {get; set;} 
        public int MemoryLimit {get;set;}

        /// <summary>
        /// Parses given command line arguments into typed program arguments with validation
        /// </summary>
        public static ProgramArgs ParseFromCommandLine(string[] args){
            int memoryLimit;
            string[] fileNames = new string[args.Length-1];
            
            bool memLimitParse = int.TryParse(args[0], out memoryLimit);

            if(args.Length < 2 || !memLimitParse) {
                throw new ArgumentException("Plesea provide a memory limit and files to parse as arguments. \nExample: dotnet run 32 log1.txt log2.txt");
            }
            if(memoryLimit < 2) throw new ArgumentException("Memory limit cannot be lower than 2");
            
            for(var i=1; i<args.Length; i++){
                fileNames[i-1] = args[i];
            }
            return new ProgramArgs { 
                FileNames = fileNames, 
                MemoryLimit = memoryLimit 
            };
        }
    }
}