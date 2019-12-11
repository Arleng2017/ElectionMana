using System;
using System.Collections.Generic;
using System.IO;
using CSVReadfile.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Linq;
namespace CSVReadfile.Controllers
{
    [Route("[Controller]/[Action]")]
    public class ElectionController
    {
        IMongoCollection<ElectionModel> electionScoreCollection { get; set; }
        IMongoCollection<PartyScoreModel> partyScoreCollection { get; set; }
        public ElectionController()
        {
            var client = new MongoClient("mongodb+srv://admin:admin1234@mydata-irpe9.azure.mongodb.net/test?retryWrites=true&w=majority");
            var database = client.GetDatabase("ElectionMana");
            electionScoreCollection = database.GetCollection<ElectionModel>("ElectionScore");
            partyScoreCollection = database.GetCollection<PartyScoreModel>("PartyScore");
        }

        [HttpGet]
        public List<ElectionModel> test()
        {
            var read = new ReadCSV();
            return read.ReadDataFromCSVFile();
        }

        [HttpPost]
        public void ReadDataFromFileCsv()
        {
            var read = new ReadCSV();
            var electionDataList = read.ReadDataFromCSVFile();
            electionScoreCollection.InsertMany(electionDataList);
        }
        [HttpGet]
        public List<ElectionModel> GetAllElectionData()
        {
            var electionScoreData = electionScoreCollection.Find(it => true).ToList();
            return electionScoreData;
        }

        [HttpGet]
        public List<ElectionModel> GetElectionDataByNameArea(string nameArea)
        {
            var electionNameAreaData = electionScoreCollection.Find(it => it.NameArea == nameArea).ToList();
            return electionNameAreaData;
        }

        [HttpGet]
        public List<ElectionModel> GetElectionDataByNameParty(string nameParty)
        {
            var electionNamePartyData = electionScoreCollection.Find(it => it.NameParty == nameParty).ToList();
            return electionNamePartyData;
        }
        [HttpGet("{nameParty}/{nameArea}")]
        public List<ElectionModel> GetByNamePartyAndNameArea(string nameParty, string nameArea)
        {
            var electionData = electionScoreCollection.Find(it => it.NameArea == nameArea && it.NameParty == nameParty).ToList();
            return electionData;
        }
        [HttpGet("{nameArea}")]
        public object GetTheWinnerDataOfParty(string nameArea)
        {
            var listTotalScore = electionScoreCollection
                .Find(it => it.NameArea == nameArea)
                .ToList()
                .GroupBy(it => it.NameParty)
                .Select(it => new
                {
                    nameParty = it.Key,
                    sumScore = it.Sum(x => x.Score)
                });
            //var maxScore = listTotalScore.Max(it => it.sumScore);
            var winnerDataOfParty = listTotalScore
                .FirstOrDefault(it => it.sumScore == listTotalScore.Max(x => x.sumScore));
            return winnerDataOfParty;
        }
        [HttpGet]
        public void GetAllScoreParty()
        {
            var read = new ReadCSV();
            var electionDataList = read.ReadDataFromCSVFile();
            var totalPartyScore = new List<PartyScoreModel>();
            var electionPartGroup = electionDataList.GroupBy(it => it.NameParty).ToList();
            foreach (var item in electionPartGroup)
            {
                totalPartyScore.Add(
                    new PartyScoreModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        NameParty = item.Key,
                        TotalScore = item.Sum(it => it.Score)
                    });
            }
            partyScoreCollection.InsertMany(totalPartyScore);
        }
    }
}
