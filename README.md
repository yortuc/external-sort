# Search Engine Compary Sort

#### Prerequisites

- Please install `dotnet core 2.1` [https://www.microsoft.com/net/download](https://www.microsoft.com/net/download)

#### Run on Windows/Mac/Linux

```
> dotnet run [memory_limit] [file_1] [file_2] ... [file_n] 
```

Example:
```
> cd SearchEngineCompanySort
> dotnet run 32 log1.txt log2.txt
```

#### Running tests
```
> cd Tests
> dotnet test
```

### How it works?

Program runs in two completely seperated phases. First, given files are splitted into sorted lists and saved on the disk. After, sorted lists are read from created intermediary files in batches and merged into bigger sorted lists and saved on the disk. This operation continues recursively until only one sorted final file remains. 

### 1. Splittig Phase

Each given file splitted seperately. Program reads as many lines as memory limit permits into a string array. Then array is sorted with `Array.Sort` method. Which uses `QuickSort` algorthm which is a in-place sorting algorithms that uses no extra memory. And finally sorted array is saved in a temporary text file in `tmp_output` folder. 

### 2. Merging Phase

Program reads every intermediatery file through `ChunkReader` class. This class opens a file and iterates over it. `ChunkReader` class holds only one search term in memory at any time which is accessed via `GetValue()` method.

Program opens as many files as memory limit permits at the same time. And reads from a file only one line at each iteration. Then applies `k-way merge` to open sorted lists. 

This operation continues recursively until only one big sorted file remains. This is the final output of the program.

#### K-way Merge Sort
Program reads one line from each open sorted list file. Puts the values into a string array. Sortes the buffer array with `Array.Sort` then gets the minimum value and writes to open output file. Then program reads one more line from the file which has the minimum value at previous step. 


### Potential Improvements

At first phase, each file is splitted seperately. This results some temp files having less lines then memory limit. If there is more than one input file, program can start reading the next file into buffer as ling as memory limit permits. So, total number of files created in first can be reduced. This may result a slight performance gain due to less disk IO.