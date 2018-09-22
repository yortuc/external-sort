using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SearchEngineCompanySort {
    public class FileSplitter {
        
        /// <summary>
        /// Splits each given file into smaller files with sorted content
        /// </summary>
        /// <returns>
        /// Created temporary file paths
        /// </returns>
        public List<string> SplitFiles(string[] fileNames, int memLimit){
            List<string> createdfileNames = new List<string>();
            foreach(var fn in fileNames){
                SplitStreamIntoSortedFiles(new StreamReader(fn), memLimit, new ChunkWriter()).ForEach(f => createdfileNames.Add(f));
            }
            return createdfileNames;
        }

        /// <summary>
        /// Split given TextReader stream into small chunks and save to disk
        /// The size of each read from given Stream depends on the memory limit
        /// </summary>
        /// <returns>
        /// Created temporary file paths
        /// </returns>
        public List<string> SplitStreamIntoSortedFiles(TextReader sr, int memLimit, IChunkWriter chunkWriter){
            var createdfileNames = new List<string>();
            var buffer = new string[memLimit];
            string line;
            var readLines = 0;

            while (true) {
                line = sr.ReadLine();

                if(line == null){
                    Array.Sort(buffer); // uses QuickSort O(nlogn) in average
                    createdfileNames.Add(chunkWriter.SaveChunk(buffer));
                    buffer = new string[memLimit];
                    break;
                }
                else{
                    if(readLines == memLimit){
                        Array.Sort(buffer); // uses QuickSort O(nlogn) in average
                        createdfileNames.Add(chunkWriter.SaveChunk(buffer));
                        buffer = new string[memLimit];
                        buffer[0] = line;
                        readLines = 1;
                    } else {
                        buffer[readLines] = line;
                        readLines++;
                    }    
                }
            }
            sr.Close();
            return createdfileNames;
        }
    }
}