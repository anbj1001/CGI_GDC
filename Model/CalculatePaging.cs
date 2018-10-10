namespace DatabaseAdmin.Model
{
    static public class CalculatePaging 
    {
        private static int defaultSetHitsPerPage;


        static public int CurrentPageNumber { get; private set; }
        static public int Offset { get; private set; }
        static public int TotalPages { get; private set; }

        //Sätter ett gigantiskt värde på HitsPerPage om något värde inte tilldelats
        static public int HitsPerPage
        {
            get
            {
                return defaultSetHitsPerPage <= 0 ? 1000000000 : defaultSetHitsPerPage;
            }
            private set
            {
                defaultSetHitsPerPage = value;
            }
        }

        /// <summary>
        /// Tilldelar HitsPerPage ett värde ist för defaultvärdet
        /// </summary>
        /// <param name="hitsPerPAgeNumber"></param>
        static public void SetHitsPerPage(int hitsPerPAgeNumber)
        {
            HitsPerPage = hitsPerPAgeNumber;
        }

        /// <summary>
        /// Beräknar vilket nummer nästa sida har och kalkylerar Offset för VisitorSearch
        /// </summary>
        static public void CalcNextPage()
        {
            CurrentPageNumber++;

            CalculateOffset();
        }
        /// <summary>
        /// Räknar ut föregående sida. Sätter värdet på propertien Offset genomm private metod.
        /// </summary>
        static public void CalcPrevPage()
        {
            CurrentPageNumber--;

            CalculateOffset();
        }

        /// <summary>
        /// Beräknar Offset och tilldelar public property värdet. Behövs ej retuneras något
        /// </summary>
        static private void CalculateOffset()
        {
            Offset = HitsPerPage * (CurrentPageNumber);
        }
        /// <summary>
        /// Använder HitsPerPage och totala hits i db för att beräkna antalet sidor. 
        /// Tillsätter property TotalPages värdet
        /// </summary>
        /// <param name="countTotalHits"></param>
        static public void ReturnTotalPages(int countTotalHits)
        {
            TotalPages = countTotalHits / HitsPerPage;

            if (countTotalHits % HitsPerPage >= 1)
            {
                TotalPages += 1;
            }
            if (TotalPages <= 0)
            {
                TotalPages = 1;
            }
        }
    }
}
