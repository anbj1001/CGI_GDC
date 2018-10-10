using System.Collections.Generic;


namespace DatabaseAdmin.Model
{
    public class VisitorSearchReturn
    {
        // man kan ha en vy-klass för typ varje önskad vy, det innebär att man har hela objektet men bara den egenskap som behövs
        public List<VisitorSearch> VisitorSearches { get; set; }
        public int CountTotalHits { get; set; }


        public VisitorSearchReturn()
        {
            VisitorSearches = new List<VisitorSearch>();
        }
    }
}
