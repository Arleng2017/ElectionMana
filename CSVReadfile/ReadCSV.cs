using System;
using System.Collections.Generic;
using System.IO;
using CSVReadfile.Models;

namespace CSVReadfile
{
    public class ReadCSV
    {

        private readonly string csvFile;
        public ReadCSV()
        {
            csvFile = @"DataElection.csv";
        }
        public List<ElectionModel> ReadDataFromCSVFile()
        {
            var electionDataList = new List<ElectionModel>();
            using (var reader = new StreamReader(csvFile))
            {
                while (!reader.EndOfStream)
                {
                    var dataFromFile = reader.ReadLine();
                    var splitData = dataFromFile.Split(",");
                    var electionData = new ElectionModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = splitData[2],
                        NameParty = splitData[4],
                        No = splitData[3],
                        NameArea = $"{splitData[0]}{splitData[1]}",
                        Score = Int32.Parse(splitData[5])
                    };
                    electionDataList.Add(electionData);
                }
            }
            return electionDataList;
        }

        public void ReadScoreParty()
        {
            ReadCSV read = new ReadCSV();
            var electionData = read.ReadDataFromCSVFile();
        }
        //private readonly string csvFile;
        //public ReadCSV()
        //{
        //    csvFile = @"DataElection.csv";
        //}
        //public List<ElectionModel> ReadDataFromCSVFile()
        //{
        //    var electionDataList = new List<ElectionModel>();
        //    using (var reader = new StreamReader(csvFile))
        //    {
        //        while (!reader.EndOfStream)
        //        {
        //            var dataFromFile = reader.ReadLine();
        //            var splitData = dataFromFile.Split(",");
        //            var electionData = new ElectionModel() {
        //                Id = Guid.NewGuid().ToString(),
        //                Name = splitData[2],
        //                NameParty = splitData[4],
        //                No = splitData[3],
        //                NameArea = $"{splitData[0]}{splitData[1]}",
        //                Score = Int32.Parse(splitData[5])
        //            };
        //            electionDataList.Add(electionData);
        //        }
        //    };
        //    return electionDataList;
        //}


        //private readonly string FilePath;
        //public ReadCSV()
        //{
        //    FilePath = @"DataElection.csv";
        //}

        //public void ReadData()
        //{
        //    using (var reader = new StreamReader(FilePath))
        //    {
        //        while (!reader.EndOfStream)
        //        {
        //            var dataFromReader = reader.ReadLine();
        //            var listDataFromFile = dataFromReader.Split(',');
        //        }
        //    }

        //}
        //private readonly string filePath;
        //public ReadCSV()
        //{
        //    filePath = @"DataElection.csv";
        //}

        //public void ReadData()
        //{
        //    using (var reader = new StreamReader(filePath))
        //    {
        //        while (!reader.EndOfStream)
        //        {
        //            var dataFromFile = reader.ReadLine();
        //            var splitData = dataFromFile.Split(',');
        //            var data = new ElectionModel()
        //            {
        //                Id = Guid.NewGuid().ToString(),
        //                Name = splitData[2],
        //                NameParty = splitData[4],
        //                No = splitData[3].ToString(),
        //                NameArea = splitData[0],
        //                score = Int32.Parse(splitData[5])
        //            };
        //        }
        //    }

        //}


        //private readonly string csvFile;

        //public ReadCSV()
        //{
        //    csvFile = @"DataElection.csv";
        //}

        //public List<ElectionModel> ReadDataFromFile()
        //{
        //    var electionDataList = new List<ElectionModel>();
        //    using (var reader = new StreamReader(csvFile))
        //    {
        //        while (!reader.EndOfStream)
        //        {
        //            var dataFromFile = reader.ReadLine();
        //            var splitData = dataFromFile.Split(',');
        //            var electionData = new ElectionModel()
        //            {
        //                Id = Guid.NewGuid().ToString(),
        //                Name = splitData[2],
        //                NameParty = splitData[4],
        //                No = splitData[3],
        //                NameArea = splitData[0],
        //                Score = Int32.Parse(splitData[5])

        //            };
        //            electionDataList.Add(electionData);
        //        }
        //    }

        //    return electionDataList;
        //}
    }
}
