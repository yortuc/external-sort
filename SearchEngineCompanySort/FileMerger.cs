using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SearchEngineCompanySort {
    public class FileMerger {
        /// <summary>
        /// Create a batch of file names from given file names respect to memory limitations
        /// For example, if we have 20 files and 12 is the memory limit, 
        /// iterator returns 12 file names at first, then 8 file names creating 2 batches in total
        /// </summary>
        /// <returns>
        /// A batch of file names which can be read in parallel
        /// </returns>
        public IEnumerable<List<string>> GetFilesBatch(List<string> fileNames, int memLimit){
            var readFileCount = Math.Min(fileNames.Count, memLimit);
            int i=0;

            while(i < fileNames.Count()){
                var takeFilesCount = (i + readFileCount <= fileNames.Count) ? readFileCount : fileNames.Count() - i;
                List<string> filesInBatch = fileNames.GetRange(i, takeFilesCount);
                yield return filesInBatch;
                i += readFileCount;
            }
        }

        public string MergeSortedFiles(List<string> fileNames, int memLimit, IChunkWriter resultWriter) {
            return MergeSortedFilesRec(fileNames, memLimit, resultWriter)[0];
        }

        public virtual IChunkReader CreateChunkReaderFromFile(string fileName){
            return new ChunkReader(fileName);
        }

        /// <summary>
        /// Recursively merges given sorted small files to form a sorted final output
        /// </summary>
        /// <returns>
        /// Final sorted output file path
        /// </returns>
        private List<string> MergeSortedFilesRec(List<string> fileNames, int memLimit, IChunkWriter resultWriter) {
            
            var mergedFileNames = new List<string>();

            foreach(var batch in GetFilesBatch(fileNames, memLimit)){
                List<IChunkReader> chunkReaders = batch.Select(f => CreateChunkReaderFromFile(f)).ToList();
            
                // if there is only one file in the batch, there is no need to merge
                if(chunkReaders.Count == 1) {
                    mergedFileNames.Add(chunkReaders[0].GetFileName());
                } else {
                    MergeSortedChunks(chunkReaders, resultWriter);
                    mergedFileNames.Add(resultWriter.GetFileName());
                }
            }

            if(mergedFileNames.Count > 1){
                return MergeSortedFilesRec(mergedFileNames, memLimit, resultWriter);
            } else {
                return mergedFileNames;
            }
        }
        
        /// <summary>
        /// Applies k-way merge sort on ChunkReader queues and writes sorted output.
        /// Each ChunkReader holds only one search term in memory at any time.
        /// After writing to output, ChunkReader reads the next line from intermediary file until the eof.
        /// If the file reaches to end, ChunkReader is removed
        /// </summary>
        public void MergeSortedChunks(List<IChunkReader> chunkReaders, IChunkWriter resultWriter){
            resultWriter.NewFile();

            chunkReaders.ForEach(c => c.Read());

            while(chunkReaders.Count > 0) {
                // uses Array.Sort (QuickSort which is in-place and uses no extra memory)
                chunkReaders.Sort(new ChunkReaderComparer());

                resultWriter.WriteLine(chunkReaders[0].GetValue());
                chunkReaders[0].Read();

                if(chunkReaders[0].GetValue() == null){
                    chunkReaders.RemoveAt(0);
                }
            }
            resultWriter.Close();
        }
    }
}