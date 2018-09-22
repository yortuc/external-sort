using System;
using System.IO;

namespace SearchEngineCompanySort {
    class Program {
        /// <summary>
        /// Command line arguments
        /// [memoryLimit] [log_file_1] ... [log_file_n]
        /// </summary>
        static void Main(string[] args) {      
            DateTime startTime = DateTime.Now;

            var progArgs = ProgramArgs.ParseFromCommandLine(args);
            var reader = new FileSplitter();
            var splittedFileNames = reader.SplitFiles(progArgs.FileNames, progArgs.MemoryLimit);
            
            var merger = new FileMerger();
            string finalSortedFilePath = merger.MergeSortedFiles(
                fileNames: splittedFileNames, 
                memLimit: progArgs.MemoryLimit, 
                resultWriter: new ChunkWriter()
            );

            Double elapsedMillisecs = ((TimeSpan)(DateTime.Now - startTime)).TotalMilliseconds;

            Console.WriteLine("Job finished in " + elapsedMillisecs.ToString() + " ms");
            Console.WriteLine("Final sorted file path:");
            Console.WriteLine(finalSortedFilePath);
        }
    }
}
