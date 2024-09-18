namespace OrbitsCounterElekta;
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var fileName = "data.txt";//Added for convenience, but in real situation, should be replaced by config, same applies to hardcoded COM and split character.
        if (args != null && args.Length > 0 && !string.IsNullOrEmpty(args[0]))
        {
            fileName = args[0];
        }
        else
        {
            Console.WriteLine($"No input file provided. Using default {fileName} instead");
        }

        var totalOrbitsSumCounter = new TotalOrbitsSumCounter();
        totalOrbitsSumCounter.ReadDataFromFile(fileName);
        var totalAmountOfOrbits = totalOrbitsSumCounter.CountTotalSumOfOrbits();

        Console.WriteLine($"Filename {fileName}");
        Console.WriteLine($"Total amount of direct and indirect orbits is {totalAmountOfOrbits}");
#if DEBUG
        Console.WriteLine("Press enter to close. . .");
        Console.ReadLine();
#endif
    }    
}

class TotalOrbitsSumCounter
{
    private Dictionary<string, List<string>> directOrbits = new Dictionary<string, List<string>>();
    private int totalOrbitsCount;

    public void ReadDataFromFile(string fileName)
    {
        directOrbits.Clear();

        //Read data line by line, put them to the dictionary, where every local core object is a key and all orbitting objects are its values
        foreach (var line in File.ReadLines(fileName))
        {
            var items = line.Split(')');
            directOrbits.TryAdd(items[0], new List<string>());//Add the key if doesn't exist yet
            directOrbits[items[0]].Add(items[1]);//Add value
        }
    }

    public int CountTotalSumOfOrbits()
    {
        totalOrbitsCount = 0;
        countNext("COM", 0);
        return totalOrbitsCount;
    }

    private void countNext(string previousNode, int localOrbitsCount)
    {
        //Add current object orbits count to the total 
        totalOrbitsCount += localOrbitsCount;
        //If the current object is orbitted by any other objects, recursively count their orbits
        if(directOrbits.TryGetValue(previousNode, out List<string> childrenOrbits))
        {
            localOrbitsCount += 1;//Increment orbit counter 
            foreach (var childrenOrbit in childrenOrbits)
            {
                countNext(childrenOrbit, localOrbitsCount);
            }
        }
    }
}


