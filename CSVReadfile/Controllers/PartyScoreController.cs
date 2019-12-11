using System;
using MongoDB.Driver;
using System.Linq;
using CSVReadfile.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CSVReadfile.Controllers
{
    [Route("[Controller]/[Action]")]
    public class PartyScoreController
    {
        IMongoCollection<PartyScoreModel> partyScoreCollection { get; set; }
        public PartyScoreController()
        {
            var client = new MongoClient("mongodb+srv://admin:admin1234@mydata-irpe9.azure.mongodb.net/test?retryWrites=true&w=majority");
            var database = client.GetDatabase("ElectionMana");
            partyScoreCollection = database.GetCollection<PartyScoreModel>("PartyScore");
        }
        [HttpGet]
        public List<PartyScoreModel> GetAllPartyScore()
        {
            var partyScoreData = partyScoreCollection.Find(it => true).ToList();
            return partyScoreData;
        }

        [HttpGet("{nameParty}")]
        public List<PartyScoreModel> GetPartyScoreDataByNameParty(string nameParty)
        {
            var partyScoreData = partyScoreCollection.Find(it => it.NameParty == nameParty).ToList();
            return partyScoreData;
        }

        [HttpGet]
        public List<PartyScoreModel>  GetMaxScore()
        {
            var maxScore = partyScoreCollection
                .Find(it => true)
                .Project(it => it.TotalScore)
                .ToList()
                .Max();
            var maxPartyScoreData = partyScoreCollection
                .Find(it => it.TotalScore == maxScore)
                .ToList();
            return maxPartyScoreData;
        }

        [HttpGet]
        public List<PartyScoreModel> GetMinScore()
        {
            var minScore = partyScoreCollection
                .Find(it => true)
                .Project(it => it.TotalScore)
                .ToList()
                .Min();
            var minPartyScoreData = partyScoreCollection
                .Find(it => it.TotalScore == minScore)
                .ToList();
            return minPartyScoreData;
        }

    }
}
