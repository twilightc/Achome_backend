using System;
using System.Collections.Generic;

namespace AchomeModels.DbModels
{
    public partial class MerchandiseQa
    {
        public string MerchandiseId { get; set; }
        public int Seq { get; set; }
        public string QuestionAccount { get; set; }
        public string Question { get; set; }
        public DateTime AskingTime { get; set; }
        public string Answer { get; set; }
        public DateTime? AnswerTime { get; set; }
    }
}
