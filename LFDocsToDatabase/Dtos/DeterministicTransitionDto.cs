using System;
using System.Collections.Generic;
using System.Text;

namespace LFDocsToDatabase.Dtos
{
    internal class DeterministicTransitionDto
    {
        public string FromClass { get; set; }
        public string BeginsAtYears { get; set; }
        public string SucceedsTo { get; set; }
        public string AfterYears { get; set; }
    }
}
