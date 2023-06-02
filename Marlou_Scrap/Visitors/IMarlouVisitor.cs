using MarlouScrap.Models.Marlou;

namespace MarlouScrap.Visitors
{
    public interface IMarlouVisitor
    {

        /// <summary>
        /// Return all beers / cidres
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        public List<AlcoolStats> GetBeers(int pages = 10);

        /// <summary>
        /// Return wines
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        public List<AlcoolStats> GetWines(int pages = 13);

        /// <summary>
        /// Return aperitifs and spirits
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        public List<AlcoolStats> GetAperitifs(int pages = 10);

        /// <summary>
        /// Return champagns
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        public List<AlcoolStats> GetChampagnes(int pages = 4);
    }
}
