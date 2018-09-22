using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace SearchEngineCompanySort {

    /// <summary>
    /// Compares ChunkReaders with their current value
    /// </summary>
    class ChunkReaderComparer: IComparer<IChunkReader> {
        public int Compare(IChunkReader p1, IChunkReader p2){
            return string.Compare(p1.GetValue(), p2.GetValue());
        }
    }
    
}