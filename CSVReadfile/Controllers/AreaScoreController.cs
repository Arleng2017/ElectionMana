using System;
using MongoDB.Driver;
using System.Linq;
using CSVReadfile.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CSVReadfile.Controllers
{
    [Route("[Controller]/[Action]")]
    public class AreaScoreController
    {
        IMongoCollection<ElectionModel> electionScoreCollection { get; set; }
        IMongoCollection<AreaScoreModel> areaScoreCollection { get; set; }

        public AreaScoreController()
        {
            var client = new MongoClient("mongodb+srv://admin:admin1234@mydata-irpe9.azure.mongodb.net/test?retryWrites=true&w=majority");
            var database = client.GetDatabase("ElectionMana");
            electionScoreCollection = database.GetCollection<ElectionModel>("ElectionScore");
            areaScoreCollection = database.GetCollection<AreaScoreModel>("AreaScore");
        }
        [HttpGet]
        public List<AreaScoreModel> GetAreaScoreData()
        {
            var winnerInArea = new List<AreaScoreModel>();
            var electionDataList = electionScoreCollection.Find(it => true).ToList();
            var areaDataGroup = electionDataList
                 .GroupBy(it => it.NameArea)
                 .ToList();
            foreach (var item in areaDataGroup)
            {
                var namePartyDataGroup = item
                   .GroupBy(it => it.NameParty)
                   .ToList()
                    .Select(x => new
                    {
                        NameParty = x.Key,
                        NameArea = item.Key,
                        TotalScore = x.Sum(it => it.Score),
                        CandidateList = x.Select(it => it.Name).ToList()
                    });
                var maxDataInArea = namePartyDataGroup.FirstOrDefault(it => it.TotalScore == namePartyDataGroup.Max(x => x.TotalScore));
                winnerInArea.Add(new AreaScoreModel
                {
                    Id = Guid.NewGuid().ToString(),
                    NameParty = maxDataInArea.NameParty,
                    NameArea = maxDataInArea.NameArea,
                    Candidate = maxDataInArea.CandidateList,
                    TotalScore = maxDataInArea.TotalScore,
                });
            }
            areaScoreCollection.InsertMany(winnerInArea);
            return winnerInArea;
        }

        [HttpGet]
        public List<AreaScoreModel> GetAllAreaScoreData()
        {
            var areaScoreDataList = areaScoreCollection.Find(it => true).ToList();
            return areaScoreDataList;
        }

        [HttpGet("{nameArea}")]
        public List<AreaScoreModel> GetDataByNameArea(string nameArea)
        {
            var areaScoreData = areaScoreCollection.Find(it=>it.NameArea==nameArea).ToList();
            return areaScoreData;
        }

        [HttpGet("{nameArea}")]
        public List<string> GetDataByCandidate(string nameArea)
        {
            var areaScoreData = areaScoreCollection.Find(it=>it.NameArea==nameArea).FirstOrDefault().Candidate;
            return areaScoreData;
        }
    }
}
