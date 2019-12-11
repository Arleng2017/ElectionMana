using System;
using System.Collections.Generic;
using System.Linq;
using CSVReadfile.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
namespace CSVReadfile.Controllers
{
    [Route("[Controller]/[Action]")]
    public class TotalAreaScoreController
    {
        IMongoCollection<ElectionModel> electionScoreCollection { get; set; }
        IMongoCollection<AreaScoreModel> totalAreaScoreCollection { get; set; }
        public TotalAreaScoreController()
        {
            var client = new MongoClient("mongodb+srv://admin:admin1234@mydata-irpe9.azure.mongodb.net/test?retryWrites=true&w=majority");
            var database = client.GetDatabase("ElectionMana");
            electionScoreCollection = database.GetCollection<ElectionModel>("ElectionScore");
            totalAreaScoreCollection = database.GetCollection<AreaScoreModel>("TotalAreaScore");
        }

        [HttpGet]
        public List<AreaScoreModel> TotalScoreAreaData()
        {
            var totalScoreAreaDataList = new List<AreaScoreModel>();
            var electionScore = electionScoreCollection.Find(it => true)
                 .ToList()
                 .GroupBy(it => it.NameArea);
            foreach (var item in electionScore)
            {
                var namePartyDataGroup = item
                    .GroupBy(it => it.NameParty)
                    .ToList()
                    .Select(x => new AreaScoreModel {
                        Id = Guid.NewGuid().ToString(),
                        NameParty = x.Key,
                        NameArea = item.Key,
                        Candidate = x.Select(it => it.Name).ToList(),
                        TotalScore = x.Sum(it => it.Score),
                    });
                foreach (var party in namePartyDataGroup)
                {
                    totalScoreAreaDataList.Add(party);
                }
            }
            totalAreaScoreCollection.InsertMany(totalScoreAreaDataList);
            return totalScoreAreaDataList;

        }
        [HttpGet]
        public List<AreaScoreModel> GetTotalScoreAreaData()
        {
            var totalScoreAreaData = totalAreaScoreCollection.Find(it=>true).ToList();
            return totalScoreAreaData;
        }
        [HttpGet("{nameParty}")]
        public List<AreaScoreModel> GetTotalScoreByNameParty(string nameParty)
        {
            var totalScoreArea = totalAreaScoreCollection.Find(it=>it.NameParty==nameParty).ToList();
            return totalScoreArea;
        }
        [HttpGet("{nameArea}")]
        public List<AreaScoreModel> GetTatalScoreByName(string nameArea) {
            var totalScoreAreaData = totalAreaScoreCollection
                .Find(it=>it.NameArea==nameArea)
                .ToList();
            return totalScoreAreaData;
        }

        [HttpGet("{name}")]
        public AreaScoreModel GetTotalScoreByName(string name)
        {
            var totalScoreAreaData = totalAreaScoreCollection.Find(it=>it.Candidate.Contains(name)).FirstOrDefault();
            return totalScoreAreaData; 
        }
    }
}
