using System;
using System.Collections.Generic;
namespace CSVReadfile.Models
{
    public class AreaScoreModel
    {
        public string Id { get; set; }
        public string NameParty { get; set; }
        public string NameArea { get; set; }
        public List <string> Candidate { get; set; }
        public int TotalScore { get; set; }

    }
}
